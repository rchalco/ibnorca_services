using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.BuscarClasificadorDTO
{
    public class RequestBuscarClasificador : BaseRequest
    {
        public int padre { get; set; }
    }
}
