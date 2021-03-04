using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca
{
    public abstract class BaseRequest
    {
        public string sIdentificador { get; set; }
        public string sKey { get; set; }
        public string accion { get; set; }
    }
}
