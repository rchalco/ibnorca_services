using System;
using Business.Main.Base;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Main.Wraper;
using Business.Main.DataMapping;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.BuscarxIdClienteEmpresaDTO;
using Newtonsoft.Json;
using PlumbingProps.Services;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.ListarContactosEmpresaDTO;
using PlumbingProps.Document;
using Business.Main.Cross;
using static PlumbingProps.Document.WordHelper;
using Business.Main.Modules.ElaboracionAuditoria.Reportes.TCS;
using Business.Main.DataMapping.DTOs;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.ListarAuditoresxCargoCalificadoDTO;
using Resportes.ReportDTO;

namespace Business.Main.Modules.ElaboracionAuditoria
{
    public partial class ElaboracionAuditoriaManager
    {
        public Response GenerarREPListaVerificacionReunionApertura(int IdCiclo, string pathPlantilla, string fechaInicio, string fechaFin, string nombreYFirmaDeAuditorLider)
        {
            Response response = new Response { Message = "", State = ResponseType.Success };
            try
            {

                ///Obtenemos la informacion del ciclo y del programa
                Praciclosprogauditorium praciclocronograma = repositoryMySql.SimpleSelect<Praciclosprogauditorium>(x => x.IdPrAcicloProgAuditoria == IdCiclo).ToList().FirstOrDefault();
                Praprogramasdeauditorium praprogramasdeauditorium = repositoryMySql.SimpleSelect<Praprogramasdeauditorium>(x => x.IdPrAprogramaAuditoria == praciclocronograma.IdPrAprogramaAuditoria).ToList().FirstOrDefault();
                if (praciclocronograma == null)
                {
                    response.State = ResponseType.Warning;
                    response.Message = "No se cuenta con informacion de este cilo en la BD";
                    return response;
                }

                praciclocronograma.Praciclocronogramas = repositoryMySql.SimpleSelect<Praciclocronograma>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Praciclonormassistemas = repositoryMySql.SimpleSelect<Praciclonormassistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pracicloparticipantes = repositoryMySql.SimpleSelect<Pracicloparticipante>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespaproductos = repositoryMySql.SimpleSelect<Pradireccionespaproducto>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespasistemas = repositoryMySql.SimpleSelect<Pradireccionespasistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);

                Cliente cliente = JsonConvert.DeserializeObject<Cliente>(praprogramasdeauditorium.OrganizacionContentWs);

                ///obtenemos los contactos del cliente
                ClientHelper clientHelper = new ClientHelper();
                ///TDO: obtenemos los datos del servicio
                RequestListarContactosEmpresa requestDato = new RequestListarContactosEmpresa { accion = "ListarContactosEmpresa", sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES, IdCliente = cliente.IdCliente };
                ResponseListarContactosEmpresa resulServices = clientHelper.Consume<ResponseListarContactosEmpresa>(Global.URIGLOBAL_SERVICES + Global.URI_CLIENTE_CONTACTO, requestDato).Result;
                if (!resulServices.estado)
                {
                    response.State = ResponseType.Warning;
                    response.Message = $"Existe problemas al consumir el servicio de ibnorca (estados): {resulServices.mensaje}";
                    return response;
                }
                ContactoEmpresa contactoEmpresa = resulServices.lstContactos?.Count > 0 ? resulServices.lstContactos[0] : null;

                string normas = "";

                praciclocronograma.Praciclonormassistemas.ToList().ForEach(x =>
                {
                    normas += x.Norma;
                });
                string alcance = "";
                praciclocronograma.Praciclonormassistemas.ToList().ForEach(x =>
                {
                    alcance += x.Alcance + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                string sitios = "";
                praciclocronograma.Pradireccionespasistemas.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                ///llenamos el reporte con la informacion de este ciclo
                REPListaVerificacionReunionApertura praListaVerificacionReunionApertura = new REPListaVerificacionReunionApertura
                {
                    NombreEmpresa = cliente.NombreRazon,
                    CodigoServicio = praprogramasdeauditorium.CodigoIafws,
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin,
                    TipoAuditoria = praciclocronograma.Referencia,
                    Norma = normas,
                    AuditorLider = nombreYFirmaDeAuditorLider


                };
                string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                WordHelper generadorWord = new WordHelper(filePlantilla);


                //generamos el documento en word

                string fileNameGenerado = generadorWord.GenerarDocumento(praListaVerificacionReunionApertura, null, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");


                response.Message = fileNameGenerado;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public Response GenerarREPListaVerificacionReunionCierre(int IdCiclo, string pathPlantilla, string nombreYFirmaDeAuditorLider)
        {
            Response response = new Response { Message = "", State = ResponseType.Success };
            try
            {

                ///Obtenemos la informacion del ciclo y del programa
                Praciclosprogauditorium praciclocronograma = repositoryMySql.SimpleSelect<Praciclosprogauditorium>(x => x.IdPrAcicloProgAuditoria == IdCiclo).ToList().FirstOrDefault();
                Praprogramasdeauditorium praprogramasdeauditorium = repositoryMySql.SimpleSelect<Praprogramasdeauditorium>(x => x.IdPrAprogramaAuditoria == praciclocronograma.IdPrAprogramaAuditoria).ToList().FirstOrDefault();
                if (praciclocronograma == null)
                {
                    response.State = ResponseType.Warning;
                    response.Message = "No se cuenta con informacion de este cilo en la BD";
                    return response;
                }

                praciclocronograma.Praciclocronogramas = repositoryMySql.SimpleSelect<Praciclocronograma>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Praciclonormassistemas = repositoryMySql.SimpleSelect<Praciclonormassistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pracicloparticipantes = repositoryMySql.SimpleSelect<Pracicloparticipante>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespaproductos = repositoryMySql.SimpleSelect<Pradireccionespaproducto>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespasistemas = repositoryMySql.SimpleSelect<Pradireccionespasistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);

                Cliente cliente = JsonConvert.DeserializeObject<Cliente>(praprogramasdeauditorium.OrganizacionContentWs);

                ///obtenemos los contactos del cliente
                ClientHelper clientHelper = new ClientHelper();
                ///TDO: obtenemos los datos del servicio
                RequestListarContactosEmpresa requestDato = new RequestListarContactosEmpresa { accion = "ListarContactosEmpresa", sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES, IdCliente = cliente.IdCliente };
                ResponseListarContactosEmpresa resulServices = clientHelper.Consume<ResponseListarContactosEmpresa>(Global.URIGLOBAL_SERVICES + Global.URI_CLIENTE_CONTACTO, requestDato).Result;
                if (!resulServices.estado)
                {
                    response.State = ResponseType.Warning;
                    response.Message = $"Existe problemas al consumir el servicio de ibnorca (estados): {resulServices.mensaje}";
                    return response;
                }
                ContactoEmpresa contactoEmpresa = resulServices.lstContactos?.Count > 0 ? resulServices.lstContactos[0] : null;

                string normas = "";

                praciclocronograma.Praciclonormassistemas.ToList().ForEach(x =>
                {
                    normas += x.Norma;
                });
                string alcance = "";
                praciclocronograma.Praciclonormassistemas.ToList().ForEach(x =>
                {
                    alcance += x.Alcance + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                string sitios = "";
                praciclocronograma.Pradireccionespasistemas.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                ///llenamos el reporte con la informacion de este ciclo
                REPListaVerificacionReunionCierre praListaVerificacionReunionCierre = new REPListaVerificacionReunionCierre
                {

                    NombreOrganizacion = cliente.NombreRazon,
                    CodigoDeServicio = praprogramasdeauditorium.CodigoIafws,
                    FechaDeAuditoria = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    TipoDeAuditoria = praciclocronograma.Referencia,
                    NormasAuditadas = normas,
                    NombreYFirmaDeAuditorLider = nombreYFirmaDeAuditorLider

                };
                string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                WordHelper generadorWord = new WordHelper(filePlantilla);


                //generamos el documento en word

                string fileNameGenerado = generadorWord.GenerarDocumento(praListaVerificacionReunionCierre, null, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");


                response.Message = fileNameGenerado;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public Response GenerarREPListaAsistencia(int IdCiclo, string pathPlantilla, string fechaInicio, string fechaFin)
        {
            Response response = new Response { Message = "", State = ResponseType.Success };
            try
            {

                ///Obtenemos la informacion del ciclo y del programa
                Praciclosprogauditorium praciclocronograma = repositoryMySql.SimpleSelect<Praciclosprogauditorium>(x => x.IdPrAcicloProgAuditoria == IdCiclo).ToList().FirstOrDefault();
                Praprogramasdeauditorium praprogramasdeauditorium = repositoryMySql.SimpleSelect<Praprogramasdeauditorium>(x => x.IdPrAprogramaAuditoria == praciclocronograma.IdPrAprogramaAuditoria).ToList().FirstOrDefault();
                if (praciclocronograma == null)
                {
                    response.State = ResponseType.Warning;
                    response.Message = "No se cuenta con informacion de este cilo en la BD";
                    return response;
                }

                praciclocronograma.Praciclocronogramas = repositoryMySql.SimpleSelect<Praciclocronograma>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Praciclonormassistemas = repositoryMySql.SimpleSelect<Praciclonormassistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pracicloparticipantes = repositoryMySql.SimpleSelect<Pracicloparticipante>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespaproductos = repositoryMySql.SimpleSelect<Pradireccionespaproducto>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespasistemas = repositoryMySql.SimpleSelect<Pradireccionespasistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);

                Cliente cliente = JsonConvert.DeserializeObject<Cliente>(praprogramasdeauditorium.OrganizacionContentWs);

                ///obtenemos los contactos del cliente
                ClientHelper clientHelper = new ClientHelper();
                ///TDO: obtenemos los datos del servicio
                RequestListarContactosEmpresa requestDato = new RequestListarContactosEmpresa { accion = "ListarContactosEmpresa", sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES, IdCliente = cliente.IdCliente };
                ResponseListarContactosEmpresa resulServices = clientHelper.Consume<ResponseListarContactosEmpresa>(Global.URIGLOBAL_SERVICES + Global.URI_CLIENTE_CONTACTO, requestDato).Result;
                if (!resulServices.estado)
                {
                    response.State = ResponseType.Warning;
                    response.Message = $"Existe problemas al consumir el servicio de ibnorca (estados): {resulServices.mensaje}";
                    return response;
                }
                ContactoEmpresa contactoEmpresa = resulServices.lstContactos?.Count > 0 ? resulServices.lstContactos[0] : null;

                string normas = "";

                praciclocronograma.Praciclonormassistemas.ToList().ForEach(x =>
                {
                    normas += x.Norma;
                });
                string alcance = "";
                praciclocronograma.Praciclonormassistemas.ToList().ForEach(x =>
                {
                    alcance += x.Alcance + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                string sitios = "";
                praciclocronograma.Pradireccionespasistemas.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                ///llenamos el reporte con la informacion de este ciclo
                REPListaAsistencia praListaAsistencia = new REPListaAsistencia
                {

                    NombreEmpresa = cliente.NombreRazon,
                    CodigoServicio = praprogramasdeauditorium.CodigoIafws,
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin,
                    TipoAuditoria = praciclocronograma.Referencia,
                    Norma = normas


                };
                string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                WordHelper generadorWord = new WordHelper(filePlantilla);


                //generamos el documento en word

                string fileNameGenerado = generadorWord.GenerarDocumento(praListaAsistencia, null, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");


                response.Message = fileNameGenerado;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public Response GenerarREPListaVerificacionAuditor(int IdCiclo, string pathPlantilla, string usuario, string cargo, string procesoAuditado, string nombreAuditado)
        {
            Response response = new Response { Message = "", State = ResponseType.Success };
            try
            {

                ///Obtenemos la informacion del ciclo y del programa
                Praciclosprogauditorium praciclocronograma = repositoryMySql.SimpleSelect<Praciclosprogauditorium>(x => x.IdPrAcicloProgAuditoria == IdCiclo).ToList().FirstOrDefault();
                Praprogramasdeauditorium praprogramasdeauditorium = repositoryMySql.SimpleSelect<Praprogramasdeauditorium>(x => x.IdPrAprogramaAuditoria == praciclocronograma.IdPrAprogramaAuditoria).ToList().FirstOrDefault();
                if (praciclocronograma == null)
                {
                    response.State = ResponseType.Warning;
                    response.Message = "No se cuenta con informacion de este cilo en la BD";
                    return response;
                }

                praciclocronograma.Praciclocronogramas = repositoryMySql.SimpleSelect<Praciclocronograma>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Praciclonormassistemas = repositoryMySql.SimpleSelect<Praciclonormassistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pracicloparticipantes = repositoryMySql.SimpleSelect<Pracicloparticipante>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespaproductos = repositoryMySql.SimpleSelect<Pradireccionespaproducto>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespasistemas = repositoryMySql.SimpleSelect<Pradireccionespasistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);

                Cliente cliente = JsonConvert.DeserializeObject<Cliente>(praprogramasdeauditorium.OrganizacionContentWs);

                ///obtenemos los contactos del cliente
                ClientHelper clientHelper = new ClientHelper();
                ///TDO: obtenemos los datos del servicio
                RequestListarContactosEmpresa requestDato = new RequestListarContactosEmpresa { accion = "ListarContactosEmpresa", sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES, IdCliente = cliente.IdCliente };
                ResponseListarContactosEmpresa resulServices = clientHelper.Consume<ResponseListarContactosEmpresa>(Global.URIGLOBAL_SERVICES + Global.URI_CLIENTE_CONTACTO, requestDato).Result;
                if (!resulServices.estado)
                {
                    response.State = ResponseType.Warning;
                    response.Message = $"Existe problemas al consumir el servicio de ibnorca (estados): {resulServices.mensaje}";
                    return response;
                }
                ContactoEmpresa contactoEmpresa = resulServices.lstContactos?.Count > 0 ? resulServices.lstContactos[0] : null;

                string normas = "";

                praciclocronograma.Praciclonormassistemas.ToList().ForEach(x =>
                {
                    normas += x.Norma;
                });
                string alcance = "";
                praciclocronograma.Praciclonormassistemas.ToList().ForEach(x =>
                {
                    alcance += x.Alcance + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                string sitios = "";
                praciclocronograma.Pradireccionespasistemas.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                ///llenamos el reporte con la informacion de este ciclo
                REPListaVerificacionAuditor praListaVerificacionAuditor = new REPListaVerificacionAuditor
                {
                    NombreEmpresa = cliente.NombreRazon,
                    Sitios = sitios,
                    TipoAuditoria = praciclocronograma.Referencia,
                    Norma = normas,
                    FechaAuditoria = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    Usuario = usuario,
                    Cargo = cargo,
                    ProcesoAuditado = procesoAuditado,
                    NombreAuditado = nombreAuditado,
                    Fecha = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    SitiosAuditado = sitios


                };
                string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                WordHelper generadorWord = new WordHelper(filePlantilla);


                //generamos el documento en word

                string fileNameGenerado = generadorWord.GenerarDocumento(praListaVerificacionAuditor, null, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");


                response.Message = fileNameGenerado;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public Response GenerarREPLIstaVerificacionBPM(int IdCiclo, string pathPlantilla, string nombreYApellidos, string responsabilidadORol)
        {
            Response response = new Response { Message = "", State = ResponseType.Success };
            try
            {

                ///Obtenemos la informacion del ciclo y del programa
                Praciclosprogauditorium praciclocronograma = repositoryMySql.SimpleSelect<Praciclosprogauditorium>(x => x.IdPrAcicloProgAuditoria == IdCiclo).ToList().FirstOrDefault();
                Praprogramasdeauditorium praprogramasdeauditorium = repositoryMySql.SimpleSelect<Praprogramasdeauditorium>(x => x.IdPrAprogramaAuditoria == praciclocronograma.IdPrAprogramaAuditoria).ToList().FirstOrDefault();
                if (praciclocronograma == null)
                {
                    response.State = ResponseType.Warning;
                    response.Message = "No se cuenta con informacion de este cilo en la BD";
                    return response;
                }

                praciclocronograma.Praciclocronogramas = repositoryMySql.SimpleSelect<Praciclocronograma>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Praciclonormassistemas = repositoryMySql.SimpleSelect<Praciclonormassistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pracicloparticipantes = repositoryMySql.SimpleSelect<Pracicloparticipante>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespaproductos = repositoryMySql.SimpleSelect<Pradireccionespaproducto>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespasistemas = repositoryMySql.SimpleSelect<Pradireccionespasistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);

                Cliente cliente = JsonConvert.DeserializeObject<Cliente>(praprogramasdeauditorium.OrganizacionContentWs);

                ///obtenemos los contactos del cliente
                ClientHelper clientHelper = new ClientHelper();
                ///TDO: obtenemos los datos del servicio
                RequestListarContactosEmpresa requestDato = new RequestListarContactosEmpresa { accion = "ListarContactosEmpresa", sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES, IdCliente = cliente.IdCliente };
                ResponseListarContactosEmpresa resulServices = clientHelper.Consume<ResponseListarContactosEmpresa>(Global.URIGLOBAL_SERVICES + Global.URI_CLIENTE_CONTACTO, requestDato).Result;
                if (!resulServices.estado)
                {
                    response.State = ResponseType.Warning;
                    response.Message = $"Existe problemas al consumir el servicio de ibnorca (estados): {resulServices.mensaje}";
                    return response;
                }
                ContactoEmpresa contactoEmpresa = resulServices.lstContactos?.Count > 0 ? resulServices.lstContactos[0] : null;

                string normas = "";

                praciclocronograma.Praciclonormassistemas.ToList().ForEach(x =>
                {
                    normas += x.Norma;
                });
                string alcance = "";
                praciclocronograma.Praciclonormassistemas.ToList().ForEach(x =>
                {
                    alcance += x.Alcance + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                string sitios = "";
                praciclocronograma.Pradireccionespasistemas.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                ///llenamos el reporte con la informacion de este ciclo
                REPLIstaVerificacionBPM praLIstaVerificacionBPM = new REPLIstaVerificacionBPM
                {

                    Organizacion = cliente.NombreRazon,
                    SitiosAuditados = sitios,
                    TipoAuditoria = praciclocronograma.Referencia,
                    FechasDeAuditoria = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    NombreYApellidos = nombreYApellidos,
                    ResponsabilidadORol = responsabilidadORol
                };
                string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                WordHelper generadorWord = new WordHelper(filePlantilla);


                //generamos el documento en word

                string fileNameGenerado = generadorWord.GenerarDocumento(praLIstaVerificacionBPM, null, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");


                response.Message = fileNameGenerado;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public Response GenerarREPDatosDeLaOrganizacion(int IdCiclo, string pathPlantilla, string nombreAuditorLider, string nombreCargoRepresentanteOrg, string coordinadorAuditoria)
        {
            Response response = new Response { Message = "", State = ResponseType.Success };
            try
            {

                ///Obtenemos la informacion del ciclo y del programa
                Praciclosprogauditorium praciclocronograma = repositoryMySql.SimpleSelect<Praciclosprogauditorium>(x => x.IdPrAcicloProgAuditoria == IdCiclo).ToList().FirstOrDefault();
                Praprogramasdeauditorium praprogramasdeauditorium = repositoryMySql.SimpleSelect<Praprogramasdeauditorium>(x => x.IdPrAprogramaAuditoria == praciclocronograma.IdPrAprogramaAuditoria).ToList().FirstOrDefault();
                if (praciclocronograma == null)
                {
                    response.State = ResponseType.Warning;
                    response.Message = "No se cuenta con informacion de este cilo en la BD";
                    return response;
                }

                praciclocronograma.Praciclocronogramas = repositoryMySql.SimpleSelect<Praciclocronograma>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Praciclonormassistemas = repositoryMySql.SimpleSelect<Praciclonormassistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pracicloparticipantes = repositoryMySql.SimpleSelect<Pracicloparticipante>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespaproductos = repositoryMySql.SimpleSelect<Pradireccionespaproducto>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespasistemas = repositoryMySql.SimpleSelect<Pradireccionespasistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);

                Cliente cliente = JsonConvert.DeserializeObject<Cliente>(praprogramasdeauditorium.OrganizacionContentWs);

                ///obtenemos los contactos del cliente
                ClientHelper clientHelper = new ClientHelper();
                ///TDO: obtenemos los datos del servicio
                RequestListarContactosEmpresa requestDato = new RequestListarContactosEmpresa { accion = "ListarContactosEmpresa", sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES, IdCliente = cliente.IdCliente };
                ResponseListarContactosEmpresa resulServices = clientHelper.Consume<ResponseListarContactosEmpresa>(Global.URIGLOBAL_SERVICES + Global.URI_CLIENTE_CONTACTO, requestDato).Result;
                if (!resulServices.estado)
                {
                    response.State = ResponseType.Warning;
                    response.Message = $"Existe problemas al consumir el servicio de ibnorca (estados): {resulServices.mensaje}";
                    return response;
                }
                ContactoEmpresa contactoEmpresa = resulServices.lstContactos?.Count > 0 ? resulServices.lstContactos[0] : null;

                string normas = "";

                praciclocronograma.Praciclonormassistemas.ToList().ForEach(x =>
                {
                    normas += x.Norma;
                });
                string alcance = "";
                praciclocronograma.Praciclonormassistemas.ToList().ForEach(x =>
                {
                    alcance += x.Alcance + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                string sitios = "";
                praciclocronograma.Pradireccionespasistemas.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                ///llenamos el reporte con la informacion de este ciclo
                REPDatosDeLaOrganizacion praDatosDeLaOrganizacion = new REPDatosDeLaOrganizacion
                {
                    FechaDeAuditoria = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    TipoDeAuditoria = praciclocronograma.Referencia,
                    NombreDeLaOrganizacion = cliente.NombreRazon,
                    Norma = normas,
                    Alcance = alcance,
                    Sitios = sitios,
                    NombreAuditorLider = nombreAuditorLider,
                    NombreCargoRepresentanteOrg = nombreCargoRepresentanteOrg,
                    CoordinadorAuditoria = coordinadorAuditoria,
                    FechaActual = DateTime.Now.ToString("dd/MM/yyyy")


                };
                string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                WordHelper generadorWord = new WordHelper(filePlantilla);


                //generamos el documento en word

                string fileNameGenerado = generadorWord.GenerarDocumento(praDatosDeLaOrganizacion, null, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");


                response.Message = fileNameGenerado;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public object GenerarREPDecisionNOFavorable(int v1, string v2, string v3, string v4, string v5, string v6, string v7)
        {
            throw new NotImplementedException();
        }
        public Response GenerarREPEvaluacionYRecomendacionesParaProceso(int IdCiclo, string pathPlantilla, string expertoCertificacion, string fechaAsignacionProceso, string redaccionSugerida)
        {
            Response response = new Response { Message = "", State = ResponseType.Success };
            try
            {

                ///Obtenemos la informacion del ciclo y del programa
                Praciclosprogauditorium praciclocronograma = repositoryMySql.SimpleSelect<Praciclosprogauditorium>(x => x.IdPrAcicloProgAuditoria == IdCiclo).ToList().FirstOrDefault();
                Praprogramasdeauditorium praprogramasdeauditorium = repositoryMySql.SimpleSelect<Praprogramasdeauditorium>(x => x.IdPrAprogramaAuditoria == praciclocronograma.IdPrAprogramaAuditoria).ToList().FirstOrDefault();
                if (praciclocronograma == null)
                {
                    response.State = ResponseType.Warning;
                    response.Message = "No se cuenta con informacion de este cilo en la BD";
                    return response;
                }

                praciclocronograma.Praciclocronogramas = repositoryMySql.SimpleSelect<Praciclocronograma>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Praciclonormassistemas = repositoryMySql.SimpleSelect<Praciclonormassistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pracicloparticipantes = repositoryMySql.SimpleSelect<Pracicloparticipante>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespaproductos = repositoryMySql.SimpleSelect<Pradireccionespaproducto>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespasistemas = repositoryMySql.SimpleSelect<Pradireccionespasistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);

                Cliente cliente = JsonConvert.DeserializeObject<Cliente>(praprogramasdeauditorium.OrganizacionContentWs);

                ///obtenemos los contactos del cliente
                ClientHelper clientHelper = new ClientHelper();
                ///TDO: obtenemos los datos del servicio
                RequestListarContactosEmpresa requestDato = new RequestListarContactosEmpresa { accion = "ListarContactosEmpresa", sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES, IdCliente = cliente.IdCliente };
                ResponseListarContactosEmpresa resulServices = clientHelper.Consume<ResponseListarContactosEmpresa>(Global.URIGLOBAL_SERVICES + Global.URI_CLIENTE_CONTACTO, requestDato).Result;
                if (!resulServices.estado)
                {
                    response.State = ResponseType.Warning;
                    response.Message = $"Existe problemas al consumir el servicio de ibnorca (estados): {resulServices.mensaje}";
                    return response;
                }
                ContactoEmpresa contactoEmpresa = resulServices.lstContactos?.Count > 0 ? resulServices.lstContactos[0] : null;

                string normas = "";

                praciclocronograma.Praciclonormassistemas.ToList().ForEach(x =>
                {
                    normas += x.Norma;
                });
                string alcance = "";
                praciclocronograma.Praciclonormassistemas.ToList().ForEach(x =>
                {
                    alcance += x.Alcance + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                string sitios = "";
                praciclocronograma.Pradireccionespasistemas.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                ///llenamos el reporte con la informacion de este ciclo
                REPEvaluacionYRecomendacionesParaProceso praEvaluacionYRecomendacionesParaProceso = new REPEvaluacionYRecomendacionesParaProceso
                {
                    Orgaizacion = cliente.NombreRazon,
                    CodigoDeServicio = praprogramasdeauditorium.CodigoServicioWs,
                    FechaDeAuditoria = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    TipoDeAuditoria = praciclocronograma.Referencia,
                    NormasAuditadas = normas,
                    //EquipoAuditoNombreCargo = null,
                    ListEquipoAuditoNombreCargo = praciclocronograma.Pracicloparticipantes.Select(x =>
                    {
                        RepDesginacionParticipante repDesginacionParticipante = new RepDesginacionParticipante();
                        repDesginacionParticipante.Cargo = string.Empty;
                        if (!string.IsNullOrEmpty(x.CargoDetalleWs))
                        {
                            ListaCalificado cargo = JsonConvert.DeserializeObject<ListaCalificado>(x.CargoDetalleWs);
                            repDesginacionParticipante.Cargo = cargo.CargoPuesto;
                        }

                        repDesginacionParticipante.Participante = string.Empty;
                        if (!string.IsNullOrEmpty(x.ParticipanteDetalleWs))
                        {
                            ListaCalificado participante = JsonConvert.DeserializeObject<ListaCalificado>(x.ParticipanteDetalleWs);
                            repDesginacionParticipante.Participante = participante.NombreCompleto;
                        }

                        return repDesginacionParticipante;
                    }).ToList(),
                    ExpertoCertificacion = expertoCertificacion,
                    FechaAsignacionProceso = fechaAsignacionProceso,
                    RedaccionSugerida = redaccionSugerida
                };
                string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                WordHelper generadorWord = new WordHelper(filePlantilla);

                //generamos el documento en word
                Dictionary<string, CellTitles[]> pTitles = new Dictionary<string, CellTitles[]>();
                CellTitles[] cellTitlesTitulo = new CellTitles[2];
                cellTitlesTitulo[0] = new CellTitles { Title = "Calificación", Visible = true, Width = "50" };
                cellTitlesTitulo[1] = new CellTitles { Title = "Auditor", Visible = true, Width = "50" };
                pTitles.Add("ListEquipoAuditoNombreCargo", cellTitlesTitulo);

                //generamos el documento en word

                string fileNameGenerado = generadorWord.GenerarDocumento(praEvaluacionYRecomendacionesParaProceso, pTitles, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");


                response.Message = fileNameGenerado;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public Response GenerarDescisionFavorableCertificacion(int IdCiclo, string pathPlantilla, string nombre, string cargo)
        {
            Response response = new Response { Message = "", State = ResponseType.Success };
            try
            {

                ///Obtenemos la informacion del ciclo y del programa
                Praciclosprogauditorium praciclocronograma = repositoryMySql.SimpleSelect<Praciclosprogauditorium>(x => x.IdPrAcicloProgAuditoria == IdCiclo).ToList().FirstOrDefault();
                Praprogramasdeauditorium praprogramasdeauditorium = repositoryMySql.SimpleSelect<Praprogramasdeauditorium>(x => x.IdPrAprogramaAuditoria == praciclocronograma.IdPrAprogramaAuditoria).ToList().FirstOrDefault();
                if (praciclocronograma == null)
                {
                    response.State = ResponseType.Warning;
                    response.Message = "No se cuenta con informacion de este cilo en la BD";
                    return response;
                }

                praciclocronograma.Praciclocronogramas = repositoryMySql.SimpleSelect<Praciclocronograma>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Praciclonormassistemas = repositoryMySql.SimpleSelect<Praciclonormassistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pracicloparticipantes = repositoryMySql.SimpleSelect<Pracicloparticipante>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespaproductos = repositoryMySql.SimpleSelect<Pradireccionespaproducto>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespasistemas = repositoryMySql.SimpleSelect<Pradireccionespasistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);

                Cliente cliente = JsonConvert.DeserializeObject<Cliente>(praprogramasdeauditorium.OrganizacionContentWs);

                ///obtenemos los contactos del cliente
                ClientHelper clientHelper = new ClientHelper();
                ///TDO: obtenemos los datos del servicio
                RequestListarContactosEmpresa requestDato = new RequestListarContactosEmpresa { accion = "ListarContactosEmpresa", sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES, IdCliente = cliente.IdCliente };
                ResponseListarContactosEmpresa resulServices = clientHelper.Consume<ResponseListarContactosEmpresa>(Global.URIGLOBAL_SERVICES + Global.URI_CLIENTE_CONTACTO, requestDato).Result;
                if (!resulServices.estado)
                {
                    response.State = ResponseType.Warning;
                    response.Message = $"Existe problemas al consumir el servicio de ibnorca (estados): {resulServices.mensaje}";
                    return response;
                }
                ContactoEmpresa contactoEmpresa = resulServices.lstContactos?.Count > 0 ? resulServices.lstContactos[0] : null;

                string normas = "";

                praciclocronograma.Praciclonormassistemas.ToList().ForEach(x =>
                {
                    normas += x.Norma;
                });

                ///llenamos el reporte con la informacion de este ciclo
                REPDecisionFavorableCertificacion praDecisionFavorable = new REPDecisionFavorableCertificacion
                {
                    FechaIbnorca = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    ReferenciaIbnorca = praciclocronograma.Referencia,
                    NombreApellidos = nombre,
                    Cargo = cargo,
                    Organizacion = cliente.NombreRazon,
                    NbIso = normas,
                    OtorgarRenovarMantener = praciclocronograma.Referencia,
                    Etapa = praciclocronograma.Referencia,
                    NroCertificacion = "PENDIENTE"


                    /*FechadeAuditoria = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    TipodeAuditoria = praciclocronograma.Referencia,
                    ModalidaddeAuditoria = $"Días insitu: {praciclocronograma.Praciclocronogramas.First().DiasInsitu}, días remoto: {praciclocronograma.Praciclocronogramas.First().DiasRemoto}",
                    FechaInicioAuditoria = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    FechaFinAuditoria = praciclocronograma.Praciclocronogramas.First().FechaDeFinDeEjecucionAuditoria?.ToString("dd/MM/yyyy"),
                    CantidadDiasAuditor = praciclocronograma.Praciclocronogramas.First().DiasInsitu + praciclocronograma.Praciclocronogramas.First().DiasRemoto,
                    OrganismoCertificador = praprogramasdeauditorium.OrganismoCertificador,
                    CodigoDeServicioIbnorca = praprogramasdeauditorium.CodigoServicioWs,
                    Organizacion = cliente.NombreRazon,
                    AltaDireccion = contactoEmpresa?.NombreDrEncargado,
                    CargoAltaDireccion = string.Empty,
                    PersonaDeContacto = contactoEmpresa?.NombreContacto,
                    CargoPersonaDeContacto = contactoEmpresa?.CargoContacto,
                    TelefonoDeContacto = contactoEmpresa?.FonoContacto,
                    CorreoElectronico = contactoEmpresa?.CorreoContacto,
                    CodigoAIF = praprogramasdeauditorium.CodigoIafws,
                    AlcanceDeCertificacion = string.Empty,
                    HorarioHabitualDeTrabajo = praciclocronograma.Praciclocronogramas.First().HorarioTrabajo,
                    FechaProximaAuditoria = praciclocronograma.Praciclocronogramas.First().MesProgramado?.AddYears(1).ToString("dd/MM/yyyy"),
                    ListRepDesginacionParticipante = praciclocronograma.Pracicloparticipantes.Select(x =>
                    {
                        RepDesginacionParticipante repDesginacionParticipante = new RepDesginacionParticipante();
                        repDesginacionParticipante.Cargo = string.Empty;
                        if (!string.IsNullOrEmpty(x.CargoDetalleWs))
                        {
                            ListaCalificado cargo = JsonConvert.DeserializeObject<ListaCalificado>(x.CargoDetalleWs);
                            repDesginacionParticipante.Cargo = cargo.CargoPuesto;
                        }

                        repDesginacionParticipante.Participante = string.Empty;
                        if (!string.IsNullOrEmpty(x.ParticipanteDetalleWs))
                        {
                            ListaCalificado participante = JsonConvert.DeserializeObject<ListaCalificado>(x.ParticipanteDetalleWs);
                            repDesginacionParticipante.Participante = participante.NombreCompleto;
                        }

                        return repDesginacionParticipante;
                    }).ToList()*/
                };
                string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                WordHelper generadorWord = new WordHelper(filePlantilla);


                //generamos el documento en word

                string fileNameGenerado = generadorWord.GenerarDocumento(praDecisionFavorable, null, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");


                response.Message = fileNameGenerado;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public Response GenerarREPDecisionNOFavorable(int IdCiclo, string pathPlantilla, string nombreApellidos, string cargo, string otorgarRenovarMantener, string sistemaDeGestionNB, string consideracionesNumeradas, string nroCertificadoIbnorca)
        {
            Response response = new Response { Message = "", State = ResponseType.Success };
            try
            {

                ///Obtenemos la informacion del ciclo y del programa
                Praciclosprogauditorium praciclocronograma = repositoryMySql.SimpleSelect<Praciclosprogauditorium>(x => x.IdPrAcicloProgAuditoria == IdCiclo).ToList().FirstOrDefault();
                Praprogramasdeauditorium praprogramasdeauditorium = repositoryMySql.SimpleSelect<Praprogramasdeauditorium>(x => x.IdPrAprogramaAuditoria == praciclocronograma.IdPrAprogramaAuditoria).ToList().FirstOrDefault();
                if (praciclocronograma == null)
                {
                    response.State = ResponseType.Warning;
                    response.Message = "No se cuenta con informacion de este cilo en la BD";
                    return response;
                }

                praciclocronograma.Praciclocronogramas = repositoryMySql.SimpleSelect<Praciclocronograma>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Praciclonormassistemas = repositoryMySql.SimpleSelect<Praciclonormassistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pracicloparticipantes = repositoryMySql.SimpleSelect<Pracicloparticipante>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespaproductos = repositoryMySql.SimpleSelect<Pradireccionespaproducto>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespasistemas = repositoryMySql.SimpleSelect<Pradireccionespasistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);

                Cliente cliente = JsonConvert.DeserializeObject<Cliente>(praprogramasdeauditorium.OrganizacionContentWs);

                ///obtenemos los contactos del cliente
                ClientHelper clientHelper = new ClientHelper();
                ///TDO: obtenemos los datos del servicio
                RequestListarContactosEmpresa requestDato = new RequestListarContactosEmpresa { accion = "ListarContactosEmpresa", sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES, IdCliente = cliente.IdCliente };
                ResponseListarContactosEmpresa resulServices = clientHelper.Consume<ResponseListarContactosEmpresa>(Global.URIGLOBAL_SERVICES + Global.URI_CLIENTE_CONTACTO, requestDato).Result;
                if (!resulServices.estado)
                {
                    response.State = ResponseType.Warning;
                    response.Message = $"Existe problemas al consumir el servicio de ibnorca (estados): {resulServices.mensaje}";
                    return response;
                }
                ContactoEmpresa contactoEmpresa = resulServices.lstContactos?.Count > 0 ? resulServices.lstContactos[0] : null;

                string normas = "";

                praciclocronograma.Praciclonormassistemas.ToList().ForEach(x =>
                {
                    normas += x.Norma;
                });
                string alcance = "";
                praciclocronograma.Praciclonormassistemas.ToList().ForEach(x =>
                {
                    alcance += x.Alcance + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                string sitios = "";
                praciclocronograma.Pradireccionespasistemas.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                ///llenamos el reporte con la informacion de este ciclo
                REPDecisionNOFavorable praDecisionNOFavorable = new REPDecisionNOFavorable
                {

                    FechaIbnorca = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    ReferenciaIbnorca = praciclocronograma.Referencia,
                    NombreApellidos = nombreApellidos,
                    Cargo = cargo,
                    Organizacion = cliente.NombreRazon,
                    OtorgarRenovar = otorgarRenovarMantener,
                    NbIso = normas,
                    CertificacionRenovacion = praciclocronograma.Referencia,
                    SistemaDeGestionNB = sistemaDeGestionNB,
                    ConsideracionesNumeradas = consideracionesNumeradas,
                    Seguimiento = praciclocronograma.Referencia,

                    NroCertificadoIbnorca = nroCertificadoIbnorca,
                    Alcance = alcance,
                    Sitios = sitios


                };
                string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                WordHelper generadorWord = new WordHelper(filePlantilla);


                //generamos el documento en word

                string fileNameGenerado = generadorWord.GenerarDocumento(praDecisionNOFavorable, null, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");


                response.Message = fileNameGenerado;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public Response GenerarRepNotaSuspensionCertifica(int IdCiclo, string pathPlantilla, string correlativoCabecera, string nombreNota, string cargo, string texto1, string nroCertificado, string directorEjecutivo)
        {
            Response response = new Response { Message = "", State = ResponseType.Success };
            try
            {

                ///Obtenemos la informacion del ciclo y del programa
                Praciclosprogauditorium praciclocronograma = repositoryMySql.SimpleSelect<Praciclosprogauditorium>(x => x.IdPrAcicloProgAuditoria == IdCiclo).ToList().FirstOrDefault();
                Praprogramasdeauditorium praprogramasdeauditorium = repositoryMySql.SimpleSelect<Praprogramasdeauditorium>(x => x.IdPrAprogramaAuditoria == praciclocronograma.IdPrAprogramaAuditoria).ToList().FirstOrDefault();
                if (praciclocronograma == null)
                {
                    response.State = ResponseType.Warning;
                    response.Message = "No se cuenta con informacion de este cilo en la BD";
                    return response;
                }

                praciclocronograma.Praciclocronogramas = repositoryMySql.SimpleSelect<Praciclocronograma>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Praciclonormassistemas = repositoryMySql.SimpleSelect<Praciclonormassistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pracicloparticipantes = repositoryMySql.SimpleSelect<Pracicloparticipante>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespaproductos = repositoryMySql.SimpleSelect<Pradireccionespaproducto>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespasistemas = repositoryMySql.SimpleSelect<Pradireccionespasistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);

                Cliente cliente = JsonConvert.DeserializeObject<Cliente>(praprogramasdeauditorium.OrganizacionContentWs);

                ///obtenemos los contactos del cliente
                ClientHelper clientHelper = new ClientHelper();
                ///TDO: obtenemos los datos del servicio
                RequestListarContactosEmpresa requestDato = new RequestListarContactosEmpresa { accion = "ListarContactosEmpresa", sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES, IdCliente = cliente.IdCliente };
                ResponseListarContactosEmpresa resulServices = clientHelper.Consume<ResponseListarContactosEmpresa>(Global.URIGLOBAL_SERVICES + Global.URI_CLIENTE_CONTACTO, requestDato).Result;
                if (!resulServices.estado)
                {
                    response.State = ResponseType.Warning;
                    response.Message = $"Existe problemas al consumir el servicio de ibnorca (estados): {resulServices.mensaje}";
                    return response;
                }
                ContactoEmpresa contactoEmpresa = resulServices.lstContactos?.Count > 0 ? resulServices.lstContactos[0] : null;

                string normas = "";

                praciclocronograma.Praciclonormassistemas.ToList().ForEach(x =>
                {
                    normas += x.Norma;
                });
                string alcance = "";
                praciclocronograma.Praciclonormassistemas.ToList().ForEach(x =>
                {
                    alcance += x.Alcance + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                string sitios = "";
                praciclocronograma.Pradireccionespasistemas.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                ///llenamos el reporte con la informacion de este ciclo
                RepNotaSuspensionCertifica praNotaSuspensionCertifica = new RepNotaSuspensionCertifica
                {

                    FechaCabecera = DateTime.Now.ToString("dd/MM/yyyy"),
                    CorrelativoCabecera = correlativoCabecera,
                    NombreNota = nombreNota,
                    Cargo = cargo,
                    NombreEmpresa = cliente.NombreRazon,
                    ReferenciaNota = praciclocronograma.Referencia,
                    Texto1 = texto1,
                    IbnorcaRTM = praprogramasdeauditorium.CodigoServicioWs,
                    NroCertificado = nroCertificado,
                    NombreEmpresaTexto = cliente.NombreRazon,
                    DescripcionOtrogado = alcance,
                    Sitios = sitios,
                    FechaLiteral1 = DateTime.Now.ToString("dd/MM/yyyy"),
                    Seguimiento = praciclocronograma.Referencia,
                    FechaLiteral2 = DateTime.Now.ToString("dd/MM/yyyy"),
                    DirectorEjecutivo = directorEjecutivo

                };
                string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                WordHelper generadorWord = new WordHelper(filePlantilla);


                //generamos el documento en word

                string fileNameGenerado = generadorWord.GenerarDocumento(praNotaSuspensionCertifica, null, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");


                response.Message = fileNameGenerado;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public Response GenerarREPNotaDeRetiroDeCertificacion(int IdCiclo, string pathPlantilla, string correlativoCabecera, string nombreApellidos, string cargo, string nroCertificado)
        {
            Response response = new Response { Message = "", State = ResponseType.Success };
            try
            {

                ///Obtenemos la informacion del ciclo y del programa
                Praciclosprogauditorium praciclocronograma = repositoryMySql.SimpleSelect<Praciclosprogauditorium>(x => x.IdPrAcicloProgAuditoria == IdCiclo).ToList().FirstOrDefault();
                Praprogramasdeauditorium praprogramasdeauditorium = repositoryMySql.SimpleSelect<Praprogramasdeauditorium>(x => x.IdPrAprogramaAuditoria == praciclocronograma.IdPrAprogramaAuditoria).ToList().FirstOrDefault();
                if (praciclocronograma == null)
                {
                    response.State = ResponseType.Warning;
                    response.Message = "No se cuenta con informacion de este cilo en la BD";
                    return response;
                }

                praciclocronograma.Praciclocronogramas = repositoryMySql.SimpleSelect<Praciclocronograma>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Praciclonormassistemas = repositoryMySql.SimpleSelect<Praciclonormassistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pracicloparticipantes = repositoryMySql.SimpleSelect<Pracicloparticipante>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespaproductos = repositoryMySql.SimpleSelect<Pradireccionespaproducto>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespasistemas = repositoryMySql.SimpleSelect<Pradireccionespasistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);

                Cliente cliente = JsonConvert.DeserializeObject<Cliente>(praprogramasdeauditorium.OrganizacionContentWs);

                ///obtenemos los contactos del cliente
                ClientHelper clientHelper = new ClientHelper();
                ///TDO: obtenemos los datos del servicio
                RequestListarContactosEmpresa requestDato = new RequestListarContactosEmpresa { accion = "ListarContactosEmpresa", sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES, IdCliente = cliente.IdCliente };
                ResponseListarContactosEmpresa resulServices = clientHelper.Consume<ResponseListarContactosEmpresa>(Global.URIGLOBAL_SERVICES + Global.URI_CLIENTE_CONTACTO, requestDato).Result;
                if (!resulServices.estado)
                {
                    response.State = ResponseType.Warning;
                    response.Message = $"Existe problemas al consumir el servicio de ibnorca (estados): {resulServices.mensaje}";
                    return response;
                }
                ContactoEmpresa contactoEmpresa = resulServices.lstContactos?.Count > 0 ? resulServices.lstContactos[0] : null;

                string normas = "";

                praciclocronograma.Praciclonormassistemas.ToList().ForEach(x =>
                {
                    normas += x.Norma;
                });
                string alcance = "";
                praciclocronograma.Praciclonormassistemas.ToList().ForEach(x =>
                {
                    alcance += x.Alcance + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                string sitios = "";
                praciclocronograma.Pradireccionespasistemas.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                ///llenamos el reporte con la informacion de este ciclo
                REPNotaDeRetiroDeCertificacion praNotaDeRetiroDeCertificacion = new REPNotaDeRetiroDeCertificacion
                {

                    FechaIbnorca = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    CorrelativoCabecera = correlativoCabecera,
                    ReferenciaIbnorca = praciclocronograma.Referencia,
                    NombreApellidos = nombreApellidos,
                    Cargo = cargo,
                    Organizacion = cliente.NombreRazon,
                    NbIso = normas,
                    SistemaDeGestion = praciclocronograma.Referencia,
                    Alcance = alcance,
                    Sitios = sitios,
                    NroCertificadoIbnorca = nroCertificado

                };
                string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                WordHelper generadorWord = new WordHelper(filePlantilla);


                //generamos el documento en word

                string fileNameGenerado = generadorWord.GenerarDocumento(praNotaDeRetiroDeCertificacion, null, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");
                response.Message = fileNameGenerado;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

    }

}
