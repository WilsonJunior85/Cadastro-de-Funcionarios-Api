using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcionarios.Infra.Data.Repositories.Parametros
{
    public class FuncionarioQuery: Query
    {
        public int? Id { get; set; }
        public string? Nome { get; set; }
        public string? Sobrenome { get; set; }
        public string? Departamento { get; set; }
        public bool? Ativo { get; set; }
        public string? Turno { get; set; }
        public DateTime? DataDeCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string? Email { get; set; }
    }
}
