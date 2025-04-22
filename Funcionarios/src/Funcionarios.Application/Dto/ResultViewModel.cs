using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Funcionarios.Application.Dto
{
    public class ResultViewModel : Result
    {

        public ResultViewModel(ModelStateDictionary data)
        {
            foreach (var erro in data)
            {
                foreach (var er in erro.Value.Errors)
                {
                    AddNotification(erro.Key, er.ErrorMessage);
                }
            }
        }

        public ResultViewModel(IdentityResult data)
        {
            foreach (var erro in data.Errors)
            {
                AddNotification(erro.Code, erro.Description);
            }
        }

        public ResultViewModel(object data, ModelStateDictionary state)
        {
            Data = data;

            foreach (var erro in state)
            {
                foreach (var er in erro.Value.Errors)
                {
                    AddNotification(erro.Key, er.ErrorMessage);
                }
            }
        }

        public ResultViewModel(object data, IdentityResult state)
        {
            Data = data;

            foreach (var erro in state.Errors)
            {
                AddNotification(erro.Code, erro.Description);
            }
        }

        public ResultViewModel() { }
        public ResultViewModel(object data) : base(data) { }
        public ResultViewModel(int count) : base(count) { }
        public ResultViewModel(bool sucesso) : base(sucesso) { }
        public ResultViewModel(object data, int count, bool sucesso) : base(data, count, sucesso) { }
    }
}
