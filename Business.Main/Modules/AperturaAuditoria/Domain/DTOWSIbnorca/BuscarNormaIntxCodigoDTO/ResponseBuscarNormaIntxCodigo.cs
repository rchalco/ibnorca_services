using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.BuscarNormaIntxCodigoDTO
{
    public class NormaInternacional
    {
        public string IdNorma { get; set; }
        public string IdEntidad { get; set; }
        public string NombreEntidad { get; set; }
        public string CodigoNorma { get; set; }
        public string NombreNorma { get; set; }
        public object Alcance { get; set; }
        public string FechaVigencia { get; set; }
        public string Vigente { get; set; }
        public object FechaRegistro { get; set; }
        public string IdTipo { get; set; }
        public string DescripcionTipo { get; set; }
    }

    public class ResponseBuscarNormaIntxCodigo
    {
        public bool estado { get; set; }
        public string mensaje { get; set; }
        public int total { get; set; }
        public List<NormaInternacional> resultado { get; set; }
    }

}
