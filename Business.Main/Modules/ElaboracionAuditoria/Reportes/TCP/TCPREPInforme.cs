﻿using Business.Main.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resportes.ReportDTO
{
    class TCPREPInforme : IObjectReport
    {
        public string IDCliente { get; set; }
        public string TipoAuditoria { get; set; }
        //public string Productos { get; set; 
        public List<TCPListProductosInforme> ListProductos { get; set; }

        public string Norma { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }

        public class TCPListProductosInforme
        {
            public string Producto { get; set; }
            public string Normas { get; set; }
        }

    }
}
