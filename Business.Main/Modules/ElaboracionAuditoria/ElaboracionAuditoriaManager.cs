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
using System.IO;
using PlumbingProps.Document;

namespace Business.Main.Modules.ElaboracionAuditoria
{
    public partial class ElaboracionAuditoriaManager : BaseManager
    {

        public ResponseObject<PlanAuditoriaDTO> RegistrarPlanAuditoria(PlanAuditoriaDTO planAuditoriaDTO)
        {
            ResponseObject<PlanAuditoriaDTO> response = new ResponseObject<PlanAuditoriaDTO>
            {
                Message = "Se registro correctamente el plan de auditoria",
                State = ResponseType.Success,
                Object = planAuditoriaDTO
            };
            try
            {
                ///TODO: verfificamos que exista la auditoria
                if (planAuditoriaDTO.Elaauditorium == null)
                {
                    response.Message = "el objeto elaauditoria llego nulo, imposible ralizar el registro";
                    response.State = ResponseType.Warning;
                    return response;
                }

                ///TODO: se registra la auditoria
                ///asignamos las llaves correctas
                planAuditoriaDTO.Elaauditorium.Elaadps?.ToList().ForEach(x =>
                {
                    x.IdelaAuditoria = planAuditoriaDTO.Elaauditorium.IdelaAuditoria;
                    x.IdelaAuditoriaNavigation = planAuditoriaDTO.Elaauditorium;
                });
                planAuditoriaDTO.Elaauditorium.Elacontenidoauditoria?.ToList().ForEach(x =>
                {
                    x.IdelaAuditoria = planAuditoriaDTO.Elaauditorium.IdelaAuditoria;
                    x.IdelaAuditoriaNavigation = planAuditoriaDTO.Elaauditorium;
                });
                planAuditoriaDTO.Elaauditorium.Elacronogamas?.ToList().ForEach(x =>
                {
                    x.Idelaauditoria = planAuditoriaDTO.Elaauditorium.IdelaAuditoria;
                    x.IdelaauditoriaNavigation = planAuditoriaDTO.Elaauditorium;
                });
                planAuditoriaDTO.Elaauditorium.Elahallazgos?.ToList().ForEach(x =>
                {
                    x.IdelaAuditoria = planAuditoriaDTO.Elaauditorium.IdelaAuditoria;
                    x.IdelaAuditoriaNavigation = planAuditoriaDTO.Elaauditorium;
                });

                ///Eliminamos los cronogramas
                var lCronogramas = repositoryMySql.SimpleSelect<Elacronogama>(x => x.Idelaauditoria == planAuditoriaDTO.Elaauditorium.IdelaAuditoria);
                lCronogramas.ToList().ForEach(x =>
                {
                    Entity<Elacronogama> entityCronograma = new Entity<Elacronogama> { EntityDB = x, stateEntity = StateEntity.remove };
                    repositoryMySql.SaveObject<Elacronogama>(entityCronograma);
                });
                //Eliminamos los hallazgos 
                var lHallazgos = repositoryMySql.SimpleSelect<Elahallazgo>(x => x.IdelaAuditoria == planAuditoriaDTO.Elaauditorium.IdelaAuditoria);
                lHallazgos.ToList().ForEach(x =>
                {
                    Entity<Elahallazgo> entityCronograma = new Entity<Elahallazgo> { EntityDB = x, stateEntity = StateEntity.remove };
                    repositoryMySql.SaveObject<Elahallazgo>(entityCronograma);
                });
                //Eliminamos los adps
                var lAdps = repositoryMySql.SimpleSelect<Elaadp>(x => x.IdelaAuditoria == planAuditoriaDTO.Elaauditorium.IdelaAuditoria);
                lAdps.ToList().ForEach(x =>
                {
                    Entity<Elaadp> entityCronograma = new Entity<Elaadp> { EntityDB = x, stateEntity = StateEntity.remove };
                    repositoryMySql.SaveObject<Elaadp>(entityCronograma);
                });

                ///guardamos la auditoria
                Entity<Elaauditorium> entity = new Entity<Elaauditorium> { EntityDB = planAuditoriaDTO.Elaauditorium, stateEntity = StateEntity.modify };
                repositoryMySql.SaveObject<Elaauditorium>(entity);
                response.Object.Elaauditorium = entity.EntityDB;
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
                    FechaAuditoria = $"{resulBDCronograma.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy")} a {resulBDCronograma.First().FechaDeFinDeEjecucionAuditoria?.ToString("dd/MM/yyyy")}", 
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
                ///TDO llenamos las listas para TCP y TCS 
                response.Object.Praciclonormassistema = repositoryMySql.SimpleSelect<Praciclonormassistema>(x => x.IdPrAcicloProgAuditoria == resulDBCicloPrograma.First().IdPrAcicloProgAuditoria);
                response.Object.Pradireccionespaproducto = repositoryMySql.SimpleSelect<Pradireccionespaproducto>(x => x.IdPrAcicloProgAuditoria == resulDBCicloPrograma.First().IdPrAcicloProgAuditoria);
                response.Object.Pradireccionespasistema = repositoryMySql.SimpleSelect<Pradireccionespasistema>(x => x.IdPrAcicloProgAuditoria == resulDBCicloPrograma.First().IdPrAcicloProgAuditoria);

                ///TDO llenamos las normas
                response.Object.Normas = new List<string>();
                response.Object.Pradireccionespaproducto?.ForEach(x =>
                {
                    response.Object.Normas.Add(x.Norma);
                });
                response.Object.Praciclonormassistema?.ForEach(x =>
                {
                    response.Object.Normas.Add(x.Norma);
                });

                ///TDO asignamos el area
                response.Object.Area = resulBDPrograma.First().IdparamArea == 39 ? "TCP" : "TCS";
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
                    ///TDO llenamos la informacion sobre el contenido del informe y otros documentos
                    response.Object.Elaauditorium.Elacontenidoauditoria = repositoryMySql.SimpleSelect<Elalistaspredefinida>(y => y.Area == response.Object.Area)
                        .Select(x => new Elacontenidoauditorium()
                        {
                            Area = response.Object.Area,
                            Categoria = x.Categoria,
                            Contenido = x.Decripcion,
                            Endocumento = x.Endocumento,
                            IdelaAuditoria = response.Object.Elaauditorium.IdelaAuditoria,
                            Label = x.Label,
                            Nemotico = x.Nemotico,
                            Seleccionado = 0,
                            Titulo = x.Titulo
                        }).ToList();
                    entity = new Entity<Elaauditorium> { EntityDB = response.Object.Elaauditorium, stateEntity = StateEntity.modify };
                    repositoryMySql.SaveObject<Elaauditorium>(entity);
                }
                else
                {
                    response.Object.Elaauditorium = resulDBAuditoria.First();
                    response.Object.Elaauditorium.Elaadps = repositoryMySql.SimpleSelect<Elaadp>(x => x.IdelaAuditoria == response.Object.Elaauditorium.IdelaAuditoria);
                    response.Object.Elaauditorium.Elacontenidoauditoria = repositoryMySql.SimpleSelect<Elacontenidoauditorium>(x => x.IdelaAuditoria == response.Object.Elaauditorium.IdelaAuditoria);
                    response.Object.Elaauditorium.Elacronogamas = repositoryMySql.SimpleSelect<Elacronogama>(x => x.Idelaauditoria == response.Object.Elaauditorium.IdelaAuditoria);
                    response.Object.Elaauditorium.Elahallazgos = repositoryMySql.SimpleSelect<Elahallazgo>(x => x.IdelaAuditoria == response.Object.Elaauditorium.IdelaAuditoria);
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
        public ResponseQuery<Elalistaspredefinida> GetListasPredefinidas(string area)
        {
            ResponseQuery<Elalistaspredefinida> response = new ResponseQuery<Elalistaspredefinida>();
            try
            {
                //response.ListEntities = repositoryMySql.SimpleSelect<Elalistaspredefinida>(y => y.Area == area).OrderBy(x => x.Nemotico).ThenBy(z => z.Orden).ToList();
                response.ListEntities = repositoryMySql.SimpleSelect<Elalistaspredefinida>(y => y.Area == area);
                response.State = ResponseType.Success;
                response.Message = "Lista obtenida correctamente";
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseQuery<Paramdocumento> GetListasDocumetos(string area, string proceso)
        {
            ResponseQuery<Paramdocumento> response = new ResponseQuery<Paramdocumento>();
            try
            {
                response.ListEntities = repositoryMySql.SimpleSelect<Paramdocumento>(x => x.Area == area && x.Proceso == proceso && x.Habilitado == 1);
                response.State = ResponseType.Success;
                response.Message = "Lista obtenida correctamente";
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public Response GenerarDocumento(string NombrePlantilla, int IdCicloAuditoria)
        {
            Response resul = new Response { State = ResponseType.Success };
            try
            {
                ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();
                var resulBDDocumento = repositoryMySql.SimpleSelect<Paramdocumento>(x => x.NombrePlantilla == NombrePlantilla);
                if (resulBDDocumento.Count == 0 || string.IsNullOrEmpty(resulBDDocumento.First().Method))
                {
                    resul.State = ResponseType.Warning;
                    resul.Message = "Plantilla  no implmentada";
                    return resul;
                }

                string pathPlantilla = Path.Combine(Global.PATH_PLANTILLAS, resulBDDocumento.First().Area, resulBDDocumento.First().Path);
                string pathOutPlantilla = Path.Combine(Global.PATH_PLANTILLAS, resulBDDocumento.First().Area, "salidas");
                string methodPlantilla = resulBDDocumento.First().Method;
                RequestDataReport requestDataReport = new RequestDataReport { IdCiclo = IdCicloAuditoria };
                //llamamos el metodo para recuperar data
                var myMethod = elaboracionAuditoriaManager.GetType().GetMethod(methodPlantilla);
                object[] parameters = new object[] { requestDataReport };
                ResponseObject<GlobalDataReport> resulMethod = myMethod.Invoke(elaboracionAuditoriaManager, parameters) as ResponseObject<GlobalDataReport>;
                if (resulMethod.State != ResponseType.Success)
                {
                    return resulMethod;
                }

                WordHelper generadorWord = new WordHelper(pathPlantilla);
                //generamos el documento en word
                string fileNameGenerado = generadorWord.GenerarDocumento(resulMethod.Object.data, resulMethod.Object.HeadersTables, pathOutPlantilla);
                resul.Message = fileNameGenerado;
            }
            catch (Exception ex)
            {
                ProcessError(ex, resul);
            }
            return resul;
        }
    }
}
