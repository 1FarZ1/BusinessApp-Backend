using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

public interface IAuthService
{
    Task<IdentityResult> RegisterUserAsync(RegisterModel model);
    Task<string> LoginUserAsync(LoginModel model);
}
