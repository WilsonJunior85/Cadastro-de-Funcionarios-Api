using Funcionarios.Application.Interfaces;
using Funcionarios.Application.Services;
using Funcionarios.Infra.Data.Interfaces;
using Funcionarios.Infra.Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Funcionarios.CrossCutting
{
    public static class Bootstrapper
    {
        public static void BootstrapDI(this IServiceCollection services, IConfiguration configuration)
        {
            //// Repositories    

            //services.AddScoped<IEstadosRepository, EstadosRepository>();
            services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();



            //// Application Services 

            //services.AddScoped<IEstadosService, EstadosService>();
            services.AddScoped<IFuncionarioService, FuncionarioService>();
        }
    }   
}

