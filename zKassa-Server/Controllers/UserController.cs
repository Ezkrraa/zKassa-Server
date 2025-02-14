using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using zKassa_Server.Attributes;
using zKassa_Server.ControllerModels;
using zKassa_Server.Models;
using zKassa_Server.Services;

namespace zKassa_Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly UserManager<Employee> _userManager;
        private readonly ZDbContext _dbContext;

        public UserController(
            [FromServices] UserManager<Employee> userManager,
            ZDbContext dbContext
        )
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [EnableRateLimiting(Program.verySlowLimitName)]
        [RoleCheck(Permission.CreateAnyAccount)]
        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee([FromBody] NewEmployee newEmployee)
        {
            Employee user = newEmployee.ToEmployee();
            if (newEmployee.Role <= Role.Manager && newEmployee.ShopId == null)
                return BadRequest("ShopId cannot be null for non-HQ employees");
            else if (newEmployee.Role >= Role.Admin && newEmployee.ShopId != null)
                return BadRequest("ShopId must be null for HQ employees");

            IdentityResult createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    createResult.Errors.Select(error => error.Description)
                );
            IdentityResult passwordResult = await _userManager.AddPasswordAsync(
                user,
                newEmployee.Password
            );
            if (!passwordResult.Succeeded)
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    passwordResult.Errors.Select(error => error.Description)
                );
            return Ok(user);
        }

        [EnableRateLimiting(Program.verySlowLimitName)]
        [RoleCheck(Permission.ViewAllEmployees)]
        [HttpGet("GetAllEmployees")]
        [Obsolete("Only for testing purposes since fetching all employees is a terrible idea")]
        public async Task<IEnumerable<EmployeeInformation>> GetEmployees()
        {
            return _dbContext.Users.Select(user => new EmployeeInformation(user)).ToList();
        }

        [EnableRateLimiting(Program.verySlowLimitName)]
        [RoleCheck(Permission.EditAnyEmployee)]
        [HttpPatch("UpdateEmployee")]
        [Obsolete(
            "Only for testing purposes since this should have WAY more checks, specific endpoints for specific things etc"
        )]
        public async Task<IActionResult> UpdateEmployee([FromBody] Employee newEmployee)
        {
            Employee foundEmployee = _dbContext.Users.First(user => user.Id == newEmployee.Id);
            if (foundEmployee == null)
                return NotFound("No such employee in DB (ID cannot be changed)");
            foundEmployee = newEmployee;
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
