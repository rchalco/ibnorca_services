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

namespace Business.Main.Modules.ElaboracionAuditoria
{
	public partial class ElaboracionAuditoriaManager
	{
        public Response GenerarDescisionFavorableCertificacion(int IdCiclo, string pathPlantilla,string nombre,string cargo)
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
                    FechaIbnorca = DateTime.Now.ToString("dd/MM/yyyy"),
                    ReferenciaIbnorca= praciclocronograma.Referencia,
                    NombreApellidos=nombre,
                    Cargo=cargo,
                    Organizacion=cliente.NombreRazon,
                    NbIso=normas,
                    OtorgarRenovarMantener= praciclocronograma.Referencia,
                    Etapa=praciclocronograma.Referencia,
                    NroCertificacion ="pendiente "


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



    }

}
