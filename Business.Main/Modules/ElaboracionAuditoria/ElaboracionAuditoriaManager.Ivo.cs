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
using static Business.Main.Modules.ElaboracionAuditoria.Reportes.TCP.TCPRepOfertaContrato;
using static Business.Main.Modules.ElaboracionAuditoria.Reportes.TCS.TCSRepOfertaContrato;
using Business.Main.Modules.ElaboracionAuditoria.DTO;

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
                    Fecha = DateTime.Now.ToString("dd/MM/yyyy"),
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
                    Fecha = DateTime.Now.ToString("dd/MM/yyyy"),
                    MarcaComercial = marcaComercial,
                    Sitio = sitios,
                    NumeroCertificado = numeroCertificado,
                    NombreEmpresa = ""

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

                    Fecha = DateTime.Now.ToString("dd/MM/yyyy"),
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
                praprogramasdeauditorium.Praciclosprogauditoria = repositoryMySql.SimpleSelect<Praciclosprogauditorium>(y => y.IdPrAprogramaAuditoria == praprogramasdeauditorium.IdPrAprogramaAuditoria);

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
                    Fecha = DateTime.Now.ToString("dd/MM/yyyy"),
                    TipoAuditoria = praciclocronograma.Referencia,
                    NumeroCertificado = numeroCertificado,
                    NombreEmpresa = cliente.NombreRazon,
                    ListProductos = new List<TCPREPDecisionCertificacion.ProductoDecision>(),
                    ListCalendario = new List<TCPREPDecisionCertificacion.Calendario>()
                };

                int cont = 1;
                praciclocronograma.Pradireccionespaproductos.ToList().ForEach(x =>
                {
                    TCPREPDecisionCertificacion.ProductoDecision prod = new TCPREPDecisionCertificacion.ProductoDecision
                    {
                        Norma = x.Norma,
                        Nro = cont,
                        Producto = x.Nombre
                    };
                    praTCPREPDecisionCertificacion.ListProductos.Add(prod);
                    cont++;
                });

                praprogramasdeauditorium.Praciclosprogauditoria.ToList().Where(h => h.Anio > praciclocronograma.Anio).ToList().ForEach(z =>
                {
                    z.Praciclocronogramas = repositoryMySql.SimpleSelect<Praciclocronograma>(u => u.IdPrAcicloProgAuditoria == z.IdPrAcicloProgAuditoria);
                    praTCPREPDecisionCertificacion.ListCalendario.Add(new TCPREPDecisionCertificacion.Calendario
                    {
                        Fecha = z.Praciclocronogramas.First().MesProgramado?.ToString("MM/yyyy"),
                        Proceso = z.Referencia
                    });
                });

                Dictionary<string, CellTitles[]> pTitles = new Dictionary<string, CellTitles[]>();
                CellTitles[] cellTitlesTituloEquipoAuditor = new CellTitles[3];
                cellTitlesTituloEquipoAuditor[0] = new CellTitles { Title = "Nro", Visible = true, Width = "50" };
                cellTitlesTituloEquipoAuditor[1] = new CellTitles { Title = "Producto", Visible = true, Width = "120" };
                cellTitlesTituloEquipoAuditor[2] = new CellTitles { Title = "Norma", Visible = true, Width = "50" };
                pTitles.Add("ListProductos", cellTitlesTituloEquipoAuditor);

                CellTitles[] cellTitlesCalendario = new CellTitles[2];
                cellTitlesCalendario[0] = new CellTitles { Title = "Proceso", Visible = true, Width = "120" };
                cellTitlesCalendario[1] = new CellTitles { Title = "Fecha", Visible = true, Width = "50" };

                pTitles.Add("ListCalendario", cellTitlesCalendario);

                response.Object = new GlobalDataReport { data = praTCPREPDecisionCertificacion, HeadersTables = pTitles };
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
                    Fecha = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    ListaProductosResolucion = new List<ProductosResolucion>()
                };
                Dictionary<string, CellTitles[]> pTitles = new Dictionary<string, CellTitles[]>();
                CellTitles[] cellTitlesTitulo = new CellTitles[5];
                cellTitlesTitulo[0] = new CellTitles { Title = "Proceso", Visible = true, Width = "50" };
                cellTitlesTitulo[1] = new CellTitles { Title = "Producto", Visible = true, Width = "80" };
                cellTitlesTitulo[2] = new CellTitles { Title = "Norma", Visible = true, Width = "50" };
                cellTitlesTitulo[3] = new CellTitles { Title = "Empresa", Visible = true, Width = "50" };
                cellTitlesTitulo[4] = new CellTitles { Title = "Ubicacion", Visible = true, Width = "50" };

                pTitles.Add("ListaProductosResolucion", cellTitlesTitulo);

                response.Object = new GlobalDataReport { data = praTCPREPResolucionAdministrativa, HeadersTables = pTitles };
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
                    Hora = hora,
                    ListaTemario = new List<TCPTemario>()
                };
                Dictionary<string, CellTitles[]> pTitles = new Dictionary<string, CellTitles[]>();
                CellTitles[] cellTitlesTitulo = new CellTitles[9];
                cellTitlesTitulo[0] = new CellTitles { Title = "No", Visible = true, Width = "30" };
                cellTitlesTitulo[1] = new CellTitles { Title = "PROCESO", Visible = true, Width = "30" };
                cellTitlesTitulo[2] = new CellTitles { Title = "COD. SERVICIO", Visible = true, Width = "30" };
                cellTitlesTitulo[3] = new CellTitles { Title = "Nº EMPRESA", Visible = true, Width = "30" };
                cellTitlesTitulo[4] = new CellTitles { Title = "PRODUCTO", Visible = true, Width = "30" };
                cellTitlesTitulo[5] = new CellTitles { Title = "NORMA", Visible = true, Width = "30" };
                cellTitlesTitulo[6] = new CellTitles { Title = "DESCRIPCIÓN DE LA REVISIÓN", Visible = true, Width = "50" };
                cellTitlesTitulo[7] = new CellTitles { Title = "CONFIRMACIÓN DEL PRODUCTO", Visible = true, Width = "50" };
                cellTitlesTitulo[8] = new CellTitles { Title = "RECOMENDACIÓN DEL CONCER", Visible = true, Width = "50" };
                pTitles.Add("ListaTemario", cellTitlesTitulo);

                response.Object = new GlobalDataReport { data = praTCPREPActaReunion, HeadersTables = pTitles };
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
                    Asistencia = asistencia,
                    ListAsistente = new List<Asistente>()
                };
                Dictionary<string, CellTitles[]> pTitles = new Dictionary<string, CellTitles[]>();
                CellTitles[] cellTitlesTitulo = new CellTitles[3];
                cellTitlesTitulo[0] = new CellTitles { Title = "Nombre y Apellido", Visible = true, Width = "150" };
                cellTitlesTitulo[1] = new CellTitles { Title = "Cargo de la directiva del CONCER", Visible = true, Width = "100" };
                cellTitlesTitulo[2] = new CellTitles { Title = "Asistencia", Visible = true, Width = "50" };
                pTitles.Add("ListAsistente", cellTitlesTitulo);
                response.Object = new GlobalDataReport { data = praTCPREPListaAsistencia, HeadersTables = pTitles };
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

                praciclocronograma.Pradireccionespaproductos.ToList().ForEach(x =>
                {
                    normas += x.Norma + ",";
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

                Elaauditorium elaauditorium = repositoryMySql.SimpleSelect<Elaauditorium>(x => x.IdPrAcicloProgAuditoria == IdCiclo).First();
                var elahallazgos = repositoryMySql.SimpleSelect<Elahallazgo>(x => x.IdelaAuditoria == elaauditorium.IdelaAuditoria);
                int cont = 1;
                string fecha = $"{praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy")} a {praciclocronograma.Praciclocronogramas.First().FechaDeFinDeEjecucionAuditoria?.ToString("dd/MM/yyyy")}";

                ListaCalificado auditoriLider = new ListaCalificado();
                if (praciclocronograma.Pracicloparticipantes.Any(x => x.ParticipanteDetalleWs.ToLower().Contains("lider")))
                {
                    auditoriLider = JsonConvert.DeserializeObject<ListaCalificado>(praciclocronograma.Pracicloparticipantes.First(x => x.ParticipanteDetalleWs.ToLower().Contains("lider")).ParticipanteDetalleWs);
                }

                ///llenamos el reporte con la informacion de este ciclo
                TCPREPPlanAccion praTCPREPPlanAccion = new TCPREPPlanAccion
                {

                    NombreEmpresa = cliente.NombreRazon,
                    TipoAuditoria = praciclocronograma.Referencia,
                    Norma = normas,
                    Fecha = fecha,
                    AuditorLider = auditoriLider.NombreCompleto,
                    ListHallazgosPAC = elahallazgos.Where(x => x.TipoNemotico == "NCM" || x.TipoNemotico == "NCm").Select(yy =>
                    {
                        HallazgosPAC objHallazgoPAC = new HallazgosPAC
                        {
                            nro = cont,
                            TipoHallazgo = yy.Tipo,
                            Descripcion = $"{yy.Proceso} {yy.Hallazgo}",
                            PresentaEvidencia = "SI/NO",
                            Verificacion = "SI/NO",
                            AccionesCorrectivas = "SI/NO"
                        };
                        cont++;
                        return objHallazgoPAC;
                    }
                    ).ToList()
                };

                Dictionary<string, CellTitles[]> pTitles = new Dictionary<string, CellTitles[]>();
                CellTitles[] cellTitlesTitulo = new CellTitles[11];
                cellTitlesTitulo[0] = new CellTitles { Title = "Nro", Visible = true, Width = "20" };
                cellTitlesTitulo[1] = new CellTitles { Title = "Tipo de Hallazgo", Visible = true, Width = "30" };
                cellTitlesTitulo[2] = new CellTitles { Title = "Descripción de la No conformidad", Visible = true, Width = "30" };
                cellTitlesTitulo[3] = new CellTitles { Title = "Corrección", Visible = true, Width = "30" };
                cellTitlesTitulo[4] = new CellTitles { Title = "Análisis de Causa", Visible = true, Width = "30" };
                cellTitlesTitulo[5] = new CellTitles { Title = "Acciones correctivas", Visible = true, Width = "30" };
                cellTitlesTitulo[6] = new CellTitles { Title = "Fecha de cumplimiento de acciones", Visible = true, Width = "30" };
                cellTitlesTitulo[7] = new CellTitles { Title = "Presenta Evidencia", Visible = true, Width = "30" };
                cellTitlesTitulo[8] = new CellTitles { Title = "Comentarios del auditor sobre las acciones planteadas (colocar despues del comentario la fecha)", Visible = true, Width = "30" };
                cellTitlesTitulo[9] = new CellTitles { Title = "Verificación de acciones (NCM)", Visible = true, Width = "30" };
                cellTitlesTitulo[10] = new CellTitles { Title = "Aprobación de las correcciones y acciones correctivas", Visible = true, Width = "30" };


                pTitles.Add("ListHallazgosPAC", cellTitlesTitulo);

                response.Object = new GlobalDataReport { data = praTCPREPPlanAccion, HeadersTables = pTitles };
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCSGenerarTCSREPPlanAccion(RequestDataReport requestDataReport)
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

                Elaauditorium elaauditorium = repositoryMySql.SimpleSelect<Elaauditorium>(x => x.IdPrAcicloProgAuditoria == IdCiclo).First();
                var elahallazgos = repositoryMySql.SimpleSelect<Elahallazgo>(x => x.IdelaAuditoria == elaauditorium.IdelaAuditoria);
                int cont = 1;

                ListaCalificado auditoriLider = new ListaCalificado();
                if (praciclocronograma.Pracicloparticipantes.Any(x => x.ParticipanteDetalleWs.ToLower().Contains("lider")))
                {
                    auditoriLider = JsonConvert.DeserializeObject<ListaCalificado>(praciclocronograma.Pracicloparticipantes.First(x => x.ParticipanteDetalleWs.ToLower().Contains("lider")).ParticipanteDetalleWs);
                    auditorLider = auditoriLider?.NombreCompleto;
                }

                string fechaAuditoria = $"{praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy")} a {praciclocronograma.Praciclocronogramas.First().FechaDeFinDeEjecucionAuditoria?.ToString("dd/MM/yyyy")}";
                ///llenamos el reporte con la informacion de este ciclo
                TCSREPPlanAccion praTCPREPPlanAccion = new TCSREPPlanAccion
                {

                    NombreEmpresa = cliente.NombreRazon,
                    TipoAuditoria = praciclocronograma.Referencia,
                    Norma = normas,
                    Fecha = fechaAuditoria,
                    AuditorLider = auditorLider,
                    ListHallazgosPAC = elahallazgos.Where(x => x.TipoNemotico == "NCM" || x.TipoNemotico == "NCm").Select(yy =>
                    {
                        HallazgosPACTCS objHallazgoPAC = new HallazgosPACTCS
                        {
                            nro = cont,
                            TipoHallazgo = yy.Tipo,
                            Descripcion = $"{yy.Proceso} {yy.Hallazgo}",
                            PresentaEvidencia = "SI/NO",
                            Verificacion = "SI/NO",
                            AccionesCorrectivas = "SI/NO"
                        };
                        cont++;
                        return objHallazgoPAC;
                    }
                    ).ToList()
                };

                Dictionary<string, CellTitles[]> pTitles = new Dictionary<string, CellTitles[]>();
                CellTitles[] cellTitlesTitulo = new CellTitles[12];
                cellTitlesTitulo[0] = new CellTitles { Title = "Nro", Visible = true, Width = "20" };
                cellTitlesTitulo[1] = new CellTitles { Title = "Tipo de Hallazgo", Visible = true, Width = "30" };
                cellTitlesTitulo[2] = new CellTitles { Title = "Descripción de la No conformidad", Visible = true, Width = "30" };
                cellTitlesTitulo[3] = new CellTitles { Title = "Sitio involucrado", Visible = true, Width = "30" };
                cellTitlesTitulo[4] = new CellTitles { Title = "Corrección", Visible = true, Width = "30" };
                cellTitlesTitulo[5] = new CellTitles { Title = "Análisis de Causa", Visible = true, Width = "30" };
                cellTitlesTitulo[6] = new CellTitles { Title = "Acciones correctivas", Visible = true, Width = "30" };
                cellTitlesTitulo[7] = new CellTitles { Title = "Fecha de cumplimiento de acciones", Visible = true, Width = "30" };
                cellTitlesTitulo[8] = new CellTitles { Title = "Presenta Evidencia", Visible = true, Width = "30" };
                cellTitlesTitulo[9] = new CellTitles { Title = "Comentarios del auditor sobre las acciones planteadas (colocar despues del comentario la fecha)", Visible = true, Width = "30" };
                cellTitlesTitulo[10] = new CellTitles { Title = "Verificación de acciones (NCM)", Visible = true, Width = "30" };
                cellTitlesTitulo[11] = new CellTitles { Title = "Aprobación de las correcciones y acciones correctivas", Visible = true, Width = "30" };


                pTitles.Add("ListHallazgosPAC", cellTitlesTitulo);

                response.Object = new GlobalDataReport { data = praTCPREPPlanAccion, HeadersTables = pTitles };
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


                string productos = "";
                praciclocronograma.Pradireccionespaproductos.ToList().ForEach(x =>
                {
                    normas += x.Norma + WordHelper.GetCodeKey(WordHelper.keys.enter);
                    productos += x.Nombre + ",";
                });


                string sitios = "";
                praciclocronograma.Pradireccionespasistemas.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                string planMuestreo = "";
                Elaauditorium elaauditorium = repositoryMySql.SimpleSelect<Elaauditorium>(x => x.IdPrAcicloProgAuditoria == IdCiclo).First();
                var contenidos = repositoryMySql.SimpleSelect<Elacontenidoauditorium>(x => x.IdelaAuditoria == elaauditorium.IdelaAuditoria);
                contenidos.Where(x => x.Nemotico == "ACTAMUESTREO_PLAN").ToList().ForEach(x =>
                {
                    planMuestreo = x.Contenido.Replace("\n", WordHelper.GetCodeKey(WordHelper.keys.enter));
                });

                string criterios = "";
                contenidos.Where(x => x.Nemotico == "PLAN_CRITERIO").ToList().ForEach(x =>
                {
                    criterios = x.Contenido.Replace("\n", WordHelper.GetCodeKey(WordHelper.keys.enter));
                });
                var elahallazgos = repositoryMySql.SimpleSelect<Elahallazgo>(x => x.IdelaAuditoria == elaauditorium.IdelaAuditoria);

                string sHallazgosNCMe = string.Empty;
                elahallazgos.Where(x => x.TipoNemotico == "NCm").ToList().ForEach(x =>
                {
                    sHallazgosNCMe += $"{x.Proceso} {WordHelper.GetCodeKey(WordHelper.keys.enter)}  {x.Hallazgo.Replace("\n", WordHelper.GetCodeKey(WordHelper.keys.enter))} {WordHelper.GetCodeKey(WordHelper.keys.enter)}";

                });

                string sHallazgosNCM = string.Empty;
                elahallazgos.Where(x => x.TipoNemotico == "NCM").ToList().ForEach(x =>
                {
                    sHallazgosNCM += $"{x.Proceso} {WordHelper.GetCodeKey(WordHelper.keys.enter)}  {x.Hallazgo.Replace("\n", WordHelper.GetCodeKey(WordHelper.keys.enter))} {WordHelper.GetCodeKey(WordHelper.keys.enter)}";

                });

                ///llenamos el reporte con la informacion de este ciclo
                TCPREPInforme praTCPREPInforme = new TCPREPInforme
                {
                    IDCliente = cliente.NombreRazon,
                    TipoAuditoria = praciclocronograma.Referencia,

                    ListProductos = praciclocronograma.Pradireccionespaproductos.Select(x =>
                    {
                        TCPListProductosInforme repRep = new TCPListProductosInforme();
                        repRep.Producto = x.Nombre;
                        return repRep;
                    }).ToList(),
                    PlanMuestreo = planMuestreo,
                    Criterios = criterios,
                    Normas = normas,
                    NoConformidadMayor = sHallazgosNCM,
                    NoConformidadMenor = sHallazgosNCMe,
                    Productos = productos,
                    FechaInicio = "Desde " + praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    FechaFin = praciclocronograma.Praciclocronogramas.First().FechaDeFinDeEjecucionAuditoria?.ToString("dd/MM/yyyy"),

                };
                Dictionary<string, CellTitles[]> pTitles = new Dictionary<string, CellTitles[]>();
                CellTitles[] cellTitlesTitulo = new CellTitles[1];
                cellTitlesTitulo[0] = new CellTitles { Title = "Producto", Visible = true, Width = "150" };
                pTitles.Add("ListProductos", cellTitlesTitulo);

                response.Object = new GlobalDataReport { data = praTCPREPInforme, HeadersTables = pTitles };
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCPRepOfertaContrato(RequestDataReport requestDataReport)
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


                string productos = "";
                praciclocronograma.Pradireccionespaproductos.ToList().ForEach(x =>
                {
                    normas += x.Norma + WordHelper.GetCodeKey(WordHelper.keys.enter);
                    productos += x.Nombre + ",";
                });


                string sitios = "";
                praciclocronograma.Pradireccionespasistemas.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                ///llenamos el reporte con la informacion de este ciclo
                TCPRepOfertaContrato praTCPREPInforme = new TCPRepOfertaContrato
                {
                    Referencia = cliente.NombreRazon,
                    FechaIbnorca = praciclocronograma.Referencia,
                    Cliente = cliente.NombreRazon, //TODO: Completar
                    DireccionCliente = cliente.Direccion, //TODO: Completar
                    Guia = normas,
                    NombreGerente = "", //TODO: Completar
                    mailIbnorca = "", //TODO: Completar

                    //TODO: Completar Pradireccionespaproductos
                    ListProductos = praciclocronograma.Pradireccionespaproductos.Select(x =>
                    {
                        ProductoOferta repRep = new ProductoOferta();
                        repRep.Nombre = x.Nombre;
                        repRep.Marca = x.Marca;
                        repRep.Norma = x.Norma;
                        repRep.NroSello = x.Sello;
                        repRep.Direccion = x.Direccion;
                        return repRep;
                    }).ToList(),
                    //TODO: Completar Pradireccionespaproductos
                    ListPresupuesto = praciclocronograma.Pradireccionespaproductos.Select(x =>
                    {
                        PresupuestoOfertaTCP repRep = new PresupuestoOfertaTCP();
                        repRep.Etapa = ""; //TODO: Completar
                        repRep.Concepto = ""; //TODO: Completar
                        repRep.DiasAuditor = ""; //TODO: Completar
                        repRep.CostoUSD = ""; //TODO: Completar
                        return repRep;
                    }).ToList(),

                };

                //praTCPREPInforme.ListPresupuesto = null;
                //praTCPREPInforme.ListProductos = null;

                Dictionary<string, CellTitles[]> pTitles = new Dictionary<string, CellTitles[]>();
                CellTitles[] cellTitlesTitulo = new CellTitles[5];
                cellTitlesTitulo[0] = new CellTitles { Title = "Producto", Visible = true, Width = "100" };
                cellTitlesTitulo[1] = new CellTitles { Title = "Marca", Visible = true, Width = "100" };
                cellTitlesTitulo[2] = new CellTitles { Title = "Norma", Visible = true, Width = "100" };
                cellTitlesTitulo[3] = new CellTitles { Title = "Sello", Visible = true, Width = "100" };
                cellTitlesTitulo[4] = new CellTitles { Title = "Direccion", Visible = true, Width = "100" };
                pTitles.Add("ListProductos", cellTitlesTitulo);

                CellTitles[] cellTitlesTitulo2 = new CellTitles[4];
                cellTitlesTitulo2[0] = new CellTitles { Title = "Etapa", Visible = true, Width = "150" };
                cellTitlesTitulo2[1] = new CellTitles { Title = "Concepto", Visible = true, Width = "80" };
                cellTitlesTitulo2[2] = new CellTitles { Title = "DiasAuditoro", Visible = true, Width = "80" };
                cellTitlesTitulo2[3] = new CellTitles { Title = "CostoUSD", Visible = true, Width = "80" };
                pTitles.Add("ListPresupuesto", cellTitlesTitulo2);

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
                praprogramasdeauditorium.Praciclosprogauditoria = repositoryMySql.SimpleSelect<Praciclosprogauditorium>(y => y.IdPrAprogramaAuditoria == praprogramasdeauditorium.IdPrAprogramaAuditoria);

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

                if (contenidos.Where(x => x.Nemotico == "PLAN_CRITERIO").Count() > 0)
                    elaContenido = contenidos.Where(x => x.Nemotico == "PLAN_CRITERIO").FirstOrDefault();


                //elaContenido = contenidos.Where(x => x.Nemotico == "PLAN_CRITERIO").First();
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

                contenidos.Where(x => x.Nemotico == "PLAN_OBJETIVOS").ToList().ForEach(x =>
                {
                    objetivosAuditoria = x.Contenido.Replace("\n", WordHelper.GetCodeKey(WordHelper.keys.enter));
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

                string modalidad = praciclocronograma.Praciclocronogramas.First().DiasRemoto > 0 && praciclocronograma.Praciclocronogramas.First().DiasInsitu > 0 ? "In Situ y Remota" : "";
                modalidad = string.IsNullOrEmpty(modalidad) && praciclocronograma.Praciclocronogramas.First().DiasRemoto > 0 ? "Remota" : "";
                modalidad = string.IsNullOrEmpty(modalidad) && praciclocronograma.Praciclocronogramas.First().DiasInsitu > 0 ? "In Situ" : "";

                string fechaAuditoria = $"{praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy")} a {praciclocronograma.Praciclocronogramas.First().FechaDeFinDeEjecucionAuditoria?.ToString("dd/MM/yyyy")}";
                string tipoAuditoriaProx = string.Empty;
                string fechaProx = string.Empty;
                if (praprogramasdeauditorium.Praciclosprogauditoria.Any(x => x.Anio == praciclocronograma.Anio + 1))
                {
                    var cicloProx = praprogramasdeauditorium.Praciclosprogauditoria.First(x => x.Anio == praciclocronograma.Anio + 1);
                    tipoAuditoriaProx = cicloProx.Referencia;
                    cicloProx.Praciclocronogramas = repositoryMySql.SimpleSelect<Praciclocronograma>(x => x.IdPrAcicloProgAuditoria == cicloProx.IdPrAcicloProgAuditoria);
                    fechaProx = cicloProx.Praciclocronogramas.First().MesProgramado?.ToString("MM/yyyy"); ;
                }


                string sHallazgosNCM = string.Empty;
                elahallazgos.Where(x => x.TipoNemotico == "NCM").ToList().ForEach(x =>
                {
                    sHallazgosNCM += $"{x.Proceso} {WordHelper.GetCodeKey(WordHelper.keys.enter)}  {x.Hallazgo.Replace("\n", WordHelper.GetCodeKey(WordHelper.keys.enter))} {WordHelper.GetCodeKey(WordHelper.keys.enter)}";

                });
                string sHallazgosOM = string.Empty;
                elahallazgos.Where(x => x.TipoNemotico == "OM").ToList().ForEach(x =>
                {
                    sHallazgosOM += $"{x.Proceso} {WordHelper.GetCodeKey(WordHelper.keys.enter)}  {x.Hallazgo.Replace("\n", WordHelper.GetCodeKey(WordHelper.keys.enter))} {WordHelper.GetCodeKey(WordHelper.keys.enter)}";

                });
                string sHallazgosNCMe = string.Empty;
                elahallazgos.Where(x => x.TipoNemotico == "NCm").ToList().ForEach(x =>
                {
                    sHallazgosNCMe += $"{x.Proceso} {WordHelper.GetCodeKey(WordHelper.keys.enter)}  {x.Hallazgo.Replace("\n", WordHelper.GetCodeKey(WordHelper.keys.enter))} {WordHelper.GetCodeKey(WordHelper.keys.enter)}";

                });
                string sHallazgosF = string.Empty;
                elahallazgos.Where(x => x.TipoNemotico == "F").ToList().ForEach(x =>
                {
                    sHallazgosF += $"{x.Hallazgo} {WordHelper.GetCodeKey(WordHelper.keys.enter)}";

                });
                string sHallazgosC = string.Empty;
                elahallazgos.Where(x => x.Tipo == "Conformidades").ToList().ForEach(x =>
                {
                    sHallazgosC += $"Conformidades:  {x.Hallazgo} {WordHelper.GetCodeKey(WordHelper.keys.enter)}";

                });

                REPInformeAuditoria praReporte = new REPInformeAuditoria
                {
                    NombreEmpresa = cliente.NombreRazon,
                    RedaccionFortalezas = sHallazgosF,
                    RedaccionNoConformidadesMayor = sHallazgosNCM,
                    RedaccionNoConformidadesMenor = sHallazgosNCMe,
                    RedaccionOportunidades = sHallazgosOM,
                    Direccion = cliente.Direccion,
                    PersonaContacto = contactos,
                    Telefono = cliente.Telefono,
                    CorreoElectronico = cliente.Correo,
                    Servicio = praprogramasdeauditorium.CodigoServicioWs,
                    TipoAuditoria = praciclocronograma.Referencia,
                    Modalidad = modalidad,
                    Norma = normas,
                    CodigoIAF = praprogramasdeauditorium.CodigoIafws,
                    FechaAuditoria = fechaAuditoria,
                    DiasAuditor = $"Días insitu: {praciclocronograma.Praciclocronogramas.First().DiasInsitu}, días remoto: {praciclocronograma.Praciclocronogramas.First().DiasRemoto}",
                    FechaInforme = DateTime.Now.ToString("dd/MM/yyyy"),
                    TipoFechaProgramada = tipoAuditoriaProx + " " + fechaProx,
                    ListCronograma = praciclocronograma.Pracicloparticipantes.Select(x =>
                    {
                        RepCronograma repRepCronograma = new RepCronograma();
                        var participante = JsonConvert.DeserializeObject<ParticipanteDetalleWS>(x.ParticipanteDetalleWs);
                        repRepCronograma.NombreCompleto = participante?.nombreCompleto;
                        repRepCronograma.TotalDiasInSitu = x.DiasInsistu.ToString();
                        repRepCronograma.TotalDiasRemoto = x.DiasRemoto.ToString();
                        return repRepCronograma;
                    }).ToList(),
                    AlcanceCertificacion = alcance,
                    ListAlcanceCert = praciclocronograma.Pradireccionespasistemas.Select(x =>
                    {
                        RepSitiosAlcance repSitiosAlcance = new RepSitiosAlcance();
                        repSitiosAlcance.NombreDireccion = x.Direccion;
                        repSitiosAlcance.Auditado = "SI/NO";
                        return repSitiosAlcance;
                    }).ToList(),
                    Confirmacion = "confirmacion",
                    ObjetivoAuditoria = objetivosAuditoria,
                    NormasEstablecidas = normas,
                    ListResumenHallazgos = new List<RepHallazgos>(),
                    CorreoElectronicoAuditor = "correoElectronicoAuditor",
                    NombreAuditor = "nombreAuditor",
                    NombreRepresentante = cliente.NombreRazon
                };
                RepHallazgos repRep = new RepHallazgos();
                repRep.Fortaleza = elahallazgos.Where(x => x.TipoNemotico == "F").Count().ToString();
                repRep.OportunidadMejora = elahallazgos.Where(x => x.TipoNemotico == "OM").Count().ToString();
                repRep.NoConformidadMayor = elahallazgos.Where(x => x.TipoNemotico == "NCM").Count().ToString();
                repRep.NoConformidadMenor = elahallazgos.Where(x => x.TipoNemotico == "NCm").Count().ToString();

                praReporte.ListResumenHallazgos.Add(repRep);

                Dictionary<string, CellTitles[]> pTitles = new Dictionary<string, CellTitles[]>();
                CellTitles[] cellTitlesTitulo = new CellTitles[4];
                cellTitlesTitulo[0] = new CellTitles { Title = "Fortaleza", Visible = true, Width = "50" };
                cellTitlesTitulo[1] = new CellTitles { Title = "Oportunidad Mejora", Visible = true, Width = "120" };
                cellTitlesTitulo[2] = new CellTitles { Title = "Conformidad Mayor", Visible = true, Width = "50" };
                cellTitlesTitulo[3] = new CellTitles { Title = "Conformidad Menor", Visible = true, Width = "50" };
                pTitles.Add("ListResumenHallazgos", cellTitlesTitulo);

                cellTitlesTitulo = new CellTitles[3];
                cellTitlesTitulo[0] = new CellTitles { Title = "Nombre", Visible = true, Width = "150" };
                cellTitlesTitulo[1] = new CellTitles { Title = "Total Dias InSitu", Visible = true, Width = "80" };
                cellTitlesTitulo[2] = new CellTitles { Title = "Total Dias Remoto", Visible = true, Width = "80" };
                pTitles.Add("ListCronograma", cellTitlesTitulo);

                cellTitlesTitulo = new CellTitles[2];
                cellTitlesTitulo[0] = new CellTitles { Title = "Direccion", Visible = true, Width = "150" };
                cellTitlesTitulo[1] = new CellTitles { Title = "Auditado", Visible = true, Width = "120" };
                pTitles.Add("ListAlcanceCert", cellTitlesTitulo);
                response.Object = new GlobalDataReport { data = praReporte, HeadersTables = pTitles };

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
                if (contenidos.Where(x => x.Nemotico == "PLAN_CRITERIO") != null)
                    elaContenido = contenidos.Where(x => x.Nemotico == "PLAN_CRITERIO").FirstOrDefault();
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

                string conlusionesAuditor = "";
                if (resulConclusiones.Count() > 0)
                    conlusionesAuditor = resulConclusiones.ToString();
                string criterioSistema = "";
                if (contenidos.Where(x => x.Nemotico == "PLAN_CRITERIO").Count() > 0)
                    criterioSistema = contenidos.Where(x => x.Nemotico == "PLAN_CRITERIO").FirstOrDefault().ToString();

                string modalidad = praciclocronograma.Praciclocronogramas.First().DiasRemoto > 0 && praciclocronograma.Praciclocronogramas.First().DiasInsitu > 0 ? "In Situ y Remota" : "";
                modalidad = string.IsNullOrEmpty(modalidad) && praciclocronograma.Praciclocronogramas.First().DiasRemoto > 0 ? "Remota" : "";
                modalidad = string.IsNullOrEmpty(modalidad) && praciclocronograma.Praciclocronogramas.First().DiasInsitu > 0 ? "In Situ" : "";

                string arasPreocupacion = "";
                var elaADP = repositoryMySql.SimpleSelect<Elaadp>(x => x.IdelaAuditoria == resulDBAuditoria.First().IdelaAuditoria);
                elaADP.ForEach(x =>
                {
                    arasPreocupacion += x.Area + WordHelper.GetCodeKey(WordHelper.keys.enter) + x.Descripcion.Replace("\n", WordHelper.GetCodeKey(WordHelper.keys.enter)) + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                //EquipoAuditoNombreCargo = null,
                ///llenamos el reporte con la informacion de este ciclo
                RepInformeAuditoriaEtapaI praReporte = new RepInformeAuditoriaEtapaI
                {
                    NombreEmpresa = cliente.NombreRazon,
                    Direccion = cliente.Direccion,
                    PersonaContacto = "",
                    Telefono = telefono,
                    CorreoElectronico = "",
                    CodigoServivio = praprogramasdeauditorium.CodigoServicioWs,
                    TipoAuditoria = praciclocronograma.Referencia,//consultar la tabla param,
                    ModalidadAuditoria = modalidad,
                    Normas = normas,
                    FechaAuditoria = $"{praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy")} a {praciclocronograma.Praciclocronogramas.First().FechaDeFinDeEjecucionAuditoria?.ToString("dd/MM/yyyy")}",
                    FechaInforme = DateTime.Now.ToString("dd/MM/yyyy"),
                    EquipoAuditor = equipoAuditor,
                    CriteriosSistema = criterioSistema,
                    ComentarioRealcionado = "ComentarioRealcionado",//TODO: Completar
                    Etapa1 = "Etapa1", //TODO: Completar
                    AreasPreocupacion = arasPreocupacion,//TODO: Completar
                    ConclusionAuditor = conlusionesAuditor,
                    AuditorLider = auditor.cargoPuesto,
                    NombreAuditorLider = auditor.nombreCompleto,
                    Fecha = DateTime.Now.ToString("dd/MM/yyyy"),
                    RepresentanteOrganizacion = "",//TODO: Completar
                    NombreCoordinadorAud = "",//TODO: Completar
                    ElectronicoCoordinador = "",//TODO: Completar
                    FechaAuditoriaEtapaII = "",//TODO: Completar
                    FechaSolicitarEdificarAudiII = "",//TODO: Completar                                        
                    CorreoElectronicoCoodinador = "",
                    ListEquipoAuditor = praciclocronograma.Pracicloparticipantes.Select(x =>
                    {
                        RepEquipoTCP repRepEquipo = new RepEquipoTCP();
                        repRepEquipo.EquipoAuditor = string.Empty;
                        if (!string.IsNullOrEmpty(x.CargoDetalleWs))
                        {
                            ListaCalificado cargo = JsonConvert.DeserializeObject<ListaCalificado>(x.CargoDetalleWs);
                            repRepEquipo.EquipoAuditor = cargo.CargoPuesto;
                        }

                        repRepEquipo.NombreCompleto = string.Empty;
                        if (!string.IsNullOrEmpty(x.ParticipanteDetalleWs))
                        {
                            ListaCalificado participante = JsonConvert.DeserializeObject<ListaCalificado>(x.ParticipanteDetalleWs);
                            repRepEquipo.NombreCompleto = participante.NombreCompleto;
                        }
                        repRepEquipo.TotalDiasInSitu = Convert.ToString(x.DiasInsistu);
                        repRepEquipo.TotalDiasRemoto = Convert.ToString(x.DiasRemoto);


                        return repRepEquipo;
                    }).ToList(),

                };
                Dictionary<string, CellTitles[]> pTitles = new Dictionary<string, CellTitles[]>();
                CellTitles[] cellTitlesTituloEquipoAuditor = new CellTitles[4];
                cellTitlesTituloEquipoAuditor[0] = new CellTitles { Title = "Cargo", Visible = true, Width = "50" };
                cellTitlesTituloEquipoAuditor[1] = new CellTitles { Title = "Nombre", Visible = true, Width = "120" };
                cellTitlesTituloEquipoAuditor[2] = new CellTitles { Title = "Días in Situ", Visible = true, Width = "50" };
                cellTitlesTituloEquipoAuditor[3] = new CellTitles { Title = "Días Remoto", Visible = true, Width = "50" };
                pTitles.Add("ListEquipoAuditor", cellTitlesTituloEquipoAuditor);

                response.Object = new GlobalDataReport { data = praReporte, HeadersTables = pTitles };
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

                fechaInicio = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy");
                fechaFin = praciclocronograma.Praciclocronogramas.First().FechaDeFinDeEjecucionAuditoria?.ToString("dd/MM/yyyy");


                ListaCalificado auditoriLider = new ListaCalificado();
                if (praciclocronograma.Pracicloparticipantes.Any(x => x.ParticipanteDetalleWs.ToLower().Contains("lider")))
                {
                    auditoriLider = JsonConvert.DeserializeObject<ListaCalificado>(praciclocronograma.Pracicloparticipantes.First(x => x.ParticipanteDetalleWs.ToLower().Contains("lider")).ParticipanteDetalleWs);
                    nombreYFirmaDeAuditorLider = auditoriLider?.NombreCompleto;
                }
                ///llenamos el reporte con la informacion de este ciclo
                REPListaVerificacionReunionApertura praListaVerificacionReunionApertura = new REPListaVerificacionReunionApertura
                {
                    NombreEmpresa = cliente.NombreRazon,
                    CodigoServicio = praprogramasdeauditorium.CodigoServicioWs,
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
                ListaCalificado auditoriLider = new ListaCalificado();
                if (praciclocronograma.Pracicloparticipantes.Any(x => x.ParticipanteDetalleWs.ToLower().Contains("lider")))
                {
                    auditoriLider = JsonConvert.DeserializeObject<ListaCalificado>(praciclocronograma.Pracicloparticipantes.First(x => x.ParticipanteDetalleWs.ToLower().Contains("lider")).ParticipanteDetalleWs);
                    nombreYFirmaDeAuditorLider = auditoriLider?.NombreCompleto;
                }
                ///llenamos el reporte con la informacion de este ciclo
                REPListaVerificacionReunionCierre praListaVerificacionReunionCierre = new REPListaVerificacionReunionCierre
                {

                    NombreOrganizacion = cliente.NombreRazon,
                    CodigoDeServicio = praprogramasdeauditorium.CodigoServicioWs,
                    FechaDeAuditoria = $"{praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy")} a {praciclocronograma.Praciclocronogramas.First().FechaDeFinDeEjecucionAuditoria?.ToString("dd/MM/yyyy")}",
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
                fechaInicio = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy");
                fechaFin = praciclocronograma.Praciclocronogramas.First().FechaDeFinDeEjecucionAuditoria?.ToString("dd/MM/yyyy");
                ///llenamos el reporte con la informacion de este ciclo
                REPListaAsistencia praListaAsistencia = new REPListaAsistencia
                {
                    NombreEmpresa = cliente.NombreRazon,
                    CodigoServicio = praprogramasdeauditorium.CodigoServicioWs,
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
                    ProcesoAuditado = procesoAuditado, //TODO: Completar
                    NombreAuditado = nombreAuditado, //TODO: Completar
                    Fecha = $"{praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy")} a {praciclocronograma.Praciclocronogramas.First().FechaDeFinDeEjecucionAuditoria?.ToString("dd/MM/yyyy")}",
                    SitiosAuditado = sitios,
                    CodigoServicio = praprogramasdeauditorium.CodigoServicioWs
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
                    ResponsabilidadORol = responsabilidadORol,
                    ProcesoAuditado = "", //TODO: Completar
                    NombreAuditado = "", //TODO: Completar
                    Fecha = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    SitiosAuditado = sitios
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

                string fechaAuditoria = $"{praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy")} a " +
                    $"{praciclocronograma.Praciclocronogramas.First().FechaDeFinDeEjecucionAuditoria?.ToString("dd/MM/yyyy")}";
                ///llenamos el reporte con la informacion de este ciclo
                REPDatosDeLaOrganizacion praDatosDeLaOrganizacion = new REPDatosDeLaOrganizacion
                {
                    FechaDeAuditoria = fechaAuditoria,
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
                ///TODO FIX WS Integración con INTRANET
                ///obtemos el id de ciclo a partir del Id de sistema

                int idCicloReal = (int)repositoryMySql.SimpleSelect<Praciclonormassistema>(x => x.IdCicloNormaSistema == requestDataReport.IdCiclo).First().IdPrAcicloProgAuditoria;

                //int IdCiclo = requestDataReport.IdCiclo;
                int IdCiclo = idCicloReal;
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

                var elaAuditoria = repositoryMySql.SimpleSelect<Elaauditorium>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria).First();
                var elahallazgos = repositoryMySql.SimpleSelect<Elahallazgo>(y => y.IdelaAuditoria == elaAuditoria.IdelaAuditoria);

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

                string fechaInicioFInal = $"{praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy")} a {praciclocronograma.Praciclocronogramas.First().FechaDeFinDeEjecucionAuditoria?.ToString("dd/MM/yyyy")} ";
                ///llenamos el reporte con la informacion de este ciclo
                REPEvaluacionYRecomendacionesParaProceso praEvaluacionYRecomendacionesParaProceso = new REPEvaluacionYRecomendacionesParaProceso
                {
                    Orgaizacion = cliente.NombreRazon,
                    CodigoDeServicio = praprogramasdeauditorium.CodigoServicioWs,
                    FechaDeAuditoria = fechaInicioFInal,
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
                    RedaccionSugerida = redaccionSugerida,
                    FechaRecomendacion = DateTime.Now.ToString("dd/MM/yyyy"),
                    ListHallazgos = new List<RepHallazgos>()
                };


                RepHallazgos repRep = new RepHallazgos();
                repRep.Fortaleza = elahallazgos.Where(x => x.TipoNemotico == "F").Count().ToString();
                repRep.OportunidadMejora = elahallazgos.Where(x => x.TipoNemotico == "OM").Count().ToString();
                repRep.NoConformidadMayor = elahallazgos.Where(x => x.TipoNemotico == "NCM").Count().ToString();
                repRep.NoConformidadMenor = elahallazgos.Where(x => x.TipoNemotico == "NCm").Count().ToString();

                praEvaluacionYRecomendacionesParaProceso.ListHallazgos.Add(repRep);


                //generamos el documento en word
                Dictionary<string, CellTitles[]> pTitles = new Dictionary<string, CellTitles[]>();
                WordHelper.CellTitles[] cellTitlesTitulo = new CellTitles[2];
                cellTitlesTitulo[0] = new CellTitles { Title = "Calificación", Visible = true, Width = "50" };
                cellTitlesTitulo[1] = new CellTitles { Title = "Auditor", Visible = true, Width = "50" };
                pTitles.Add("ListEquipoAuditoNombreCargo", cellTitlesTitulo);

                cellTitlesTitulo = new CellTitles[4];
                cellTitlesTitulo[0] = new CellTitles { Title = "Fortaleza (F)", Visible = true, Width = "20" };
                cellTitlesTitulo[1] = new CellTitles { Title = "Oportunidad de mejora (OM)", Visible = true, Width = "20" };
                cellTitlesTitulo[2] = new CellTitles { Title = "No conformidad mayor (NCM)", Visible = true, Width = "20" };
                cellTitlesTitulo[3] = new CellTitles { Title = "No conformidad menor (NCm)", Visible = true, Width = "20" };
                pTitles.Add("ListHallazgos", cellTitlesTitulo);

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
                praciclocronograma.Praciclonormassistemas = repositoryMySql.SimpleSelect<Praciclonormassistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);

                Cliente cliente = JsonConvert.DeserializeObject<Cliente>(praprogramasdeauditorium.OrganizacionContentWs);
                string alcance = "";
                praciclocronograma.Praciclonormassistemas.ToList().ForEach(x =>
                {
                    alcance += x.Alcance + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

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
                    normas += x.Norma + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                string sitios = "";
                praciclocronograma.Pradireccionespasistemas.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
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
                    NroCertificacion = "PENDIENTE",
                    Alcance = alcance,
                    Sitios = sitios
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
                    //ReferenciaIbnorca = praciclocronograma.Referencia,
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

        public ResponseObject<GlobalDataReport> TCSRepOfertaContrato(RequestDataReport requestDataReport)
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


                string productos = "";
                praciclocronograma.Pradireccionespaproductos.ToList().ForEach(x =>
                {
                    normas += x.Norma + WordHelper.GetCodeKey(WordHelper.keys.enter);
                    productos += x.Nombre + ",";
                });


                string sitios = "";
                praciclocronograma.Pradireccionespasistemas.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                ///llenamos el reporte con la informacion de este ciclo
                TCSRepOfertaContrato praTCPREPInforme = new TCSRepOfertaContrato
                {
                    Referencia = cliente.NombreRazon,
                    FechaIbnorca = praciclocronograma.Referencia,
                    Cliente = "", //TODO: Completar
                    Norma = cliente.Direccion, //TODO: Completar
                    Guia = normas,
                    NombreGerente = "", //TODO: Completar
                    Alcance = "", //TODO: Completar

                    //TODO: Completar Pradireccionespaproductos
                    ListSitios = praciclocronograma.Pradireccionespaproductos.Select(x =>
                    {
                        SitosOferta repRep = new SitosOferta();
                        repRep.Sitio = x.Nombre;
                        return repRep;
                    }).ToList(),
                    //TODO: Completar Pradireccionespaproductos
                    ListPresupuesto = praciclocronograma.Pradireccionespaproductos.Select(x =>
                    {
                        PresupuestoOfertaTCS repRep = new PresupuestoOfertaTCS();
                        repRep.Etapa = ""; //TODO: Completar
                        repRep.Concepto = ""; //TODO: Completar
                        repRep.DiasAuditor = ""; //TODO: Completar
                        repRep.CostoUSD = ""; //TODO: Completar
                        return repRep;
                    }).ToList(),

                };
                Dictionary<string, CellTitles[]> pTitles = new Dictionary<string, CellTitles[]>();
                CellTitles[] cellTitlesTitulo = new CellTitles[1];
                cellTitlesTitulo[0] = new CellTitles { Title = "Sitio", Visible = true, Width = "300" };
                pTitles.Add("ListSitios", cellTitlesTitulo);

                cellTitlesTitulo = new CellTitles[4];
                cellTitlesTitulo[0] = new CellTitles { Title = "Etapa", Visible = true, Width = "150" };
                cellTitlesTitulo[1] = new CellTitles { Title = "Concepto", Visible = true, Width = "80" };
                cellTitlesTitulo[2] = new CellTitles { Title = "DiasAuditoro", Visible = true, Width = "80" };
                cellTitlesTitulo[3] = new CellTitles { Title = "CostoUSD", Visible = true, Width = "80" };
                pTitles.Add("ListPresupuesto", cellTitlesTitulo);

                response.Object = new GlobalDataReport { data = praTCPREPInforme, HeadersTables = pTitles };
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
