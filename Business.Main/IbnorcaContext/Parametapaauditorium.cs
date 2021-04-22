using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.IbnorcaContext
{
    public partial class Parametapaauditorium
    {
        public Parametapaauditorium()
        {
            Praciclosprogauditoria = new HashSet<Praciclosprogauditorium>();
        }

        public int IdParametapaauditoria { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Praciclosprogauditorium> Praciclosprogauditoria { get; set; }
    }
}
