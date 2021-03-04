using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.DatosServicioDTO
{
    public class ListaDireccion
    {
        public string codigo { get; set; }
        public string cod_simulacionservicio { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string cod_tipoatributo { get; set; }
        public string habilitado { get; set; }
        public string marca { get; set; }
        public string norma { get; set; }
        public string nro_sello { get; set; }
        public string cod_ciudad { get; set; }
        public string cod_estado { get; set; }
        public string cod_pais { get; set; }
    }

    public class DatosServicio
    {
        public string IdServicio { get; set; }
        public string idPropuesta { get; set; }
        public string fecharegistro { get; set; }
        public string IdOficina { get; set; }
        public string oficina { get; set; }
        public string IdCliente { get; set; }
        public string Codigo { get; set; }
        public string IdArea { get; set; }
        public string area { get; set; }
        public string alcance_propuesta { get; set; }
        public string descripcion_servicio { get; set; }
        public string cod_responsable { get; set; }
        public string responsable { get; set; }
        public string cod_iaf_primario { get; set; }
        public string cod_iaf_secundario { get; set; }
        //public List<object> ListaProducto { get; set; }
        public List<ListaDireccion> ListaDireccion { get; set; }
    }

    public class ResponseDatosServicio
    {
        public bool estado { get; set; }
        public string mensaje { get; set; }
        public int IdServicio { get; set; }
        public DatosServicio DatosServicio { get; set; }
    }
}
