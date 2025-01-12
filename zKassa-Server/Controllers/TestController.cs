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
        _dbContext.Products.Add(product.ToProduct());
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
}
