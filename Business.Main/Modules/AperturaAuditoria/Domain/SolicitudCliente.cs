using Business.Main.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.AperturaAuditoria.Domain
{
    [Serializable]
    public class SolicitudCliente
    {
        [Coordenada(10, 3)]
        public string razonSocial { get; set; }
        [Coordenada(10, 8)]
        public string nit { get; set; }
        [Coordenada(11, 3)]
        public string direccionPrincipal { get; set; }
        [Coordenada(12, 3)]
        public string ciudad { get; set; }
        [Coordenada(12, 7)]
        public string departamentoPais { get; set; }
        [Coordenada(13, 3)]
        public string telefono { get; set; }
        [Coordenada(13, 7)]
        public string fax { get; set; }
        [Coordenada(14, 3)]
        public string mail { get; set; }
        [Coordenada(14, 7)]
        public string pagina_web { get; set; }
        [Coordenada(16, 3)]
        public string mae_nombre { get; set; }
        [Coordenada(17, 3)]
        public string mae_cargo { get; set; }
        [Coordenada(18, 3)]
        public string mae_telefono { get; set; }
        [Coordenada(19, 3)]
        public string mae_mail { get; set; }
        [Coordenada(21, 3)]
        public string contacto_nombre { get; set; }
        [Coordenada(22, 3)]
        public string contacto_cargo { get; set; }
        [Coordenada(23, 3)]
        public string contacto_telefono { get; set; }
        [Coordenada(24, 3)]
        public string contacto_mail { get; set; }
        [Coordenada(26, 6)]
        public string holding { get; set; }
        [Coordenada(27, 6)]
        public string promedioVentaAnual { get; set; }
        [Coordenada(29, 1)]
        public string direccionFabricacion { get; set; }
        [Coordenada(31, 1)]
        public string direccionAlmacenes { get; set; }
        [Rango(37, 42)]
        public List<SolicitudProducto> listSolicitudProducto { get; set; }
        [Coordenada(48, 5)]
        public string certificacion_previa { get; set; }
        [Rango(50, 52)]
        public List<ListSimple> listReglamentacion { get; set; }
        [Rango(54, 56)]
        public List<ListSimple> listMateriaPrima { get; set; }
        [Coordenada(58, 1)]
        public string actividadesTerceros { get; set; }
        [Rango(63, 68)]
        public List<ListSimple> laboratorios { get; set; }
        [Coordenada(73, 5)]
        public string fecha_aproximada { get; set; }
        [Coordenada(81, 4)]
        public string nombre { get; set; }
        [Coordenada(82, 4)]
        public string cargo { get; set; }
        [Coordenada(82, 4)]
        public string fecha { get; set; }
    }
    [Serializable]
    public class Laboratorio
    {
        [Coordenada(0, 1)]
        public string ensayo { get; set; }
        [Coordenada(0, 4)]
        public string laboratorio { get; set; }
        [Coordenada(0, 6)]
        public string acreditado { get; set; }
        [Coordenada(0, 8)]
        public string externo_empresa { get; set; }
    }
    [Serializable]
    public class SolicitudProducto
    {
        [Coordenada(0, 1)]
        public string nombre { get; set; }
        [Coordenada(0, 4)]
        public string marca { get; set; }
        [Coordenada(0, 5)]
        public string norma { get; set; }
        [Coordenada(0, 7)]
        public string cantidad { get; set; }
        [Coordenada(0, 8)]
        public string ubicacion { get; set; }
    }
    public class ListSimple
    {
        [Coordenada(0, 1)]
        public string column1 { get; set; }
        [Coordenada(0, 5)]
        public string column2 { get; set; }
    }
}
