using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Pradireccionespasistema
    {
        public Pradireccionespasistema()
        {
            Elacronogamas = new HashSet<Elacronogama>();
        }

        public long IdDireccionPasistema { get; set; }
        public long? IdPrAcicloProgAuditoria { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Pais { get; set; }
        public string Departamento { get; set; }
        public string Ciudad { get; set; }
        public int? Dias { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }

        public virtual Praciclosprogauditorium IdPrAcicloProgAuditoriaNavigation { get; set; }
        public virtual ICollection<Elacronogama> Elacronogamas { get; set; }
    }
}
