using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Services.ApplicationLayer.Users.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace Conduit.Auth.Infrastructure.JwtTokens
{
    public static class JwtRegistration
    {
        public static IServiceCollection AddJwtServices(
            this IServiceCollection services,
            Action<JwtTokenProviderOptions> optionsAction)
        {
            var options = new JwtTokenProviderOptions();
            optionsAction(options);
            return services.AddSingleton<JwtSecurityTokenHandler>()
                .AddScoped<ITokenProvider, JwtTokenProvider>()
                .Configure(optionsAction)
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                    JwtBearerDefaults.AuthenticationScheme,
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
                            }
                        };
                    })
                .Services.AddAuthorization(
                    authorizationOptions =>
                    {
                        authorizationOptions.AddPolicy(
                            JwtBearerDefaults.AuthenticationScheme,
                            builder => builder
                                .RequireAuthenticatedUser()
                                .RequireClaim(ClaimTypes.NameIdentifier)
                                .RequireClaim(JwtRegisteredClaimNames.Jti));

                        authorizationOptions.DefaultPolicy =
                            authorizationOptions.GetPolicy(
                                JwtBearerDefaults.AuthenticationScheme) ??
                            throw new InvalidOperationException();
                    });
        }

        private static Task ReceiveToken(MessageReceivedContext context)
        {
            var header = context.HttpContext.Request.Headers.Authorization[0];
            const string prefix = "Token ";
            if (header != null &&
                header.StartsWith(prefix))
            {
                header = header.Remove(0, prefix.Length);
            }

            context.Token = header;
            return Task.CompletedTask;
        }
    }
}
