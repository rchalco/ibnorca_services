using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.DataMapping.DTOs
{
    public class PraDocDesignacion
    {
        public int IdCliente { get; set; }
        //Datos de la auditoría
        public String FechadeAuditoria { get; set; }
        public String TipodeAuditoria { get; set; }
        public String ModalidaddeAuditoria { get; set; }
        public String FechaInicioAuditoria { get; set; }
        public String FechaFinAuditoria { get; set; }
        public int CantidadDiasAuditor { get; set; }
        public String OrganismoCertificador { get; set; }
        //Datos de la empresa
        public String CodigoDeServicioIbnorca { get; set; }
        public String Organizacion { get; set; }
        public String AltaDireccion { get; set; }
        public String CargoAltaDireccion { get; set; }
        public String PersonaDeContacto { get; set; }
        public String CargoPersonaDeContacto { get; set; }
        public String TelefonoDeContacto { get; set; }
        public String CorreoElectronico { get; set; }
        public String CodigoAIF { get; set; }
        public String AlcanceDeCertificacion { get; set; }
        public String SitiosAAuditar { get; set; }
        public String Exclusiones { get; set; }
        public String HorarioHabitualDeTrabajo { get; set; }
        public String FechaProximaAuditoria { get; set; }
        public List<praDesignaEquipoAuditor> userOptions = new List<praDesignaEquipoAuditor>();
    }
}
