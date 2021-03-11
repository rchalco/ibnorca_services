using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.BuscarNormaxCodigoDTO
{
    public class RequestBuscarNormaxCodigo : BaseRequest
    {
        public string Codigo { get; set; }
    }
}
