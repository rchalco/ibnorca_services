using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.BuscarxIdClienteEmpresaDTO
{

    public class Cliente
    {
        public string IdCliente { get; set; }
        public string TipoCliente { get; set; }
        public string TipoNalInt { get; set; }
        public string NombreRazon { get; set; }
        public string Identificacion { get; set; }
        public string IdPais { get; set; }
        public string Pais { get; set; }
        public string IdDepartamento { get; set; }
        public string Departamento { get; set; }
        public string IdCiudad { get; set; }
        public string Ciudad { get; set; }
        public string CiudadOtro { get; set; }
        public string Direccion { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Web { get; set; }
        public string Factura { get; set; }
        public string FacturaRazon { get; set; }
        public string NIT { get; set; }
        public string Vigencia { get; set; }
        public string EsCliente { get; set; }
    }

    public class ResponseBusquedaCliente
    {
        public bool estado { get; set; }
        public string mensaje { get; set; }
        public int totalResultados { get; set; }
        public List<Cliente> resultados { get; set; }
    }
}
