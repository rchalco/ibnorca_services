using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.ElaboracionAuditoria.DTO
{
    public class ResumeCicloDTO
    {
        public int IdCicloAuditoria { get; set; }
        public string CodigoServicio { get; set; }
        public string NombreCliente { get; set; }
        public string ReferenciaCiclo { get; set; }
        public string Responsable { get; set; }
        public string FechaAuditoria { get; set; }
    }
}
