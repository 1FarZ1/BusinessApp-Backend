using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;



public class AssignRoleModel
{
    public required string UserId { get; set; }
    public required string Role { get; set; }
}


[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var result = await _authService.RegisterUserAsync(model);
        if (result.Succeeded)
        {
            return Ok(new { Message = "User registered successfully!" });
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var token = await _authService.LoginUserAsync(model);
        if (token != null)
        {
            return Ok(new { Token = token });
        }

        return Unauthorized();
    }


    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleModel model)
    {
    try
    {
        var result = await _authService.AssignRole(model.UserId, model.Role);
        if (result)
        {
            return Ok(new { Message = "Role assigned successfully!" });
        }

        return BadRequest(new { Message = "Role assignment failed!" });
    }
    catch (Exception ex)
    {
        return BadRequest(new { Message = ex.Message });
    }
    }
}
