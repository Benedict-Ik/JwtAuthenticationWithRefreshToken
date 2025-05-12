using JwtAuthenticationWithRefreshToken.Models;
using JwtAuthenticationWithRefreshToken.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace JwtAuthenticationWithRefreshToken.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthRequest _authRequest;
        private readonly JwtService _jwtService;

        public AuthController(IOptions<AuthRequest> options, JwtService jwtService)
        {
            this._authRequest = options.Value;
            this._jwtService = jwtService;
        }

        [HttpPost]
        public IActionResult Login([FromBody] AuthRequest authRequest)
        {
            if (authRequest.Username == _authRequest.Username && authRequest.Password == _authRequest.Password)
            {
                Console.WriteLine($"Loaded user: {_authRequest.Username}, pass: {_authRequest.Password}");
                var accessToken = _jwtService.GenerateAccessToken(authRequest.Username);
                var refreshToken = _jwtService.GenerateRefreshToken(authRequest.Username);
                return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
            }
            Console.WriteLine($"Loaded user: {_authRequest.Username}, pass: {_authRequest.Password}");
            return Unauthorized("Invalid username or password.");
        }

        [HttpPost]
        public IActionResult RefreshToken([FromBody] string refreshToken)
        {
            if (_jwtService.ValidateRefreshToken(refreshToken, out var username))
            {
                var newAccessToken = _jwtService.GenerateAccessToken(username);
                var newRefreshToken = _jwtService.GenerateRefreshToken(username);
                return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
            }
            return Unauthorized("Invalid or expired refresh token.");
        }
    }

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SecretController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult Get()
            => Ok("You accessed a protected resource!");
    }

}
