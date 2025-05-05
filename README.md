# JwtAuthenticationWithRefreshToken

## Overview
Here is what we did in this branch:

- Created a `Services` folder which houses our , `JwtService` which implements the contract defined in the `IJwtService` class.  
- This class will:
    - Generating JWT (JSON Web Tokens) –> Short-lived access tokens for authentication.
    - Generating Refresh Tokens –> Long-lived tokens used to get new access tokens.
    - Validating Refresh Tokens –> Checking if they’re still valid and not expired.


## Code Explanation

### Why use JwtSettings claass rather than IConfiguration directly?
- IOptions\<JwtSettings> is cleaner and type-safe (avoids magic strings like "Jwt:Key").
- It automatically binds config values from appsettings.json to the JwtSettings class.

### The JwtService Constructor
- IOptions\<JwtSettings> injects the settings from appsettings.json.
- options.Value extracts the actual JwtSettings object.

### Important Concepts
- **Signing Credentials**: Used to sign the JWT. In this case, we use HMAC SHA256 algorithm with a secret key.
- **Claims**: Pieces of user info stored in the token (e.g., username, role). They are Key-value pairs that provide information about the user (like their ID and roles).
- **Token Expiration**: Access tokens are short-lived (e.g., 15 minutes), while refresh tokens can last longer (e.g., 7 days).
- **Token Validation**: The ValidateToken method checks if the token is valid, not expired, and has the correct audience and issuer.
- **Refresh Token Generation**: The GenerateRefreshToken method creates a new refresh token and saves it to the database.

### Why was ConcurrentDictionary used for Refresh Tokens?
- It allows for thread-safe operations, ensuring that multiple threads can access and modify the dictionary without causing data corruption.
- This is important in a web application where multiple requests might try to access or modify the same refresh token at the same time.
- Works for single-server apps (not distributed systems).
- For distributed apps, use a database.

### Refresh Token Flow
1. User logs in and receives an access token and a refresh token.
1. Access token is used for authentication.
1. When the access token expires, the user sends the refresh token to get a new access token.
1. The server checks if the refresh token is valid and not expired.
1. If valid, a new access token is generated and sent back to the user.
1. The old refresh token is invalidated, and a new one is generated.
1. The new refresh token is sent back to the user.
1. The user can now use the new access token for authentication.
1. This process can be repeated until the refresh token expires.
1. If the refresh token expires, the user must log in again to get a new access token and refresh token.
1. The refresh token can be stored in a secure place (e.g., HttpOnly cookie) to prevent XSS attacks.
1. The access token can be stored in memory or local storage, but it should be protected from XSS attacks.