using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

public interface IAuthService
{
    Task<IdentityResult> RegisterUserAsync(RegisterModel model);
    Task<string> LoginUserAsync(LoginModel model);

    Task<bool> AssignRole(string userId, string role);

    //get user  from a token in headers
    // Task<UserModel> GetUserFromTokenAsync(string token);
}
