using System.Numerics;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using zKassa_Server.Attributes;
using zKassa_Server.ControllerModels;
using zKassa_Server.Models;
using zKassa_Server.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace zKassa_Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoryController : ControllerBase
    {
        private readonly ZDbContext _dbContext;

        public CategoryController(ZDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [RoleCheck(Permission.Categories)]
        [HttpGet]
        public IEnumerable<string> GetAll()
        {
            return _dbContext.Categories.Select(cat => cat.Name);
        }

        [RoleCheck(Permission.Categories)]
        [HttpPost("{name}")]
        public IActionResult Create(string name)
        {
            if (_dbContext.Categories.Any(c => c.Name == name))
                return Conflict("Category already exists");
            _dbContext.Categories.Add(new(name));
            _dbContext.SaveChanges();
            return Ok();
        }

        [RoleCheck(Permission.Categories)]
        [HttpGet("{name}")]
        public ActionResult<IEnumerable<ProductInfo>> GetByCategory(string name)
        {
            Category? category = _dbContext.Categories.FirstOrDefault(cat => cat.Name == name);
            if (category == null)
                return NotFound("No such category is known");
            return Ok(category.Products.Select(product => new ProductInfo(product)));
        }

        [RoleCheck(Permission.Categories)]
        [HttpDelete("{name}")]
        public IActionResult Delete(string name)
        {
            Category? category = _dbContext.Categories.FirstOrDefault(cat => cat.Name == name);
            if (category == null)
                return NotFound("No such category is known");
            if (category.Products.Any())
                return BadRequest(category.Products.Select(item => item.Name));
            _dbContext.Remove(category);
            _dbContext.SaveChanges();
            return NoContent();
        }
    }
}
