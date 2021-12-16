namespace ThesisERP.Application.Models;

public class AuthSuccessResponse
{
    public string Token { get; private set; }

    public AuthSuccessResponse(AuthSuccess authSuccess)
    {
        Token = authSuccess.JwtToken;
    }
}
