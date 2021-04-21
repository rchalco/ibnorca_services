using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Paramdocumento
    {
        public int Idparamdocumentos { get; set; }
        public string NombrePlantilla { get; set; }
        public string Descripcion { get; set; }
        public string Path { get; set; }
        public string Area { get; set; }
        public int? Habilitado { get; set; }
        public string Proceso { get; set; }
        public string Method { get; set; }
    }
}
