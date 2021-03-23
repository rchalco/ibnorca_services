using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.DatosServicioDTO
{

    public class ListaProductoCertificado
    {
        public string IdCertificadoServicios { get; set; }
        public string tipoCertificado { get; set; }
        public string FechaValido { get; set; }
        public string ProductoServicio { get; set; }
        public string Norma { get; set; }
        public string nombre { get; set; }
        public string marca { get; set; }
        public string direccion { get; set; }
        public string sub_partida_arancelaria { get; set; }
        public string FechaEmision { get; set; }
    }

    public class ListaDireccionCertificado
    {
        public string IdCertificadoServicios { get; set; }
        public string tipoCertificado { get; set; }
        public string FechaValido { get; set; }
        public string FechaEmision { get; set; }
        public string ProductoServicio { get; set; }
        public string Norma { get; set; }
        public List<string> direcciones { get; set; }
    }

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
        public string ciudad { get; set; }
        public string cod_estado { get; set; }
        public string estado { get; set; }
        public string cod_pais { get; set; }
        public string pais { get; set; }
    }

    public class ListaProducto
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
        public string ciudad { get; set; }
        public string cod_estado { get; set; }
        public string estado { get; set; }
        public string cod_pais { get; set; }
        public string pais { get; set; }
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
        public string iaf_primario_codigo { get; set; }
        public string iaf_primario_descripcion { get; set; }
        public string cod_iaf_secundario { get; set; }
        public List<ListaProducto> ListaProducto { get; set; }
        public List<ListaDireccion> ListaDireccion { get; set; }
        public List<ListaProductoCertificado> ListaProductoCertificado { get; set; }
        public List<ListaDireccionCertificado> ListaDireccionCertificado { get; set; }
        
    }

    public class ResponseDatosServicio
    {
        public bool estado { get; set; }
        public string mensaje { get; set; }
        public int IdServicio { get; set; }
        public DatosServicio DatosServicio { get; set; }
    }
}
