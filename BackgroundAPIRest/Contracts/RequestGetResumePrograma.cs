using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackgroundAPIRest.Contracts
{
    public class RequestGetResumePrograma
    {
        public string tipo { get; set; }
        public int idCiclo { get; set; }
    }
}
