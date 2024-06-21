











using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<UserModel> _userManager;
    private readonly SignInManager<UserModel> _signInManager;

    public UserService(ApplicationDbContext context, UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }


    public async Task<GetUserDto> GetUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return null;

        return new GetUserDto
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,

            //TODO
            Roles = (string[])await _userManager.GetRolesAsync(user),
        };

        
    }

    public async  Task<IEnumerable<GetUserDto>> GetUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        return users.Select(user => new GetUserDto
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            Roles = (string[])_userManager.GetRolesAsync(user).Result
        });
    }

    public async  Task<IdentityResult> UpdateUserAsync(string userId, UpdateUserDto model)
    {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            
            var preperties = typeof(UpdateUserDto).GetProperties();
            foreach (var property in preperties)
            {
                var value = property.GetValue(model);
                if (value != null)
                {
                    typeof(UserModel).GetProperty(property.Name)?.SetValue(user, value);
                }
            }
            await _userManager.UpdateAsync(user);

            return IdentityResult.Success;

    }
}