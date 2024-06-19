public interface IJwtService
{
    string GenerateToken(JwtPayloadModel payload);
}