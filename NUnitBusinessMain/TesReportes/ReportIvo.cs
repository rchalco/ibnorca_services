using Business.Main.Modules.ElaboracionAuditoria;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitBusinessMain.TesReportes
{
    public class ReportIvo
    {
        [Test]
        public void GenerarDescisionFavorableCertificacion()
        {
            ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
            //var response = elaboracionAuditoriaManager.GenerarNotaSuspension(81, @"C:\Ivo\Projects\ibnorca\Documentos\00 Dcumentos\Repositorio\REG-PRO-TCS-07-01.00 Nota de suspensión de certificacion V.1.0.doc", "Ruben Chalco", "Director Ejecutivo");
            var response = elaboracionAuditoriaManager.GenerarDescisionFavorableCertificacion(81, @"C:\Ivo\Projects\ibnorca\Documentos\00 Dcumentos\Repositorio\REG-PRO-TCS-07-01.00 Nota de suspensión de certificacion V.1.0.doc", "Ruben Chalco", "Director Ejecutivo");
        }
    }
}
