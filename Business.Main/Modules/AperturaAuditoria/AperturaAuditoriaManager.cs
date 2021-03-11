using Business.Main.Base;
using Business.Main.Cross;
using Business.Main.DataMapping;
using Business.Main.Modules.ApeeturaAuditoria.Domain;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.BuscarNormaxCodigoDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.BuscarPaisDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.BuscarxIdClienteEmpresaDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.CiudadesDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.DatosPropuestaDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.DatosServicioDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.EstadosDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.ListarAuditoresxCargoCalificadoDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.ListarCargosCalificadosDTO;
using CoreAccesLayer.Wraper;
using Domain.Main.AperturaAuditoria;
using Domain.Main.Wraper;
using Newtonsoft.Json;
using PlumbingProps.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.ApeeturaAuditoria
{
    public class AperturaAuditoriaManager : BaseManager
    {
        public ResponseObject<Praprogramasdeauditorium> RegisterProgramaAuditoria(Praprogramasdeauditorium req)
        {
            ResponseObject<Praprogramasdeauditorium> response = new ResponseObject<Praprogramasdeauditorium>();
            try
            {
                //Logica del negocio
                if (req == null)
                {
                    response.State = ResponseType.Warning;
                    response.Message = "el parametro de la persona a registrar no debe ser nulo";
                    response.Object = null;
                    response.Code = "404";
                    return response;
                }
                //full validacion

                Entity<Praprogramasdeauditorium> entity = new Entity<Praprogramasdeauditorium> { EntityDB = req, stateEntity = StateEntity.add };
                if (req.IdPrAprogramaAuditoria != 0)
                {
                    entity.stateEntity = StateEntity.modify;
                }

                repositoryMySql.SaveObject<Praprogramasdeauditorium>(entity);

                response.State = ResponseType.Success;
                response.Message = "El programa fue grabado";
                response.Object = req;
                response.Code = "000";

            }


            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        public ResponseObject<Praprogramasdeauditorium> ObtenerProgramaAuditoria(int pIdServicio, string pUsuario)
        {
            ResponseObject<Praprogramasdeauditorium> resul = new ResponseObject<Praprogramasdeauditorium> { Object = new Praprogramasdeauditorium(), Code = "000", Message = "Programa obtenido correctamente", State = ResponseType.Success };
            try
            {
                var resulDB = repositoryMySql.GetDataByProcedure<Praprogramasdeauditorium>("spGetProgramaAuditoriaByIdServicio", pIdServicio.ToString());
                if (resulDB.Count == 0)
                {
                    AperturaAuditoriaManager objProgramaAudi = new AperturaAuditoriaManager();
                    ComplexProgramaAuditoria objComplex = new ComplexProgramaAuditoria();

                    #region Consumo de servicios
                    ClientHelper clientHelper = new ClientHelper();

                    ///TDO: obtenemos los datos del servicio
                    RequestDatosServicio requestDato = new RequestDatosServicio { accion = "DatosServicio", IdServicio = pIdServicio, sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES };
                    ResponseDatosServicio resulServices = clientHelper.Consume<ResponseDatosServicio>(Global.URIGLOBAL_SERVICES + Global.URI_SERVICIO, requestDato).Result;
                    if (!resulServices.estado)
                    {
                        resul.State = ResponseType.Warning;
                        resul.Message = $"Existe problemas al consumir el servicio de ibnorca (DatosServicio): {resulServices.mensaje}";
                        return resul;
                    }

                    ///TDO obtenemos los datos del cliente
                    RequestBusquedaCliente requestBusquedaCliente = new RequestBusquedaCliente { accion = "BuscarxIdClienteEmpresa", sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES, IdCliente = resulServices.DatosServicio.IdCliente };
                    ResponseBusquedaCliente responseBusquedaCliente = clientHelper.Consume<ResponseBusquedaCliente>(Global.URIGLOBAL_SERVICES + Global.URI_CLIENTE, requestBusquedaCliente).Result;
                    if (!responseBusquedaCliente.estado || responseBusquedaCliente.totalResultados <= 0)
                    {
                        resul.State = ResponseType.Warning;
                        resul.Message = $"Existe problemas al consumir el servicio de ibnorca (BuscarxIdClienteEmpresa): {responseBusquedaCliente.mensaje}";
                        return resul;
                    }

                    ///TDO obtenemos los datos de la propuesta para los ciclos
                    RequestDatosPropuestaDTO requestDatosPropuestaDTO = new RequestDatosPropuestaDTO { accion = "DatosPropuesta", sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES, IdPropuesta = resulServices.DatosServicio.idPropuesta };
                    ResponseDatosPropuestaDTO responseDatosPropuestaDTO = clientHelper.Consume<ResponseDatosPropuestaDTO>(Global.URIGLOBAL_SERVICES + Global.URI_SIMULACION, requestDatosPropuestaDTO).Result;
                    if (!responseDatosPropuestaDTO.estado)
                    {
                        resul.State = ResponseType.Warning;
                        resul.Message = $"Existe problemas al consumir el servicio de ibnorca (DatosPropuesta): {responseDatosPropuestaDTO.mensaje}";
                        return resul;
                    }
                    #endregion

                    Praprogramasdeauditorium objPrograma = new Praprogramasdeauditorium
                    {
                        IdparamArea = Convert.ToInt32(resulServices.DatosServicio.IdArea),
                        DetalleServicioWs = JsonConvert.SerializeObject(resulServices.DatosServicio),
                        Fecha = resulServices.DatosServicio.fecharegistro,
                        Oficina = resulServices.DatosServicio.oficina,
                        IdOrganizacionWs = resulServices.DatosServicio.IdCliente,
                        OrganizacionContentWs = JsonConvert.SerializeObject(responseBusquedaCliente.resultados[0]),
                        Nit = responseBusquedaCliente.resultados[0].NIT,
                        CodigoServicioWs = resulServices.DatosServicio.Codigo,
                        IdparamTipoServicio = 1,/*CERTIFICACION - RENOVACION*///no se tiene del servicio
                        CodigoIafws = resulServices.DatosServicio.cod_iaf_primario,
                        NumeroAnios = 0,
                        Estado = "INICIAL",
                        UsuarioRegistro = pUsuario,
                        FechaDesde = DateTime.Now,
                        FechaHasta = null
                    };
                    string mode = objPrograma.IdparamArea == 38 ? "TCS" : "TCP";
                    responseDatosPropuestaDTO.ListaServicios.ForEach(x =>
                    {
                        if (!objPrograma.Praciclosprogauditoria.Any(yy => yy.Anio == (short)Convert.ToInt32(x.cod_anio)))
                        {
                            Praciclosprogauditorium ciclosprogauditorium = new Praciclosprogauditorium
                            {
                                UsuarioRegistro = pUsuario,
                                FechaDesde = DateTime.Now,
                                FechaHasta = null,
                                Anio = (short)Convert.ToInt32(x.cod_anio),
                                Referencia = x.descripcion,
                                IdparamTipoAuditoria = 1,
                                NombreOrganizacionCertificado = responseBusquedaCliente.resultados[0].NombreRazon,
                                EstadoDescripcion = "SIN FECHA DE AUDITORIA"
                            };

                            ///TDO: TCP - Cert. de Productos 
                            if (mode.Equals("TCP"))
                            {
                                ciclosprogauditorium.Pradireccionespaproductos = new List<Pradireccionespaproducto>();
                                resulServices.DatosServicio.ListaProducto.ForEach(dir =>
                                {
                                    Pradireccionespaproducto objDirProd = new Pradireccionespaproducto
                                    {
                                        Nombre = dir.nombre,
                                        Direccion = dir.direccion,
                                        Marca = dir.marca,
                                        Sello = dir.nro_sello,
                                        Ciudad = dir.ciudad,
                                        Estado = dir.estado,
                                        Pais = dir.pais,
                                        Norma = dir.norma,
                                        FechaEmisionPrimerCertificado = null,
                                        FechaVencimientoUltimoCertificado = null,
                                        FechaVencimientoCertificado = null,
                                        UsuarioRegistro = pUsuario,
                                        FechaDesde = DateTime.Now,
                                        FechaHasta = null
                                    };
                                    ciclosprogauditorium.Pradireccionespaproductos.Add(objDirProd);
                                });
                            }


                            ///TDO: TCS - Cert.Sistemas de Gestion
                            if (mode.Equals("TCS"))
                            {
                                ///TDO: direcciones
                                ciclosprogauditorium.Pradireccionespasistemas = new List<Pradireccionespasistema>();
                                resulServices.DatosServicio.ListaDireccion.ForEach(dir =>
                                {
                                    Pradireccionespasistema objDirSis = new Pradireccionespasistema
                                    {
                                        Ciudad = dir.ciudad,
                                        Departamento = dir.estado,
                                        Dias = 0,
                                        Direccion = dir.direccion,
                                        FechaDesde = DateTime.Now,
                                        FechaHasta = null,
                                        Pais = dir.pais,
                                        UsuarioRegistro = pUsuario,
                                        Nombre = dir.nombre
                                    };
                                    ciclosprogauditorium.Pradireccionespasistemas.Add(objDirSis);
                                });

                                ///TDO: normas
                                ciclosprogauditorium.Praciclonormassistemas = new List<Praciclonormassistema>();
                                ciclosprogauditorium.Praciclonormassistemas.Add(new Praciclonormassistema
                                {
                                    Alcance = resulServices.DatosServicio.alcance_propuesta,
                                    IdparamNorma = null,
                                    Norma = "S/A",
                                    FechaDesde = DateTime.Now,
                                    FechaEmisionPrimerCertificado = null,
                                    FechaHasta = null,
                                    FechaVencimientoUltimoCertificado = null,
                                    NumeroDeCertificacion = "",
                                    UsuarioRegistro = pUsuario
                                });
                            }

                            ///TDO: Cronograma 
                            Praciclocronograma cronograma = new Praciclocronograma
                            {
                                CantidadDeDiasTotal = (int)Convert.ToDecimal(x.cantidad),
                                FechaDeFinDeEjecucionAuditoria = null,
                                FechaDesde = DateTime.Now,
                                FechaHasta = null,
                                FechaInicioDeEjecucionDeAuditoria = null,
                                MesProgramado = CalcularMesProgramado(mode, Convert.ToInt32(x.cod_anio)),
                                MesReprogramado = null,
                                UsuarioRegistro = pUsuario
                            };
                            ciclosprogauditorium.Praciclocronogramas = new List<Praciclocronograma>();
                            ciclosprogauditorium.Praciclocronogramas.Add(cronograma);

                            ///TDO: lista de personal
                            ciclosprogauditorium.Pracicloparticipantes = new List<Pracicloparticipante>();
                            responseDatosPropuestaDTO.ListaAuditores.ForEach(auditor =>
                            {
                                if (Convert.ToInt32(auditor.cod_anio) == ciclosprogauditorium.Anio)
                                {
                                    Pracicloparticipante participante = new Pracicloparticipante
                                    {
                                        FechaDesde = DateTime.Now,
                                        CargoDetalleWs = JsonConvert.SerializeObject(auditor),
                                        IdCargoWs = Convert.ToInt32(auditor.cod_tipoauditor),
                                        IdParticipanteWs = null,
                                        ParticipanteDetalleWs = null,
                                        UsuarioRegistro = pUsuario
                                    };
                                    ciclosprogauditorium.Pracicloparticipantes.Add(participante);
                                }

                            });
                            objPrograma.Praciclosprogauditoria.Add(ciclosprogauditorium);
                        }

                    });

                    ///Inserta el programa de auditoria
                    Entity<Praprogramasdeauditorium> entity = new Entity<Praprogramasdeauditorium> { EntityDB = objPrograma, stateEntity = StateEntity.add };
                    repositoryMySql.SaveObject<Praprogramasdeauditorium>(entity);
                    objPrograma.IdPrAprogramaAuditoria = entity.EntityDB.IdPrAprogramaAuditoria;
                    resul.Object = objPrograma;
                }
                else
                {
                    resul.Object = resulDB[0];
                    resul.Object.Praciclosprogauditoria = repositoryMySql.SimpleSelect<Praciclosprogauditorium>(x => x.IdPrAprogramaAuditoria == resul.Object.IdPrAprogramaAuditoria);
                    List<Praciclosprogauditorium> lAuxiliar = resul.Object.Praciclosprogauditoria.ToList();
                    lAuxiliar.ForEach(x =>
                    {
                        x.Praciclocronogramas = repositoryMySql.SimpleSelect<Praciclocronograma>(y => y.IdPrAcicloProgAuditoria == x.IdPrAcicloProgAuditoria);
                        x.Praciclonormassistemas = repositoryMySql.SimpleSelect<Praciclonormassistema>(y => y.IdPrAcicloProgAuditoria == x.IdPrAcicloProgAuditoria);
                        x.Pracicloparticipantes = repositoryMySql.SimpleSelect<Pracicloparticipante>(y => y.IdPrAcicloProgAuditoria == x.IdPrAcicloProgAuditoria);
                        x.Pradireccionespaproductos = repositoryMySql.SimpleSelect<Pradireccionespaproducto>(y => y.IdPrAcicloProgAuditoria == x.IdPrAcicloProgAuditoria);
                        x.Pradireccionespasistemas = repositoryMySql.SimpleSelect<Pradireccionespasistema>(y => y.IdPrAcicloProgAuditoria == x.IdPrAcicloProgAuditoria);
                    });
                    resul.Object.Praciclosprogauditoria = lAuxiliar;
                }
                resul.State = ResponseType.Success;

            }
            catch (Exception ex)
            {
                ProcessError(ex, resul);
            }
            return resul;
        }

        public ResponseQuery<ListaCargosCalificados> ObtenerCargos()
        {
            ResponseQuery<ListaCargosCalificados> response = new ResponseQuery<ListaCargosCalificados> { Message = "Cargos obtenidos correctamente.", State = ResponseType.Success };
            try
            {
                ClientHelper clientHelper = new ClientHelper();
                ///TDO: obtenemos los datos del servicio
                RequestListarCargosCalificados requestDato = new RequestListarCargosCalificados { accion = "ListarCargosCalificados", sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES };
                ResponseListarCargosCalificados resulServices = clientHelper.Consume<ResponseListarCargosCalificados>(Global.URIGLOBAL_SERVICES + Global.URI_CARGOS, requestDato).Result;
                if (!resulServices.estado)
                {
                    response.State = ResponseType.Warning;
                    response.Message = $"Existe problemas al consumir el servicio de ibnorca (ListarCargosCalificados): {resulServices.mensaje}";
                    return response;
                }
                response.ListEntities = resulServices.ListaCargosCalificados;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        public ResponseQuery<ListaCalificado> BuscarPersonalCargos(int IdCargoCalificado)
        {
            ResponseQuery<ListaCalificado> response = new ResponseQuery<ListaCalificado> { Message = "Cargos obtenidos obtenido correctamente.", State = ResponseType.Success };
            try
            {
                ClientHelper clientHelper = new ClientHelper();
                ///TDO: obtenemos los datos del servicio
                RequestListarAuditoresxCargoCalificado requestDato = new RequestListarAuditoresxCargoCalificado { accion = "ListarAuditoresxCargoCalificado", sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES, IdCargoCalificado = IdCargoCalificado };
                ResponseListarAuditoresxCargoCalificado resulServices = clientHelper.Consume<ResponseListarAuditoresxCargoCalificado>(Global.URIGLOBAL_SERVICES + Global.URI_CARGOS, requestDato).Result;
                if (!resulServices.estado)
                {
                    response.State = ResponseType.Warning;
                    response.Message = $"Existe problemas al consumir el servicio de ibnorca (ListarAuditoresxCargoCalificado): {resulServices.mensaje}";
                    return response;
                }
                response.ListEntities = resulServices.ListaCalificados;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        public ResponseQuery<Norma> BuscarNormas(string Codigo)
        {
            ResponseQuery<Norma> response = new ResponseQuery<Norma> { Message = "Parametros obtenidos correctamente.", State = ResponseType.Success, ListEntities = new List<Norma>() };
            try
            {
                ClientHelper clientHelper = new ClientHelper();
                ///TDO: obtenemos los datos del servicio
                RequestBuscarNormaxCodigo requestDato = new RequestBuscarNormaxCodigo { accion = "BuscarNormaxCodigo", sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES, Codigo = Codigo };
                ResponseBuscarNormaxCodigo resulServices = clientHelper.Consume<ResponseBuscarNormaxCodigo>(Global.URIGLOBAL_SERVICES + Global.URI_NORMAS, requestDato).Result;
                if (!resulServices.estado)
                {
                    response.State = ResponseType.Warning;
                    response.Message = $"Existe problemas al consumir el servicio de ibnorca (BuscarNormaxCodigo): {resulServices.mensaje}";
                    return response;
                }
                response.ListEntities = resulServices.resultado;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        public ResponseQuery<Pais> BuscarPais(string pais)
        {
            ResponseQuery<Pais> response = new ResponseQuery<Pais> { Message = "Parametros obtenidos correctamente.", State = ResponseType.Success, ListEntities = new List<Pais>() };
            try
            {
                ClientHelper clientHelper = new ClientHelper();
                ///TDO: obtenemos los datos del servicio
                RequestBuscarPais requestDato = new RequestBuscarPais { accion = "BuscarPais", sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES, palabra = pais };
                ResponseBuscarPais resulServices = clientHelper.Consume<ResponseBuscarPais>(Global.URIGLOBAL_SERVICES + Global.URI_PAISES, requestDato).Result;
                if (!resulServices.estado)
                {
                    response.State = ResponseType.Warning;
                    response.Message = $"Existe problemas al consumir el servicio de ibnorca (BuscarPais): {resulServices.mensaje}";
                    return response;
                }
                response.ListEntities = resulServices.resultado;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseQuery<Estado> BuscarEstado(string IdPais)
        {
            ResponseQuery<Estado> response = new ResponseQuery<Estado> { Message = "Parametros obtenidos correctamente.", State = ResponseType.Success, ListEntities = new List<Estado>() };
            try
            {
                ClientHelper clientHelper = new ClientHelper();
                ///TDO: obtenemos los datos del servicio
                RequestEstados requestDato = new RequestEstados { accion = "", sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES, IdPais = IdPais, TipoLista = "estados" };
                ResponseEstados resulServices = clientHelper.Consume<ResponseEstados>(Global.URIGLOBAL_SERVICES + Global.URI_PAISES, requestDato).Result;
                if (!resulServices.estado)
                {
                    response.State = ResponseType.Warning;
                    response.Message = $"Existe problemas al consumir el servicio de ibnorca (estados): {resulServices.mensaje}";
                    return response;
                }
                response.ListEntities = resulServices.lista;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseQuery<Ciudad> BuscarCiudad(string IdEstado)
        {
            ResponseQuery<Ciudad> response = new ResponseQuery<Ciudad> { Message = "Parametros obtenidos correctamente.", State = ResponseType.Success, ListEntities = new List<Ciudad>() };
            try
            {
                ClientHelper clientHelper = new ClientHelper();
                ///TDO: obtenemos los datos del servicio
                RequestCiudades requestDato = new RequestCiudades { accion = "", sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES, IdEstado = IdEstado, TipoLista = "ciudades" };
                ResponseCiudades resulServices = clientHelper.Consume<ResponseCiudades>(Global.URIGLOBAL_SERVICES + Global.URI_PAISES, requestDato).Result;
                if (!resulServices.estado)
                {
                    response.State = ResponseType.Warning;
                    response.Message = $"Existe problemas al consumir el servicio de ibnorca (estados): {resulServices.mensaje}";
                    return response;
                }
                response.ListEntities = resulServices.lista;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseObject<ComplexParametricas> GetParametricas(ComplexParametricas req)
        {
            ResponseObject<ComplexParametricas> response = new ResponseObject<ComplexParametricas> { Message = "Parametros obtenidos correctamente.", State = ResponseType.Success, Object = new ComplexParametricas() };
            try
            {
                response.Object.ListCargosParticipante = repositoryMySql.GetDataByProcedure<Paramcargosparticipante>("spGetCargosParticipante", 1);
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        private DateTime CalcularMesProgramado(string area, int año)
        {
            DateTime resul = DateTime.Now;
            resul = resul.AddMonths(9 * año);
            if (area.Equals("TCS"))
            {
                resul = resul.AddMonths(1);
            }
            return resul;
        }


    }
}
