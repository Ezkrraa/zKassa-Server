using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web.Resource;
using zKassa_Server.Attributes;
using zKassa_Server.ControllerModels;
using zKassa_Server.Models;
using zKassa_Server.Services;

namespace zKassa_Server.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly UserManager<Employee> _userManager;
    private readonly ZDbContext _dbContext;

    public ProductController(
        ILogger<ProductController> logger,
        UserManager<Employee> userManager,
        ZDbContext dbContext
    )
    {
        _logger = logger;
        _userManager = userManager;
        _dbContext = dbContext;
    }

    [RoleCheck(Permission.GetProductInfo)]
    [HttpGet("{EanCode}")]
    public IActionResult GetProductInfo(string EanCode)
    {
        EanCode? code = _dbContext.EanCodes.FirstOrDefault(code => code.EAN == EanCode);
        if (code == null)
            return NotFound("No such item in the database");
        if (code.Product == null)
        {
            return StatusCode(
                (int)HttpStatusCode.InternalServerError,
                "Ean did not have an associated product whilst being in the system"
            );
        }
        Employee currentUser = GetEmployee();
        ProductInfo productInfo = new(
            code.Product,
            currentUser
                .Shop.DistributionCenter.ProductStatuses.OrderByDescending(status =>
                    status.TimeStamp
                )
                .FirstOrDefault(status => status.ProductId == code.ProductId)
                ?.Status
        );
        return Ok(productInfo);
    }

    [RoleCheck(Permission.GetExpandedProductInfo)]
    [HttpGet("Expanded/{EanCode}")]
    public IActionResult GetExpandedInfo(string EanCode)
    {
        EanCode? code = _dbContext.EanCodes.FirstOrDefault(code => code.EAN == EanCode);
        if (code == null)
            return NotFound("No such item in the database");
        if (code.Product == null)
        {
            return StatusCode(
                (int)HttpStatusCode.InternalServerError,
                "Ean did not have an associated product whilst being in the system"
            );
        }
        return Ok(new ExpandedProductInfo(code.Product));
    }

    [RoleCheck(Permission.CreateProduct)]
    [HttpPost]
    public IActionResult NewProduct([FromBody] NewProduct product)
    {
        Product newProduct = product.ToProduct();
        foreach (string productEan in product.EanCodes)
        {
            EanCode? foundEan = _dbContext.EanCodes.FirstOrDefault(code => code.EAN == productEan);
            if (foundEan != null)
                return Conflict(
                    $"Found conflicting product '{foundEan.Product.Name}' with EAN '{foundEan.EAN}'"
                );
            _dbContext.EanCodes.Add(new EanCode(newProduct.Id, productEan));
        }

        if (!_dbContext.Categories.Any(cat => cat.Name == newProduct.CategoryName))
            return BadRequest("Category does not exist");

        _dbContext.Products.Add(newProduct);
        _dbContext.PriceLogs.Add(new PriceLog(newProduct.Id, newProduct.Price));
        _dbContext.SaveChanges();
        return Ok();
    }

    [RoleCheck(Permission.UpdateProductAvailability)]
    [HttpPatch("UpdateStatus")]
    public IActionResult UpdateStatus([FromBody] UpdatedProductStatus status)
    {
        Product? existingProduct = _dbContext.Products.FirstOrDefault(product =>
            product.Id == status.ProductId
        );
        if (existingProduct == null)
            return NotFound("No product with that id in database");
        DistributionCenter? distributionCenter = _dbContext.DistributionCenters.FirstOrDefault(
            center => center.Id == status.DistributionCenterId
        );
        if (distributionCenter == null)
            return NotFound("No such distribution center exists in database");
        _dbContext.ProductStatuses.Add(
            new(existingProduct.Id, distributionCenter.Id, status.Status, DateTime.UtcNow)
        );
        _dbContext.SaveChanges();
        return Ok();
    }

    [RoleCheck(Permission.UpdateProductAvailability)]
    [HttpPatch("Recall")]
    public async Task<IActionResult> RecallProduct([FromBody] Guid productId)
    {
        Product? existingProduct = _dbContext.Products.FirstOrDefault(product =>
            product.Id == productId
        );
        if (existingProduct == null)
            return NotFound("No such product in db");

        await _dbContext.DistributionCenters.ForEachAsync(center =>
            center.ProductStatuses.Add(
                new ProductStatus(productId, center.Id, ProductStatusType.Recall, DateTime.UtcNow)
            )
        );
        await _dbContext.SaveChangesAsync();
        return Ok();
    }

    [NonAction]
    private Employee GetEmployee()
    {
        Employee? employee = _userManager.GetUserAsync(User).GetAwaiter().GetResult();

        return employee ?? throw new Exception("Unauthorized user in [Authorize] Controller???");
    }
}
