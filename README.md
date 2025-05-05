# JwtAuthenticationWithRefreshToken

## Overview
- Here we registered the `JwtService` in the `Program.cs` class.

## Configuring JWT Settings
```csharp
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
```

- Binds the Jwt section from `appsettings.json` to the `JwtSettings` class.
- Stores the settings in the variable, `jwtSettings`, for later use.
 
## Registering `JwtService` as a Singleton
```csharp
builder.Services.AddSingleton<JwtService>();
```

- Makes JwtService available for dependency injection (DI).
- Singleton: Only one instance is created for the entire app.

## Setting up Authentication Middleware
```csharp
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer           = true,
        ValidateAudience         = true,
        ValidateLifetime         = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer              = jwtSettings.Issuer,
        ValidAudience            = jwtSettings.Audience,
        IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
        ClockSkew                = TimeSpan.Zero
    };
});
```

- Uses JwtBearerDefaults.AuthenticationScheme (standard for JWT).
- Sets validation rules for incoming tokens:
    - Checks if the Issuer matches (ValidIssuer).
    - Checks if the Audience matches (ValidAudience).
    - Validates the token’s expiry time (ValidateLifetime).
    - Ensures the token is signed correctly (IssuerSigningKey).
    - ClockSkew = TimeSpan.Zero: No tolerance for expired tokens.


## Adding Authentication Middleware
```csharp
app.UseAuthentication();
```

- Enables the authentication middleware in the request pipeline.