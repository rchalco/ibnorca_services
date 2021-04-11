﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Elaadp
    {
        public int Idelaadp { get; set; }
        public int? IdelaAuditoria { get; set; }
        public string Area { get; set; }
        public string Descripcion { get; set; }

        public virtual Elaauditorium IdelaadpNavigation { get; set; }
    }
}