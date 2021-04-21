using BackgroundAPIRest.Contracts;
using Business.Main.DataMapping;
using Business.Main.Modules.ElaboracionAuditoria;
using Business.Main.Modules.ElaboracionAuditoria.DTO;
using Domain.Main.Wraper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlumbingProps.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackgroundAPIRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElaboracionAuditoriaController : ControllerBase
    {
        [HttpPost("RegistrarPlanAuditoria")]
        [EnableCors("MyPolicy")]
        public ResponseObject<PlanAuditoriaDTO> RegistrarPlanAuditoria(PlanAuditoriaDTO planAuditoriaDTO)
        {
            ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
            return elaboracionAuditoriaManager.RegistrarPlanAuditoria(planAuditoriaDTO);
        }

        [HttpPost("GetListasVerificacion")]
        [EnableCors("MyPolicy")]
        public ResponseQuery<Paramitemselect> GetListasVerificacion(RequestGetListasVerificacion requestGetListasVerificacion)
        {
            ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
            return elaboracionAuditoriaManager.GetListasVerificacion(requestGetListasVerificacion.IdLista);
        }
        [HttpPost("ObtenerCiclosPorIdAuditor")]
        [EnableCors("MyPolicy")]
        public ResponseQuery<ResumeCicloDTO> ObtenerCiclosPorIdAuditor(RequestObtenerCiclosPorIdAuditor requestObtenerCiclosPorIdAuditor)
        {
            ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
            return elaboracionAuditoriaManager.ObtenerCiclosPorIdAuditor(requestObtenerCiclosPorIdAuditor.IdAuditor);
        }
        [HttpPost("ObtenerPlanAuditoria")]
        [EnableCors("MyPolicy")]
        public ResponseObject<PlanAuditoriaDTO> ObtenerPlanAuditoria(RequestObtenerPlanAuditoria requestObtenerPlanAuditoria)
        {
            ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
            return elaboracionAuditoriaManager.ObtenerPlanAuditoria(requestObtenerPlanAuditoria.IdCicloPrograma, requestObtenerPlanAuditoria.usuario);
        }
        [HttpPost("GetListasPredefinidas")]
        [EnableCors("MyPolicy")]
        public ResponseQuery<Elalistaspredefinida> GetListasPredefinidas(RequestGetListasPredefinidas requestGetListasPredefinidas)
        {
            ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
            return elaboracionAuditoriaManager.GetListasPredefinidas(requestGetListasPredefinidas.area);
        }
        [HttpPost("GetListasDocumetos")]
        [EnableCors("MyPolicy")]
        public ResponseQuery<Paramdocumento> GetListasDocumetos(RequestGetListasDocumetos requestGetListasDocumetos)
        {
            ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
            return elaboracionAuditoriaManager.GetListasDocumetos(requestGetListasDocumetos.area, requestGetListasDocumetos.proceso);
        }
        [HttpPost("GenerarDocumento")]
        [EnableCors("MyPolicy")]
        public Response GenerarDocumento(RequestGenerarDocumento requestGenerarDocumento)
        {
            ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
            return elaboracionAuditoriaManager.GenerarDocumento(requestGenerarDocumento.nombrePlantilla, requestGenerarDocumento.idCicloAuditoria);
        }

        [HttpGet("ObtenerArchivo")]
        [EnableCors("MyPolicy")]
        public IActionResult ObtenerArchivo(string fileName)
        {
            Binnacle.ProcessEvent(new Event { category = Event.Category.Information, description = $"Metodo BuscarPais llamado" });
            fileName = fileName.StartsWith("\\") ? "\\" + fileName : fileName;
            return new PhysicalFileResult(fileName, System.Net.Mime.MediaTypeNames.Application.Octet);
        }
    }
}
