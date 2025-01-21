using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    private readonly SignInManager<Employee> _authManager;
    private readonly JwtService _jwtService;
    private readonly IConfiguration _configuration;

    public AuthenticationController(
        ILogger<AuthenticationController> logger,
        SignInManager<Employee> authManager,
        UserManager<Employee> userManager,
    JwtService jwtService,
        IConfiguration configuration
    )
    {
        _logger = logger;
        _authManager = authManager;
        _userManager = userManager;
        _jwtService = jwtService;
        _configuration = configuration;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
        
        Employee? user = await _userManager.FindByNameAsync(loginModel.UserName);
        if (user == null)
            return NotFound("No such user");
        // TODO: check if user is permitted to use their drawer on that day etc.
        Microsoft.AspNetCore.Identity.SignInResult? signinResult = await _authManager.PasswordSignInAsync(user, loginModel.Password, true, false);
        if (signinResult == null || !signinResult.Succeeded)
            return Unauthorized("Invalid password");
        return Ok(new LoginResponse(_jwtService.GenerateToken(user, _configuration), user.Role));
    }
}
