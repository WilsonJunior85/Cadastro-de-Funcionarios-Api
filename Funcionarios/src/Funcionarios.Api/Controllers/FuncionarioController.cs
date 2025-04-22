using Funcionarios.Application.Dto;
using Funcionarios.Application.Interfaces;
using Funcionarios.Application.Services;
using Funcionarios.Infra.Data.Context;
using Funcionarios.Infra.Data.Repositories;
using Funcionarios.Infra.Data.Repositories.Parametros;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Funcionarios.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuncionarioController : ControllerBase
    {
        private readonly IFuncionarioService _funcionarioService;
        private readonly FuncionarioRepository _funcionarioRepository;
        public ApiContext _apiContext;

        public FuncionarioController(IFuncionarioService funcionarioService, ApiContext apiContext)
        {
            _funcionarioService = funcionarioService;
            _apiContext = apiContext;
            //_downloadService = downloadService;
        }



        [HttpGet]
        public async Task<ActionResult> GetFuncionarios([FromQuery] FuncionarioQuery args)
        {
            try
            {
                var result = await _funcionarioService.GetFuncionarios(args);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var er = new ResultViewModel();
                er.AddNotification("Erro", ex.Message);
                return BadRequest(er);
            }

        }



        [HttpGet("{id}")]
        public async Task<ActionResult> GetFuncionariosById(int id)
        {
            try
            {
                var result = await _funcionarioService.GetFuncionariosById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var er = new ResultViewModel();
                er.AddNotification("Erro", ex.Message);
                return BadRequest(er);
            }

        }



        [HttpPost]
        public async Task<ActionResult> CreateFuncionario(FuncionarioCadastro args)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(new ResultViewModel(args, ModelState));
                var result = await _funcionarioService.CreateFuncionarios(args);
                return Ok(result);
            }
            catch (Exception ex)
            {

                var er = new ResultViewModel();
                er.AddNotification("Erro", ex.Message);
                return BadRequest(er);
            }
        }



        [HttpPut]
        public async Task<ActionResult> UpdateFuncionario(FuncionarioUpdate funcionarioUpdate)

        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(new ResultViewModel(funcionarioUpdate, ModelState));
                var result = await _funcionarioService.UpdateFuncionarios(funcionarioUpdate);
                return Ok(result);
            }
            catch (Exception ex)
            {

                var er = new ResultViewModel();
                er.AddNotification("Erro", ex.Message);
                return BadRequest(er);
            }




        }




    }
}
