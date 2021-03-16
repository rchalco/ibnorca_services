using BackgroundAPIRest.Contracts;
using Business.Main.DataMapping;
using Business.Main.Modules.ApeeturaAuditoria;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.BuscarNormaIntxCodigoDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.BuscarNormaxCodigoDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.BuscarPaisDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.CiudadesDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.EstadosDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.ListarAuditoresxCargoCalificadoDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.ListarCargosCalificadosDTO;
using Domain.Main.AperturaAuditoria;
using Domain.Main.Wraper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlumbingProps.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackgroundAPIRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AperturaAuditoriaController : ControllerBase
    {
        [HttpGet]
        public string index(string name)
        {
            return "API para la gestion de programas de Auditoria";
        }

        [HttpPost("RegisterProgramaAuditoria")]
        [EnableCors("MyPolicy")]
        public ResponseObject<Praprogramasdeauditorium> RegisterProgramaAuditoria(Praprogramasdeauditorium req)
        {

            Binnacle.ProcessEvent(new Event { category = Event.Category.Information, description = $"Metodo GetPersonsTest llamdo con parametro {JsonConvert.SerializeObject(req)}" });
            AperturaAuditoriaManager objAperturaManager = new AperturaAuditoriaManager();
            return objAperturaManager.RegisterProgramaAuditoria(req);
        }

        [HttpPost("ObtenerProgramaAuditoria")]
        [EnableCors("MyPolicy")]
        public ResponseObject<Praprogramasdeauditorium> ObtenerProgramaAuditoria(RequestObtenerProgramaAuditoria req)
        {
            Binnacle.ProcessEvent(new Event { category = Event.Category.Information, description = $"Metodo ObtenerProgramaAuditoria llamdo con parametro {JsonConvert.SerializeObject(req.IdServicio)}" });
            AperturaAuditoriaManager objAperturaManager = new AperturaAuditoriaManager();
            return objAperturaManager.ObtenerProgramaAuditoria(req.IdServicio, req.Usuario);
        }

        [HttpPost("ObtenerCargos")]
        [EnableCors("MyPolicy")]
        public ResponseQuery<ListaCargosCalificados> ObtenerCargos()
        {
            Binnacle.ProcessEvent(new Event { category = Event.Category.Information, description = $"Metodo ObtenerCargos llamado" });
            AperturaAuditoriaManager objAperturaManager = new AperturaAuditoriaManager();
            return objAperturaManager.ObtenerCargos();
        }

        [HttpPost("BuscarPersonalCargos")]
        [EnableCors("MyPolicy")]
        public ResponseQuery<ListaCalificado> BuscarPersonalCargos(RequestBuscarPersonalCargos requestBuscarPersonalCargos)
        {
            Binnacle.ProcessEvent(new Event { category = Event.Category.Information, description = $"Metodo BuscarPersonalCargos llamado" });
            AperturaAuditoriaManager objAperturaManager = new AperturaAuditoriaManager();
            return objAperturaManager.BuscarPersonalCargos(requestBuscarPersonalCargos.IdCargoCalificado);
        }

        [HttpPost("BuscarNormas")]
        [EnableCors("MyPolicy")]
        public ResponseQuery<Norma> BuscarNormas(RequestBuscarNormas requestBuscarNormas)
        {
            Binnacle.ProcessEvent(new Event { category = Event.Category.Information, description = $"Metodo BuscarNormas llamado" });
            AperturaAuditoriaManager objAperturaManager = new AperturaAuditoriaManager();
            return objAperturaManager.BuscarNormas(requestBuscarNormas.Codigo);
        }

        [HttpPost("BuscarNormasInternacionales")]
        [EnableCors("MyPolicy")]
        public ResponseQuery<NormaInternacional> BuscarNormasInternacionales(RequestBuscarNormasInternacionales req)
        {
            Binnacle.ProcessEvent(new Event { category = Event.Category.Information, description = $"Metodo BuscarNormasInternacionales llamado" });
            AperturaAuditoriaManager objAperturaManager = new AperturaAuditoriaManager();
            return objAperturaManager.BuscarNormasInternacionales(req.Codigo);
        }


        [HttpPost("BuscarPais")]
        [EnableCors("MyPolicy")]
        public ResponseQuery<Pais> BuscarPais(Contracts.RequestBuscarPais req)
        {
            Binnacle.ProcessEvent(new Event { category = Event.Category.Information, description = $"Metodo BuscarPais llamado" });
            AperturaAuditoriaManager objAperturaManager = new AperturaAuditoriaManager();
            return objAperturaManager.BuscarPais(req.pais);
        }
        [HttpPost("BuscarEstado")]
        [EnableCors("MyPolicy")]
        public ResponseQuery<Estado> BuscarEstado(RequestBuscarEstado req)
        {
            Binnacle.ProcessEvent(new Event { category = Event.Category.Information, description = $"Metodo BuscarPais llamado" });
            AperturaAuditoriaManager objAperturaManager = new AperturaAuditoriaManager();
            return objAperturaManager.BuscarEstado(req.IdPais);
        }
        [HttpPost("BuscarCiudad")]
        [EnableCors("MyPolicy")]
        public ResponseQuery<Ciudad> BuscarCiudad(RequestBuscarCiudad req)
        {
            Binnacle.ProcessEvent(new Event { category = Event.Category.Information, description = $"Metodo BuscarPais llamado" });
            AperturaAuditoriaManager objAperturaManager = new AperturaAuditoriaManager();
            return objAperturaManager.BuscarCiudad(req.IdEstado);
        }
    }
}
