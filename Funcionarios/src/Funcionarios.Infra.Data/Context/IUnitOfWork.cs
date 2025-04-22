using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcionarios.Infra.Data.Context
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
