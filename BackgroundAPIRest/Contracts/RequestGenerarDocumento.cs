using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackgroundAPIRest.Contracts
{
    public class RequestGenerarDocumento
    {
        public int idCicloAuditoria { get; set; }
        public string nombrePlantilla { get; set; }
        public string area { get; set; }
    }
}
