using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.BuscarPaisDTO
{
    public class RequestBuscarPais: BaseRequest
    {
        public string palabra { get; set; }
        public string TipoLista { get; set; }
    }
}
