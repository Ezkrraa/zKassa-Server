using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using zKassa_Server.ControllerModels;
using zKassa_Server.Models;
using zKassa_Server.Services;

namespace zKassa_Server.Controllers
{
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
            return Ok(product);
        }
    }
}
