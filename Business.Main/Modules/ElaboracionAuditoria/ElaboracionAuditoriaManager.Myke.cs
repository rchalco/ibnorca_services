using Business.Main.Base;
using Business.Main.Cross;
using Business.Main.DataMapping;
using Business.Main.DataMapping.DTOs;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.BuscarxIdClienteEmpresaDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.ListarAuditoresxCargoCalificadoDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.ListarContactosEmpresaDTO;
using Business.Main.Modules.ElaboracionAuditoria.Reportes.TCP;
using Business.Main.Modules.ElaboracionAuditoria.Reportes.TCS;
using Business.Main.Modules.ElaboracionAuditoria.Resportes.TCS;
using Domain.Main.CrossEntities;
using Domain.Main.Wraper;
using Newtonsoft.Json;
using PlumbingProps.Document;
using PlumbingProps.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PlumbingProps.Document.WordHelper;

namespace Business.Main.Modules.ElaboracionAuditoria
{
    public partial class ElaboracionAuditoriaManager
    {
        #region TCS       
        public ResponseObject<GlobalDataReport> TCSGenerarNotaSuspension(RequestDataReport requestDataReport)
        {
            ResponseObject<GlobalDataReport> response = new ResponseObject<GlobalDataReport> { Message = "", State = ResponseType.Success };
            try
            {
                int IdCiclo = requestDataReport.IdCiclo;
                string nombre = string.Empty, cargo = string.Empty, fechaLiteral = string.Empty;
                ///Obtenemos la informacion del ciclo y del programa
                Praciclosprogauditorium praciclocronograma = repositoryMySql.SimpleSelect<Praciclosprogauditorium>(x => x.IdPrAcicloProgAuditoria == IdCiclo).ToList().FirstOrDefault();
                Praprogramasdeauditorium praprogramasdeauditorium = repositoryMySql.SimpleSelect<Praprogramasdeauditorium>(x => x.IdPrAprogramaAuditoria == praciclocronograma.IdPrAprogramaAuditoria).ToList().FirstOrDefault();
                if (praciclocronograma == null)
                {
                    response.State = ResponseType.Warning;
                    response.Message = "No se cuenta con informacion de este ciclo en la BD";
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
                RepNotaSuspensionCertifica praSuspension = new RepNotaSuspensionCertifica
                {
                    FechaCabecera = "-" + DateTime.Now.ToString("dd/MM/yyyy"),
                    CorrelativoCabecera = "--",
                    NombreNota = nombre,
                    Cargo = cargo,
                    ReferenciaNota = normas,
                    NroCertificado = "XXXX", //TODO: Completar
                    NombreEmpresaTexto = cliente.NombreRazon,
                    DescripcionOtrogado = alcance,
                    Sitios = sitios,
                    FechaLiteral1 = "", //TODO: Completar
                    Seguimiento = praciclocronograma.Referencia,
                    FechaLiteral2 = fechaLiteral,
                    DirectorEjecutivo = "" //TODO: Completar

                };
                response.Object = new GlobalDataReport { data = praSuspension, HeadersTables = null };

            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCSGenerarPlanAuditoria(RequestDataReport requestDataReport)
        {
            ResponseObject<GlobalDataReport> response = new ResponseObject<GlobalDataReport> { Message = "", State = ResponseType.Success };
            try
            {
                int IdCiclo = requestDataReport.IdCiclo;
                string pathPlantilla = string.Empty, objetivosAuditoria = string.Empty, cambiosAlcance = string.Empty, certificacion = string.Empty;
                ///Obtenemos la informacion del ciclo y del programa
                Praciclosprogauditorium praciclocronograma = repositoryMySql.SimpleSelect<Praciclosprogauditorium>(x => x.IdPrAcicloProgAuditoria == IdCiclo).ToList().FirstOrDefault();
                Praprogramasdeauditorium praprogramasdeauditorium = repositoryMySql.SimpleSelect<Praprogramasdeauditorium>(x => x.IdPrAprogramaAuditoria == praciclocronograma.IdPrAprogramaAuditoria).ToList().FirstOrDefault();
                if (praciclocronograma == null)
                {
                    response.State = ResponseType.Warning;
                    response.Message = "No se cuenta con informacion de este ciclo en la BD";
                    return response;
                }

                praciclocronograma.Praciclocronogramas = repositoryMySql.SimpleSelect<Praciclocronograma>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Praciclonormassistemas = repositoryMySql.SimpleSelect<Praciclonormassistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pracicloparticipantes = repositoryMySql.SimpleSelect<Pracicloparticipante>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespaproductos = repositoryMySql.SimpleSelect<Pradireccionespaproducto>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespasistemas = repositoryMySql.SimpleSelect<Pradireccionespasistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);

                Elaauditorium elaauditorium = repositoryMySql.SimpleSelect<Elaauditorium>(y => y.IdPrAcicloProgAuditoria == IdCiclo).First();
                List<Elacronogama> elacronogama = repositoryMySql.SimpleSelect<Elacronogama>(y => y.Idelaauditoria == elaauditorium.IdelaAuditoria);

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
                string alcance = string.Empty;
                praciclocronograma.Praciclonormassistemas.ToList().ForEach(x =>
                {
                    normas += x.Norma + WordHelper.GetCodeKey(WordHelper.keys.enter);
                    alcance += x.Alcance + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                string sitios = "";

                praciclocronograma.Pradireccionespasistemas.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                string fechaInicioAuditoria = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy");
                string fechaFinAuditoria = praciclocronograma.Praciclocronogramas.First().FechaDeFinDeEjecucionAuditoria?.ToString("dd/MM/yyyy");
                
                var contenidos = repositoryMySql.SimpleSelect<Elacontenidoauditorium>(x => x.IdelaAuditoria == elaauditorium.IdelaAuditoria);
                contenidos.Where(x => x.Nemotico == "PLAN_OBJETIVOS").ToList().ForEach(x =>
                {
                    objetivosAuditoria = x.Contenido.Replace("\n", WordHelper.GetCodeKey(WordHelper.keys.enter));
                });

                //EquipoAuditoNombreCargo = null,
                ///llenamos el reporte con la informacion de este ciclo
                REPPlanAuditoria praPlanAuditoria = new REPPlanAuditoria
                {
                    NombreEmpresa = cliente.NombreRazon,
                    CodigoServicio = praprogramasdeauditorium.CodigoServicioWs,
                    TipoAuditoria = praciclocronograma.Referencia,
                    ModalidadAuditoria = $"Días insitu: {praciclocronograma.Praciclocronogramas.First().DiasInsitu}, días remoto: {praciclocronograma.Praciclocronogramas.First().DiasRemoto}",
                    NormaAuditadas = normas,
                    FechaAuditoria = $"Desde {fechaInicioAuditoria} hasta {fechaFinAuditoria}",
                    Alcance = alcance,
                    ObjetivosAuditoria = objetivosAuditoria,
                    CambiosAlcance = cambiosAlcance,
                    Certificacion = certificacion,
                    SitiosFisicos = sitios,
                    Normas = normas,
                    ListEquipoAuditor = praciclocronograma.Pracicloparticipantes.Select(x =>
                    {
                        RepEquipo repRepEquipo = new RepEquipo();
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
                    ListCronograma = elacronogama.Select(x =>
                    {
                        RepCronogramaEquipo repRepCronograma = new RepCronogramaEquipo();
                        repRepCronograma.Fecha = x.FechaInicio;
                        repRepCronograma.Hora = x.Horario;
                        repRepCronograma.RequisitoEsquema = x.RequisitosEsquema;
                        repRepCronograma.ResponsableOrganiza = x.PersonaEntrevistadaCargo;
                        repRepCronograma.SitioAuditado = x.Direccion;
                        repRepCronograma.EquipoAuditado = x.Auditor;

                        return repRepCronograma;
                    }).ToList(),
                    FechaElaboracion = DateTime.Now.ToString("dd/MM/yyyy"),
                    FechaAprobacion = DateTime.Now.ToString("dd/MM/yyyy")

                };

                //generamos el documento en word
                Dictionary<string, CellTitles[]> pTitles = new Dictionary<string, CellTitles[]>();
                CellTitles[] cellTitlesTitulo = new CellTitles[4];
                cellTitlesTitulo[0] = new CellTitles { Title = "Cargo", Visible = true, Width = "50" };
                cellTitlesTitulo[1] = new CellTitles { Title = "Nombre", Visible = true, Width = "120" };
                cellTitlesTitulo[2] = new CellTitles { Title = "Días in Situ", Visible = true, Width = "50" };
                cellTitlesTitulo[3] = new CellTitles { Title = "Días Remoto", Visible = true, Width = "50" };
                pTitles.Add("ListEquipoAuditor", cellTitlesTitulo);
                cellTitlesTitulo = new CellTitles[6];
                cellTitlesTitulo[0] = new CellTitles { Title = "Fecha", Visible = true, Width = "50" };
                cellTitlesTitulo[1] = new CellTitles { Title = "Hora", Visible = true, Width = "120" };
                cellTitlesTitulo[2] = new CellTitles { Title = "Sitio Auditado", Visible = true, Width = "50" };
                cellTitlesTitulo[3] = new CellTitles { Title = "Requisito Esquema", Visible = true, Width = "50" };
                cellTitlesTitulo[4] = new CellTitles { Title = "Equipo Auditado", Visible = true, Width = "50" };
                cellTitlesTitulo[5] = new CellTitles { Title = "Responsable Organiza", Visible = true, Width = "50" };
                pTitles.Add("ListCronograma", cellTitlesTitulo);
                response.Object = new GlobalDataReport { data = praPlanAuditoria, HeadersTables = pTitles };

            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        public ResponseObject<GlobalDataReport> TCSDesignacionAuditoria(RequestDataReport requestDataReport)
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
                    response.Message = "No se cuenta con informacion de este ciclo en la BD";
                    return response;
                }

                praciclocronograma.Praciclocronogramas = repositoryMySql.SimpleSelect<Praciclocronograma>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Praciclonormassistemas = repositoryMySql.SimpleSelect<Praciclonormassistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pracicloparticipantes = repositoryMySql.SimpleSelect<Pracicloparticipante>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespaproductos = repositoryMySql.SimpleSelect<Pradireccionespaproducto>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespasistemas = repositoryMySql.SimpleSelect<Pradireccionespasistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);

                praprogramasdeauditorium.Praciclosprogauditoria = repositoryMySql.SimpleSelect<Praciclosprogauditorium>(y => y.IdPrAprogramaAuditoria == praprogramasdeauditorium.IdPrAprogramaAuditoria);

                DateTime? fechaProxima = null;

                if (praprogramasdeauditorium.Praciclosprogauditoria.Any(x => x.Anio == praciclocronograma.Anio + 1))
                {
                    var cicloProximo = praprogramasdeauditorium.Praciclosprogauditoria.First(x => x.Anio == praciclocronograma.Anio + 1);
                    cicloProximo.Praciclocronogramas = repositoryMySql.SimpleSelect<Praciclocronograma>(y => y.IdPrAcicloProgAuditoria == cicloProximo.IdPrAcicloProgAuditoria);
                    fechaProxima = praprogramasdeauditorium.Praciclosprogauditoria.First(x => x.Anio == praciclocronograma.Anio + 1).Praciclocronogramas.First().MesProgramado;
                }

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

                string sitiosAuditar = "";
                praciclocronograma.Pradireccionespasistemas.Where(x => x.Dias > 0).ToList().ForEach(x =>
                {
                    sitiosAuditar += "Días: " + x.Dias + WordHelper.GetCodeKey(WordHelper.keys.enter) + x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                string contactos = "";
                string telefono = "";
                string correoElectronico = "";

                resulServices.lstContactos.ToList().ForEach(x =>
                {
                    contactos += x.NombreContacto;
                    telefono += x.FonoContacto;
                    correoElectronico += x.CorreoContacto;
                });

                ListaCalificado auditoriLider = new ListaCalificado();
                if (praciclocronograma.Pracicloparticipantes.Any(x => x.ParticipanteDetalleWs.ToLower().Contains("lider")))
                {
                    auditoriLider = JsonConvert.DeserializeObject<ListaCalificado>(praciclocronograma.Pracicloparticipantes.First(x => x.ParticipanteDetalleWs.ToLower().Contains("lider")).ParticipanteDetalleWs);
                }
                ///llenamos el reporte con la informacion de este ciclo
                REPDesignacionAuditoria praReporte = new REPDesignacionAuditoria
                {
                    TipoAuditoria = praciclocronograma.Referencia,
                    NombreEmpresa = cliente.NombreRazon,
                    ModalidadAuditoria = praciclocronograma.Praciclocronogramas.Any(x => x.DiasRemoto > 0) ? "Remoto" : "In Situ",
                    FechaInicio = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    FechaFin = praciclocronograma.Praciclocronogramas.First().FechaDeFinDeEjecucionAuditoria?.ToString("dd/MM/yyyy"),
                    DiasAuditor = praciclocronograma.Praciclocronogramas.Sum(x => x.DiasRemoto + x.DiasInsitu).ToString(),
                    AuditorLider = auditoriLider.NombreCompleto,
                    CorreoAuditorLider = auditoriLider.Correo,
                    Auditor = string.Empty,//TODO: Completar
                    CorreoAuditor = "", //TODO: Completar
                    Experto = "", //TODO: Completar
                    CorreoExperto = "", //TODO: Completar
                    AuditorEnsayos = "", //TODO: Completar
                    CorreoEnsayos = "", //TODO: Completar
                    OrganismoCertificador = praprogramasdeauditorium.OrganismoCertificador,
                    CodigoServicio = praprogramasdeauditorium.CodigoServicioWs,
                    IDServicio = praprogramasdeauditorium.IdOrganizacionWs,
                    AltaDireccion = "", //TODO: Completar
                    Cargo = "", //TODO: Completar
                    Contacto = contactos,
                    CargoContacto = "", //TODO: Completar
                    CorreoElectronicoContacto = correoElectronico, //TODO: Completar
                    CodigoIAF = praprogramasdeauditorium.CodigoIafws,
                    Alcance = alcance, //TODO: Completar
                    SitiosAAuditar = sitiosAuditar,
                    SitiosDentroDeAlcance = sitios, //TODO: Completar                    
                    HorarioTrabajo = praciclocronograma.Praciclocronogramas.First().HorarioTrabajo,
                    FechaProxima = fechaProxima?.ToString("dd/MM/yyyy"), //TODO: Completar
                    Adjunto = "", //TODO: Completar
                    Usuario = "", //TODO: Completar
                    Logistica = "", //TODO: Completar
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
                    }).ToList()

                };

                Dictionary<string, CellTitles[]> pTitles = new Dictionary<string, CellTitles[]>();
                CellTitles[] cellTitlesTitulo = new CellTitles[2];
                cellTitlesTitulo[0] = new CellTitles { Title = "Calificación", Visible = true, Width = "50" };
                cellTitlesTitulo[1] = new CellTitles { Title = "Auditor", Visible = true, Width = "50" };
                pTitles.Add("ListRepDesginacionParticipante", cellTitlesTitulo);

                response.Object = new GlobalDataReport { data = praReporte, HeadersTables = pTitles };

            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        #endregion

        #region TCP
        public ResponseObject<GlobalDataReport> TCPRepDesignacionAuditoria(RequestDataReport requestDataReport)
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
                    response.Message = "No se cuenta con informacion de este ciclo en la BD";
                    return response;
                }

                praciclocronograma.Praciclocronogramas = repositoryMySql.SimpleSelect<Praciclocronograma>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Praciclonormassistemas = repositoryMySql.SimpleSelect<Praciclonormassistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pracicloparticipantes = repositoryMySql.SimpleSelect<Pracicloparticipante>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespaproductos = repositoryMySql.SimpleSelect<Pradireccionespaproducto>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespasistemas = repositoryMySql.SimpleSelect<Pradireccionespasistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);

                praprogramasdeauditorium.Praciclosprogauditoria = repositoryMySql.SimpleSelect<Praciclosprogauditorium>(y => y.IdPrAprogramaAuditoria == praprogramasdeauditorium.IdPrAprogramaAuditoria);

                DateTime? fechaProxima = null;

                if (praprogramasdeauditorium.Praciclosprogauditoria.Any(x => x.Anio == praciclocronograma.Anio + 1))
                {
                    var cicloProximo = praprogramasdeauditorium.Praciclosprogauditoria.First(x => x.Anio == praciclocronograma.Anio + 1);
                    cicloProximo.Praciclocronogramas = repositoryMySql.SimpleSelect<Praciclocronograma>(y => y.IdPrAcicloProgAuditoria == cicloProximo.IdPrAcicloProgAuditoria);
                    fechaProxima = praprogramasdeauditorium.Praciclosprogauditoria.First(x => x.Anio == praciclocronograma.Anio + 1).Praciclocronogramas.First().MesProgramado;
                }

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

                string sitios = "";
                praciclocronograma.Pradireccionespaproductos.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                List<TCPREPDesignacionAuditoria.Producto> productos = new List<TCPREPDesignacionAuditoria.Producto>();
                praciclocronograma.Pradireccionespaproductos.ToList().ForEach(x =>
                {
                    productos.Add(new TCPREPDesignacionAuditoria.Producto { marca = x.Marca, nombre = x.Nombre });
                });

                string contactos = "";
                string telefono = "";
                string correoElectronico = "";

                resulServices.lstContactos.ToList().ForEach(x =>
                {
                    contactos += x.NombreContacto;
                    telefono += x.FonoContacto;
                    correoElectronico += x.CorreoContacto;
                });

                ///llenamos el reporte con la informacion de este ciclo
                TCPREPDesignacionAuditoria praReporte = new TCPREPDesignacionAuditoria
                {
                    TipoAuditoria = praciclocronograma.Referencia,
                    NombreEmpresa = cliente.NombreRazon,
                    ModalidadAuditoria = praciclocronograma.Praciclocronogramas.Any(x => x.DiasRemoto > 0) ? "Remoto" : "In Situ",
                    FechaInicio = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    FechaFin = praciclocronograma.Praciclocronogramas.First().FechaDeFinDeEjecucionAuditoria?.ToString("dd/MM/yyyy"),
                    DiasAuditor = praciclocronograma.Praciclocronogramas.Sum(x => x.DiasRemoto + x.DiasInsitu).ToString(),
                    FechaInicioEnsayos = "", //TODO: Completar                    
                    AuditorLider = "", //TODO: Completar
                    CorreoAuditorLider = "", //TODO: Completar
                    Auditor = "", //TODO: Completar
                    CorreoAuditor = "", //TODO: Completar
                    Experto = "", //TODO: Completar
                    CorreoExperto = "", //TODO: Completar
                    AuditorEnsayos = "", //TODO: Completar
                    CorreoEnsayos = "", //TODO: Completar
                    OrganismoCertificador = praprogramasdeauditorium.OrganismoCertificador,
                    CodigoServicio = praprogramasdeauditorium.CodigoServicioWs,
                    IDServicio = praprogramasdeauditorium.IdOrganizacionWs,
                    AltaDireccion = "", //TODO: Completar
                    Cargo = "", //TODO: Completar
                    Contacto = contactos,
                    CargoContacto = "", //TODO: Completar
                    TelefonoContacto = telefono, //TODO: Completar
                    CorreoElectronicoContacto = correoElectronico, //TODO: Completar
                    CodigoIAF = praprogramasdeauditorium.CodigoIafws,
                    Alcance = "", //TODO: Completar
                    Sitios = sitios, //TODO: Completar
                    HorarioTrabajo = praciclocronograma.Praciclocronogramas.First().HorarioTrabajo,
                    FechaProxima = fechaProxima?.ToString("dd/MM/yyyy"), //TODO: Completar
                    Adjunto = "", //TODO: Completar
                    Usuario = "", //TODO: Completar
                    Logistica = "", //TODO: Completar
                    Productos = "",
                    ListProductos = productos,
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
                    }).ToList()

                };

                Dictionary<string, CellTitles[]> pTitles = new Dictionary<string, CellTitles[]>();
                CellTitles[] cellTitlesTitulo = new CellTitles[2];
                cellTitlesTitulo[0] = new CellTitles { Title = "Calificación", Visible = true, Width = "50" };
                cellTitlesTitulo[1] = new CellTitles { Title = "Auditor", Visible = true, Width = "50" };
                pTitles.Add("ListRepDesginacionParticipante", cellTitlesTitulo);


                CellTitles[] cellTitlesTituloProducto = new CellTitles[2];
                cellTitlesTituloProducto[0] = new CellTitles { Title = "Producto", Visible = true, Width = "50" };
                cellTitlesTituloProducto[1] = new CellTitles { Title = "Marca", Visible = true, Width = "50" };
                pTitles.Add("ListProductos", cellTitlesTituloProducto);


                response.Object = new GlobalDataReport { data = praReporte, HeadersTables = pTitles };

            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCPREPPlanAuditoria(RequestDataReport requestDataReport)
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
                    response.Message = "No se cuenta con informacion de este ciclo en la BD";
                    return response;
                }

                praciclocronograma.Praciclocronogramas = repositoryMySql.SimpleSelect<Praciclocronograma>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Praciclonormassistemas = repositoryMySql.SimpleSelect<Praciclonormassistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pracicloparticipantes = repositoryMySql.SimpleSelect<Pracicloparticipante>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                praciclocronograma.Pradireccionespaproductos = repositoryMySql.SimpleSelect<Pradireccionespaproducto>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);
                //praciclocronograma.Pradireccionespasistemas = repositoryMySql.SimpleSelect<Pradireccionespasistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);

                Elaauditorium elaauditorium = repositoryMySql.SimpleSelect<Elaauditorium>(x => x.IdPrAcicloProgAuditoria == IdCiclo).First();
                var contenidos = repositoryMySql.SimpleSelect<Elacontenidoauditorium>(x => x.IdelaAuditoria == elaauditorium.IdelaAuditoria);
                string criterios = "";
                contenidos.Where(x => x.Nemotico == "PLAN_CRITERIO").ToList().ForEach(x =>
                {
                    criterios = x.Contenido.Replace("\n", WordHelper.GetCodeKey(WordHelper.keys.enter));
                });

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

                ///TDO obtener contenido para documento
                var resulDBAuditoria = repositoryMySql.SimpleSelect<Elaauditorium>(x => x.IdPrAcicloProgAuditoria == IdCiclo);
                var resulDBContenidoAuditoria = repositoryMySql.SimpleSelect<Elacontenidoauditorium>(x => x.IdelaAuditoria == resulDBAuditoria.First().IdelaAuditoria);

                //INFPROD_CONCLUSION
                var resulConclusiones = resulDBContenidoAuditoria.Where(x => x.Nemotico == "INFPROD_CONCLUSION");
                //resulConclusiones.First().Contenido

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
                var cronogramas = repositoryMySql.SimpleSelect<Elacronogama>(x => x.Idelaauditoria == elaauditorium.IdelaAuditoria);

                //EquipoAuditoNombreCargo = null,
                ///llenamos el reporte con la informacion de este ciclo
                TCPREPPlanAuditoria praReporte = new TCPREPPlanAuditoria
                {
                    NombreEmpresa = cliente.NombreRazon,
                    Direccion = direccion,
                    Criterios = criterios,
                    Contacto = contactos,
                    TelefonoCelular = telefono,
                    CorreoElectronico = correoElectronico,
                    CodigoServicio = praprogramasdeauditorium.CodigoServicioWs,
                    TipoAuditoria = praciclocronograma.Referencia,
                    FechaInicio = fechaInicio,
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
                    Productos = productos,
                    CambiosAlcances = "", ///No se tiene de donde obtener
                    Ensayos = praciclocronograma.Referencia,
                    FechaElaboracionPlan = DateTime.Now.ToString("dd/MM/yyyy"),
                    ListCronograma = cronogramas.Select(x =>
                    {
                        RepCronogramaEquipoTCP repRepCronograma = new RepCronogramaEquipoTCP();
                        repRepCronograma.Fecha = x.FechaInicio;
                        repRepCronograma.Hora = x.Horario;
                        repRepCronograma.RequisitoEsquema = x.RequisitosEsquema;
                        repRepCronograma.ResponsableOrganiza = x.PersonaEntrevistadaCargo;
                        repRepCronograma.SitioAuditado = x.Direccion;
                        repRepCronograma.EquipoAuditado = x.Auditor;

                        return repRepCronograma;
                    }).ToList(),
                };

                //generamos el documento en word
                Dictionary<string, CellTitles[]> pTitles = new Dictionary<string, CellTitles[]>();
                CellTitles[] cellTitlesTitulo = new CellTitles[4];
                cellTitlesTitulo[0] = new CellTitles { Title = "Cargo", Visible = true, Width = "50" };
                cellTitlesTitulo[1] = new CellTitles { Title = "Nombre", Visible = true, Width = "120" };
                cellTitlesTitulo[2] = new CellTitles { Title = "Días in Situ", Visible = true, Width = "50" };
                cellTitlesTitulo[3] = new CellTitles { Title = "Días Remoto", Visible = true, Width = "50" };
                pTitles.Add("ListEquipoAuditor", cellTitlesTitulo);

                cellTitlesTitulo = new CellTitles[6];
                cellTitlesTitulo[0] = new CellTitles { Title = "Fecha", Visible = true, Width = "50" };
                cellTitlesTitulo[1] = new CellTitles { Title = "Hora", Visible = true, Width = "120" };
                cellTitlesTitulo[2] = new CellTitles { Title = "Sitio Auditado", Visible = true, Width = "50" };
                cellTitlesTitulo[3] = new CellTitles { Title = "Requisito Esquema", Visible = true, Width = "50" };
                cellTitlesTitulo[4] = new CellTitles { Title = "Equipo Auditado", Visible = true, Width = "50" };
                cellTitlesTitulo[5] = new CellTitles { Title = "Responsable Organiza", Visible = true, Width = "50" };
                pTitles.Add("ListCronograma", cellTitlesTitulo);
                //generamos el documento en word
                response.Object = new GlobalDataReport { data = praReporte, HeadersTables = pTitles };
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCPREPListaVerificacionReunionApertura(RequestDataReport requestDataReport)
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
                    response.Message = "No se cuenta con informacion de este ciclo en la BD";
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

                string sitios = "";
                praciclocronograma.Pradireccionespasistemas.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                string fechaAuditoria = "";
                fechaAuditoria = "Desde " + praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy") +
                    " Hasta " + praciclocronograma.Praciclocronogramas.First().FechaDeFinDeEjecucionAuditoria?.ToString("dd/MM/yyyy");
                ParticipanteDetalleWS auditor = new Domain.Main.CrossEntities.ParticipanteDetalleWS();
                string equipoAuditor = "";
                praciclocronograma.Pracicloparticipantes.ToList().ForEach(x =>
                {
                    auditor = JsonConvert.DeserializeObject<ParticipanteDetalleWS>(x.ParticipanteDetalleWs);
                    equipoAuditor = auditor.cargoPuesto + ":" + auditor.nombreCompleto + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                //EquipoAuditoNombreCargo = null,
                ///llenamos el reporte con la informacion de este ciclo
                TCPREPListaVerificacionReunionApertura praReporte = new TCPREPListaVerificacionReunionApertura
                {
                    NombreEmpresa = cliente.NombreRazon,
                    TipoAuditoria = praciclocronograma.Referencia,
                    CodigoServicio = praprogramasdeauditorium.CodigoServicioWs,
                    FechaAuditoria = fechaAuditoria,
                    AuditorLider = auditor.nombreCompleto //TODO: Completar
                };
                response.Object = new GlobalDataReport { data = praReporte, HeadersTables = null };

            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCPREPListaVerificacionReunionCierre(RequestDataReport requestDataReport)
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
                    response.Message = "No se cuenta con informacion de este ciclo en la BD";
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

                string sitios = "";
                praciclocronograma.Pradireccionespasistemas.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });
                ParticipanteDetalleWS auditor = new Domain.Main.CrossEntities.ParticipanteDetalleWS();
                string equipoAuditor = "";
                praciclocronograma.Pracicloparticipantes.ToList().ForEach(x =>
                {
                    auditor = JsonConvert.DeserializeObject<ParticipanteDetalleWS>(x.ParticipanteDetalleWs);
                    equipoAuditor = auditor.cargoPuesto + ":" + auditor.nombreCompleto + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                //EquipoAuditoNombreCargo = null,
                ///llenamos el reporte con la informacion de este ciclo
                TCPREPListaVerificacionReunionCierre praReporte = new TCPREPListaVerificacionReunionCierre
                {
                    NombreEmpresa = cliente.NombreRazon,
                    TipoAuditoria = praciclocronograma.Referencia,
                    CodigoServicio = praprogramasdeauditorium.CodigoServicioWs,
                    FechaInicio = "Desde " + praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    FechaFin = " Hasta " + praciclocronograma.Praciclocronogramas.First().FechaDeFinDeEjecucionAuditoria?.ToString("dd/MM/yyyy"),
                    AuditorLider = auditor.nombreCompleto //TODO: Completar
                };
                response.Object = new GlobalDataReport { data = praReporte, HeadersTables = null };

            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCPREPListaAsistencia(RequestDataReport requestDataReport)
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
                    response.Message = "No se cuenta con informacion de este ciclo en la BD";
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

                string sitios = "";
                praciclocronograma.Pradireccionespasistemas.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                //EquipoAuditoNombreCargo = null,
                ///llenamos el reporte con la informacion de este ciclo
                TCPREPListaAsistencia praReporte = new TCPREPListaAsistencia
                {
                    NombreEmpresa = cliente.NombreRazon,
                    TipoAuditoria = praciclocronograma.Referencia,
                    CodigoServicio = praprogramasdeauditorium.CodigoServicioWs,
                    FechaInicio = "Desde " + praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
                    FechaFin = " Hasta " + praciclocronograma.Praciclocronogramas.First().FechaDeFinDeEjecucionAuditoria?.ToString("dd/MM/yyyy"),
                };
                response.Object = new GlobalDataReport { data = praReporte, HeadersTables = null };

            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCPREPActaMuestreo(RequestDataReport requestDataReport)
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
                    response.Message = "No se cuenta con informacion de este ciclo en la BD";
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
                    normas += x.Norma;
                });

                string sitios = "";
                praciclocronograma.Pradireccionespasistemas.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                string fechaAuditoria = "";
                fechaAuditoria = "Desde " + praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy") +
                    " Hasta " + praciclocronograma.Praciclocronogramas.First().FechaDeFinDeEjecucionAuditoria?.ToString("dd/MM/yyyy");
                string planMuestreo = "";
                Elaauditorium elaauditorium = repositoryMySql.SimpleSelect<Elaauditorium>(x => x.IdPrAcicloProgAuditoria == IdCiclo).First();
                var contenidos = repositoryMySql.SimpleSelect<Elacontenidoauditorium>(x => x.IdelaAuditoria == elaauditorium.IdelaAuditoria);
                contenidos.Where(x => x.Nemotico == "ACTAMUESTREO_PLAN").ToList().ForEach(x =>
                {
                    planMuestreo = x.Contenido.Replace("\n", WordHelper.GetCodeKey(WordHelper.keys.enter));
                });
                string labExterno = string.Empty;
                string labInterno = string.Empty;
                contenidos.Where(x => x.Nemotico == "ACT_LAB_ENSAYO").Where(x => x.Label == "Interno").ToList().ForEach(x =>
                {
                    labInterno = x.Contenido;
                });

                contenidos.Where(x => x.Nemotico == "ACT_LAB_ENSAYO").Where(x => x.Label == "Externo").ToList().ForEach(x =>
                {
                    labExterno = x.Contenido;
                });
                //EquipoAuditoNombreCargo = null,
                ///llenamos el reporte con la informacion de este ciclo
                TCPREPActaMuestreo praReporte = new TCPREPActaMuestreo
                {
                    NombreEmpresa = cliente.NombreRazon,
                    TipoAuditoria = praciclocronograma.Referencia,
                    CodigoServicio = praprogramasdeauditorium.CodigoServicioWs,
                    Norma = normas,
                    Fecha = fechaAuditoria,
                    PlanMuestreo = planMuestreo,
                    Interno = labInterno,
                    Externo = labExterno,
                    DescripcionExterno = "", //TODO: Completar
                };
                response.Object = new GlobalDataReport { data = praReporte, HeadersTables = null };
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCPREPListaVerificacionAuditor(RequestDataReport requestDataReport)
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
                    response.Message = "No se cuenta con informacion de este ciclo en la BD";
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
                    normas += x.Norma;
                });

                string sitios = "";
                praciclocronograma.Pradireccionespaproductos.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                string fechaAuditoria = "";
                fechaAuditoria = "Desde " + praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy") +
                    " Hasta " + praciclocronograma.Praciclocronogramas.First().FechaDeFinDeEjecucionAuditoria?.ToString("dd/MM/yyyy");

                Elaauditorium elaauditorium = repositoryMySql.SimpleSelect<Elaauditorium>(x => x.IdPrAcicloProgAuditoria == IdCiclo).First();
                var elahallazgos = repositoryMySql.SimpleSelect<Elahallazgo>(x => x.IdelaAuditoria == elaauditorium.IdelaAuditoria);
                string sHallazgos = string.Empty;
                elahallazgos.ToList().ForEach(x =>
                {
                    sHallazgos += $"Tipo:  {x.Tipo} {WordHelper.GetCodeKey(WordHelper.keys.enter)}";
                    sHallazgos += $"Proceso:  {x.Proceso} {WordHelper.GetCodeKey(WordHelper.keys.enter)}";
                    sHallazgos += $"Hallazgo:  {x.Hallazgo} {WordHelper.GetCodeKey(WordHelper.keys.enter)}";
                    sHallazgos += $"Sitio:  {x.Sitio} {WordHelper.GetCodeKey(WordHelper.keys.enter)}";
                    sHallazgos += $"Fecha:  {x.Fecha} {WordHelper.GetCodeKey(WordHelper.keys.enter)}";
                    sHallazgos += $"Auditor:  {x.Usuario} {WordHelper.GetCodeKey(WordHelper.keys.enter)} +{ WordHelper.GetCodeKey(WordHelper.keys.enter)}";
                });

                //EquipoAuditoNombreCargo = null,
                ///llenamos el reporte con la informacion de este ciclo
                TCPREPListaVerificacionAuditor praReporte = new TCPREPListaVerificacionAuditor
                {
                    NombreEmpresa = cliente.NombreRazon,
                    TipoAuditoria = praciclocronograma.Referencia,
                    Sitios = sitios,
                    Norma = normas,
                    Fecha = fechaAuditoria,
                    Usuario = "", //TODO: Completar
                    Cargo = "", //TODO: Completar
                    ProcesoAuditado = "", //TODO: Completar
                    NombreAuditado = "", //TODO: Completar
                    Sitio = sitios, //TODO: Completar
                    Hallazgos = sHallazgos
                };
                response.Object = new GlobalDataReport { data = praReporte, HeadersTables = null };
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<GlobalDataReport> TCPREPInformePreliminar(RequestDataReport requestDataReport)
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
                    response.Message = "No se cuenta con informacion de este ciclo en la BD";
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
                ParticipanteDetalleWS auditor = new ParticipanteDetalleWS();
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
                    planMuestreo = x.Contenido.Replace("\n", WordHelper.GetCodeKey(WordHelper.keys.enter));
                });

                var elahallazgos = repositoryMySql.SimpleSelect<Elahallazgo>(x => x.IdelaAuditoria == elaauditorium.IdelaAuditoria);
                int nroFortaleza = 0;

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

                int oportunidad = 0;
                oportunidad = elahallazgos.Count(x => x.TipoNemotico == "OM");

                int noConformidadMayor = 0;
                noConformidadMayor = elahallazgos.Count(x => x.TipoNemotico == "NCM");

                int noConformidadMenor = 0;
                noConformidadMenor = elahallazgos.Count(x => x.TipoNemotico == "NCm");


                //EquipoAuditoNombreCargo = null,
                ///llenamos el reporte con la informacion de este ciclo
                TCPREPInformePreliminar praReporte = new TCPREPInformePreliminar
                {
                    NombreEmpresa = cliente.NombreRazon,
                    TipoAuditoria = praciclocronograma.Referencia,
                    Direccion = direccion,
                    Contacto = contactos,
                    TelefonoCelular = telefono,
                    CorreoElectronico = correoElectronico,
                    CodigoServicio = praprogramasdeauditorium.CodigoServicioWs,
                    FechaInforme = DateTime.Now.ToString("dd/MM/yyyy"),
                    FechaAuditoria = fechaInicio,
                    //AuditorLider = auditor.nombreCompleto, //No se tiene
                    EquipoAuditor = equipoAuditor,
                    CriterioAuditoria = elaContenido.Contenido?.Replace("\n", WordHelper.GetCodeKey(WordHelper.keys.enter)),
                    Cont7ConModificaciones = cadenaSINO,
                    Cont7SinModificaciones = cadenaSINO1,
                    PlanMuestreo = planMuestreo,
                    RedaccionFortalezas = sHallazgosF,
                    RedaccionOportunidades = sHallazgosOM, //TODO: Completar
                    ConformidadesMenores = sHallazgosNCMe,
                    ConformidadMayores = sHallazgosNCM,
                    ComentariosIBNORCA = "", //TODO: Completar
                    SiNoDescripcion3 = "", //TODO: Completar
                    CoordinadorAuditoria = "", //TODO: Completar
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
                    ListHallazgos = new List<TCPListaHallazgos>(),
                    ListCorrecciones = praciclocronograma.Pracicloparticipantes.Select(x =>
                    {
                        TCPListCorrecciones repRep = new TCPListCorrecciones();
                        repRep.Nm_Ncm = string.Empty;
                        repRep.PuntoForma = string.Empty;
                        repRep.EvidenciaResolucion = string.Empty;
                        repRep.Resuelta = string.Empty;
                        repRep.Pendiente = string.Empty;

                        return repRep;
                    }).ToList(),
                    ListProductos = praciclocronograma.Pradireccionespaproductos.Select(x =>
                    {
                        TCPListProductos repRep = new TCPListProductos();
                        repRep.Producto = x.Nombre;
                        repRep.Normas = x.Norma;

                        return repRep;
                    }).ToList(),

                };

                TCPListaHallazgos repRepHallazo = new TCPListaHallazgos();
                repRepHallazo.Fortaleza = elahallazgos.Where(yyy => yyy.TipoNemotico == "F").Count().ToString();
                repRepHallazo.OportunidadMejora = elahallazgos.Where(yyy => yyy.TipoNemotico == "OM").Count().ToString();
                repRepHallazo.ConformidadMayor = elahallazgos.Where(yyy => yyy.TipoNemotico == "NCM").Count().ToString();
                repRepHallazo.ConformidadMenor = elahallazgos.Where(yyy => yyy.TipoNemotico == "NCm").Count().ToString();
                praReporte.ListHallazgos.Add(repRepHallazo);

                Dictionary<string, CellTitles[]> pTitles = new Dictionary<string, CellTitles[]>();
                CellTitles[] cellTitlesTitulo = new CellTitles[4];
                cellTitlesTitulo[0] = new CellTitles { Title = "Fortaleza", Visible = true, Width = "50" };
                cellTitlesTitulo[1] = new CellTitles { Title = "Oportunidad Mejora", Visible = true, Width = "120" };
                cellTitlesTitulo[2] = new CellTitles { Title = "Conformidad Mayor", Visible = true, Width = "50" };
                cellTitlesTitulo[3] = new CellTitles { Title = "Conformidad Menor", Visible = true, Width = "50" };
                pTitles.Add("ListHallazgos", cellTitlesTitulo);

                cellTitlesTitulo = new CellTitles[5];
                cellTitlesTitulo[0] = new CellTitles { Title = "Nm o Ncm", Visible = true, Width = "50" };
                cellTitlesTitulo[1] = new CellTitles { Title = "Punto Forma", Visible = true, Width = "120" };
                cellTitlesTitulo[2] = new CellTitles { Title = "Evidencia Resolucion", Visible = true, Width = "50" };
                cellTitlesTitulo[3] = new CellTitles { Title = "Resuelta", Visible = true, Width = "50" };
                cellTitlesTitulo[4] = new CellTitles { Title = "Pendiente", Visible = true, Width = "50" };
                pTitles.Add("ListCorrecciones", cellTitlesTitulo);

                cellTitlesTitulo = new CellTitles[2];
                cellTitlesTitulo[0] = new CellTitles { Title = "Producto", Visible = true, Width = "50" };
                cellTitlesTitulo[1] = new CellTitles { Title = "Norma", Visible = true, Width = "120" };
                pTitles.Add("ListProductos", cellTitlesTitulo);

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
        #endregion
    }
}
