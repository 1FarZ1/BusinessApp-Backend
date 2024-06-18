using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

public interface IAuthService
{
    Task<IdentityResult> RegisterUserAsync(RegisterModel model);
    Task<string> LoginUserAsync(LoginModel model);

    //get user  from a token in headers
    Task<UserModel> GetUserFromTokenAsync(string token);
}
