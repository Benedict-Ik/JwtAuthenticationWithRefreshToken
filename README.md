# JwtAuthenticationWithRefreshToken

Here is what we did in this branch:

- Added a Jwt section in the `appsettings.json` file:
```json
{
  "Jwt": {
    "Key": "<Secret key>",
    "Issuer": "<Issuer>",
    "Audience": "<Audience>",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  }
}
```

- You can get a random secret key from [JwtSecret](https://jwtsecret.com/generate).
- The `Issuer` and `Audience` properties are set to the same URL which can be gotten from the `launchSettings.json` file found in the properties folder.
- Note that we are concerned with the URL of the applicationURL found in the https scheme.
- Also note that your `Key` must be greater than or equals to 32 characters for HMAC‑SHA256.