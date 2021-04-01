using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackgroundAPIRest.Contracts
{
    public class RequestObtenerPlanAuditoria
    {
        public int IdCicloPrograma { get; set; }
        public string usuario { get; set; }
    }
}
