using Microsoft.Extensions.DependencyInjection;
using SCC.Api.Settings;

namespace SCC.Api.Extensions
{
    public static class AuthorizationExtensions
    {
        public static void ConfigureAuthorization(this IServiceCollection services, IdentitySettings config)
        {
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("scopes", builder =>
            //    {
            //        foreach (string scope in config.Scopes)
            //            builder.RequireScope(scope);

            //    });
            //});
            services.AddAuthorization(options =>
            {
                options.AddPolicy("scopes", builder =>
                {
                    if (config?.Scopes != null && config.Scopes.Any())
                    {
                        foreach (string scope in config.Scopes)
                            builder.RequireScope(scope);
                    }
                    else
                    {
                        // Se não houver scopes, cria uma policy "vaga" só pra evitar erro
                        builder.RequireAssertion(_ => true);
                    }
                });
            });

        }

    }
}
