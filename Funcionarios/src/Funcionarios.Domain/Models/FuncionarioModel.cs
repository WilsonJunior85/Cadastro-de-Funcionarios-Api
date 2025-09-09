using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcionarios.Domain.Models
{
    public class FuncionarioModel: Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]


        public int? Id { get; set; }
        public string? Nome { get; set; }
        public string? Sobrenome { get; set; }
        public string? Departamento { get; set; }
        public bool? Ativo { get; set; }
        public string? Turno { get; set; }
        public DateTime? DataDeCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string?Email { get; set; }

        public override void Validar()
        {
            throw new NotImplementedException();
        }
    }
}
