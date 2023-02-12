using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Conduit.Shared.Tokens;

public static class JwtRegistrationExtensions
{
    public static IServiceCollection AddJwtServices(
        this IServiceCollection services,
        Action<JwtTokenProviderOptions> optionsAction)
    {
        var options = new JwtTokenProviderOptions();
        optionsAction(options);
        return services.Configure(optionsAction)
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                bearerOptions =>
                {
                    bearerOptions.Events = new()
                    {
                        OnMessageReceived = ReceiveToken
                    };
                    bearerOptions.MapInboundClaims = true;
                    bearerOptions.TokenValidationParameters = new()
                    {
                        ValidateAudience = true,
                        ValidAudience = options.Audience,
                        ValidateIssuer = true,
                        ValidIssuer = options.Issuer,
                        IssuerSigningKey = options.SymmetricSecurityKey,
                        ValidAlgorithms = new[]
                        {
                            options.SecurityKeyAlgorithm
                        },
                        ClockSkew = TimeSpan.Zero
                    };
                }).Services.AddAuthorization(authorizationOptions =>
            {
                authorizationOptions.AddPolicy(
                    JwtBearerDefaults.AuthenticationScheme,
                    builder => builder.RequireAuthenticatedUser()
                        .RequireClaim(ClaimTypes.NameIdentifier)
                        .RequireClaim(ClaimTypes.Name)
                        .RequireClaim(JwtRegisteredClaimNames.Jti)
                        .RequireClaim(JwtRegisteredClaimNames.Typ, "access"));
                authorizationOptions.DefaultPolicy =
                    authorizationOptions.GetPolicy(JwtBearerDefaults
                        .AuthenticationScheme) ??
                    throw new InvalidOperationException();
            });
    }

    private static Task ReceiveToken(
        MessageReceivedContext context)
    {
        var header =
            context.HttpContext.Request.Headers.Authorization.ToString();
        const string prefix = "Token ";
        if (header != null && header.StartsWith(prefix))
        {
            context.Token = header.Remove(0, prefix.Length).Trim();
        }

        return Task.CompletedTask;
    }
}
