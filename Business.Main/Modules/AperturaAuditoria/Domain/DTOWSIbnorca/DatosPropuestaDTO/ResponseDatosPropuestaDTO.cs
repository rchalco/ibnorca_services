using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.DatosPropuestaDTO
{

    public class DatosPropuesta
    {
        public string codigo { get; set; }
        public string cod_area { get; set; }
        public string cod_unidadorganizacional { get; set; }
        public string fecha { get; set; }
        public string cod_personal { get; set; }
        public string cod_estado { get; set; }
        public string estado { get; set; }
        public string cod_cliente { get; set; }
        public string utilidad_minima { get; set; }
        public string id_tiposervicio { get; set; }
        public string cod_objetoservicio { get; set; }
        public string alcance_propuesta { get; set; }
        public string idServicio { get; set; }
        public string descripcion_servicio { get; set; }
    }

    public class ListaServicio
    {
        public string codigo { get; set; }
        public string cod_anio { get; set; }
        public string cod_simulacionservicio { get; set; }
        public string cod_claservicio { get; set; }
        public string descripcion { get; set; }
        public string observaciones { get; set; }
        public string cantidad { get; set; }
        public string monto { get; set; }
        public string monto_total { get; set; }
        public string cod_tipounidad { get; set; }
    }

    public class ListaAuditore
    {
        public string codigo { get; set; }
        public string cod_anio { get; set; }
        public string cod_simulacionservicio { get; set; }
        public string descripcion { get; set; }
        public string cod_tipoauditor { get; set; }
        public string dias { get; set; }
        public string monto { get; set; }
        public string monto_total { get; set; }
        public string monto_solicitado { get; set; }
    }

    public class ResponseDatosPropuestaDTO
    {
        public bool estado { get; set; }
        public string mensaje { get; set; }
        public int IdPropuesta { get; set; }
        public DatosPropuesta DatosPropuesta { get; set; }
        public List<ListaServicio> ListaServicios { get; set; }
        public List<ListaAuditore> ListaAuditores { get; set; }
    }
   
}
