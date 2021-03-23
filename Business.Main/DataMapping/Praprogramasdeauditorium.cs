using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Praprogramasdeauditorium
    {
        public Praprogramasdeauditorium()
        {
            Praciclosprogauditoria = new HashSet<Praciclosprogauditorium>();
        }

        public long IdPrAprogramaAuditoria { get; set; }
        public int? IdparamArea { get; set; }
        public string Estado { get; set; }
        public string Nit { get; set; }
        public string Fecha { get; set; }
        public string Oficina { get; set; }
        public string IdOrganizacionWs { get; set; }
        public string OrganizacionContentWs { get; set; }
        public string CodigoServicioWs { get; set; }
        public string DetalleServicioWs { get; set; }
        public int? IdparamTipoServicio { get; set; }
        public string CodigoIafws { get; set; }
        public int? NumeroAnios { get; set; }
        public string OrganismoCertificador { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }

        public virtual ICollection<Praciclosprogauditorium> Praciclosprogauditoria { get; set; }
    }
}
