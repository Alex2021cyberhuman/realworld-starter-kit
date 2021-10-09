using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Services.ApplicationLayer.Users;
using Conduit.Auth.Domain.Services.ApplicationLayer.Users.Tokens;
using Conduit.Auth.Domain.Users;
using Microsoft.Extensions.Options;
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

    public static class JwtTokenExtensions
    {
        public static SecurityKey GetSecurityKey(
            this JwtTokenProviderOptions opt)
        {
            return new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(opt.SecurityKey));
        }


        public static IEnumerable<Claim> GetCommonClaims(this User user)
        {
            yield return new(ClaimTypes.NameIdentifier, user.Id.ToString());
            yield return new(ClaimTypes.Name, user.Username);
            yield return new(
                ClaimTypes.Email,
                user.Email,
                ClaimValueTypes.Email);
        }

        public static SigningCredentials GetSecurityCredentials(
            this JwtTokenProviderOptions opt)
        {
            return new(opt.GetSecurityKey(), opt.SecurityKeyAlgorithm);
        }
    }

    public class JwtTokenProvider : ITokenProvider
    {
        private readonly JwtSecurityTokenHandler _handler;
        private readonly JwtTokenProviderOptions _options;

        public JwtTokenProvider(
            IOptions<JwtTokenProviderOptions> options,
            JwtSecurityTokenHandler handler)
        {
            _handler = handler;
            _options = options.Value;
        }

        #region ITokenProvider Members

        public Task<TokenOutput> CreateTokenAsync(User user)
        {
            var now = DateTime.UtcNow;
            var jti = Guid.NewGuid().ToString();
            var accessToken = GetAccessToken(user, jti, now);
            var accessTokenString = _handler.WriteToken(accessToken);
            return Task.FromResult(new TokenOutput(accessTokenString));
        }

        #endregion

        private JwtSecurityToken GetAccessToken(
            User user,
            string jti,
            DateTime now)
        {
            var claims = user.GetCommonClaims()
                .Append(new(JwtRegisteredClaimNames.Jti, jti));
            var header = new JwtHeader(_options.GetSecurityCredentials());
            var payload = new JwtPayload(
                _options.Issuer,
                null,
                claims,
                now,
                now.Add(_options.AccessTokenExpires),
                now);
            var accessToken = new JwtSecurityToken(header, payload);
            return accessToken;
        }
    }
}
