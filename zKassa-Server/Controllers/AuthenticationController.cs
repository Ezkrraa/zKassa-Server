using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using zKassa_Server.ControllerModels;
using zKassa_Server.Models;
using zKassa_Server.Services;

namespace zKassa_Server.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly UserManager<Employee> _userManager;
    private readonly JwtService _jwtService;
    private readonly IConfiguration _configuration;
    private readonly SignInManager<Employee> _signInManager;

    public AuthenticationController(
        ILogger<AuthenticationController> logger,
        UserManager<Employee> userManager,
        JwtService jwtService,
        IConfiguration configuration,
        SignInManager<Employee> signInManager
    )
    {
        _logger = logger;
        _userManager = userManager;
        _jwtService = jwtService;
        _configuration = configuration;
        _signInManager = signInManager;
    }

    [EnableRateLimiting(Program.verySlowLimitName)]
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
        Employee? user = await _userManager.FindByNameAsync(loginModel.UserName);
        if (user == null)
            return NotFound("No such user");
        Microsoft.AspNetCore.Identity.SignInResult result =
            await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, true);
        if (!result.Succeeded)
            return BadRequest("Incorrect password");
        // TODO: check if user is permitted to use their drawer on that day etc.
        return Ok(new LoginResponse(_jwtService.GenerateToken(user, _configuration), user.Role));
    }
}
