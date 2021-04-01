using BackgroundAPIRest.Contracts;
using Business.Main.DataMapping;
using Business.Main.Modules.ElaboracionAuditoria;
using Business.Main.Modules.ElaboracionAuditoria.DTO;
using Domain.Main.Wraper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
