using ThesisERP.Application.DTOs;
using ThesisERP.Application.Models;

namespace ThesisERP.Application.Interfaces;

public interface IAuthManager
{
    Task<AuthResult> ValidateUser(LoginUserDTO userDTO, string ipAddress);
    Task<AuthResult> RefreshUser(string token, string ipAddress);
}
