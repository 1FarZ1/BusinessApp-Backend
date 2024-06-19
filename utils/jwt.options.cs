public class JwtOptions {

    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required string Secret { get; set; }
    public int ExpirationInMinutes { get; set; }



}