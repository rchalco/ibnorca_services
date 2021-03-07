using BackgroundAPIRest.Contracts;
using Business.Main.DataMapping;
using Business.Main.Modules.ElaboracionAuditoria;
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
    }
}
