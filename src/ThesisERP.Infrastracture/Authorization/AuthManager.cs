using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

using ThesisERP.Core.Entities;
using ThesisERP.Application.DTOs;
using ThesisERP.Application.Models;
using ThesisERP.Application.Interfaces;

using AutoMapper;

namespace ThesisERP.Infrastracture.Authorization;

public class AuthManager : IAuthManager
{
    private readonly UserManager<AppUser> _userManager;
    private readonly JwtSettings _jwtSettings;
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    private AppUser _user;

    public AuthManager(UserManager<AppUser> userManager, IOptions<JwtSettings> jwtSettings, IAppDbContext context, IMapper mapper)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
        _context = context;
        _mapper = mapper;
    }

    #region Actions

    public async Task<AuthResult> ValidateUser(LoginUserDTO userDTO, string ipAddress)
    {
        _user = await _userManager.FindByNameAsync(userDTO.Email);

        if (_user == null || !await _userManager.CheckPasswordAsync(_user, userDTO.Password))
        {
            return new AuthError();
        }

        var jwtToken = await _CreateJwtToken();
        var refreshToken = _user.AddRefreshToken(ipAddress);

        _user.RemoveOldRefreshTokens(_jwtSettings.RefreshTokenTTL);

        _context.AppUsers.Update(_user);

        await _context.SaveChangesAsync();

        return new AuthSuccess(jwtToken, refreshToken.Token);
    }
    public async Task<AuthResult> RefreshUser(string token, string ipAddress)
    {
        try
        {
            _user = await _GetUserByRefreshToken(token);
        }
        catch (Exception)
        {
            return new AuthError();
        }

        var refreshToken = _user.RefreshTokens.Single(x => x.Token == token);

        if (refreshToken.IsRevoked)
        {
            // revoke all descendant tokens in case this token has been compromised
            _user.RevokeAllDescendantRefreshTokens(refreshToken,
                                                   ipAddress,
                                                   $"Attempted reuse of revoked ancestor token: {token}");

            _context.AppUsers.Update(_user);

            await _context.SaveChangesAsync();
        }

        if (!refreshToken.IsActive) { return new AuthError(); }

        // replace old refresh token with a new one (rotate token)
        var newRefreshToken = RefreshToken.RotateRefreshToken(refreshToken, ipAddress);

        _user.RefreshTokens.Add(newRefreshToken);

        // remove old refresh tokens from user
        _user.RemoveOldRefreshTokens(_jwtSettings.RefreshTokenTTL);

        // save changes to db
        _context.AppUsers.Update(_user);
        await _context.SaveChangesAsync();

        // generate new jwt
        var jwtToken = await _CreateJwtToken();

        return new AuthSuccess(jwtToken, newRefreshToken.Token);

    }
    public async Task<IdentityResult> RegisterUser(RegisterUserDTO registerUserDTO)
    {
        var user = _mapper.Map<AppUser>(registerUserDTO);

        user.UserName = registerUserDTO.Email;

        var result = await _userManager.CreateAsync(user, registerUserDTO.Password);

        if (!result.Succeeded)
        {
            return result;
        }

        await _userManager.AddToRolesAsync(user, new[] { "User" });

        return result;
    }

    public async Task<IdentityResult> ChangePassword(string username, ChangeUserPasswordDTO changePasswordDTO)
    {

        if (string.IsNullOrEmpty(username)) { return null; }

        var user = await _userManager.FindByNameAsync(username);

        if (user is null)
        {
            return null;
        }

        var response = await _userManager.ChangePasswordAsync(user, changePasswordDTO.OldPassword, changePasswordDTO.NewPassword);

        return response;
    }


    #endregion

    #region Helpers

    private async Task<string> _CreateJwtToken()
    {
        var signingCredentials = _GetSigningCredentials();
        var claims = await _GetClaims();
        var tokenOptions = _GenerateTokenOptions(signingCredentials, claims);

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }
    private JwtSecurityToken _GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {

        var expiration = DateTime.UtcNow
                         .AddMinutes(_jwtSettings.Lifetime);

        var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials
            );

        return token;
    }

    private async Task<List<Claim>> _GetClaims()
    {
        var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, _user.UserName),
                new Claim(ClaimTypes.GivenName, $"{_user.FirstName} {_user.LastName}"),
            };

        var roles = await _userManager.GetRolesAsync(_user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    private SigningCredentials _GetSigningCredentials()
    {
        var key = Environment.GetEnvironmentVariable("THESIS_JWT_KEY");
        var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

    }

    private async Task<AppUser> _GetUserByRefreshToken(string token)
    {
        var user = await _context.AppUsers.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

        return user ?? throw new Exception("Invalid token");
    }


    #endregion
}
