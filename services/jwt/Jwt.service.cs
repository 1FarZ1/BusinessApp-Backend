using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;



public class JwtService : IJwtService
{
    private readonly JwtOptions jwtOptions;

    public JwtService(IOptions<JwtOptions> jwtOptions)
    {
        this.jwtOptions = jwtOptions.Value;
    }

    public string GenerateToken(
        JwtPayloadModel payload)
    {
        var tokenHandler = new JwtSecurityTokenHandler()
        ;

        var roles = payload.roles;

        foreach (var role in roles)
        {
            Console.WriteLine(role);
        }
     
        var key = Encoding.ASCII.GetBytes(jwtOptions.Secret);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, payload.sub),
            new Claim(JwtRegisteredClaimNames.Email, payload.email),
            new Claim(JwtRegisteredClaimNames.Jti, payload.jti),
        };

        foreach (string role in roles)
        {
                claims = claims.Append(new Claim(ClaimTypes.Role, role)).ToArray();
            }
        int expireTimeMinutes = jwtOptions.ExpirationInMinutes ;
        var tokenDescriptor = new SecurityTokenDescriptor
        {

            Issuer = jwtOptions.Issuer,
            Audience = jwtOptions.Audience,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(expireTimeMinutes)),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}