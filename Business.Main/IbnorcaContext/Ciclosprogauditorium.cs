using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.IbnorcaContext
{
    public partial class Ciclosprogauditorium
    {
        public long IdCicloProgAuditoria { get; set; }
        public long? IdProgramaAuditoria { get; set; }
        public string NombreOrganizacionCertificado { get; set; }
        public short? Ano { get; set; }
        public short? IdpTipoAuditoria { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
    }
}
