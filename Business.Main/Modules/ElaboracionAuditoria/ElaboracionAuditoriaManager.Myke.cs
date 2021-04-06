using Business.Main.Cross;
using Business.Main.DataMapping;
using Business.Main.DataMapping.DTOs;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.BuscarxIdClienteEmpresaDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.ListarAuditoresxCargoCalificadoDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.ListarContactosEmpresaDTO;
using Business.Main.Modules.ElaboracionAuditoria.Reportes.TCS;
using Business.Main.Modules.ElaboracionAuditoria.Resportes.TCS;
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
                        RepCronogramaEquipo repRepCronograma = new RepCronogramaEquipo();
                        repRepCronograma.EquipoAuditor = string.Empty;
                        if (!string.IsNullOrEmpty(x.CargoDetalleWs))
                        {
                            ListaCalificado cargo = JsonConvert.DeserializeObject<ListaCalificado>(x.CargoDetalleWs);
                            repRepCronograma.EquipoAuditor = cargo.CargoPuesto;
                        }

                        repRepCronograma.NombreCompleto = string.Empty;
                        if (!string.IsNullOrEmpty(x.ParticipanteDetalleWs))
                        {
                            ListaCalificado participante = JsonConvert.DeserializeObject<ListaCalificado>(x.ParticipanteDetalleWs);
                            repRepCronograma.NombreCompleto = participante.NombreCompleto;
                        }
                        repRepCronograma.TotalDiasInSitu = Convert.ToString(x.DiasInsistu);
                        repRepCronograma.TotalDiasRemoto = Convert.ToString(x.DiasRemoto);


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
    }
}
