using Business.Main.Base;
using Business.Main.DataMapping;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.ListarAuditoresxCargoCalificadoDTO;
using Business.Main.Modules.TomaDecision.DTO;
using CoreAccesLayer.Wraper;
using Domain.Main.Wraper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.TomaDecision
{
    public class TomaDecisionManager : BaseManager
    {
       

        public ResponseObject<int> DevuelveCorrelativoDocAuditoria(long idElaAuditoria, int gestion, int idTipoDocumento)
        {
            ResponseObject<int> response = new ResponseObject<int> { Message = "Cargos obtenidos obtenido correctamente.", State = ResponseType.Success };
            try
            {

                ///TDO: obtenemos los datos del servicio
                var resultBd = repositoryMySql.GetDataByProcedure<Dto_spTmdConsecutivoDocAudi>("spTmdConsecutivoDocAudi", idElaAuditoria, gestion, idTipoDocumento);
                response.Object = resultBd[0].ConsecutivoDocAudi;

            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<Tmddocumentacionauditorium> RegistrarTmddocumentacionauditorium(Tmddocumentacionauditorium tmdDocumentacionAudit)
        {
            ResponseObject<Tmddocumentacionauditorium> response = new ResponseObject<Tmddocumentacionauditorium>
            {
                Message = "Se registro correctamente el plan de auditoria",
                State = ResponseType.Success,
                Object = tmdDocumentacionAudit
            };
            try
            {
                ///TODO: verfificamos que exista la auditoria
                if (tmdDocumentacionAudit == null)
                {
                    response.Message = "el objeto elaauditoria llego nulo, imposible ralizar el registro";
                    response.State = ResponseType.Warning;
                    return response;
                }
                ///guardamos la auditoria
                Entity<Tmddocumentacionauditorium> entity = new Entity<Tmddocumentacionauditorium> { EntityDB = tmdDocumentacionAudit, stateEntity = StateEntity.modify };
                repositoryMySql.SaveObject<Tmddocumentacionauditorium>(entity);
                response.Object = entity.EntityDB;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
    }
}
