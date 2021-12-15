using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisERP.Application.Models;

public abstract class AuthResponse
{
    public string JwtToken { get; } = string.Empty;
    public string RefreshToken { get; } = string.Empty;
    public bool Authorized { get; }

    public AuthResponse()
    {
        Authorized = false;
    }

    public AuthResponse(string jwtToken, string refreshToken)
    {
        JwtToken = jwtToken;
        RefreshToken = refreshToken;
    }
}
