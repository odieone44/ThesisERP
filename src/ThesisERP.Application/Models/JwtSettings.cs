namespace ThesisERP.Application.Models;

public class JwtSettings
{
    public string Issuer { get; set; }
    public int Lifetime { get; set; }
    public int RefreshTokenTTL { get; set; }
}
