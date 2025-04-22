using Funcionarios.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcionarios.Infra.Data.Mappings
{
    public class FuncionarioMap : IEntityTypeConfiguration<FuncionarioModel>
    {
        public void Configure(EntityTypeBuilder<FuncionarioModel> builder)
        {

            builder.ToTable("Funcionarios");

            builder.HasKey(x => x.Id);


            //builder.HasOne(x => x.funcionariosModel)
            //.WithMany()
            // .HasForeignKey(x => x.isnLinha);

        }
    }
}
