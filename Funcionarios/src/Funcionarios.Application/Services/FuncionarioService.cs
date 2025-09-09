using Microsoft.AspNetCore.Mvc;
using Funcionarios.Application.Dto;
using Funcionarios.Application.Interfaces;
using Funcionarios.Domain.Models;
using Funcionarios.Infra.Data.Context;
using Funcionarios.Infra.Data.Interfaces;
using Funcionarios.Infra.Data.Repositories;
using Funcionarios.Infra.Data.Repositories.Parametros;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcionarios.Application.Services
{
    public class FuncionarioService: IFuncionarioService
    {
        public ApiContext _apiContext;
        private IFuncionarioRepository _funcionarioRepository;


        public FuncionarioService(IFuncionarioRepository funcionarioRepository, ApiContext apiContext)
        {

            _funcionarioRepository = funcionarioRepository;
            _apiContext = apiContext;

        }



        public async Task<ResultViewModel> GetFuncionarios(FuncionarioQuery funcionariosQuery)
        {
            var result = new ResultViewModel();

            try
            {
                var data = await _funcionarioRepository.GetAll(funcionariosQuery);
                result.Data = data;
                result.Count = data.Count();

                if (result.Count == 0)
                {
                    result.Mensagem = "Nenhum dado encontrado";
                }

            }
            catch (Exception ex)
            {
                result.Mensagem = ex.Message;
                result.Sucesso = false;
            }
            return result;
        }



        public async Task<ResultViewModel> CreateFuncionarios(FuncionarioCadastro funcionarioCadastro)
        {
            var result = new ResultViewModel();

            try
            {
                if (funcionarioCadastro == null)
                {
                    result.Data = null;
                    result.Mensagem = "Favor informar os dados";
                    result.Sucesso = false;
                }
                else
                {
                    var funcionario = new FuncionarioModel()

                    {
                        Id = funcionarioCadastro.Id,
                        Nome = funcionarioCadastro.Nome,
                        Sobrenome = funcionarioCadastro.Sobrenome,
                        //Ativo = true,                   
                        Departamento = funcionarioCadastro.Departamento,
                        Ativo = funcionarioCadastro.Ativo ?? true,
                        Turno = funcionarioCadastro.Turno,
                        DataDeCriacao = DateTime.Now,
                        DataAlteracao = DateTime.Now,
                        Email = funcionarioCadastro.Email,

                    };
                    //result.Dados = await _apiContext.UnitOfWork.Commit();
                    _apiContext.Add(funcionario); //Adicionou o elemento
                }

                await _apiContext.SaveChangesAsync(); // Salvou no banco


                result.Data = _apiContext.Funcionarios.ToList(); // Fazendo uma consulta geral no banco novamente
                return result;
            }
            catch (Exception ex)
            {
                result.Mensagem = ex.Message;
                result.Sucesso = false;
            }
            return result;
        }


        public async Task<ResultViewModel> GetFuncionariosById(int id)
        {
            var result = new ResultViewModel(await _funcionarioRepository.GetById(id));
            var operadora = await _apiContext.Funcionarios.FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<ResultViewModel> UpdateFuncionarios(FuncionarioUpdate funcionarioUpdate)
        {
            var result = new ResultViewModel();
            var funcionarios = await _apiContext.Funcionarios.FirstOrDefaultAsync(x => x.Id == funcionarioUpdate.Id);

            // Verifica se o funcionario foi encontrado
            if (funcionarioUpdate == null)
            {
                result.Sucesso = false;
                result.Mensagem = "Plano não encontrado.";
                return result;
            }



            // Atualiza os campos 

            funcionarios.Id = funcionarioUpdate.Id;
            funcionarios.Nome = funcionarioUpdate.Nome;
            funcionarios.Sobrenome = funcionarioUpdate.Sobrenome;
            funcionarios.Departamento = funcionarioUpdate.Departamento;
            funcionarios.Ativo = funcionarioUpdate.Ativo;
            funcionarios.Turno = funcionarioUpdate.Turno;
            funcionarios.DataDeCriacao = funcionarioUpdate.DataDeCriacao;
            funcionarios.DataAlteracao = DateTime.Now;
            funcionarios.Email = funcionarioUpdate.Email;



            // Salva as mudanças no contexto
            await _apiContext.SaveChangesAsync();



            // Adiciona o Funcionario atualizado ao resultado
            result.Data = _apiContext.Funcionarios.ToList();



            //result.Dados = await _planoRepository.UnitOfWork.Commit();


            // Prepara o resultado
            result.Sucesso = true;
            result.Mensagem = "Funcionario atualizado com sucesso.";
            result.Data = _apiContext.Funcionarios.ToList();


            return result;
        }


    }       
    }

