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
        public void GenerarREPDatosDeLaOrganizacion()
        {
            ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();            
            var response = elaboracionAuditoriaManager.GenerarREPDatosDeLaOrganizacion(81, @"REG-PRO-TCS-05-08.03 Datos de la organizacion (Ver 1.0).doc", "Ruben Chalco", "Director Ejecutivo", "Juan Perez");
        }
    }
}
