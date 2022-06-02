using Microsoft.AspNetCore.Identity;
using ThesisERP.Application.DTOs;
using ThesisERP.Application.Models;

namespace ThesisERP.Application.Interfaces;

public interface IAuthManager
{
    Task<AuthResult> ValidateUser(LoginUserDTO userDTO, string ipAddress);
    Task<AuthResult> RefreshUser(string token, string ipAddress);
    Task<IdentityResult> RegisterUser(RegisterUserDTO registerUserDTO);
    Task<IdentityResult> ChangePassword(string username, ChangeUserPasswordDTO changePasswordDTO);
}
