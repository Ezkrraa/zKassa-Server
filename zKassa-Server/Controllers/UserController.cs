﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using zKassa_Server.Attributes;
using zKassa_Server.ControllerModels;
using zKassa_Server.Models;
using zKassa_Server.Services;

namespace zKassa_Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [RoleCheck(Permission.CreateAccount)]
        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee([FromBody] NewEmployee newEmployee)
        {
            Employee user = newEmployee.ToEmployee();
            await _userManager.CreateAsync(user);
            await _userManager.AddPasswordAsync(user, newEmployee.Password);
            return Ok(user);
        }

        [RoleCheck(Permission.ViewAllEmployees)]
        [HttpGet("GetAllEmployees")]
        [Obsolete("Only for testing purposes since fetching all employees is a terrible idea")]
        public async Task<IEnumerable<EmployeeInformation>> GetEmployees()
        {
            return _dbContext.Users.Select(user => new EmployeeInformation(user)).ToList();
        }

        [RoleCheck(Permission.ChangeEmployee)]
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
