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

        //bien
        [Test]
        public void GenerarREPListaVerificacionReunionApertura()
        {
            ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
            var response = elaboracionAuditoriaManager.GenerarREPListaVerificacionReunionApertura(81, @"REG-PRO-TCS-05-02.06 Lista de Verificación Reunión de Apertura V.1.0.doc", "05/04/2021", "06/04/2021", "Ruben Chalco");
        }

        //bien
        [Test]
        public void GenerarREPListaVerificacionReunionCierre()
        {
            ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
            var response = elaboracionAuditoriaManager.GenerarREPListaVerificacionReunionCierre(81, @"REG-PRO-TCS-05-03.04 Lista de verificación reunión de cierre (Ver1.0).doc", "Ruben Chalco");
        }
        //bien
        [Test]
        public void GenerarREPListaAsistencia()
        {
            ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
            var response = elaboracionAuditoriaManager.GenerarREPListaAsistencia(81, @"REG-PRO-TCS-05-04.05 Lista de asistencia V.1.0.doc", "05/04/2021", "06/04/2021");
        }
        //bien
        [Test]
        public void GenerarREPListaVerificacionAuditor()
        {
            ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
            var response = elaboracionAuditoriaManager.GenerarREPListaVerificacionAuditor(81, @"REG-PRO-TCS-05-05.00 Lista de verificación auditor V.1.0.doc", "Ruben Chalco", "Director Ejecutivo", "Sistema soboce", "SOBOCE");
        }
        //Bien
        [Test]
        public void GenerarREPLIstaVerificacionBPM()
        {
            ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
            var response = elaboracionAuditoriaManager.GenerarREPLIstaVerificacionBPM(81, @"REG-PRO-TCS-05-05B_00 Lista de verificación auditoria BPM - (Ver 1.0).doc", "Ruben Chalco", "Director Ejecutivo");
        }
        //bien
        [Test]
        public void GenerarREPDatosDeLaOrganizacion()
        {
            ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();            
            var response = elaboracionAuditoriaManager.GenerarREPDatosDeLaOrganizacion(81, @"REG-PRO-TCS-05-08.03 Datos de la organizacion (Ver 1.0).doc", "Ruben Chalco", "Director Ejecutivo", "Juan Perez");
        }
        //bien
        [Test]
        public void GenerarREPEvaluacionYRecomendacionesParaProceso()
        {
            ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
            //var response = elaboracionAuditoriaManager.GenerarREPEvaluacionYRecomendacionesParaProceso(81, @"EquipoAuditor.doc", "Ruben Chalco","03/04/2021","Redacción sugerida");
            var response = elaboracionAuditoriaManager.GenerarREPEvaluacionYRecomendacionesParaProceso(81, @"REG-PRO-TCS-06-01_00 EvaluacionYRecomendacionParaUnProcesoDeCertificacion(Ver 1.0).doc", "Ruben Chalco", "03/04/2021", "Redacción sugerida");
        }
        //bien
        [Test]
        public void GenerarDescisionFavorableCertificacion()
        {
            ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
            var response = elaboracionAuditoriaManager.GenerarDescisionFavorableCertificacion(81, @"REG-PRO-TCS-06-02_04_DecisionFavorableDeLaCertificacion (Ver 1.0).doc", "Ruben Chalco",  "Perkins");
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
        //bien
        [Test]
        public void GenerarREPNotaDeRetiroDeCertificacion()
        {
            ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
            var response = elaboracionAuditoriaManager.GenerarREPNotaDeRetiroDeCertificacion(81, @"REG-PRO-TCS-07-02.00 Nota de retiro de certificacion.doc", "CorrelativoCabecera", "Ruben Chalco", "Perkins", "NroCertificadoIbnorca");
        }

        //GenerarREPEvaluacionYRecomendacionesParaProceso
    }
}
