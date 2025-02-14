using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using zKassa_Server.Attributes;
using zKassa_Server.ControllerModels;
using zKassa_Server.Models;
using zKassa_Server.Services;

namespace zKassa_Server.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DistributionCenterController : ControllerBase
{
    private readonly ZDbContext _dbContext;
    private readonly UserManager<Employee> _userManager;
    private readonly ILogger<DistributionCenterController> _logger;

    public DistributionCenterController(
        ZDbContext dbContext,
        UserManager<Employee> userManager,
        ILogger<DistributionCenterController> logger
    )
    {
        _dbContext = dbContext;
        _logger = logger;
        _userManager = userManager;
    }

    [EnableRateLimiting(Program.verySlowLimitName)]
    [RoleCheck(Permission.CreateDistCenter)]
    [HttpPost]
    public IActionResult CreateNew([FromBody] string name)
    {
        if (_dbContext.DistributionCenters.Any(center => center.Name == name))
            return Conflict("Distribution center with this name already exists");

        Guid distCenterGuid = Guid.NewGuid();
        DistributionCenter distCenter = new(distCenterGuid, name);
        _dbContext.DistributionCenters.Add(distCenter);
        _dbContext.SaveChanges();
        return Ok(distCenterGuid);
    }

    [EnableRateLimiting(Program.slowLimitName)]
    [RoleCheck(Permission.GetDistCenterNames)]
    [HttpGet("GetAllNames")]
    public IActionResult GetAllNames()
    {
        return Ok(_dbContext.DistributionCenters.Select(center => new DistCenterInfo(center)));
    }
}
