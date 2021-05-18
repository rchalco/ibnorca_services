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
    }
}
