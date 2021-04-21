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
        //[Test]
        //public void GenerarNotaSuspension()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    var response = elaboracionAuditoriaManager.GenerarNotaSuspension(81, @"E:\PROYECTOS\ibnorca\Documentos\Plantillas_Oficial\REG-PRO-TCS-07-01.00 Nota de suspensión de certificacion V.1.0.doc", "Ruben Chalco", "Director Ejecutivo", DateTime.Now);

        //}

        //[Test]
        //public void GenerarPlanAuditoria()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    //var response = elaboracionAuditoriaManager.GenerarPlanAuditoria(81, @"E:\PROYECTOS\ibnorca\Documentos\Plantillas_Oficial\REG-PRO-TCS-07-01.00 Nota de suspensión de certificacion V.1.0.doc", "Ruben Chalco", "Director Ejecutivo", DateTime.Now);
        //    var response = elaboracionAuditoriaManager.GenerarPlanAuditoria(81, @"REG-PRO-TCS-05-01.07 Plan de auditoria V.1.0.doc", "Objetios a llenar", "Cambio a llenar", "certificacion");

        //}

        //[Test]
        //public void TCPRepDesignacionAuditoria()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    //var response = elaboracionAuditoriaManager.GenerarPlanAuditoria(81, @"E:\PROYECTOS\ibnorca\Documentos\Plantillas_Oficial\REG-PRO-TCS-07-01.00 Nota de suspensión de certificacion V.1.0.doc", "Ruben Chalco", "Director Ejecutivo", DateTime.Now);
        //    var response = elaboracionAuditoriaManager.TCPRepDesignacionAuditoria(91, @"REG-PRO-TCP-03-01 Designación auditoria V.1.0.doc");

        //}

        //[Test]
        //public void TCPREPPlanAuditoria()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    //var response = elaboracionAuditoriaManager.GenerarPlanAuditoria(81, @"E:\PROYECTOS\ibnorca\Documentos\Plantillas_Oficial\REG-PRO-TCS-07-01.00 Nota de suspensión de certificacion V.1.0.doc", "Ruben Chalco", "Director Ejecutivo", DateTime.Now);
        //    var response = elaboracionAuditoriaManager.TCPREPPlanAuditoria(91, @"REG-PRO-TCP-05-01.01 Plan de Auditoria V.1.0.doc");

        //}

        //[Test]
        //public void TCPREPListaVerificacionReunionApertura()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    //var response = elaboracionAuditoriaManager.GenerarPlanAuditoria(81, @"E:\PROYECTOS\ibnorca\Documentos\Plantillas_Oficial\REG-PRO-TCS-07-01.00 Nota de suspensión de certificacion V.1.0.doc", "Ruben Chalco", "Director Ejecutivo", DateTime.Now);
        //    var response = elaboracionAuditoriaManager.TCPREPListaVerificacionReunionApertura(91, @"REG-PRO-TCP-05-02.01 Lista de verificación reunión de apertura V.1.0.doc");

        //}

        //[Test]
        //public void TCPREPListaVerificacionReunionCierre()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    //var response = elaboracionAuditoriaManager.GenerarPlanAuditoria(81, @"E:\PROYECTOS\ibnorca\Documentos\Plantillas_Oficial\REG-PRO-TCS-07-01.00 Nota de suspensión de certificacion V.1.0.doc", "Ruben Chalco", "Director Ejecutivo", DateTime.Now);
        //    var response = elaboracionAuditoriaManager.TCPREPListaVerificacionReunionCierre(91, @"REG-PRO-TCP-05-03.01 Lista de Verificación reunión de cierre V.1.0.doc");

        //}

        //[Test]
        //public void TCPREPListaAsistencia()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    //var response = elaboracionAuditoriaManager.GenerarPlanAuditoria(81, @"E:\PROYECTOS\ibnorca\Documentos\Plantillas_Oficial\REG-PRO-TCS-07-01.00 Nota de suspensión de certificacion V.1.0.doc", "Ruben Chalco", "Director Ejecutivo", DateTime.Now);
        //    var response = elaboracionAuditoriaManager.TCPREPListaAsistencia(91, @"REG-PRO-TCP-05-04.01 Lista de Asistencia V.1.0.doc");

        //}

        //[Test]
        //public void TCPREPActaMuestreo()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    //var response = elaboracionAuditoriaManager.GenerarPlanAuditoria(81, @"E:\PROYECTOS\ibnorca\Documentos\Plantillas_Oficial\REG-PRO-TCS-07-01.00 Nota de suspensión de certificacion V.1.0.doc", "Ruben Chalco", "Director Ejecutivo", DateTime.Now);
        //    var response = elaboracionAuditoriaManager.TCPREPActaMuestreo(91, @"REG-PRO-TCP-05-05.01 Acta de Muestreo V.1.0.doc");

        //}

        //[Test]
        //public void TCPREPListaVerificacionAuditor()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    //var response = elaboracionAuditoriaManager.GenerarPlanAuditoria(81, @"E:\PROYECTOS\ibnorca\Documentos\Plantillas_Oficial\REG-PRO-TCS-07-01.00 Nota de suspensión de certificacion V.1.0.doc", "Ruben Chalco", "Director Ejecutivo", DateTime.Now);
        //    var response = elaboracionAuditoriaManager.TCPREPListaVerificacionAuditor(91, @"REG-PRO-TCP-05-06.01 Lista de Verificación Auditor V.1.0.doc");

        //}

        //[Test]
        //public void TCPREPInformePreliminar()
        //{
        //    ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
        //    //var response = elaboracionAuditoriaManager.GenerarPlanAuditoria(81, @"E:\PROYECTOS\ibnorca\Documentos\Plantillas_Oficial\REG-PRO-TCS-07-01.00 Nota de suspensión de certificacion V.1.0.doc", "Ruben Chalco", "Director Ejecutivo", DateTime.Now);
        //    var response = elaboracionAuditoriaManager.TCPREPInformePreliminar(91, @"REG-PRO-TCP-05-07.01 Informe Preliminar V.1.0.doc");

        //}

       
    }
}
