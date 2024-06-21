using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{

    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize(policy: "Admin")]
    [HttpGet("")]
    public async Task<IActionResult> GetUsers(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10
    )
    {
        IEnumerable<GetUserDto> users = await _userService.GetUsersAsync(
            // pageIndex,
            // pageSize
        );
        if (users == null)
        {
            return NotFound();
        }
        return Ok(users);

    }


    [Authorize(policy: "Admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(string id)
    {
        GetUserDto user = await _userService.GetUserAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [Authorize(policy: "User")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto model)
    {
        var result = await _userService.UpdateUserAsync(id, model);
        if (!result.Succeeded)
        {   
            return BadRequest(result.Errors);
        
        }
        return Ok(new { Message = "User updated successfully!" });
        

    }


}
