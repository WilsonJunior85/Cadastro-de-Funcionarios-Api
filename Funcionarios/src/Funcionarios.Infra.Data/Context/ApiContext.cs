using Funcionarios.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcionarios.Infra.Data.Context
{
    public class ApiContext : BaseContext, IUnitOfWork
    {
        public ApiContext(DbContextOptions<ApiContext> opt) : base(opt) { }

        //public DbSet<PlanoModel> tbPlano { get; set; }
        //public DbSet<DddModel> tbDDDs { get; set; }
        public DbSet<FuncionarioModel> Funcionarios { get; set; }
    }
}
