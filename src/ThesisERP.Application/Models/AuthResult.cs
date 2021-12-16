namespace ThesisERP.Application.Models;

public abstract class AuthResult
{
    public string JwtToken { get; } = string.Empty;
    public string RefreshToken { get; } = string.Empty;
    public bool Authorized { get; }

    public AuthResult()
    {
        Authorized = false;
    }

    public AuthResult(string jwtToken, string refreshToken)
    {
        JwtToken = jwtToken;
        RefreshToken = refreshToken;
    }
}
