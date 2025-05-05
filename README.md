# JwtAuthenticationWithRefreshToken

Here is what we did in this branch:

- Created an `Interface` folder which houses our interface, `IJwtService` which defines a contract for the JWT service with the below methods:  
1. GenerateAccessToken(string username):
Generates a short-lived access token for the given username. It should take in a username parameter which is to be included in the JWT claims, and return a signed JWT access token

1. GenerateRefreshToken:  
Generates a secure, random refresh token and stores it with an expiration. It should take in a username parameter which is associated with the refresh token, and should return a newly generated refresh token string.

1. ValidateAccessToken  
Validates the refresh token, checks expiration, and removes it if valid. It should take in `refreshToken` parameter and output the associated `username` if valid. Finally, it should return "true" if the token is valid and not expired; otherwise, "false".