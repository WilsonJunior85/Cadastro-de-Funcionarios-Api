using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Funcionarios.Domain.Results
{
    public class ResultDomain: Result
    {
        public ResultDomain(object data)
        {
            Data = data;
        }

        public ResultDomain(int count)
        {
            Count = count;
        }

        public ResultDomain(object data, int count)
        {
            Data = data;
            Count = count;
        }

        public ResultDomain() { }
    }
}
