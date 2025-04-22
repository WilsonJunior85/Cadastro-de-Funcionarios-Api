using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace SCC.Api.Extensions
{
    public static class ApiBehaviorExtensionExtension
    {
        public static void SuppressModelStateInvalid(this IServiceCollection services, bool val = true)
        {
            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = val;
            });
        }
    }
}
