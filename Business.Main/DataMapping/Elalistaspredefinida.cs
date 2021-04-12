using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Elalistaspredefinida
    {
        public int Idelalistaspredefinidas { get; set; }
        public string Decripcion { get; set; }
        public string Nemotico { get; set; }
        public string Titulo { get; set; }
        public string Categoria { get; set; }
        public string Label { get; set; }
        public string Area { get; set; }
        public int? Orden { get; set; }
        public int? Endocumento { get; set; }
    }
}
