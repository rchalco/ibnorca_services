using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.BuscarNormaIntxCodigoDTO
{
    public class RequestBuscarNormaIntxCodigo : BaseRequest
    {
        public string Codigo { get; set; }
    }
}
