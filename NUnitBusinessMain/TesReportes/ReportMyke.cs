using Business.Main.Modules.ElaboracionAuditoria;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitBusinessMain.TesReportes
{

    public class ReportMyke
    {
        [Test]
        public void GenerarNotaSuspension()
        {
            ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
            var response = elaboracionAuditoriaManager.GenerarNotaSuspension(81, @"E:\PROYECTOS\ibnorca\Documentos\Plantillas_Oficial\REG-PRO-TCS-07-01.00 Nota de suspensión de certificacion V.1.0.doc", "Ruben Chalco", "Director Ejecutivo", DateTime.Now);

        }
    }
}
