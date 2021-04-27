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
using Business.Main.Modules.ElaboracionAuditoria.Reportes.TCS;
using Business.Main.DataMapping.DTOs;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.ListarAuditoresxCargoCalificadoDTO;
using Resportes.ReportDTO;
using Business.Main.Modules.ElaboracionAuditoria.Reportes.TCP;
using static PlumbingProps.Document.WordHelper;
using static Resportes.ReportDTO.TCPREPInforme;
using Domain.Main.CrossEntities;

namespace Business.Main.Modules.ElaboracionAuditoria
{
    public partial class ElaboracionAuditoriaManager
    {
        #region TCP
        public ResponseObject<GlobalDataReport> TCPGenerarTCPREPNotaRetiroCertificacion(RequestDataReport requestDataReport)
        {
            ResponseObject<GlobalDataReport> response = new ResponseObject<GlobalDataReport> { Message = "", State = ResponseType.Success };
            try
            {
                int IdCiclo = requestDataReport.IdCiclo;
                //datos que no estan conectados 
                string marcaComercial = string.Empty;
                string numeroCertificado = string.Empty;

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
                TCPREPNotaRetiroCertificacion praTCPREPNotaRetiroCertificacion = new TCPREPNotaRetiroCertificacion
                {
                    Fecha = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    MarcaComercial = marcaComercial,
                    Sitio = sitios,
                    NumeroCertificado = numeroCertificado
                };
                response.Object = new GlobalDataReport { data = praTCPREPNotaRetiroCertificacion, HeadersTables = null };
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCPGenerarTCPREPNotaSuspencionCertificado(RequestDataReport requestDataReport)
        {
            ResponseObject<GlobalDataReport> response = new ResponseObject<GlobalDataReport> { Message = "", State = ResponseType.Success };
            try
            {
                int IdCiclo = requestDataReport.IdCiclo;
                //Datos no conectados
                string marcaComercial = string.Empty;
                string numeroCertificado = string.Empty;
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
                TCPREPNotaSuspencionCertificado praTCPREPNotaSuspencionCertificado = new TCPREPNotaSuspencionCertificado
                {
                    Fecha = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    MarcaComercial = marcaComercial,
                    Sitio = sitios,
                    NumeroCertificado = numeroCertificado


                };

                response.Object = new GlobalDataReport { data = praTCPREPNotaSuspencionCertificado, HeadersTables = null };
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCPGenerarTCPREPDecisionConformeReglamento(RequestDataReport requestDataReport)
        {
            ResponseObject<GlobalDataReport> response = new ResponseObject<GlobalDataReport> { Message = "", State = ResponseType.Success };
            try
            {
                int IdCiclo = requestDataReport.IdCiclo;
                //datos no conectados
                string marcaComercial = string.Empty;
                string producto = string.Empty;
                string arancel = string.Empty;
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
                TCPREPDecisionConformeReglamento praTCPREPDecisionConformeReglamento = new TCPREPDecisionConformeReglamento
                {

                    Fecha = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    NombreEmpresa = cliente.NombreRazon,
                    MarcaComercial = marcaComercial,
                    Producto = producto,
                    Arancel = arancel,
                    Sitio = sitios


                };
                response.Object = new GlobalDataReport { data = praTCPREPDecisionConformeReglamento, HeadersTables = null };
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCPGenerarTCPREPDecisionCertificacion(RequestDataReport requestDataReport)
        {
            ResponseObject<GlobalDataReport> response = new ResponseObject<GlobalDataReport> { Message = "", State = ResponseType.Success };
            try
            {
                int IdCiclo = requestDataReport.IdCiclo;
                //TDO datos no conectado 
                string numeroCertificado = string.Empty;
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
                TCPREPDecisionCertificacion praTCPREPDecisionCertificacion = new TCPREPDecisionCertificacion
                {
                    Fecha = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    TipoAuditoria = praciclocronograma.Referencia,
                    NumeroCertificado = numeroCertificado,
                    NombreEmpresa = cliente.NombreRazon,

                };
                response.Object = new GlobalDataReport { data = praTCPREPDecisionCertificacion, HeadersTables = null };
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCPGenerarTCPREPResolucionAdministrativa(RequestDataReport requestDataReport)
        {
            ResponseObject<GlobalDataReport> response = new ResponseObject<GlobalDataReport> { Message = "", State = ResponseType.Success };
            try
            {
                int IdCiclo = requestDataReport.IdCiclo;
                //TDO datos no conectados
                string acta = string.Empty;
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
                TCPREPResolucionAdministrativa praTCPREPResolucionAdministrativa = new TCPREPResolucionAdministrativa
                {
                    Acta = acta,
                    Fecha = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy")


                };
                response.Object = new GlobalDataReport { data = praTCPREPResolucionAdministrativa, HeadersTables = null };
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCPGenerarTCPREPActaReunion(RequestDataReport requestDataReport)
        {
            ResponseObject<GlobalDataReport> response = new ResponseObject<GlobalDataReport> { Message = "", State = ResponseType.Success };
            try
            {
                int IdCiclo = requestDataReport.IdCiclo;
                //TDO Datos no conectados
                string acta = string.Empty, hora = string.Empty;
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
                TCPREPActaReunion praTCPREPActaReunion = new TCPREPActaReunion
                {
                    Acta = acta,
                    Fecha = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    Modalidad = $"Días insitu: {praciclocronograma.Praciclocronogramas.First().DiasInsitu}, días remoto: {praciclocronograma.Praciclocronogramas.First().DiasRemoto}",
                    Hora = hora

                };

                response.Object = new GlobalDataReport { data = praTCPREPActaReunion, HeadersTables = null };
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCPGenerarTCPREPListaAsistencia(RequestDataReport requestDataReport)
        {
            ResponseObject<GlobalDataReport> response = new ResponseObject<GlobalDataReport> { Message = "", State = ResponseType.Success };
            try
            {
                int IdCiclo = requestDataReport.IdCiclo;
                //TODO datos no conectados
                string tipoReunion = string.Empty, cargoConcer = string.Empty, asistencia = string.Empty;
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
                TCPREPListaAsistenciaCONCER praTCPREPListaAsistencia = new TCPREPListaAsistenciaCONCER
                {
                    Fecha = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    TipoReunion = tipoReunion,
                    Nombre = cliente.NombreRazon,
                    CargoConcer = cargoConcer,
                    Asistencia = asistencia

                };
                response.Object = new GlobalDataReport { data = praTCPREPListaAsistencia, HeadersTables = null };
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCPGenerarTCPREPPlanAccion(RequestDataReport requestDataReport)
        {
            ResponseObject<GlobalDataReport> response = new ResponseObject<GlobalDataReport> { Message = "", State = ResponseType.Success };
            try
            {
                int IdCiclo = requestDataReport.IdCiclo;
                ///Todo datos no conectados
                string auditorLider = string.Empty;
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
                TCPREPPlanAccion praTCPREPPlanAccion = new TCPREPPlanAccion
                {

                    NombreEmpresa = cliente.NombreRazon,
                    TipoAuditoria = praciclocronograma.Referencia,
                    Norma = normas,
                    Fecha = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    AuditorLider = auditorLider


                };
                response.Object = new GlobalDataReport { data = praTCPREPPlanAccion, HeadersTables = null };
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCPGenerarTCPREPInforme(RequestDataReport requestDataReport)
        {
            ResponseObject<GlobalDataReport> response = new ResponseObject<GlobalDataReport> { Message = "", State = ResponseType.Success };
            try
            {
                int IdCiclo = requestDataReport.IdCiclo;
                //datos no conectados
                string fechaInicio = string.Empty;
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
                TCPREPInforme praTCPREPInforme = new TCPREPInforme
                {
                    IDCliente = cliente.NombreRazon,
                    TipoAuditoria = praciclocronograma.Referencia,
                    
                    ListProductos = praciclocronograma.Pracicloparticipantes.Select(x =>
                    {
                        TCPListProductosInforme repRep = new TCPListProductosInforme();
                        repRep.Producto = string.Empty;
                        repRep.Normas = string.Empty;

                        return repRep;
                    }).ToList(),

                    Norma = normas,
                    FechaInicio = "Desde " + fechaInicio,
                    FechaFin = " Hasta " + praciclocronograma.Praciclocronogramas.First().FechaDeFinDeEjecucionAuditoria?.ToString("dd/MM/yyyy"),

            };
                Dictionary<string, CellTitles[]> pTitles = new Dictionary<string, CellTitles[]>();
                CellTitles[] cellTitlesTitulo = new CellTitles[2];
                cellTitlesTitulo[0] = new CellTitles { Title = "Producto", Visible = true, Width = "150" };
                cellTitlesTitulo[1] = new CellTitles { Title = "Normas", Visible = true, Width = "150" };
                pTitles.Add("ListProductos", cellTitlesTitulo);

                response.Object = new GlobalDataReport { data = praTCPREPInforme, HeadersTables = pTitles };
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        #endregion

        #region TCS
        public ResponseObject<GlobalDataReport> TCSGenerarREPInformeAuditoria(RequestDataReport requestDataReport)
        {
            ResponseObject<GlobalDataReport> response = new ResponseObject<GlobalDataReport> { Message = "", State = ResponseType.Success };
            try
            {
                int IdCiclo = requestDataReport.IdCiclo;
                string objetivosAuditoria = string.Empty;
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

                string contactos = "";
                string telefono = "";
                string correoElectronico = "";

                resulServices.lstContactos.ToList().ForEach(x =>
                {
                    contactos += x.NombreContacto;
                    telefono += x.FonoContacto;
                    correoElectronico += x.CorreoContacto;
                });

                ContactoEmpresa contactoEmpresa = resulServices.lstContactos?.Count > 0 ? resulServices.lstContactos[0] : null;

                string normas = "";

                praciclocronograma.Praciclonormassistemas.ToList().ForEach(x =>
                {
                    normas += x.Norma;
                });

                string sitios = "";
                praciclocronograma.Pradireccionespasistemas.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                string direccion = "";
                string productos = "";
                praciclocronograma.Pradireccionespaproductos.ToList().ForEach(x =>
                {
                    direccion += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                    productos += x.Nombre + ", de acuerdo a la Norma Técnica " + x.Norma + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                string fechaInicio = "";
                fechaInicio = "Desde " + praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy") +
                    " Hasta " + praciclocronograma.Praciclocronogramas.First().FechaDeFinDeEjecucionAuditoria?.ToString("dd/MM/yyyy");
                ParticipanteDetalleWS auditor = new Domain.Main.CrossEntities.ParticipanteDetalleWS();
                string equipoAuditor = "";
                praciclocronograma.Pracicloparticipantes.ToList().ForEach(x =>
                {
                    //if (x.ParticipanteDetalleWs.Contains("AUDITOR LIDER"))
                    //{
                    auditor = JsonConvert.DeserializeObject<ParticipanteDetalleWS>(x.ParticipanteDetalleWs);
                    equipoAuditor = auditor.cargoPuesto + ":" + auditor.nombreCompleto + WordHelper.GetCodeKey(WordHelper.keys.enter);
                    //}
                });

                Elaauditorium elaauditorium = repositoryMySql.SimpleSelect<Elaauditorium>(x => x.IdPrAcicloProgAuditoria == IdCiclo).First();
                var contenidos = repositoryMySql.SimpleSelect<Elacontenidoauditorium>(x => x.IdelaAuditoria == elaauditorium.IdelaAuditoria);
                Elacontenidoauditorium elaContenido = new Elacontenidoauditorium();
                elaContenido = contenidos.Where(x => x.Nemotico == "PLAN_CRITERIO").First();
                string cadenaSINO = "";
                contenidos.Where(x => x.Nemotico == "INFPRE_DESVIACION").ToList().ForEach(x =>
                {
                    cadenaSINO = (x.Seleccionado == 1 ? WordHelper.GetCodeKey(WordHelper.keys.SI) : WordHelper.GetCodeKey(WordHelper.keys.NO)) + " " + x.Label + " " + x.Contenido + WordHelper.GetCodeKey(WordHelper.keys.enter);

                });

                string cadenaSINO1 = "";
                contenidos.Where(x => x.Nemotico == "INFPRE_REMOTO").ToList().ForEach(x =>
                {
                    cadenaSINO1 = (x.Seleccionado == 1 ? WordHelper.GetCodeKey(WordHelper.keys.SI) : WordHelper.GetCodeKey(WordHelper.keys.NO)) + " " + x.Label + " " + x.Contenido + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                string planMuestreo = "";
                contenidos.Where(x => x.Nemotico == "ACTAMUESTREO_PLAN").ToList().ForEach(x =>
                {
                    planMuestreo = x.Contenido;
                });
                string alcance = "";
                praciclocronograma.Praciclonormassistemas.ToList().ForEach(x =>
                {
                    alcance += x.Alcance + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });
              

                var elahallazgos = repositoryMySql.SimpleSelect<Elahallazgo>(x => x.IdelaAuditoria == elaauditorium.IdelaAuditoria);
                int nroFortaleza = 0;
                nroFortaleza = elahallazgos.Count(x => x.TipoNemotico == "F");

                int oportunidad = 0;
                oportunidad = elahallazgos.Count(x => x.TipoNemotico == "OM");

                int noConformidadMayor = 0;
                noConformidadMayor = elahallazgos.Count(x => x.TipoNemotico == "NCM");

                int noConformidadMenor = 0;
                noConformidadMenor = elahallazgos.Count(x => x.TipoNemotico == "NCm");

                var resulDBAuditoria = repositoryMySql.SimpleSelect<Elaauditorium>(x => x.IdPrAcicloProgAuditoria == IdCiclo);
                var resulDBContenidoAuditoria = repositoryMySql.SimpleSelect<Elacontenidoauditorium>(x => x.IdelaAuditoria == resulDBAuditoria.First().IdelaAuditoria);

                var resulConclusiones = resulDBContenidoAuditoria.Where(x => x.Nemotico == "INFPROD_CONCLUSION");

                //EquipoAuditoNombreCargo = null,
                ///llenamos el reporte con la informacion de este ciclo
                REPInformeAuditoria praReporte = new REPInformeAuditoria
                {
                    NombreEmpresa = cliente.NombreRazon,
                    Direccion = cliente.Direccion,
                    PersonaContacto = contactos,
                    Telefono = cliente.Telefono,
                    CorreoElectronico = cliente.Correo,
                    Servicio = praprogramasdeauditorium.CodigoServicioWs,
                    TipoAuditoria = praprogramasdeauditorium.IdparamTipoServicio.ToString(),//consultar la tabla param
                    Modalidad = praprogramasdeauditorium.DetalleServicioWs,
                    Norma = normas,
                    CodigoIAF = praprogramasdeauditorium.CodigoIafws,
                    FechaAuditoria = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    DiasAuditor = $"Días insitu: {praciclocronograma.Praciclocronogramas.First().DiasInsitu}, días remoto: {praciclocronograma.Praciclocronogramas.First().DiasRemoto}",
                    FechaInforme = DateTime.Now.ToString("dd/MM/yyyy"),
                    TipoFechaProgramada = "tipoFechaProgramada",
                    ListCronograma = praciclocronograma.Pracicloparticipantes.Select(x =>
                    {
                        RepCronograma repRepCronograma = new RepCronograma();
                        repRepCronograma.NombreCompleto = x.IdParticipanteWs.Value.ToString();
                        repRepCronograma.TotalDiasInSitu = x.DiasInsistu.ToString();
                        repRepCronograma.TotalDiasRemoto = x.DiasInsistu.ToString();

                        return repRepCronograma;
                    }).ToList(),
                    AlcanceCertificacion = alcance,

                    ListAlcanceCert = praciclocronograma.Pradireccionespasistemas.Select(x =>
                    {
                        RepSitiosAlcance repSitiosAlcance = new RepSitiosAlcance();
                        repSitiosAlcance.NombreDireccion = x.Direccion;
                        repSitiosAlcance.Auditado = WordHelper.GetCodeKey(WordHelper.keys.enter);

                        return repSitiosAlcance;
                    }).ToList(),
                    
                    Confirmacion = "confirmacion",
                    ObjetivoAuditoria = objetivosAuditoria,
                    NormasEstablecidas = normas,
                    ListResumenHallazgos = elahallazgos.Select(x =>
                    {
                        RepHallazgos repRep = new RepHallazgos();
                        repRep.Fortaleza = nroFortaleza.ToString();
                        repRep.OportunidadMejora = oportunidad.ToString();
                        repRep.NoConformidadMayor = noConformidadMayor.ToString();
                        repRep.NoConformidadMenor = noConformidadMenor.ToString();

                        return repRep;
                    }).ToList(),
                    CorreoElectronicoAuditor = "correoElectronicoAuditor",
                    NombreAuditor = "nombreAuditor",
                    NombreRepresentante = cliente.NombreRazon
                };
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCSRepInformeAuditoriaEtapaI(RequestDataReport requestDataReport)
        {
            ResponseObject<GlobalDataReport> response = new ResponseObject<GlobalDataReport> { Message = "", State = ResponseType.Success };
            try
            {
                int IdCiclo = requestDataReport.IdCiclo;
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

                string contactos = "";
                string telefono = "";
                string correoElectronico = "";

                resulServices.lstContactos.ToList().ForEach(x =>
                {
                    contactos += x.NombreContacto;
                    telefono += x.FonoContacto;
                    correoElectronico += x.CorreoContacto;
                });

                ContactoEmpresa contactoEmpresa = resulServices.lstContactos?.Count > 0 ? resulServices.lstContactos[0] : null;

                string normas = "";

                praciclocronograma.Praciclonormassistemas.ToList().ForEach(x =>
                {
                    normas += x.Norma;
                });

                string sitios = "";
                praciclocronograma.Pradireccionespasistemas.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                string direccion = "";
                string productos = "";
                praciclocronograma.Pradireccionespaproductos.ToList().ForEach(x =>
                {
                    direccion += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                    productos += x.Nombre + ", de acuerdo a la Norma Técnica " + x.Norma + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                string fechaInicio = "";
                fechaInicio = "Desde " + praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy") +
                    " Hasta " + praciclocronograma.Praciclocronogramas.First().FechaDeFinDeEjecucionAuditoria?.ToString("dd/MM/yyyy");
                ParticipanteDetalleWS auditor = new Domain.Main.CrossEntities.ParticipanteDetalleWS();
                string equipoAuditor = "";
                praciclocronograma.Pracicloparticipantes.ToList().ForEach(x =>
                {
                    //if (x.ParticipanteDetalleWs.Contains("AUDITOR LIDER"))
                    //{
                    auditor = JsonConvert.DeserializeObject<ParticipanteDetalleWS>(x.ParticipanteDetalleWs);
                    equipoAuditor = auditor.cargoPuesto + ":" + auditor.nombreCompleto + WordHelper.GetCodeKey(WordHelper.keys.enter);
                    //}
                });

                Elaauditorium elaauditorium = repositoryMySql.SimpleSelect<Elaauditorium>(x => x.IdPrAcicloProgAuditoria == IdCiclo).First();
                var contenidos = repositoryMySql.SimpleSelect<Elacontenidoauditorium>(x => x.IdelaAuditoria == elaauditorium.IdelaAuditoria);
                Elacontenidoauditorium elaContenido = new Elacontenidoauditorium();
                elaContenido = contenidos.Where(x => x.Nemotico == "PLAN_CRITERIO").First();
                string cadenaSINO = "";
                contenidos.Where(x => x.Nemotico == "INFPRE_DESVIACION").ToList().ForEach(x =>
                {
                    cadenaSINO = (x.Seleccionado == 1 ? WordHelper.GetCodeKey(WordHelper.keys.SI) : WordHelper.GetCodeKey(WordHelper.keys.NO)) + " " + x.Label + " " + x.Contenido + WordHelper.GetCodeKey(WordHelper.keys.enter);

                });

                string cadenaSINO1 = "";
                contenidos.Where(x => x.Nemotico == "INFPRE_REMOTO").ToList().ForEach(x =>
                {
                    cadenaSINO1 = (x.Seleccionado == 1 ? WordHelper.GetCodeKey(WordHelper.keys.SI) : WordHelper.GetCodeKey(WordHelper.keys.NO)) + " " + x.Label + " " + x.Contenido + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                string planMuestreo = "";
                contenidos.Where(x => x.Nemotico == "ACTAMUESTREO_PLAN").ToList().ForEach(x =>
                {
                    planMuestreo = x.Contenido;
                });

                var elahallazgos = repositoryMySql.SimpleSelect<Elahallazgo>(x => x.IdelaAuditoria == elaauditorium.IdelaAuditoria);
                int nroFortaleza = 0;
                nroFortaleza = elahallazgos.Count(x => x.TipoNemotico == "F");

                int oportunidad = 0;
                oportunidad = elahallazgos.Count(x => x.TipoNemotico == "OM");

                int noConformidadMayor = 0;
                noConformidadMayor = elahallazgos.Count(x => x.TipoNemotico == "NCM");

                int noConformidadMenor = 0;
                noConformidadMenor = elahallazgos.Count(x => x.TipoNemotico == "NCm");

                var resulDBAuditoria = repositoryMySql.SimpleSelect<Elaauditorium>(x => x.IdPrAcicloProgAuditoria == IdCiclo);
                var resulDBContenidoAuditoria = repositoryMySql.SimpleSelect<Elacontenidoauditorium>(x => x.IdelaAuditoria == resulDBAuditoria.First().IdelaAuditoria);

                var resulConclusiones = resulDBContenidoAuditoria.Where(x => x.Nemotico == "INFPROD_CONCLUSION");

                //EquipoAuditoNombreCargo = null,
                ///llenamos el reporte con la informacion de este ciclo
                RepInformeAuditoriaEtapaI praReporte = new RepInformeAuditoriaEtapaI
                {
                    NombreEmpresa = cliente.NombreRazon,
                    Direccion = cliente.Direccion,
                    PersonaContacto = contactos,
                    Telefono = telefono,
                    CorreoElectronico = correoElectronico,
                    CodigoServivio = praprogramasdeauditorium.CodigoIafws,
                    TipoAuditoria = praprogramasdeauditorium.IdparamTipoServicio.ToString(),//consultar la tabla param,
                    ModalidadAuditoria = praprogramasdeauditorium.DetalleServicioWs,
                    Normas = normas,
                    FechaAuditoria = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    FechaInforme = DateTime.Now.ToString("dd/MM/yyyy"),
                    EquipoAuditor = equipoAuditor,
                    CriteriosSistema = contenidos.Where(x => x.Nemotico == "PLAN_CRITERIO").First().ToString(),
                    ComentarioRealcionado = "ComentarioRealcionado",// comentario
                    Etapa1 = "Etapa1", //resultados de etapa I
                    AreasPreocupacion = "AreasPreocupacion",
                    ConclusionAuditor = resulConclusiones.ToString(),
                    AuditorLider = auditor.cargoPuesto,
                    NombreAuditorLider = auditor.nombreCompleto,
                    Fecha = DateTime.Now.ToString("dd/MM/yyyy"),
                    RepresentanteOrganizacion = "RepresentanteOrganizacion",
                    NombreCoordinadorAud = "NombreCoordinadorAud",
                    CorreoElectronicoCoodinador = "CorreoElectronicoCoodinador"
                };
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCSGenerarREPListaVerificacionReunionApertura(RequestDataReport requestDataReport)
        {
            ResponseObject<GlobalDataReport> response = new ResponseObject<GlobalDataReport> { Message = "", State = ResponseType.Success };
            try
            {
                int IdCiclo = requestDataReport.IdCiclo;
                string fechaInicio = string.Empty, fechaFin = string.Empty, nombreYFirmaDeAuditorLider = string.Empty;
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
                response.Object = new GlobalDataReport { data = praListaVerificacionReunionApertura, HeadersTables = null };
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCSGenerarREPListaVerificacionReunionCierre(RequestDataReport requestDataReport)
        {
            ResponseObject<GlobalDataReport> response = new ResponseObject<GlobalDataReport> { Message = "", State = ResponseType.Success };
            try
            {
                int IdCiclo = requestDataReport.IdCiclo;
                string nombreYFirmaDeAuditorLider = string.Empty;
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
                response.Object = new GlobalDataReport { data = praListaVerificacionReunionCierre, HeadersTables = null };
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCSGenerarREPListaAsistencia(RequestDataReport requestDataReport)
        {
            ResponseObject<GlobalDataReport> response = new ResponseObject<GlobalDataReport> { Message = "", State = ResponseType.Success };
            try
            {
                int IdCiclo = requestDataReport.IdCiclo;
                string fechaInicio = string.Empty, fechaFin = string.Empty;
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
                response.Object = new GlobalDataReport { data = praListaAsistencia, HeadersTables = null };
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCSGenerarREPListaVerificacionAuditor(RequestDataReport requestDataReport)
        {
            ResponseObject<GlobalDataReport> response = new ResponseObject<GlobalDataReport> { Message = "", State = ResponseType.Success };
            try
            {
                int IdCiclo = requestDataReport.IdCiclo;
                string usuario = string.Empty, cargo = string.Empty, procesoAuditado = string.Empty, nombreAuditado = string.Empty;
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
                response.Object = new GlobalDataReport { data = praListaVerificacionAuditor, HeadersTables = null };

            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCSGenerarREPLIstaVerificacionBPM(RequestDataReport requestDataReport)
        {
            ResponseObject<GlobalDataReport> response = new ResponseObject<GlobalDataReport> { Message = "", State = ResponseType.Success };
            try
            {
                int IdCiclo = requestDataReport.IdCiclo;
                string nombreYApellidos = string.Empty, responsabilidadORol = string.Empty;
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
                response.Object = new GlobalDataReport { data = praLIstaVerificacionBPM, HeadersTables = null };
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCSGenerarREPDatosDeLaOrganizacion(RequestDataReport requestDataReport)
        {
            ResponseObject<GlobalDataReport> response = new ResponseObject<GlobalDataReport> { Message = "", State = ResponseType.Success };
            try
            {
                int IdCiclo = requestDataReport.IdCiclo;
                string nombreAuditorLider = string.Empty, nombreCargoRepresentanteOrg = string.Empty, coordinadorAuditoria = string.Empty;
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
                response.Object = new GlobalDataReport { data = praDatosDeLaOrganizacion, HeadersTables = null };
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCSGenerarREPEvaluacionYRecomendacionesParaProceso(RequestDataReport requestDataReport)
        {
            ResponseObject<GlobalDataReport> response = new ResponseObject<GlobalDataReport> { Message = "", State = ResponseType.Success };
            try
            {
                int IdCiclo = requestDataReport.IdCiclo;
                string expertoCertificacion = string.Empty, fechaAsignacionProceso = string.Empty, redaccionSugerida = string.Empty;
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

                //generamos el documento en word
                Dictionary<string, CellTitles[]> pTitles = new Dictionary<string, CellTitles[]>();
                WordHelper.CellTitles[] cellTitlesTitulo = new CellTitles[2];
                cellTitlesTitulo[0] = new CellTitles { Title = "Calificación", Visible = true, Width = "50" };
                cellTitlesTitulo[1] = new CellTitles { Title = "Auditor", Visible = true, Width = "50" };
                pTitles.Add("ListEquipoAuditoNombreCargo", cellTitlesTitulo);
                response.Object = new GlobalDataReport { data = praEvaluacionYRecomendacionesParaProceso, HeadersTables = pTitles };

            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCSGenerarDescisionFavorableCertificacion(RequestDataReport requestDataReport)
        {
            ResponseObject<GlobalDataReport> response = new ResponseObject<GlobalDataReport> { Message = "", State = ResponseType.Success };
            try
            {
                int IdCiclo = requestDataReport.IdCiclo;
                string nombre = string.Empty, cargo = string.Empty;
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
                response.Object = new GlobalDataReport { data = praDecisionFavorable, HeadersTables = null };
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCSGenerarREPDecisionNOFavorable(RequestDataReport requestDataReport)
        {
            ResponseObject<GlobalDataReport> response = new ResponseObject<GlobalDataReport> { Message = "", State = ResponseType.Success };
            try
            {
                int IdCiclo = requestDataReport.IdCiclo;
                string pathPlantilla = string.Empty, nombreApellidos = string.Empty, cargo = string.Empty, otorgarRenovarMantener = string.Empty, sistemaDeGestionNB = string.Empty, consideracionesNumeradas = string.Empty, nroCertificadoIbnorca = string.Empty;
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
                response.Object = new GlobalDataReport { data = praDecisionNOFavorable, HeadersTables = null };

            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCSGenerarREPNotaDeRetiroDeCertificacion(RequestDataReport requestDataReport)
        {
            ResponseObject<GlobalDataReport> response = new ResponseObject<GlobalDataReport> { Message = "", State = ResponseType.Success };
            try
            {
                int IdCiclo = requestDataReport.IdCiclo;
                string correlativoCabecera = string.Empty, nombreApellidos = string.Empty, cargo = string.Empty, nroCertificado = string.Empty;
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
                response.Object = new GlobalDataReport { data = praNotaDeRetiroDeCertificacion, HeadersTables = null };
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        #endregion
    }

}
