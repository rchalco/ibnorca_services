using Business.Main.Base;
using Domain.Main.Wraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Main.DataMapping;
using Business.Main.Modules.ElaboracionAuditoria.DTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.BuscarxIdClienteEmpresaDTO;
using PlumbingProps.Services;
using Business.Main.Cross;
using CoreAccesLayer.Wraper;

namespace Business.Main.Modules.ElaboracionAuditoria
{
    public class ElaboracionAuditoriaManager : BaseManager
    {
        public Response RegistrarPlanAuditoria(PlanAuditoriaDTO planAuditoriaDTO)
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

        public ResponseQuery<ResumeCicloDTO> ObtenerCiclosPorIdAuditor(int IdAuditor)
        {
            ResponseQuery<ResumeCicloDTO> response = new ResponseQuery<ResumeCicloDTO> { Message = "Ciclos obtenidos correctamente", ListEntities = new List<ResumeCicloDTO>(), State = ResponseType.Success };
            try
            {
                response.ListEntities = repositoryMySql.GetDataByProcedure<ResumeCicloDTO>("GetResumenCicloPorAuditor", IdAuditor);
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        public ResponseObject<PlanAuditoriaDTO> ObtenerPlanAuditoria(int IdCicloPrograma, string usuario)
        {
            ResponseObject<PlanAuditoriaDTO> response = new ResponseObject<PlanAuditoriaDTO>
            {
                Code = "000",
                Message = "Plan obtenido correctamente",
                State = ResponseType.Success,
                Object = new PlanAuditoriaDTO()
            };
            try
            {
                //obtenemso el ciclo
                var resulDBCicloPrograma = repositoryMySql.SimpleSelect<Praciclosprogauditorium>(x => x.IdPrAcicloProgAuditoria == IdCicloPrograma);
                //obtensmo el programa
                var resulBDPrograma = repositoryMySql.SimpleSelect<Praprogramasdeauditorium>(x => x.IdPrAprogramaAuditoria == resulDBCicloPrograma.First().IdPrAprogramaAuditoria);
                //obtenemos los datos del cronograma
                var resulBDCronograma = repositoryMySql.SimpleSelect<Praciclocronograma>(x => x.IdPrAcicloProgAuditoria == resulDBCicloPrograma.First().IdPrAcicloProgAuditoria);
                //obtenemos los participantes del ciclo
                response.Object.Pracicloparticipante = repositoryMySql.SimpleSelect<Pracicloparticipante>(x => x.IdPrAcicloProgAuditoria == IdCicloPrograma);
                //llenamos los datos de desgingacion
                response.Object.Designacion = new Designacion
                {
                    CodigoServicio = resulBDPrograma.First().CodigoServicioWs,
                    FechaAuditoria = resulBDCronograma.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    TipoAuditroria = resulDBCicloPrograma.First().Referencia
                };
                //llenamos los datos del cliente
                ClientHelper clientHelper = new ClientHelper();

                RequestBusquedaCliente requestBusquedaCliente = new RequestBusquedaCliente { accion = "BuscarxIdClienteEmpresa", sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES, IdCliente = resulBDPrograma.First().IdOrganizacionWs };
                ResponseBusquedaCliente responseBusquedaCliente = clientHelper.Consume<ResponseBusquedaCliente>(Global.URIGLOBAL_SERVICES + Global.URI_CLIENTE, requestBusquedaCliente).Result;
                if (!responseBusquedaCliente.estado || responseBusquedaCliente.totalResultados <= 0)
                {
                    response.State = ResponseType.Warning;
                    response.Message = $"Existe problemas al consumir el servicio de ibnorca (BuscarxIdClienteEmpresa): {responseBusquedaCliente.mensaje}";
                    return response;
                }

                response.Object.Cliente = responseBusquedaCliente.resultados.First();
                response.Object.NombreClienteCertificado = resulDBCicloPrograma.First().NombreOrganizacionCertificado;
                ///TDO falta corregir la relacion 1 - n del equipo auditor con el cronograma o dejarlo como JSON
                ///TDO llenamos las listas para TCP y TCS 
                response.Object.Praciclonormassistema = repositoryMySql.SimpleSelect<Praciclonormassistema>(x => x.IdPrAcicloProgAuditoria == resulDBCicloPrograma.First().IdPrAcicloProgAuditoria);
                response.Object.Pradireccionespaproducto = repositoryMySql.SimpleSelect<Pradireccionespaproducto>(x => x.IdPrAcicloProgAuditoria == resulDBCicloPrograma.First().IdPrAcicloProgAuditoria);
                response.Object.Pradireccionespasistema = repositoryMySql.SimpleSelect<Pradireccionespasistema>(x => x.IdPrAcicloProgAuditoria == resulDBCicloPrograma.First().IdPrAcicloProgAuditoria);

                ///verfificamos si existe la auditoria asociada a un ciclo
                var resulDBAuditoria = repositoryMySql.SimpleSelect<Elaauditorium>(x => x.IdPrAcicloProgAuditoria == IdCicloPrograma);
                if (resulDBAuditoria.Count == 0)
                {
                    //Registramos el item de auditoria en la BD
                    response.Object.Elaauditorium = new Elaauditorium
                    {
                        Elacronogamas = new LinkedList<Elacronogama>(),
                        FechaRegistro = DateTime.Now,
                        IdPrAcicloProgAuditoria = resulDBCicloPrograma.First().IdPrAcicloProgAuditoria,
                        UsuarioRegistro = usuario
                    };
                    Entity<Elaauditorium> entity = new Entity<Elaauditorium> { EntityDB = response.Object.Elaauditorium, stateEntity = StateEntity.add };
                    repositoryMySql.SaveObject<Elaauditorium>(entity);
                    response.Object.Elaauditorium.IdelaAuditoria = entity.EntityDB.IdelaAuditoria;
                }
                //por si no existe en el plan
                else
                {
                    response.Object.Elaauditorium = resulDBAuditoria.First();
                }


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
