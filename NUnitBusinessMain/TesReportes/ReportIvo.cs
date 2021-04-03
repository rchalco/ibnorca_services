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
        [Test]
        public void GenerarREPEvaluacionYRecomendacionesParaProceso()
        {
            ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
            var response = elaboracionAuditoriaManager.GenerarREPEvaluacionYRecomendacionesParaProceso(81, @"REG-PRO-TCS-06-01_00 EvaluacionYRecomendacionParaUnProcesoDeCertificacion(Ver 1.0).doc", "Ruben Chalco","03/04/2021","Redacción sugerida");
        }
        [Test]
        public void GenerarDescisionFavorableCertificacion()
        {
            ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
            var response = elaboracionAuditoriaManager.GenerarDescisionFavorableCertificacion(81, @"REG-PRO-TCS-06-01_00 EvaluacionYRecomendacionParaUnProcesoDeCertificacion(Ver 1.0)REG-PRO-TCS-06-02_04_DecisionFavorableDeLaCertificacion (Ver 1.0).doc", "Ruben Chalco",  "Perkins");
        }
        //bien 
        [Test]
        public void GenerarREPDecisionNOFavorable()
        {
            ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
            var response = elaboracionAuditoriaManager.GenerarREPDecisionNOFavorable(81, @"REG-PRO-TCS-06-03_00_DecisionNoFavorable (Ver 1.0).doc", "Ruben Chalco", "Perkins","Remueve","Nombre de sistema","Consideraciones","NroCertificadoIbnorca");
        }
        //bien
        [Test]
        public void GenerarRepNotaSuspensionCertifica()
        {
            ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
            var response = elaboracionAuditoriaManager.GenerarRepNotaSuspensionCertifica(81, @"REG-PRO-TCS-07-01.00 Nota de suspensión de certificacion V.1.0.doc","CorrelativoCabecera", "Ruben Chalco", "Perkins", "texto1111", "NroCertificadoIbnorca","Director ejecutivo");
        }
        [Test]
        public void GenerarREPNotaDeRetiroDeCertificacion()
        {
            ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
            var response = elaboracionAuditoriaManager.GenerarREPNotaDeRetiroDeCertificacion(81, @"REG-PRO-TCS-07-02.00 Nota de retiro de certificacion.doc", "CorrelativoCabecera", "Ruben Chalco", "Perkins", "NroCertificadoIbnorca");
        }

        //GenerarREPEvaluacionYRecomendacionesParaProceso
    }
}
