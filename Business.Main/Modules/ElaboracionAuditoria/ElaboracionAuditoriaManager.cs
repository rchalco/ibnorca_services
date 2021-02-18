using Business.Main.Base;
using Domain.Main.Wraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
