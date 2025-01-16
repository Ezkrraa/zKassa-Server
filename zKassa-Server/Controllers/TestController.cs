using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using zKassa_Server.Attributes;
using zKassa_Server.ControllerModels;
using zKassa_Server.Models;
using zKassa_Server.Services;

namespace zKassa_Server.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;
    private readonly UserManager<Employee> _userManager;
    private readonly ZDbContext _dbContext;

    public TestController(
        ILogger<TestController> logger,
        UserManager<Employee> userManager,
        ZDbContext dbContext
    )
    {
        _logger = logger;
        _userManager = userManager;
        _dbContext = dbContext;
    }

    [HttpPost("NewProduct")]
    public IActionResult NewProduct([FromBody] NewProduct product)
    {
        Product newProduct = product.ToProduct();
        _dbContext.Products.Add(newProduct);
        _dbContext.PriceLogs.Add(new PriceLog(newProduct.Id, newProduct.Price));
        _dbContext.SaveChanges();
        return Ok();
    }

    [HttpPost("NewAccount")]
    public async Task<IActionResult> NewEmployee([FromBody] NewEmployee newEmployee)
    {
        Employee user = newEmployee.ToEmployee();
        await _userManager.CreateAsync(user);
        await _userManager.AddPasswordAsync(user, newEmployee.Password);
        return Ok(user);
    }

    [HttpGet("GetAllProducts")]
    public async Task<IActionResult> GetAllProducts()
    {
        return Ok(
            _dbContext.Products.Select(product => new Tuple<ProductInfo, string>(
                new ProductInfo(product),
                product.EanCodes.First().EAN
            ))
        );
    }

    [HttpGet("{EanCode}")]
    public IActionResult GetProductInfo(string EanCode)
    {
        EanCode? code = _dbContext.EanCodes.FirstOrDefault(code => code.EAN == EanCode);
        if (code == null)
            return NotFound();
        if (code.Product == null)
        {
            return StatusCode(
                (int)HttpStatusCode.InternalServerError,
                "Ean did not have an associated product whilst being in the system"
            );
        }
        ProductInfo productInfo = new(code.Product);
        return Ok(productInfo);
    }

    [HttpPost("NewShop")]
    public IActionResult CreateShop([FromBody] NewShop newShop)
    {
        DistributionCenter? DistCenter = _dbContext.DistributionCenters.FirstOrDefault(center => center.Id == newShop.DistCenterId);
        if (DistCenter == null)
            return NotFound("No such distribution center is known");
        if (_dbContext.Shops.Any(shop => shop.Name == newShop.ShopName))
            return Conflict("Shop with this name already exists");

        Guid shopId = Guid.NewGuid();
        _dbContext.Shops.Add(new Shop(shopId, newShop.ShopName, DistCenter.Id));
        _dbContext.SaveChanges();
        return Ok(shopId);
    }

    [HttpPost("NewDistCenter")]
    public IActionResult CreateDistCenter([FromBody] string name)
    {
        if (_dbContext.DistributionCenters.Any(center => center.Name == name))
            return Conflict("Distribution center with this name already exists");

        Guid distCenterGuid = Guid.NewGuid();
        var distCenter = new DistributionCenter(distCenterGuid, name);
        _dbContext.DistributionCenters.Add(distCenter);
        _dbContext.SaveChanges();
        return Ok(distCenterGuid);
    }
}
