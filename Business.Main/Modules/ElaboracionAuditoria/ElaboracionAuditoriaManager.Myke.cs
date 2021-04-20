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
        /// <summary>
        /// RRG.......
        /// </summary>
        /// <param name="IdCiclo"></param>
        /// <param name="pathPlantilla"></param>
        /// <returns></returns>
        public Response GenerarNotaSuspension(int IdCiclo, string pathPlantilla, string nombre, string cargo, DateTime fechaLiteral)
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
                RepNotaSuspensionCertifica praSuspension = new RepNotaSuspensionCertifica
                {
                    FechaCabecera = "-" + DateTime.Now.ToString("dd/MM/yyyy"),
                    CorrelativoCabecera = "--",
                    NombreNota = nombre,
                    Cargo = cargo,
                    ReferenciaNota = normas,
                    NroCertificado = "XXXX", // TODO:Pendiente 
                    NombreEmpresaTexto = cliente.NombreRazon,
                    DescripcionOtrogado = alcance,
                    Sitios = sitios,
                    FechaLiteral1 = "XXXXX", // TODO:Pendiente 
                    Seguimiento = praciclocronograma.Referencia,
                    FechaLiteral2 = fechaLiteral.ToString("dd/MM/yyyy"),
                    DirectorEjecutivo = "XXXXXX" /// TODO:Pendiente

                };
                //string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                string filePlantilla = pathPlantilla;
                WordHelper generadorWord = new WordHelper(filePlantilla);

                //generamos el documento en word
                //string fileNameGenerado = generadorWord.GenerarDocumento(praSuspension, null, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");
                string fileNameGenerado = generadorWord.GenerarDocumento(praSuspension, null, @$"c:\Salidas");
                response.Message = fileNameGenerado;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdCiclo"></param>
        /// <param name="pathPlantilla"></param>
        /// <returns></returns>
        public Response GenerarPlanAuditoria(int IdCiclo, string pathPlantilla, string objetivosAuditoria, string cambiosAlcance, string certificacion)
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

                string sitios = "";
                praciclocronograma.Pradireccionespasistemas.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                //EquipoAuditoNombreCargo = null,
                ///llenamos el reporte con la informacion de este ciclo
                REPPlanAuditoria praPlanAuditoria = new REPPlanAuditoria
                {
                    NombreEmpresa = cliente.NombreRazon,
                    CodigoServicio = praprogramasdeauditorium.CodigoIafws,
                    TipoAuditoria = praciclocronograma.Referencia,
                    ModalidadAuditoria = $"Días insitu: {praciclocronograma.Praciclocronogramas.First().DiasInsitu}, días remoto: {praciclocronograma.Praciclocronogramas.First().DiasRemoto}",
                    NormaAuditadas = normas,
                    FechaAuditoria = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
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
                    ListCronograma = praciclocronograma.Pracicloparticipantes.Select(x =>
                    {
                        RepCronogramaEquipo repRepCronograma = new RepCronogramaEquipo();
                        repRepCronograma.Fecha = string.Empty;
                        repRepCronograma.Hora = string.Empty;
                        repRepCronograma.RequisitoEsquema = string.Empty;
                        repRepCronograma.ResponsableOrganiza = string.Empty;
                        repRepCronograma.SitioAuditado = string.Empty;
                        repRepCronograma.EquipoAuditado = string.Empty;

                        return repRepCronograma;
                    }).ToList(),
                    FechaElaboracion = DateTime.Now.ToString("dd/MM/yyyy"),
                    FechaAprobacion = DateTime.Now.ToString("dd/MM/yyyy")

                };
                //string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                //string filePlantilla = pathPlantilla;
                string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                WordHelper generadorWord = new WordHelper(filePlantilla);

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
                //string fileNameGenerado = generadorWord.GenerarDocumento(praSuspension, null, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");
                string fileNameGenerado = generadorWord.GenerarDocumento(praPlanAuditoria, pTitles, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");
                response.Message = fileNameGenerado;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdCiclo"></param>
        /// <param name="pathPlantilla"></param>
        /// <returns></returns>
        public Response TCPRepDesignacionAuditoria(int IdCiclo, string pathPlantilla)
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

                string sitios = "";
                praciclocronograma.Pradireccionespasistemas.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                //EquipoAuditoNombreCargo = null,
                ///llenamos el reporte con la informacion de este ciclo
                TCPREPDesignacionAuditoria praReporte = new TCPREPDesignacionAuditoria
                {
                    TipoAuditoria = praciclocronograma.Referencia,

                };
                //string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                //string filePlantilla = pathPlantilla;
                string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                WordHelper generadorWord = new WordHelper(filePlantilla);


                //generamos el documento en word
                //string fileNameGenerado = generadorWord.GenerarDocumento(praSuspension, null, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");
                string fileNameGenerado = generadorWord.GenerarDocumento(praReporte, null, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");
                response.Message = fileNameGenerado;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdCiclo"></param>
        /// <param name="pathPlantilla"></param>
        /// <returns></returns>
        public Response TCPREPPlanAuditoria(int IdCiclo, string pathPlantilla)
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
                //praciclocronograma.Pradireccionespasistemas = repositoryMySql.SimpleSelect<Pradireccionespasistema>(y => y.IdPrAcicloProgAuditoria == praciclocronograma.IdPrAcicloProgAuditoria);


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
                Elaauditorium elaauditorium = repositoryMySql.SimpleSelect<Elaauditorium>(x => x.IdPrAcicloProgAuditoria == IdCiclo).First();
                var cronogramas = repositoryMySql.SimpleSelect<Elacronogama>(x => x.Idelaauditoria == elaauditorium.IdelaAuditoria);

                //EquipoAuditoNombreCargo = null,
                ///llenamos el reporte con la informacion de este ciclo
                TCPREPPlanAuditoria praReporte = new TCPREPPlanAuditoria
                {
                    NombreEmpresa = cliente.NombreRazon,
                    Direccion = direccion,
                    Contacto = contactos,
                    TelefonoCelular = telefono,
                    CorreoElectronico = correoElectronico,
                    CodigoServicio = praprogramasdeauditorium.CodigoIafws,
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
                    CambiosAlcances = "XXXXXXXX", ///No se tiene de donde obtener
                    Ensayos = praciclocronograma.Referencia,
                    FechaElaboracionPlan = DateTime.Now.ToString("dd/MM/yyyy"),
                    ListCronograma = cronogramas.Select(x =>
                    {
                        RepCronogramaEquipoTCP repRepCronograma = new RepCronogramaEquipoTCP();
                        repRepCronograma.Fecha = x.FechaRegistro?.ToString("dd/MM/yyyy");
                        repRepCronograma.Hora = x.Horario;
                        repRepCronograma.RequisitoEsquema = x.RequisitosEsquema;
                        repRepCronograma.ResponsableOrganiza = x.PersonaEntrevistadaCargo;
                        repRepCronograma.SitioAuditado = repositoryMySql.SimpleSelect<Pradireccionespaproducto>(y => y.IdDireccionPaproducto == x.IdDireccionPaproducto).First().Direccion;
                        repRepCronograma.EquipoAuditado = x.Auditor;

                        return repRepCronograma;
                    }).ToList(),
                };
                //string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                //string filePlantilla = pathPlantilla;
                string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                WordHelper generadorWord = new WordHelper(filePlantilla);

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
                //string fileNameGenerado = generadorWord.GenerarDocumento(praSuspension, null, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");
                string fileNameGenerado = generadorWord.GenerarDocumento(praReporte, pTitles, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");
                response.Message = fileNameGenerado;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdCiclo"></param>
        /// <param name="pathPlantilla"></param>
        /// <returns></returns>
        public Response TCPREPListaVerificacionReunionApertura(int IdCiclo, string pathPlantilla)
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

                string sitios = "";
                praciclocronograma.Pradireccionespasistemas.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                //EquipoAuditoNombreCargo = null,
                ///llenamos el reporte con la informacion de este ciclo
                TCPREPListaVerificacionReunionApertura praReporte = new TCPREPListaVerificacionReunionApertura
                {
                    NombreEmpresa = cliente.NombreRazon,
                    TipoAuditoria = praciclocronograma.Referencia,

                };
                //string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                //string filePlantilla = pathPlantilla;
                string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                WordHelper generadorWord = new WordHelper(filePlantilla);


                //generamos el documento en word
                //string fileNameGenerado = generadorWord.GenerarDocumento(praSuspension, null, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");
                string fileNameGenerado = generadorWord.GenerarDocumento(praReporte, null, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");
                response.Message = fileNameGenerado;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdCiclo"></param>
        /// <param name="pathPlantilla"></param>
        /// <returns></returns>
        public Response TCPREPListaVerificacionReunionCierre(int IdCiclo, string pathPlantilla)
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

                string sitios = "";
                praciclocronograma.Pradireccionespasistemas.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                //EquipoAuditoNombreCargo = null,
                ///llenamos el reporte con la informacion de este ciclo
                TCPREPListaVerificacionReunionCierre praReporte = new TCPREPListaVerificacionReunionCierre
                {
                    NombreEmpresa = cliente.NombreRazon,
                    TipoAuditoria = praciclocronograma.Referencia,

                };
                //string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                //string filePlantilla = pathPlantilla;
                string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                WordHelper generadorWord = new WordHelper(filePlantilla);
                //generamos el documento en word
                //string fileNameGenerado = generadorWord.GenerarDocumento(praSuspension, null, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");
                string fileNameGenerado = generadorWord.GenerarDocumento(praReporte, null, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");
                response.Message = fileNameGenerado;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdCiclo"></param>
        /// <param name="pathPlantilla"></param>
        /// <returns></returns>
        public Response TCPREPListaAsistencia(int IdCiclo, string pathPlantilla)
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

                };
                //string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                //string filePlantilla = pathPlantilla;
                string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                WordHelper generadorWord = new WordHelper(filePlantilla);
                //generamos el documento en word
                //string fileNameGenerado = generadorWord.GenerarDocumento(praSuspension, null, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");
                string fileNameGenerado = generadorWord.GenerarDocumento(praReporte, null, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");
                response.Message = fileNameGenerado;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdCiclo"></param>
        /// <param name="pathPlantilla"></param>
        /// <returns></returns>
        public Response TCPREPActaMuestreo(int IdCiclo, string pathPlantilla)
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

                string sitios = "";
                praciclocronograma.Pradireccionespasistemas.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                //EquipoAuditoNombreCargo = null,
                ///llenamos el reporte con la informacion de este ciclo
                TCPREPActaMuestreo praReporte = new TCPREPActaMuestreo
                {
                    NombreEmpresa = cliente.NombreRazon,
                    TipoAuditoria = praciclocronograma.Referencia,

                };
                //string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                //string filePlantilla = pathPlantilla;
                string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                WordHelper generadorWord = new WordHelper(filePlantilla);
                //generamos el documento en word
                //string fileNameGenerado = generadorWord.GenerarDocumento(praSuspension, null, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");
                string fileNameGenerado = generadorWord.GenerarDocumento(praReporte, null, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");
                response.Message = fileNameGenerado;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdCiclo"></param>
        /// <param name="pathPlantilla"></param>
        /// <returns></returns>
        public Response TCPREPListaVerificacionAuditor(int IdCiclo, string pathPlantilla)
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

                string sitios = "";
                praciclocronograma.Pradireccionespasistemas.ToList().ForEach(x =>
                {
                    sitios += x.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                //EquipoAuditoNombreCargo = null,
                ///llenamos el reporte con la informacion de este ciclo
                TCPREPListaVerificacionAuditor praReporte = new TCPREPListaVerificacionAuditor
                {
                    NombreEmpresa = cliente.NombreRazon,
                    TipoAuditoria = praciclocronograma.Referencia,

                };
                //string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                //string filePlantilla = pathPlantilla;
                string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                WordHelper generadorWord = new WordHelper(filePlantilla);
                //generamos el documento en word
                //string fileNameGenerado = generadorWord.GenerarDocumento(praSuspension, null, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");
                string fileNameGenerado = generadorWord.GenerarDocumento(praReporte, null, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");
                response.Message = fileNameGenerado;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdCiclo"></param>
        /// <param name="pathPlantilla"></param>
        /// <returns></returns>
        public Response TCPREPInformePreliminar(int IdCiclo, string pathPlantilla)
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
                var contenidos = repositoryMySql.SimpleSelect<Elacontenidoauditorium>(x=>x.IdelaAuditoria == elaauditorium.IdelaAuditoria);
                Elacontenidoauditorium elaContenido = new Elacontenidoauditorium();
                elaContenido = contenidos.Where(x => x.Nemotico == "PLAN_CRITERIO").First();
                string cadenaSINO;
                contenidos.Where(x => x.Nemotico == "INFPRE_DESVIACION").ToList().ForEach(x=>{
                    cadenaSINO = x.Label + " " + x.Contenido + (x.Seleccionado == 1 ? WordHelper.GetCodeKey(WordHelper.keys.enter):WordHelper.GetCodeKey(WordHelper.keys.enter));
                });


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
                    CodigoServicio = praprogramasdeauditorium.CodigoIafws,
                    FechaInicio = fechaInicio,
                    Fecha = DateTime.Now.ToString("dd/MM/yyyy"),
                    //AuditorLider = auditor.nombreCompleto, //No se tiene
                    EquipoAuditor = equipoAuditor,
                    CriterioAuditoria = elaContenido.Contenido,



                    ListHallazgos = praciclocronograma.Pracicloparticipantes.Select(x =>
                    {
                        TCPListaHallazgos repRep = new TCPListaHallazgos();
                        repRep.Fortaleza = string.Empty;
                        repRep.OportunidadMejora = string.Empty;
                        repRep.ConformidadMayor = string.Empty;
                        repRep.ConformidadMenor = string.Empty;


                        return repRep;
                    }).ToList(),
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
                //string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                //string filePlantilla = pathPlantilla;
                string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                WordHelper generadorWord = new WordHelper(filePlantilla);

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

                //generamos el documento en word
                //string fileNameGenerado = generadorWord.GenerarDocumento(praSuspension, null, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");
                string fileNameGenerado = generadorWord.GenerarDocumento(praReporte, pTitles, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");
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
