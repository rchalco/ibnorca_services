using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.BuscarPaisDTO
{

    public class Pais
    {
        public string idPais { get; set; }
        public string paisAbrev { get; set; }
        public string paisNombre { get; set; }
        public string paisCodigoTel { get; set; }
        public string gentilicio { get; set; }
    }

    public class ResponseBuscarPais
    {
        public bool estado { get; set; }
        public string mensaje { get; set; }
        public int totalResultado { get; set; }
        public List<Pais> resultado { get; set; }
    }

}
