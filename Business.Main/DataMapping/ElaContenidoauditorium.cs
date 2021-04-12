using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Elacontenidoauditorium
    {
        public int IdelaContenidoauditoria { get; set; }
        public int? IdelaAuditoria { get; set; }
        public string Label { get; set; }
        public string Nemotico { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public string Categoria { get; set; }
        public string Area { get; set; }
        public int? Seleccionado { get; set; }
        public int? Endocumento { get; set; }

        public virtual Elaauditorium IdelaAuditoriaNavigation { get; set; }
    }
}
