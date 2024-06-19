using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;



// jwt  payload


public class JwtPayloadModel {
    public required string sub { get; set; }

    public required string email { get; set; }
}

public class LoginResponse {
    public required string Token { get; set; }
    
}

public class AuthService : IAuthService
{
    private readonly UserManager<UserModel> _userManager;
    private readonly IJwtService    _jwtService;


    public AuthService(UserManager<UserModel> userManager, IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }
    

    public async Task<IdentityResult> RegisterUserAsync(RegisterModel model)
    {
        var user = new UserModel { UserName = model.Username, Email = model.Email };
        IdentityResult? result = await _userManager.CreateAsync(user: user, model.Password);

            return result;

    }

    public async Task<bool> AssignRole(string userId, string role)
{
    var user = await _userManager.FindByIdAsync(userId);
    if (user == null)
    {
        throw new Exception("User not found");
    }

    var result = await _userManager.AddToRoleAsync(user, role);
    if (result.Succeeded)
    {
        return  true;
    }

    return      false;
}

#pragma warning disable CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
    public async Task<string?> LoginUserAsync(
#pragma warning restore CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
        [FromBody] LoginModel model
    )
    {
        UserModel? user = await _userManager.FindByEmailAsync(model.email);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            //TODO
            if (user.UserName == null || user.Email == null)
            {
                return null;
            }

            var payload = new JwtPayloadModel { sub = user.UserName, email = user.Email };
            var roles =  await  _userManager.GetRolesAsync(user);

            Console.WriteLine(roles.Count);
            for (int i = 0; i < roles.Count; i++)
            {
                    Console.WriteLine(roles[i]);
                }
            return _jwtService.GenerateToken(
                payload
            );
        }
        else
        {
            return null;
        }

        
    }
    

    // public async Task<UserModel> GetUserFromTokenAsync(string token)
    // {
    //     var tokenHandler = new JwtSecurityTokenHandler();
    //     var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]);
    //     tokenHandler.ValidateToken(token, new TokenValidationParameters
    //     {
    //         ValidateIssuerSigningKey = true,
    //         IssuerSigningKey = new SymmetricSecurityKey(key),
    //         ValidateIssuer = true,
    //         ValidateAudience = true,
    //         ValidIssuer = _configuration["JwtSettings:Issuer"],
    //         ValidAudience = _configuration["JwtSettings:Audience"],
    //         ValidateLifetime = true
    //     }, out SecurityToken validatedToken);

    //     var jwtToken = (JwtSecurityToken)validatedToken;
    //     var username = jwtToken.Claims.First(claim => claim.Type == "sub").Value;
    //     Console.WriteLine(username);
    //     return await _userManager.FindByNameAsync(username);
    // }
}
