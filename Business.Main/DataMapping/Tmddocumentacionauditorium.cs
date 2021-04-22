using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Tmddocumentacionauditorium
    {
        public long IdTmdDocumentacionAuditoria { get; set; }
        public long? IdElaAuditoria { get; set; }
        public int? IdparamDocumentos { get; set; }
        public int? Gestion { get; set; }
        public int? CorrelativoDocumento { get; set; }
        public string CiteDocumento { get; set; }
        public string TmdDocumentoAuditoria { get; set; }
        public DateTime? FechaDeRegistro { get; set; }
    }
}
