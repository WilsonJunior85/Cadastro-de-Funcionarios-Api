using Funcionarios.Domain.Models;
using Funcionarios.Infra.Data.Repositories.Parametros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcionarios.Infra.Data.Interfaces
{
    public interface IFuncionarioRepository : IRepository<FuncionarioModel>
    {
         Task<IEnumerable<FuncionarioModel>> GetAll(FuncionarioQuery args);
       
        void Insert(FuncionarioModel funcionariosModel);
         void Update(FuncionarioModel funcionariosModel);
        Task<FuncionarioModel> GetById(int id, bool track = false);
    }
}
