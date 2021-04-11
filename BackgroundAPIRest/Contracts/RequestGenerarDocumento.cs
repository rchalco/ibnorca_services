using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackgroundAPIRest.Contracts
{
    public class RequestGenerarDocumento
    {
        public string plantilla { get; set; }
        public int IdCicloAuditoria { get; set; }
    }
}
