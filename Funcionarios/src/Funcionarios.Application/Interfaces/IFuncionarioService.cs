using Funcionarios.Application.Dto;
using Funcionarios.Infra.Data.Repositories.Parametros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcionarios.Application.Interfaces
{
    public interface IFuncionarioService
    {

        //Task<ResultViewModel> GetFuncionarios(FuncionariosQuery funcionariosQuery);
        // Task<ResultViewModel> CreateFuncionarios(FuncionariosCadastro funcionariosModelCadastro);
        // Task<ResultViewModel> UpdateFuncionarios(UpdateFuncionarios updateFuncionarios);
        //Task<ResultViewModel> GetFuncionariosByName(string nome);

        Task<ResultViewModel> GetFuncionarios(FuncionarioQuery funcionariosQuery);
        Task<ResultViewModel> CreateFuncionarios(FuncionarioCadastro funcionarioCadastro);
        Task<ResultViewModel> UpdateFuncionarios(FuncionarioUpdate funcionarioUpdate);
        Task<ResultViewModel> GetFuncionariosById(int id);
    }
}
