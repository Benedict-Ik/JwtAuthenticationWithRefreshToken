namespace JwtAuthenticationWithRefreshToken.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(string username);

        string GenerateRefreshToken(string username);

        bool ValidateRefreshToken(string refreshToken, out string? username);
    }
}
