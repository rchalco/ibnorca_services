using Business.Main.Base;
using Business.Main.Modules.ElaboracionAuditoria.Reportes.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.ElaboracionAuditoria.Reportes.TCS
{
    /// <summary>
    /// REG-PRO-TCS-05-06.01 Informe Etapa I V.1.0
    /// </summary>
    public class RepInformeAuditoriaEtapaI : IObjectReport
    {
        public string NombreEmpresa { get; set; }
        public string Direccion { get; set; }
        public string PersonaContacto { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public string CodigoServivio { get; set; }
        public string TipoAuditoria { get; set; }
        public string ModalidadAuditoria { get; set; }
        public string Normas { get; set; }
        public string FechaAuditoria { get; set; }
        public string FechaInforme { get; set; }
        public string EquipoAuditor { get; set; }
        public string CriteriosSistema { get; set; }
        public string ComentarioRealcionado { get; set; }
        public string Etapa1 { get; set; }
        public string AreasPreocupacion { get; set; }
        public string ConclusionAuditor { get; set; }
        public string AuditorLider { get; set; }
        public string NombreAuditorLider { get; set; }
        public string Fecha { get; set; }
        public string RepresentanteOrganizacion { get; set; }
        public string CorreoElectronicoCoodinador { get; set; }        
        public string NombreCoordinadorAud { get; set; }
        public string ElectronicoCoordinador { get; set; }
        public string FechaAuditoriaEtapaII { get; set; }
        public string FechaSolicitarEdificarAudiII { get; set; }
        public List<RepEquipoTCP> ListEquipoAuditor { get; set; }

    }
}
