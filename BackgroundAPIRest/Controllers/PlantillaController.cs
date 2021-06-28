using BackgroundAPIRest.Contracts;
using Business.Main.Modules.TomaDecision;
using Business.Main.Modules.TomaDecision.DTO;
using Business.Main.Modules.TomaDecision.DTOExternal;
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
    public class PlantillaController : ControllerBase
    {
        [HttpPost("GenerarDocumento")]
        [EnableCors("MyPolicy")]
        public IActionResult GenerarDocumento(RequestExternalReport requestGenerarDocumento)
        {
            Binnacle.ProcessEvent(new Event { category = Event.Category.Information, description = $"Metodo GenerarDocumento llamado" });
            TomaDecisionManager tomaDecisionManager = new TomaDecisionManager();
            var resul = tomaDecisionManager.GenerarDocumentoByIdCiclo(requestGenerarDocumento);
            if (resul.State == Domain.Main.Wraper.ResponseType.Success)
            {
                string fileName = resul.Message;
                fileName = fileName.StartsWith("\\") ? "\\" + fileName : fileName;
                return new PhysicalFileResult(fileName, System.Net.Mime.MediaTypeNames.Application.Octet);
            }
            return Problem(resul.Message);
        }

        [HttpPost("GetResumePrograma")]
        [EnableCors("MyPolicy")]
        public ResponseQuery<DTOspWSGetResumePrograma> GetResumePrograma(RequestGetResumePrograma requestGetResumePrograma)
        {
            Binnacle.ProcessEvent(new Event { category = Event.Category.Information, description = $"Metodo GetResumePrograma llamado" });
            TomaDecisionManager tomaDecisionManager = new TomaDecisionManager();
            var resul = tomaDecisionManager.GetResumePrograma(requestGetResumePrograma.tipo, requestGetResumePrograma.idCiclo);
            return resul;
        }

        [HttpPost("GetDetalleProgramaTCP")]
        [EnableCors("MyPolicy")]
        public ResponseQuery<DTOspWSGetDetalleProgramaTCP> GetDetalleProgramaTCP(RequestGetDetalleProgramaTCP requestGetDetalleProgramaTCP)
        {
            Binnacle.ProcessEvent(new Event { category = Event.Category.Information, description = $"Metodo GetDetalleProgramaTCP llamado" });
            TomaDecisionManager tomaDecisionManager = new TomaDecisionManager();
            var resul = tomaDecisionManager.GetDetalleProgramaTCP(requestGetDetalleProgramaTCP.idProducto);
            return resul;
        }

        [HttpPost("GetDetalleProgramaTCS")]
        [EnableCors("MyPolicy")]
        public ResponseQuery<DTOspWSGetDetalleProgramaTCS> GetDetalleProgramaTCS(RequestGetDetalleProgramaTCS requestGetDetalleProgramaTCS)
        {
            Binnacle.ProcessEvent(new Event { category = Event.Category.Information, description = $"Metodo GetDetalleProgramaTCS llamado" });
            TomaDecisionManager tomaDecisionManager = new TomaDecisionManager();
            var resul = tomaDecisionManager.GetDetalleProgramaTCS(requestGetDetalleProgramaTCS.idSistema);
            return resul;
        }

    }
}
