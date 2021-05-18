using Business.Main.Base;
using Business.Main.DataMapping.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Main.Modules.ElaboracionAuditoria.Reportes.TCS
{
    public class REPDesignacionAuditoria : IObjectReport
    {
        public REPDesignacionAuditoria()
        {
            ListRepDesginacionParticipante = new List<RepDesginacionParticipante>();
        }
        public string TipoAuditoria { get; set; }
        public string ModalidadAuditoria { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        //public string FechaInicioEnsayos { get; set; }
        public string DiasAuditor { get; set; }
        public string AuditorLider { get; set; }
        public string CorreoAuditorLider { get; set; }
        public string Auditor { get; set; }
        public string CorreoAuditor { get; set; }
        public string Experto { get; set; }
        public string CorreoExperto { get; set; }
        public string AuditorEnsayos { get; set; }
        public string CorreoEnsayos { get; set; }
        public string OrganismoCertificador { get; set; }
        public string CodigoServicio { get; set; }
        public string IDServicio { get; set; }
        public string NombreEmpresa { get; set; }
        public string AltaDireccion { get; set; }
        public string Cargo { get; set; }
        public string Contacto { get; set; }
        public string CargoContacto { get; set; }
        public string TelefonoCargo { get; set; }
        public string CorreoElectronicoContacto { get; set; }
        public string CodigoIAF { get; set; }
        public string Alcance { get; set; }
        public string SitiosAAuditar { get; set; }
        public string SitiosDentroDeAlcance { get; set; }
        public string HorarioTrabajo { get; set; }
        public string FechaProxima { get; set; }
        public string Adjunto { get; set; }
        public string Usuario { get; set; }
        public string Logistica { get; set; }
        public List<RepDesginacionParticipante> ListRepDesginacionParticipante { get; set; }

    }


}
