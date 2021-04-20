using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Main.Modules.ElaboracionAuditoria.Reportes.TCP
{
    public class TCPREPInformePreliminar
    {

        public string NombreEmpresa { get; set; }
        public string Direccion { get; set; }
        public string Contacto { get; set; }
        public string TelefonoCelular { get; set; }
        public string CorreoElectronico { get; set; }
        public string CodigoServicio { get; set; }
        public string TipoAuditoria { get; set; }
        public string FechaInicio { get; set; }
        public string Fecha { get; set; }
        public string EquipoAuditor { get; set; }
        public string CriterioAuditoria { get; set; }
        public string SiNODescripcion1 { get; set; }
        public string SiNoDescripcion2 { get; set; }
        

        public string Si2 { get; set; }
        public string No2 { get; set; }
        public string NA2 { get; set; }
        public string Descripcion2 { get; set; }

        public string PlanMuestreo { get; set; }

        public List<TCPListaHallazgos> ListHallazgos { get; set; }
        public string RedaccionFortalezas { get; set; }
        public string RedaccionOportunidades { get; set; }
        public string ConformidadesMenores { get; set; }
        public string ConformidadMayores { get; set; }
        public string ComentariosIBNORCA { get; set; }

        public string Si3 { get; set; }
        public string No3 { get; set; }
        public string Descripcion3 { get; set; }
        public List<TCPListCorrecciones> ListCorrecciones { get; set; }
        public string CoordinadorAuditoria { get; set; }
        public List<TCPListProductos> ListProductos { get; set; }
        

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
