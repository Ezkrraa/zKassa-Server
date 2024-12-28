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
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly UserManager<Employee> _userManager;
        private readonly JwtService _jwtService;
        private readonly IConfiguration _configuration;

        public AuthenticationController(
            ILogger<AuthenticationController> logger,
            UserManager<Employee> userManager,
            JwtService jwtService,
            IConfiguration configuration
        )
        {
            _logger = logger;
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
            bool result = await _userManager.CheckPasswordAsync(user, loginModel.Password);
            if (result)
                return Ok(_jwtService.GenerateToken(user, _configuration)); // TODO: return new token
            return BadRequest("Bad password combo"); //TODO: replace in prod (shouldn't leak such info)
        }
    }
}
