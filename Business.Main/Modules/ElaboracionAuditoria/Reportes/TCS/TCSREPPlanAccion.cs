using Business.Main.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.ElaboracionAuditoria.Reportes.TCS
{
    class TCSREPPlanAccion: IObjectReport
    {
        public string NombreEmpresa { get; set; }
        public string TipoAuditoria { get; set; }
        public string Norma { get; set; }
        public string Fecha { get; set; }
        public string AuditorLider { get; set; }
        public List<HallazgosPACTCS> ListHallazgosPAC { get; set; }
    }

    class HallazgosPACTCS
    {
        public int nro { get; set; }
        public string TipoHallazgo { get; set; }
        public string Descripcion { get; set; }
        public string Sitio { get; set; }
        public string Correccion { get; set; }
        public string AnalisisCausa { get; set; }
        public string AccionesCorrectivas { get; set; }
        public string Responsable { get; set; }
        public string FechaCumplimiento { get; set; }
        public string PresentaEvidencia { get; set; }
        public string ComentariosAuditor { get; set; }
        public string Verificacion { get; set; }
        public string Aprobacion { get; set; }
    }
}
