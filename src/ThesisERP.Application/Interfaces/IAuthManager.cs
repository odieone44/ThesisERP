using ThesisERP.Application.DTOs;
using ThesisERP.Application.Models;

namespace ThesisERP.Application.Interfaces;

public interface IAuthManager
{
    Task<AuthResponse> ValidateUser(LoginUserDTO userDTO, string ipAddress);
    Task<AuthResponse> RefreshUser(string token, string ipAddress);
}
