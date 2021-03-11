using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.ListarAuditoresxCargoCalificadoDTO
{
    public class ResponseListarAuditoresxCargoCalificado
    {
        public bool estado { get; set; }
        public string mensaje { get; set; }
        public int IdCargoCalificado { get; set; }
        public int TotalCalificados { get; set; }
        public List<ListaCalificado> ListaCalificados { get; set; }
    }

    public class ListaCalificado
    {
        public string IdCalificacion { get; set; }
        public string IdCliente { get; set; }
        public string NombreCompleto { get; set; }
        public string Identificacion { get; set; }
        public string IdCiudad { get; set; }
        public string Ciudad { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string CalificacionId { get; set; }
        public string Calificacion { get; set; }
        public string Norma { get; set; }
        public string CodsIAF { get; set; }
        public string IdCargoPuesto { get; set; }
        public string CargoPuesto { get; set; }
    }

}
