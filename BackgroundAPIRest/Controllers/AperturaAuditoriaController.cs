using Business.Main.DataMapping;
using Business.Main.Modules.ApeeturaAuditoria;
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
        public ResponseObject<Praprogramasdeauditorium> ObtenerProgramaAuditoria(int pIdServicio, string pUsuario)
        {
            Binnacle.ProcessEvent(new Event { category = Event.Category.Information, description = $"Metodo ObtenerProgramaAuditoria llamdo con parametro {JsonConvert.SerializeObject(pIdServicio)}" });
            AperturaAuditoriaManager objAperturaManager = new AperturaAuditoriaManager();
            return objAperturaManager.ObtenerProgramaAuditoria(pIdServicio, pUsuario);
        }
    }
}
