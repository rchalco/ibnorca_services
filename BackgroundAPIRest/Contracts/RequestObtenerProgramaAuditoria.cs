using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackgroundAPIRest.Contracts
{
    public class RequestObtenerProgramaAuditoria
    {
        public int IdServicio { get; set; }
        public string Usuario { get; set; }
    }
}
