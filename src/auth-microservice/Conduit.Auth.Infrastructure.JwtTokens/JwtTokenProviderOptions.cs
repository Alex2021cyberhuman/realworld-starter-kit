using System;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;

namespace Conduit.Auth.Infrastructure.JwtTokens
{
    public class JwtTokenProviderOptions
    {
        public string SecurityKey { get; set; } = new('0', 32);

        public string SecurityKeyAlgorithm { get; set; } =
            SecurityAlgorithms.HmacSha256;

        public string Issuer { get; set; } =
            Assembly.GetEntryAssembly()?.GetName().Name!;

        public TimeSpan AccessTokenExpires { get; set; } =
            TimeSpan.FromHours(1);
    }
}
