using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcionarios.Infra.Services.EmailService.Settings
{
    public class EmailSettings
    {
        public string Remetente { get; set; }
        public string Nome { get; set; }
        public string Smtp { get; set; }
        public int Porta { get; set; }
        public string CabecarioEnvio { get; set; }
        public string[] Emails { get; set; }
        public bool EmailTeste { get; set; }
        public string LinkSistema { get; set; }


        public EmailSettings() { }
    }
}
