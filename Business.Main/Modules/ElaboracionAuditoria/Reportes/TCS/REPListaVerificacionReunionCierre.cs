﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Resportes.ReportDTO
{
    /// <summary>
    /// PLAN_REG-PRO-TCS-05-03.04 Lista de verificación reunión de cierre (Ver1.1)
    /// </summary>
    public class REPListaVerificacionReunionCierre
    {

        public string NombreOrganizacion { get; set; }
        public string CodigoDeServicio { get; set; }
        public string FechaDeAuditoria { get; set; }
        public string TipoDeAuditoria { get; set; }
        public string NormasAuditadas { get; set; }

        public string NombreYFirmaDeAuditorLider { get; set; }
    }
}