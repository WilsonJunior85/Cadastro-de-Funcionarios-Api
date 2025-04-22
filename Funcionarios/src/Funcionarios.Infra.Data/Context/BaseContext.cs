using Flunt.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcionarios.Infra.Data.Context
{
    public abstract class BaseContext : DbContext, IUnitOfWork
    {
        private readonly ILoggerFactory _loggerFactory;

        protected BaseContext(DbContextOptions options) : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder opt)
        {
            opt.UseLoggerFactory(_loggerFactory)
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                    e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            modelBuilder.Ignore<Notification>();
        }

        public async Task<bool> Commit()
        {
            try
            {
                return await base.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }

        }





    }
}
