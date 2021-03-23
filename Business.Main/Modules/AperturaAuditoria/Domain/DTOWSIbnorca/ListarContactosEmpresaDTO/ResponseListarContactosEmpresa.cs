using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.ListarContactosEmpresaDTO
{
    public class ContactoEmpresa
    {
        public string IdContacto { get; set; }
        public string AdminCuentaTienda { get; set; }
        public string IdCliente { get; set; }
        public string NombreContacto { get; set; }
        public string PaternoContacto { get; set; }
        public string MaternoContacto { get; set; }
        public string CargoContacto { get; set; }
        public string FonoContacto { get; set; }
        public string CorreoContacto { get; set; }
        public string Vigencia { get; set; }
        public string VigenciaDescripcion { get; set; }
        public string IdTipoContacto { get; set; }
        public string TipoContacto { get; set; }
        public string Orden { get; set; }
        public string Identificacion { get; set; }
        public string IdentificacionExt { get; set; }
        public string IdentificacionExtAbrev { get; set; }
        public string TieneFirma { get; set; }
        public string DescripcionTieneFirma { get; set; }
        public string IdUsuario { get; set; }
        public string IdClienteDocumento { get; set; }
        public string IdAdjunto { get; set; }
        public string IdClienteDocumentoTPoder { get; set; }
        public string IdAdjuntoTPoder { get; set; }
        public string IdClienteTestimonio { get; set; }
        public string NumeroTestimonio { get; set; }
        public string FechaTestimonio { get; set; }
        public string NumeroNotaria { get; set; }
        public string DistritoJudicial { get; set; }
        public string NombreDrEncargado { get; set; }
        public string IdAdjuntoTestimonio { get; set; }
    }

    public class ResponseListarContactosEmpresa
    {
        public bool estado { get; set; }
        public string mensaje { get; set; }
        public int totalContactos { get; set; }
        public List<ContactoEmpresa> lstContactos { get; set; }
    }


}
