using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flunt.Notifications;

namespace Funcionarios.Domain.Models
{
    public abstract class Model : Notifiable<Notification>
    {
        public abstract void Validar();
    }
}
