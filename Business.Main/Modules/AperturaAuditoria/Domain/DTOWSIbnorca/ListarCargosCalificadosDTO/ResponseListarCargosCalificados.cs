using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.ListarCargosCalificadosDTO
{
    public class ResponseListarCargosCalificados
    {
        public bool estado { get; set; }
        public string mensaje { get; set; }
        public List<ListaCargosCalificados> ListaCargosCalificados { get; set; }
    }

    public class ListaCargosCalificados
    {
        public string IdCargoPuesto { get; set; }
        public string CargoPuesto { get; set; }
    }
}
