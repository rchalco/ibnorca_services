using Business.Main.Modules.ElaboracionAuditoria;
using Business.Main.Modules.TomaDecision;
using NUnit.Framework;

namespace NUnitBusinessMain.TesReportes
{
    public class ReportIvo
    {
        [Test]
        public void DevuelveCorrelativoDocAuditoria()
        {
            TomaDecisionManager elaboracionAuditoriaManager = new TomaDecisionManager();
            var response = elaboracionAuditoriaManager.DevuelveCorrelativoDocAuditoria(1,2021,3);
        }

        ////TCP
        ////GenerarTCPREPNotaRetiroCertificacion
        ////bien
        //[Test]
        //public void GenerarTCPREPNotaRetiroCertificacion()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    var response = elaboracionAuditoriaManager.GenerarTCPREPNotaRetiroCertificacion(91, @"REG-PRO-TCP-07-02.00 Nota de retiro de certificacion de producto V.1.0.doc", "marca", "NumeroProducto");
        //}
        ////bien
        //[Test]
        //public void GenerarTCPREPNotaSuspencionCertificado()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    var response = elaboracionAuditoriaManager.GenerarTCPREPNotaSuspencionCertificado(91, @"REG-PRO-TCP-07-01.00 Nota de suspensión de certificación de producto V.1.0.doc", "marca", "NumeroProducto");
        //}
        ////bien
        //[Test]
        //public void GenerarTCPREPDecisionConformeReglamento()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    var response = elaboracionAuditoriaManager.GenerarTCPREPDecisionConformeReglamento(91, @"REG-PRO-TCP-06-06.00 Decisión conforme a reglamento V.1.0.doc", "marca", "Producto", "Arncel");
        //}
        ////bien
        //[Test]
        //public void GenerarTCPREPDecisionCertificacion()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    var response = elaboracionAuditoriaManager.GenerarTCPREPDecisionCertificacion(91, @"REG-PRO-TCP-06-05.00 Decision de la certificación V.1.0.doc", "Numero de certificado 01");
        //}
        ////pendiente
        //[Test]
        //public void GenerarTCPREPResolucionAdministrativa()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    var response = elaboracionAuditoriaManager.GenerarTCPREPResolucionAdministrativa(91, @"REG-PRO-TCP-06-04.02 Resolución administrativa V.1.0.doc", "acta");
        //}

        ////pendiente
        //[Test]
        //public void GenerarTCPREPActaReunion()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    var response = elaboracionAuditoriaManager.GenerarTCPREPActaReunion(91, @"REG-PRO-TCP-06-03.01 Acta de Reunión V.1.0.doc", "acta", "hora");
        //}
        ////bien
        //[Test]
        //public void GenerarTCPREPListaAsistencia()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    var response = elaboracionAuditoriaManager.GenerarTCPREPListaAsistencia(91, @"REG-PRO-TCP-06-02.01 Lista de Asistencia V.1.0.doc", "tipoReunion","Cargo", "ListaAsistencia");
        //}

        ////bien
        //[Test]
        //public void GenerarTCPREPPlanAccion()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    var response = elaboracionAuditoriaManager.GenerarTCPREPPlanAccion(91, @"REG-PRO-TCP-05-09.00 Plan de accion V.1.0.doc", "RubenChalco");
        //}
        ////pendiente
        //[Test]
        //public void GenerarTCPREPInforme()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    var response = elaboracionAuditoriaManager.GenerarTCPREPInforme(91, @"REG-PRO-TCP-05-08.01 Informe.doc", "05/04/2021");
        //}
        ////TCSSSSSSS
        ////bien
        //[Test]
        //public void GenerarREPListaVerificacionReunionApertura()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    var response = elaboracionAuditoriaManager.GenerarREPListaVerificacionReunionApertura(81, @"REG-PRO-TCS-05-02.06 Lista de Verificación Reunión de Apertura V.1.0.doc", "05/04/2021", "06/04/2021", "Ruben Chalco");
        //}

        ////bien
        //[Test]
        //public void GenerarREPListaVerificacionReunionCierre()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    var response = elaboracionAuditoriaManager.GenerarREPListaVerificacionReunionCierre(81, @"REG-PRO-TCS-05-03.04 Lista de verificación reunión de cierre (Ver1.0).doc", "Ruben Chalco");
        //}
        ////bien
        //[Test]
        //public void GenerarREPListaAsistencia()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    var response = elaboracionAuditoriaManager.GenerarREPListaAsistencia(81, @"REG-PRO-TCS-05-04.05 Lista de asistencia V.1.0.doc", "05/04/2021", "06/04/2021");
        //}
        ////bien
        //[Test]
        //public void GenerarREPListaVerificacionAuditor()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    var response = elaboracionAuditoriaManager.GenerarREPListaVerificacionAuditor(81, @"REG-PRO-TCS-05-05.00 Lista de verificación auditor V.1.0.doc", "Ruben Chalco", "Director Ejecutivo", "Sistema soboce", "SOBOCE");
        //}
        ////Bien
        //[Test]
        //public void GenerarREPLIstaVerificacionBPM()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    var response = elaboracionAuditoriaManager.GenerarREPLIstaVerificacionBPM(81, @"REG-PRO-TCS-05-05B_00 Lista de verificación auditoria BPM - (Ver 1.0).doc", "Ruben Chalco", "Director Ejecutivo");
        //}
        ////bien
        //[Test]
        //public void GenerarREPDatosDeLaOrganizacion()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    var response = elaboracionAuditoriaManager.GenerarREPDatosDeLaOrganizacion(81, @"REG-PRO-TCS-05-08.03 Datos de la organizacion (Ver 1.0).doc", "Ruben Chalco", "Director Ejecutivo", "Juan Perez");
        //}
        ////bien
        //[Test]
        //public void GenerarREPEvaluacionYRecomendacionesParaProceso()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    //var response = elaboracionAuditoriaManager.GenerarREPEvaluacionYRecomendacionesParaProceso(81, @"EquipoAuditor.doc", "Ruben Chalco","03/04/2021","Redacción sugerida");
        //    var response = elaboracionAuditoriaManager.GenerarREPEvaluacionYRecomendacionesParaProceso(81, @"REG-PRO-TCS-06-01_00 EvaluacionYRecomendacionParaUnProcesoDeCertificacion(Ver 1.0).doc", "Ruben Chalco", "03/04/2021", "Redacción sugerida");
        //}
        ////bien
        //[Test]
        //public void GenerarDescisionFavorableCertificacion()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    var response = elaboracionAuditoriaManager.GenerarDescisionFavorableCertificacion(81, @"REG-PRO-TCS-06-02_04_DecisionFavorableDeLaCertificacion (Ver 1.0).doc", "Ruben Chalco", "Perkins");
        //}
        ////bien 
        //[Test]
        //public void GenerarREPDecisionNOFavorable()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    var response = elaboracionAuditoriaManager.GenerarREPDecisionNOFavorable(81, @"REG-PRO-TCS-06-03_00_DecisionNoFavorable (Ver 1.0).doc", "Ruben Chalco", "Perkins", "Remueve", "Nombre de sistema", "Consideraciones", "NroCertificadoIbnorca");
        //}
        ////bien
        //[Test]
        //public void GenerarRepNotaSuspensionCertifica()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    var response = elaboracionAuditoriaManager.GenerarRepNotaSuspensionCertifica(81, @"REG-PRO-TCS-07-01.00 Nota de suspensión de certificacion V.1.0.doc", "CorrelativoCabecera", "Ruben Chalco", "Perkins", "texto1111", "NroCertificadoIbnorca", "Director ejecutivo");
        //}
        ////bien
        //[Test]
        //public void GenerarREPNotaDeRetiroDeCertificacion()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    var response = elaboracionAuditoriaManager.GenerarREPNotaDeRetiroDeCertificacion(81, @"REG-PRO-TCS-07-02.00 Nota de retiro de certificacion.doc", "CorrelativoCabecera", "Ruben Chalco", "Perkins", "NroCertificadoIbnorca");
        //}

        //GenerarREPEvaluacionYRecomendacionesParaProceso
    }
}
