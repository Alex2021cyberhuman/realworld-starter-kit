using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Conduit.Shared.Tokens;

public class JwtTokenProviderOptions
{
    public string SecurityKey { get; set; } = new('0', 32);

    public string SecurityKeyAlgorithm { get; set; } =
        SecurityAlgorithms.HmacSha256;

    public string Issuer { get; set; } = "Conduit.Auth";

    public string Audience { get; set; } = "Conduit.App";

    public TimeSpan AccessTokenExpires { get; set; } = TimeSpan.FromDays(1);

    public SymmetricSecurityKey SymmetricSecurityKey =>
        new(Encoding.ASCII.GetBytes(SecurityKey));
}
