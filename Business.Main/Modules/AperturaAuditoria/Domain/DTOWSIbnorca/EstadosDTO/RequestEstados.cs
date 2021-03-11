using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.EstadosDTO
{
    public class RequestEstados : BaseRequest
    {
        public string TipoLista { get; set; }
        public string IdPais { get; set; }
    }
}
