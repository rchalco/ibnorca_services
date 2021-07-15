using Business.Main.Base;
using Business.Main.Cross;
using Business.Main.DataMapping;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.ListarAuditoresxCargoCalificadoDTO;
using Business.Main.Modules.ElaboracionAuditoria;
using Business.Main.Modules.TomaDecision.DTO;
using Business.Main.Modules.TomaDecision.DTOExternal;
using CoreAccesLayer.Wraper;
using Domain.Main.Wraper;
using MySqlConnector;
using PlumbingProps.Document;
using System;
using System.Collections.Generic;
using System.IO;
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
        public Response GenerarDocumento(RequestExternalReport requestExternalReport)
        {
            Response resul = new Response { State = ResponseType.Success };
            try
            {
                var resulBDDocumento = repositoryMySql.SimpleSelect<Paramdocumento>(x => x.NombrePlantilla == requestExternalReport.NombrePlantilla);
                if (resulBDDocumento.Count == 0 || string.IsNullOrEmpty(resulBDDocumento.First().Method))
                {
                    resul.State = ResponseType.Warning;
                    resul.Message = "Plantilla  no implmentada";
                    return resul;
                }
                string pathPlantilla = Path.Combine(Global.PATH_PLANTILLAS, resulBDDocumento.First().Area, resulBDDocumento.First().Path);
                string pathOutPlantilla = Path.Combine(Global.PATH_PLANTILLAS, resulBDDocumento.First().Area, "salidas");
                string fileNameGenerado = string.Empty;

                WordHelper generadorWord = new WordHelper(pathPlantilla);

                if (requestExternalReport.SoloGenerar)
                {
                    List<WordHelper.ItemValues> itemValues = new List<WordHelper.ItemValues>();
                    requestExternalReport.ListItemReporte.ForEach(x =>
                    {
                        itemValues.Add(new WordHelper.ItemValues { key = x.Key, values = x.Value });
                    });

                    //generamos el documento en word
                    fileNameGenerado = generadorWord.GenerarDocumento(itemValues, null, pathOutPlantilla);
                    resul.Message = fileNameGenerado;
                }

                ///TDO rescatamos el id del ciclo
                var ciclo = repositoryMySql.GetDataByProcedure<Praciclosprogauditorium>("spGetCilcloByIdServicioAnio", requestExternalReport.IdServicio, requestExternalReport.Anio);
                if (ciclo.Count == 0)
                {
                    resul.State = ResponseType.Warning;
                    resul.Message = $"No se tiene programas y/o auditorias con el Id Servicio: {requestExternalReport.IdServicio} y anio {requestExternalReport.Anio}";
                    return resul;
                }


                ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();


                string methodPlantilla = resulBDDocumento.First().Method;
                RequestDataReport requestDataReport = new RequestDataReport { IdCiclo = (int)ciclo.First().IdPrAcicloProgAuditoria };
                //llamamos el metodo para recuperar data
                var myMethod = elaboracionAuditoriaManager.GetType().GetMethod(methodPlantilla);
                object[] parameters = new object[] { requestDataReport };
                ResponseObject<GlobalDataReport> resulMethod = myMethod.Invoke(elaboracionAuditoriaManager, parameters) as ResponseObject<GlobalDataReport>;
                if (resulMethod.State != ResponseType.Success)
                {
                    return resulMethod;
                }

                ///TDO completamos la informacion con los parametros de entrada
                requestExternalReport.ListItemReporte?.ForEach(x =>
                {
                    resulMethod.Object.data.GetType().GetProperty(x.Key)?.SetValue(resulMethod.Object.data, x.Value);
                });


                //generamos el documento en word
                fileNameGenerado = generadorWord.GenerarDocumento(resulMethod.Object.data, resulMethod.Object.HeadersTables, pathOutPlantilla);
                resul.Message = fileNameGenerado;
            }
            catch (Exception ex)
            {
                ProcessError(ex, resul);
            }
            return resul;
        }
        public Response GenerarDocumentoByIdCiclo(RequestExternalReport requestExternalReport)
        {
            Response resul = new Response { State = ResponseType.Success };
            try
            {
                var resulBDDocumento = repositoryMySql.SimpleSelect<Paramdocumento>(x => x.NombrePlantilla == requestExternalReport.NombrePlantilla);
                if (resulBDDocumento.Count == 0 || string.IsNullOrEmpty(resulBDDocumento.First().Method))
                {
                    resul.State = ResponseType.Warning;
                    resul.Message = "Plantilla  no implmentada";
                    return resul;
                }
                string pathPlantilla = Path.Combine(Global.PATH_PLANTILLAS, resulBDDocumento.First().Area, resulBDDocumento.First().Path);
                string pathOutPlantilla = Path.Combine(Global.PATH_PLANTILLAS, resulBDDocumento.First().Area, "salidas");
                string fileNameGenerado = string.Empty;

                WordHelper generadorWord = new WordHelper(pathPlantilla);

                if (requestExternalReport.SoloGenerar)
                {
                    List<WordHelper.ItemValues> itemValues = new List<WordHelper.ItemValues>();
                    requestExternalReport.ListItemReporte.ForEach(x =>
                    {
                        itemValues.Add(new WordHelper.ItemValues { key = x.Key, values = x.Value });
                    });

                    //generamos el documento en word
                    fileNameGenerado = generadorWord.GenerarDocumento(itemValues, null, pathOutPlantilla);
                    resul.Message = fileNameGenerado;
                }

                ///TDO rescatamos el id del ciclo
                ElaboracionAuditoriaManager elaboracionAuditoriaManager = new ElaboracionAuditoriaManager();

                string methodPlantilla = resulBDDocumento.First().Method;
                RequestDataReport requestDataReport = new RequestDataReport { IdCiclo = requestExternalReport.IdCiclo };
                //llamamos el metodo para recuperar data
                var myMethod = elaboracionAuditoriaManager.GetType().GetMethod(methodPlantilla);
                object[] parameters = new object[] { requestDataReport };
                ResponseObject<GlobalDataReport> resulMethod = myMethod.Invoke(elaboracionAuditoriaManager, parameters) as ResponseObject<GlobalDataReport>;
                if (resulMethod.State != ResponseType.Success)
                {
                    return resulMethod;
                }

                ///TDO completamos la informacion con los parametros de entrada
                requestExternalReport.ListItemReporte?.ForEach(x =>
                {
                    resulMethod.Object.data.GetType().GetProperty(x.Key)?.SetValue(resulMethod.Object.data, x.Value);
                });

                ///TDO completamos la inforamcion de las listas
                requestExternalReport.ListasAdicionales?.GetType().GetProperties().ToList().ForEach(x =>
                {
                    resulMethod.Object.data.GetType().GetProperty(x.Name)?.SetValue(resulMethod.Object.data, x.GetValue(requestExternalReport.ListasAdicionales));
                });

                //generamos el documento en word
                fileNameGenerado = generadorWord.GenerarDocumento(resulMethod.Object.data, resulMethod.Object.HeadersTables, pathOutPlantilla);
                resul.Message = fileNameGenerado;
            }
            catch (Exception ex)
            {
                ProcessError(ex, resul);
            }
            return resul;
        }
        public ResponseQuery<DTOspWSGetResumePrograma> GetResumePrograma(string tipo, int idCiclo)
        {
            ResponseQuery<DTOspWSGetResumePrograma> resul = new ResponseQuery<DTOspWSGetResumePrograma> { State = ResponseType.Success, Message = "Programas obtenidos correctamente" };
            try
            {
                resul.ListEntities = repositoryMySql.GetDataByProcedure<DTOspWSGetResumePrograma>("spWSGetResumePrograma", tipo, idCiclo);
                resul.ListEntities.ForEach(x =>
                {
                    x.detalle = new DTOspWSGetResumePrograma.Detalle();
                    if (x.area == "TCS")
                    {
                        x.detalle.detalleTCS = repositoryMySql.SimpleSelect<Praciclonormassistema>(yy => yy.IdPrAcicloProgAuditoria == x.idCiclo).Select(
                            zz =>
                            {
                                return new DTOspWSGetResumePrograma.DetalleTCS
                                {
                                    Alcance = zz.Alcance,
                                    IdCicloNormaSistema = (int)zz.IdCicloNormaSistema,
                                    IdPrAcicloProgAuditoria = (int)zz.IdPrAcicloProgAuditoria,
                                    Norma = zz.Norma,
                                    NumeroDeCertificacion = zz.NumeroDeCertificacion
                                };
                            }).ToList();

                        string direcciones = string.Empty;

                        repositoryMySql.SimpleSelect<Pradireccionespasistema>(hh => hh.IdPrAcicloProgAuditoria == x.idCiclo).ForEach(jj =>
                        {
                            direcciones += jj.Direccion + "|";
                        });

                        x.detalle.detalleTCS.ForEach(ff =>
                        {
                            ff.Direcciones = direcciones;
                        });
                    }
                    else
                    {
                        x.detalle.detalleTCP = repositoryMySql.SimpleSelect<Pradireccionespaproducto>(yy => yy.IdPrAcicloProgAuditoria == x.idCiclo).Select(
                            zz =>
                            {
                                return new DTOspWSGetResumePrograma.DetalleTCP
                                {
                                    IdPrAcicloProgAuditoria = (int)zz.IdPrAcicloProgAuditoria,
                                    Norma = zz.Norma,
                                    NumeroDeCertificacion = zz.NumeroDeCertificacion,
                                    Direccion = zz.Direccion,
                                    IdDireccionPaproducto = zz.IdDireccionPaproducto,
                                    Marca = zz.Marca,
                                    Nombre = zz.Nombre,
                                    Sello = zz.Sello
                                };
                            }).ToList();
                    }
                });
            }
            catch (Exception ex)
            {
                ProcessError(ex, resul);
            }
            return resul;
        }

        public ResponseQuery<DTOspWSGetResumeProgramaProducto> GetResumeProgramaProducto()
        {
            ResponseQuery<DTOspWSGetResumeProgramaProducto> resul = new ResponseQuery<DTOspWSGetResumeProgramaProducto> { State = ResponseType.Success, Message = "Programas obtenidos correctamente" };
            try
            {
                resul.ListEntities = repositoryMySql.GetDataByProcedure<DTOspWSGetResumeProgramaProducto>("spWSGetResumeProgramaProducto");
            }
            catch (Exception ex)
            {
                ProcessError(ex, resul);
            }
            return resul;
        }

        public ResponseQuery<DTOspWSGetDetalleProgramaTCP> GetDetalleProgramaTCP(int idProducto)
        {
            ResponseQuery<DTOspWSGetDetalleProgramaTCP> resul = new ResponseQuery<DTOspWSGetDetalleProgramaTCP> { State = ResponseType.Success, Message = "detalles obtenidos correctamente" };
            try
            {
                resul.ListEntities = repositoryMySql.GetDataByProcedure<DTOspWSGetDetalleProgramaTCP>("spWSGetDetalleProgramaTCP", idProducto);
            }
            catch (Exception ex)
            {
                ProcessError(ex, resul);
            }
            return resul;
        }

        public ResponseQuery<DTOspWSGetDetalleProgramaTCS> GetDetalleProgramaTCS(int idSistema)
        {
            ResponseQuery<DTOspWSGetDetalleProgramaTCS> resul = new ResponseQuery<DTOspWSGetDetalleProgramaTCS> { State = ResponseType.Success, Message = "detalles obtenidos correctamente" };
            try
            {
                resul.ListEntities = repositoryMySql.GetDataByProcedure<DTOspWSGetDetalleProgramaTCS>("spWSGetDetalleProgramaTCS", idSistema);
            }
            catch (Exception ex)
            {
                ProcessError(ex, resul);
            }
            return resul;
        }

    }

}
