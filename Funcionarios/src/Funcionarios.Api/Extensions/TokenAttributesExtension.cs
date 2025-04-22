using System.IdentityModel.Tokens.Jwt;

namespace SCC.Api.Extensions
{
    public static class TokenAttributesExtension
    {
        public static string GetClaimValue(this JwtSecurityToken jwt, string claim)
        {
            return jwt.Claims
                .FirstOrDefault(x => x.Type.Equals(claim))
                .Value;
        }
    }
}
