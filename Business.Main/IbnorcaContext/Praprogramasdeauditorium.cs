using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.IbnorcaContext
{
    public partial class Praprogramasdeauditorium
    {
        public Praprogramasdeauditorium()
        {
            Praciclosprogauditoria = new HashSet<Praciclosprogauditorium>();
        }

        public long IdPrAprogramaAuditoria { get; set; }
        public int? IdpArea { get; set; }
        public string IdExternalsWs { get; set; }
        public string Nit { get; set; }
        public int? Gestion { get; set; }
        public int? IdpPais { get; set; }
        public int? IdpDepartamento { get; set; }
        public string IdOrganizacionWs { get; set; }
        public string OrganizacionContentWs { get; set; }
        public string CodigoServicioWs { get; set; }
        public string DetalleServicio { get; set; }
        public short? IdpTipoServicio { get; set; }
        public string IdCodigoDeServicioCodigoIafWs { get; set; }
        public int? NumeroAnios { get; set; }
        public int? IdpEstadosProgAuditoria { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }

        public virtual ICollection<Praciclosprogauditorium> Praciclosprogauditoria { get; set; }
    }
}
