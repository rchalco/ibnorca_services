using Business.Main.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Main.Modules.ElaboracionAuditoria.Reportes.TCP
{
    public class TCPREPInformePreliminar : IObjectReport
    {

        public string NombreEmpresa { get; set; }
        public string Direccion { get; set; }
        public string Contacto { get; set; }
        public string TelefonoCelular { get; set; }
        public string CorreoElectronico { get; set; }
        public string CodigoServicio { get; set; }
        public string TipoAuditoria { get; set; }
        public string FechaInforme { get; set; }
        public string FechaAuditoria { get; set; }
        public string EquipoAuditor { get; set; }
        public string CriterioAuditoria { get; set; }
        public string Cont7ConModificaciones { get; set; }
        public string Cont7SinModificaciones { get; set; }
        public string SiNoDescripcion3 { get; set; }
        public string PlanMuestreo { get; set; }

        public List<TCPListaHallazgos> ListHallazgos { get; set; }
        public string RedaccionFortalezas { get; set; }
        public string RedaccionOportunidades { get; set; }
        public string ConformidadesMenores { get; set; }
        public string ConformidadMayores { get; set; }
        public string ComentariosIBNORCA { get; set; }


        public List<TCPListCorrecciones> ListCorrecciones { get; set; }
        public string CoordinadorAuditoria { get; set; }
        public List<TCPListProductos> ListProductos { get; set; }
        public List<RepEquipoTCP> ListEquipoAuditor { get; set; }

        //public string CorreoElectronico { get; set; }

    }

    public class TCPListProductos
    {
        public string Producto { get; set; }
        public string Normas { get; set; }
    }
    public class TCPListaHallazgos
    {
        public string Fortaleza { get; set; }
        public string OportunidadMejora { get; set; }
        public string ConformidadMayor { get; set; }
        public string ConformidadMenor { get; set; }
    }
    public class TCPListCorrecciones
    {
        public string Nm_Ncm { get; set; }
        public string PuntoForma { get; set; }
        public string EvidenciaResolucion { get; set; }
        public string Resuelta { get; set; }
        public string Pendiente { get; set; }
    }


}
