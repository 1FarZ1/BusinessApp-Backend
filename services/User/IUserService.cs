



using Microsoft.AspNetCore.Identity;

public class GetUserDto 
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public string[] Roles { get; set; }
}


public class UpdateUserDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}



public interface IUserService
{
    Task<GetUserDto> GetUserAsync(string userId);
    
    Task<IEnumerable<GetUserDto>> GetUsersAsync();

    Task<IdentityResult> UpdateUserAsync(string userId, UpdateUserDto model);
}