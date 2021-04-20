using Business.Main.DataMapping;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.BuscarxIdClienteEmpresaDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.ElaboracionAuditoria.DTO
{
    public class PlanAuditoriaDTO
    {
        public Designacion Designacion { get; set; }
        public List<Pracicloparticipante> Pracicloparticipante { get; set; }
        public Elaauditorium Elaauditorium { get; set; }
        public Cliente Cliente { get; set; }
        public List<Praciclonormassistema> Praciclonormassistema { get; set; }
        public List<Pradireccionespaproducto> Pradireccionespaproducto { get; set; }
        public List<Pradireccionespasistema> Pradireccionespasistema { get; set; }
        public string NombreClienteCertificado { get; set; }
        public string Area { get; set; }
        public List<string> Normas { get; set; }
    }
    public class Designacion
    {
        public string CodigoServicio { get; set; }
        public string TipoAuditroria { get; set; }
        public string FechaAuditoria { get; set; }
    }

}
