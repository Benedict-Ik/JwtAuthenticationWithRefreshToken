using JwtAuthenticationWithRefreshToken.Interfaces;
using JwtAuthenticationWithRefreshToken.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JwtAuthenticationWithRefreshToken.Services
{
    public class JwtService : IJwtService
    {
        private static readonly ConcurrentDictionary<string, (string Username, DateTime Expiration)> _refreshTokens = new();
        //private readonly IOptions<JwtSettings> options;
        private readonly JwtSettings _jwtSettings;

        public JwtService(IOptions<JwtSettings> options)
        {
            this._jwtSettings = options.Value;
        }

        public string GenerateAccessToken(string username)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Get properties from JwtSettings class
            var secretKey = _jwtSettings.Key;
            var expires = DateTime.Now.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);
            var issuer = _jwtSettings.Issuer;
            var audience = _jwtSettings.Audience;


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: cred
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken(string username)
        {
            // Create a 32‑byte random string
            var randomBytes = RandomNumberGenerator.GetBytes(32);
            var refreshToken = Convert.ToBase64String(randomBytes);
            var expiry = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);

            // Store (or replace) in memory
            _refreshTokens[refreshToken] = (username, expiry);

            return refreshToken;
        }

        public bool ValidateRefreshToken(string refreshToken, out string? username)
        {
            username = null;
            if (!_refreshTokens.TryGetValue(refreshToken, out var info))
                return false;

            // Check expiry
            if (info.Expiration < DateTime.UtcNow)
            {
                // if expired, remove
                _refreshTokens.TryRemove(refreshToken, out _);
                return false;
            }

            // if valid, remove
            _refreshTokens.TryRemove(refreshToken, out _);
            username = info.Username;
            return true;
        }
    }
}
