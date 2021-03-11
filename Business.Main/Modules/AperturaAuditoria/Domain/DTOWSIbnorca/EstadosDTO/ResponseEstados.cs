using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.EstadosDTO
{
    public class Estado
    {
        public string idEstado { get; set; }
        public string estNombre { get; set; }
        public string estIdPais { get; set; }
        public string abrev { get; set; }
    }

    public class ResponseEstados
    {
        public bool estado { get; set; }
        public string mensaje { get; set; }
        public List<Estado> lista { get; set; }
    }
}
