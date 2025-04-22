using Microsoft.AspNetCore.Authorization;

namespace SCC.Api.Extensions
{
    public static class AuthorizationPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder RequireScope(this AuthorizationPolicyBuilder builder, params string[] scope)
        {
            return builder.RequireClaim("scope", scope);
        }
    }
}