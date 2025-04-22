using Flunt.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcionarios.Application.Dto
{
    public class Result : Notifiable<Notification>
    {
        public object Data { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public bool Sucesso { get; set; } = true;
        public int Count { get; set; }

        public Result(object data)
        {
            Data = data;
        }

        public Result(int count)
        {
            Count = count;
        }

        public Result(string mensagem)
        {
            Mensagem = mensagem;
        }

        public Result(bool sucesso)
        {
            Sucesso = sucesso;
        }

        public Result(object data, string mensagem, bool sucesso, int count)
        {
            Data = data;
            Mensagem = mensagem;
            Sucesso = sucesso;
            Count = count;
        }

        public Result() { }

        public Result(object data, int count, bool sucesso) : this(data)
        {
        }
    }
}
