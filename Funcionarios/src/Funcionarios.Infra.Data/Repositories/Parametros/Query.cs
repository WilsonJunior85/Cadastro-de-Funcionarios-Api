using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcionarios.Infra.Data.Repositories.Parametros
{
    public class Query
    {
        public int Page { get; set; } = 0;
        public int Limit { get; set; } = 0;
        public string OrderBy { get; set; } = "";
        public string OrderByOrder { get; set; } = "ASC";

        public int Skip()
        {
            return (Page - 1) * Limit;
        }
    }
}
