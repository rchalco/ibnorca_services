using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.BuscarNormaxCodigoDTO
{
    public class Norma
    {
        public string IdNorma { get; set; }
        public string CodigoNorma { get; set; }
        public string NombreNorma { get; set; }
        public string Alcance { get; set; }
        public string FechaVigencia { get; set; }
        public string FechaBaja { get; set; }
        public string IdSector { get; set; }
        public string NombreSector { get; set; }
        public string IdComite { get; set; }
        public string NombreComite { get; set; }
        public string Precio { get; set; }
        public string PrecioFisico { get; set; }
        public string PrecioDigital { get; set; }
        public string vigente { get; set; }
        public string activa { get; set; }
        public string web { get; set; }
    }

    public class ResponseBuscarNormaxCodigo
    {
        public bool estado { get; set; }
        public string mensaje { get; set; }
        public int total { get; set; }
        public List<Norma> resultado { get; set; }
    }
}
