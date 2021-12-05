using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using ThesisERP.Application.DTOs;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Models;
using ThesisERP.Core.Entities;

namespace ThesisERP.Api
{    
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;        
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;

        public AccountController(UserManager<AppUser> userManager,            
                                 ILogger<AccountController> logger,
                                 IMapper mapper,
                                 IAuthManager authManager)
        {
            _userManager = userManager;            
            _logger = logger;
            _mapper = mapper;
            _authManager = authManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            _logger.LogInformation($"Registration attempt for {userDTO.Email} ");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _mapper.Map<AppUser>(userDTO);
            user.UserName = userDTO.Email;
            var result = await _userManager.CreateAsync(user, userDTO.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            await _userManager.AddToRolesAsync(user, userDTO.Roles);
            return Accepted();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO userDTO)
        {
            _logger.LogInformation($"Logging attempt for {userDTO.Email} ");
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _authManager.ValidateUser(userDTO, ipAddress());

            return _handleAuthorizationAttempt(response);
        }

        [HttpPost("refresh-user")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (refreshToken == null) return BadRequest();

            var response = await _authManager.RefreshUser(refreshToken, ipAddress());

            return _handleAuthorizationAttempt(response);
        }

        private void setTokenCookie(string token)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "0.0.0.0";
        }

        private IActionResult _handleAuthorizationAttempt(AuthResponse response)
        {
            if (response is AuthError)
            {
                return Unauthorized();
            }

            setTokenCookie(response.RefreshToken);

            return Ok(new { Token = response.JwtToken });
        }
    }
}
