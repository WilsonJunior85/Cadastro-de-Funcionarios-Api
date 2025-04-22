using Funcionarios.Domain.Models;
using Funcionarios.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcionarios.Infra.Data.Interfaces
{
    public interface IRepository<T> : IDisposable where T : Model
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
