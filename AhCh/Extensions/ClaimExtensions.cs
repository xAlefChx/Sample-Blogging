using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AhCh.Extensions;

public static class ClaimExtensions
{
    public static int GetUserId(this IEnumerable<Claim> claims)
    {
        var userId = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid)!.Value;
        return int.Parse(userId);
    }
}
