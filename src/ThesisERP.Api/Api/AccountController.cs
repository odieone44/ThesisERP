using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ThesisERP.Application.DTOs;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Models;

namespace ThesisERP.Api;

/// <summary>
/// Login and other account functions. 
/// </summary>
public class AccountController : BaseApiController
{    
    private readonly ILogger<AccountController> _logger;    
    private readonly IAuthManager _authManager;

    public AccountController(ILogger<AccountController> logger,
                             IAuthManager authManager)
    {
        _logger = logger;
        _authManager = authManager;
    }

    /// <summary>
    /// Register a new user account.
    /// </summary>
    /// <remarks>
    /// Only an administrator account can register a new user.
    /// </remarks>
    /// <param name="userDTO">The user details</param>
    /// <returns></returns>
    [HttpPost]
    [Route("register")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDTO userDTO)
    {
        _logger.LogInformation($"Registration attempt for {userDTO.Email} ");
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authManager.RegisterUser(userDTO);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        return Accepted();
    }

    /// <summary>
    /// Login to authenticate further requests.
    /// </summary>    
    /// <response code="200">Returns a JWT used to authenticate requests, and a cookie containing a Refresh Token used to refresh the JWT after expiration.</response>
    /// <response code="401">If account information is not correct or account does not exist.</response>
    /// <response code="400">If the request body is not valid.</response>
    /// <param name="userDTO"></param>
    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthSuccessResponse))]
    public async Task<IActionResult> Login([FromBody] LoginUserDTO userDTO)
    {
        _logger.LogInformation($"Logging attempt for {userDTO.Email} ");

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var response = await _authManager.ValidateUser(userDTO, _ipAddress());

        return _handleAuthorizationAttempt(response);
    }

    /// <summary>
    /// Change your account's password.
    /// </summary>    
    /// <response code="204">On Success.</response>    
    /// <response code="401">If account information is not correct or account does not exist.</response>    
    /// <param name="userDTO"></param>    
    [HttpPost]
    [Route("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangeUserPasswordDTO userDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var username = HttpContext.User.Identity?.Name;

        _logger.LogInformation($"Password change attempt for {username} ");

        var response = await _authManager.ChangePassword(username!, userDTO);

        if (response is null) { return Unauthorized(); }

        return response.Succeeded ? NoContent() : Unauthorized();
    }

    /// <summary>
    /// Get a new JWT by using your Refresh Token. 
    /// </summary>
    /// <remarks>
    /// A refreshToken cookie should be sent with the request, containing the refresh token value. 
    /// </remarks>
    /// <response code="200">Returns a JWT used to authenticate requests, and a cookie containing a Refresh Token used to refresh the JWT after expiration.</response>
    /// <response code="401">If token is invalid.</response>
    /// <response code="400">If no token is provided.</response>
    [AllowAnonymous]
    [HttpPost("refresh-user")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthSuccessResponse))]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return BadRequest();
        }

        var response = await _authManager.RefreshUser(refreshToken, _ipAddress());

        return _handleAuthorizationAttempt(response);
    }

    private void _setTokenCookie(string token)
    {
        // append cookie with refresh token to the http response
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            Secure = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("refreshToken", token, cookieOptions);
    }

    private string _ipAddress()
    {
        // get source ip address for the current request
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            return Request.Headers["X-Forwarded-For"];
        }
        else
        {
            return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "0.0.0.0";
        }
    }

    private IActionResult _handleAuthorizationAttempt(AuthResult result)
    {
        if (result is AuthSuccess success)
        {
            _setTokenCookie(success.RefreshToken);

            return Ok(new AuthSuccessResponse(success));
        }

        return Unauthorized();
    }
}
