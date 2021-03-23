using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.BuscarClasificadorDTO
{
    public class Clasificador
    {
        public string IdClasificador { get; set; }
        public string Descripcion { get; set; }
        public string Abrev { get; set; }
        public object Auxiliar { get; set; }
        public object Orden { get; set; }
        public string IdPadre { get; set; }
        public string Vigencia { get; set; }
    }

    public class ResponseBuscarClasificador
    {
        public bool estado { get; set; }
        public string mensaje { get; set; }
        public int IdClasificador { get; set; }
        public string Descripcion { get; set; }
        public List<Clasificador> lista { get; set; }
    }
}
