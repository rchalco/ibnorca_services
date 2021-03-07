using Business.Main.Base;
using Domain.Main.Wraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Main.DataMapping;

namespace Business.Main.Modules.ElaboracionAuditoria
{
    public class ElaboracionAuditoriaManager : BaseManager
    {
        public Response RegistrarPlanAuditoria()
        {
            Response response = new Response();
            try
            {

            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }


        public ResponseQuery<Paramitemselect> GetListasVerificacion(int IdLista)
        {
            ResponseQuery<Paramitemselect> response = new ResponseQuery<Paramitemselect>();
            try
            {
                response.ListEntities = repositoryMySql.SimpleSelect<Paramitemselect>(x => x.IdParamListaItemSelect == IdLista);
                response.State = ResponseType.Success;
                response.Message = "Lista obtenida correctamente";
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
    }
}
