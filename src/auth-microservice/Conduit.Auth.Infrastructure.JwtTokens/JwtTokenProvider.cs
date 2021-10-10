using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Services.ApplicationLayer.Users;
using Conduit.Auth.Domain.Services.ApplicationLayer.Users.Tokens;
using Conduit.Auth.Domain.Users;
using Microsoft.Extensions.Options;

namespace Conduit.Auth.Infrastructure.JwtTokens
{
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

        public Task<TokenOutput> CreateTokenAsync(
            User user,
            CancellationToken cancellationToken = default)
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
