using Flunt.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcionarios.Domain.Results
{
    public class Result : Notifiable<Notification>
    {
        public object Data { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public bool Sucesso { get; set; } = true;
        public int Count { get; set; }
    }
}
