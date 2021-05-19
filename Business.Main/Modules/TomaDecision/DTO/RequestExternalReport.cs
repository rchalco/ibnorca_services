using Resportes.ReportDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.TomaDecision.DTO
{
    public class RequestExternalReport
    {
        public string IdServicio { get; set; }
        public int Anio { get; set; }
        public int IdCiclo { get; set; }
        public string NombrePlantilla { get; set; }
        public bool SoloGenerar { get; set; }
        public List<ItemReporte> ListItemReporte { get; set; }
        public ListasAdicionales ListasAdicionales { get; set; }
    }
    public class ListasAdicionales
    {
        public List<Asistente> ListAsistente { get; set; }
        public List<TCPTemario> ListaTemario { get; set; }
        public List<ProductosResolucion> ListaProductosResolucion { get; set; }
    }
}
