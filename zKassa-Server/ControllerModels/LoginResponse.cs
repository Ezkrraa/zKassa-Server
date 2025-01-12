using zKassa_Server.Models;

namespace zKassa_Server.ControllerModels;

public class LoginResponse
{
    public string JWT { get; set; }
    public Role Role { get; set; }

    public LoginResponse(string jWT, Role role)
    {
        JWT = jWT;
        Role = role;
    }
}
