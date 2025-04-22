using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SCC.Api.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SCC.Api.Extensions
{
    public static class AuthenticationExtension
    {
        public static void ConfigureAuthentication(this IServiceCollection services, IdentitySettings config)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer("Bearer", opt =>
           {
               opt.RequireHttpsMetadata = false;
               opt.Authority = $"{config.IdentityServerUrl}";
               opt.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateAudience = false
               };

               opt.Events = new JwtBearerEvents()
               {
                   OnTokenValidated = ctx =>
                   {
                       var token = (JwtSecurityToken)ctx.SecurityToken;
                       (ctx.Principal.Identity as ClaimsIdentity).AddClaim(new Claim("token", token.RawData));

                       if (ctx.Principal?.Identity is ClaimsIdentity claimsIdentity)
                       {
                           var scopeClaims = claimsIdentity.FindFirst("scope");
                           if (scopeClaims != null)
                           {
                               claimsIdentity.RemoveClaim(scopeClaims);
                               claimsIdentity.AddClaims(scopeClaims.Value.Split(' ').Select(scope => new Claim("scope", scope)));
                           }
                       }


                       return Task.FromResult(0);
                   },
                   OnAuthenticationFailed = c =>
                   {
                       c.NoResult();

                       c.Response.StatusCode = 500;
                       c.Response.ContentType = "text/plain";
                       return c.Response.WriteAsync("Ocorreu um erro no Processo de autenticação");
                   }
               };
           });
        }
    }
}
