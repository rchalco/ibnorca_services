using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.CiudadesDTO
{
    public class Ciudad
    {
        public string idCiudad { get; set; }
        public string nomCiudad { get; set; }
        public string ciuIdEstado { get; set; }
    }

    public class ResponseCiudades
    {
        public bool estado { get; set; }
        public string mensaje { get; set; }
        public List<Ciudad> lista { get; set; }
    }
}
