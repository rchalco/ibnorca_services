using Business.Main.Base;
using Business.Main.Cross;
using Business.Main.DataMapping;
using Business.Main.DataMapping.DTOs;
using Business.Main.Modules.ApeeturaAuditoria.Domain;
using Business.Main.Modules.AperturaAuditoria.Domain;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.BuscarClasificadorDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.BuscarNormaIntxCodigoDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.BuscarNormaxCodigoDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.BuscarPaisDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.BuscarxIdClienteEmpresaDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.CiudadesDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.DatosPropuestaDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.DatosServicioDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.EstadosDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.ListaCertificadosxClienteyTipoDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.ListarAuditoresxCargoCalificadoDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.ListarCargosCalificadosDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.ListarContactosEmpresaDTO;
using Business.Main.Modules.AperturaAuditoria.Domain.SP.DTOs;
using CoreAccesLayer.Wraper;
using Domain.Main.AperturaAuditoria;
using Domain.Main.Wraper;
using ExcelDataReader;
using Newtonsoft.Json;
using PlumbingProps.Document;
using PlumbingProps.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PlumbingProps.Document.WordHelper;

namespace Business.Main.Modules.ApeeturaAuditoria
{
    public class AperturaAuditoriaManager : BaseManager
    {
        public ResponseObject<Praprogramasdeauditorium> RegisterProgramaAuditoria(Praprogramasdeauditorium req)
        {
            ResponseObject<Praprogramasdeauditorium> response = new ResponseObject<Praprogramasdeauditorium> { State = ResponseType.Success };
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

                ///validamos la cantidad de dias 

                req.Praciclosprogauditoria.ToList().ForEach(x =>
                {
                    decimal totalDiasCronograma = Convert.ToDecimal(x.Praciclocronogramas.ToList().First().DiasRemoto + x.Praciclocronogramas.ToList().First().DiasInsitu);
                    decimal totalDiasAuditor = Convert.ToDecimal(x.Pracicloparticipantes.ToList().Where(yy => yy.IdCargoWs == 2408 || yy.IdCargoWs == 2409).Sum(zz => zz.DiasInsistu + zz.DiasRemoto));
                    if (totalDiasCronograma != totalDiasAuditor)
                    {
                        response.State = ResponseType.Warning;
                        response.Message = $"Los dias del cronograma no coinciden con los del auditor en el año {x.Anio }";

                    }
                });

                //if (response.State != ResponseType.Success)
                //{
                //    return response;
                //}

                Entity<Praprogramasdeauditorium> entity = new Entity<Praprogramasdeauditorium> { EntityDB = req, stateEntity = StateEntity.add };
                if (req.IdPrAprogramaAuditoria != 0)
                {
                    entity.stateEntity = StateEntity.modify;
                }
                ///tratamos a  los hijos para la designacion de llaves
                req.Praciclosprogauditoria.ToList().ForEach(ciclo =>
                {
                    ciclo.Praciclocronogramas?.ToList().ForEach(cronograma =>
                    {
                        cronograma.IdPrAcicloProgAuditoria = ciclo.IdPrAcicloProgAuditoria;
                        if (cronograma.FechaInicioDeEjecucionDeAuditoria != null)
                        {
                            ciclo.EstadoDescripcion = "Con fecha de auditoría";
                        }
                    });
                    ciclo.Praciclonormassistemas?.ToList().ForEach(norma => { norma.IdPrAcicloProgAuditoria = ciclo.IdPrAcicloProgAuditoria; });
                    ciclo.Pracicloparticipantes?.ToList().ForEach(participante => { participante.IdPrAcicloProgAuditoria = ciclo.IdPrAcicloProgAuditoria; });
                    ciclo.Pradireccionespaproductos?.ToList().ForEach(producto => { producto.IdPrAcicloProgAuditoria = ciclo.IdPrAcicloProgAuditoria; });
                    ciclo.Pradireccionespasistemas?.ToList().ForEach(sistema => { sistema.IdPrAcicloProgAuditoria = ciclo.IdPrAcicloProgAuditoria; });
                });

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
        //public ResponseObject<Praprogramasdeauditorium> ActualizarCicloAuditoria(int pIdCiclo, string pUsuario)
        //{
        //    ResponseObject<Praprogramasdeauditorium> resul = new ResponseObject<Praprogramasdeauditorium> { Object = new Praprogramasdeauditorium(), Code = "000", Message = "Programa obtenido correctamente", State = ResponseType.Success };
        //    try
        //    {
        //        var vPraIdServicio = repositoryMySql.GetDataByProcedure<PraIdServicio>("GetIdServicioByIdCiclo", pIdCiclo);
        //        if (vPraIdServicio.Count == 0)
        //        {
        //            resul.State = ResponseType.Warning;
        //            resul.Message = $"Id de Ciclo no valido, no se cuenta con informacion del programa para: {pIdCiclo}";
        //            return resul;
        //        }
        //        string pIdServicio = vPraIdServicio.First().IdServicio;
        //        var resulDB = repositoryMySql.GetDataByProcedure<Praprogramasdeauditorium>("spGetProgramaAuditoriaByIdServicio", pIdServicio);

        //        AperturaAuditoriaManager objProgramaAudi = new AperturaAuditoriaManager();
        //        ComplexProgramaAuditoria objComplex = new ComplexProgramaAuditoria();

        //        #region Consumo de servicios
        //        ClientHelper clientHelper = new ClientHelper();

        //        ///TDO: obtenemos los datos del servicio
        //        RequestDatosServicio requestDato = new RequestDatosServicio { accion = "DatosServicio", IdServicio = Convert.ToInt32(pIdServicio), sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES };
        //        ResponseDatosServicio resulServices = clientHelper.Consume<ResponseDatosServicio>(Global.URIGLOBAL_SERVICES + Global.URI_SERVICIO, requestDato).Result;
        //        if (!resulServices.estado)
        //        {
        //            resul.State = ResponseType.Warning;
        //            resul.Message = $"Existe problemas al consumir el servicio de ibnorca (DatosServicio): {resulServices.mensaje}";
        //            return resul;
        //        }

        //        ///TDO obtenemos los datos del cliente
        //        RequestBusquedaCliente requestBusquedaCliente = new RequestBusquedaCliente { accion = "BuscarxIdClienteEmpresa", sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES, IdCliente = resulServices.DatosServicio.IdCliente };
        //        ResponseBusquedaCliente responseBusquedaCliente = clientHelper.Consume<ResponseBusquedaCliente>(Global.URIGLOBAL_SERVICES + Global.URI_CLIENTE, requestBusquedaCliente).Result;
        //        if (!responseBusquedaCliente.estado || responseBusquedaCliente.totalResultados <= 0)
        //        {
        //            resul.State = ResponseType.Warning;
        //            resul.Message = $"Existe problemas al consumir el servicio de ibnorca (BuscarxIdClienteEmpresa): {responseBusquedaCliente.mensaje}";
        //            return resul;
        //        }

        //        ///TDO obtenemos los datos de la propuesta para los ciclos
        //        RequestDatosPropuestaDTO requestDatosPropuestaDTO = new RequestDatosPropuestaDTO { accion = "DatosPropuesta", sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES, IdPropuesta = resulServices.DatosServicio.idPropuesta };
        //        ResponseDatosPropuestaDTO responseDatosPropuestaDTO = clientHelper.Consume<ResponseDatosPropuestaDTO>(Global.URIGLOBAL_SERVICES + Global.URI_SIMULACION, requestDatosPropuestaDTO).Result;
        //        if (!responseDatosPropuestaDTO.estado)
        //        {
        //            resul.State = ResponseType.Warning;
        //            resul.Message = $"Existe problemas al consumir el servicio de ibnorca (DatosPropuesta): {responseDatosPropuestaDTO.mensaje}";
        //            return resul;
        //        }

        //        ///TDO obtenemos de los certificados del cliente
        //        RequestListaCertificadosxClienteyTipo requestListaCertificadosxClienteyTipoDTO = new RequestListaCertificadosxClienteyTipo { accion = "ListaCertificadosxClienteyTipo", sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES, IdCliente = requestBusquedaCliente.IdCliente, Tipo = resulServices.DatosServicio.area };
        //        ResponseListaCertificadosxClienteyTipo responseListaCertificadosxClienteyTipoDTO = clientHelper.Consume<ResponseListaCertificadosxClienteyTipo>(Global.URIGLOBAL_SERVICES + Global.URI_CERTIFICADO, requestListaCertificadosxClienteyTipoDTO).Result;
        //        if (!responseListaCertificadosxClienteyTipoDTO.estado)
        //        {
        //            resul.State = ResponseType.Warning;
        //            resul.Message = $"Existe problemas al consumir el servicio de ibnorca (ListaCertificadosxClienteyTipoDTO ): {responseListaCertificadosxClienteyTipoDTO.mensaje}";
        //            return resul;
        //        }

        //        #endregion

        //        ///TDO verificamos que exista certificados vigentes 
        //        ///TCS
        //        bool? existeCertificadosVigentes = responseListaCertificadosxClienteyTipoDTO.ListaCertifcados?.Any(x => x.idEstado == "474");
        //        string alcance = string.Empty;
        //        DateTime? fechaVencimientoCertificado = null;
        //        DateTime? fechaEmisionCertificado = null;
        //        string nroCertificado = string.Empty;
        //        string nomreClienteCertificado = string.Empty;
        //        List<ListaCertifcado> certificadosValidos = new List<ListaCertifcado>();

        //        List<string> direcciones = new List<string>();
        //        if (existeCertificadosVigentes == true && resulServices.DatosServicio.area == "TCS")
        //        {
        //            alcance = responseListaCertificadosxClienteyTipoDTO.ListaCertifcados.First(x => x.idEstado == "474").Alcance;
        //            direcciones = responseListaCertificadosxClienteyTipoDTO.ListaCertifcados.First(x => x.idEstado == "474").ProductoServicio.Split('|').ToList();
        //            fechaVencimientoCertificado = Convert.ToDateTime(responseListaCertificadosxClienteyTipoDTO.ListaCertifcados.First(x => x.idEstado == "474").FechaValido);
        //            fechaEmisionCertificado = Convert.ToDateTime(responseListaCertificadosxClienteyTipoDTO.ListaCertifcados.First(x => x.idEstado == "474").FechaEmision);
        //            nroCertificado = responseListaCertificadosxClienteyTipoDTO.ListaCertifcados.First(x => x.idEstado == "474").IdCertificadoServicios;
        //            nomreClienteCertificado = responseListaCertificadosxClienteyTipoDTO.ListaCertifcados.First(x => x.idEstado == "474").cliente.ToUpper();
        //        }
        //        ///TCP
        //        if (existeCertificadosVigentes == true && resulServices.DatosServicio.area == "TCP")
        //        {
        //            alcance = string.Empty;
        //            direcciones = responseListaCertificadosxClienteyTipoDTO.ListaCertifcados.Where(x => x.idEstado == "474").Select(x => x.ProductoServicio
        //            .Replace("Al Producto:", "")
        //            .Replace("Marca Comercial:", "")
        //            .Replace("Lugar de Fabricación:", "")
        //            ).ToList();
        //            fechaVencimientoCertificado = Convert.ToDateTime(responseListaCertificadosxClienteyTipoDTO.ListaCertifcados.First(x => x.idEstado == "474").FechaValido);
        //            fechaEmisionCertificado = Convert.ToDateTime(responseListaCertificadosxClienteyTipoDTO.ListaCertifcados.First(x => x.idEstado == "474").FechaEmision);
        //            nroCertificado = responseListaCertificadosxClienteyTipoDTO.ListaCertifcados.First(x => x.idEstado == "474").IdCertificadoServicios;
        //            nomreClienteCertificado = responseListaCertificadosxClienteyTipoDTO.ListaCertifcados.First(x => x.idEstado == "474").cliente.ToUpper();
        //            certificadosValidos = responseListaCertificadosxClienteyTipoDTO.ListaCertifcados.Where(x => x.idEstado == "474").ToList();
        //        }



        //        Praprogramasdeauditorium objPrograma = new Praprogramasdeauditorium
        //        {
        //            IdparamArea = Convert.ToInt32(resulServices.DatosServicio.IdArea),
        //            DetalleServicioWs = JsonConvert.SerializeObject(resulServices.DatosServicio),
        //            Fecha = resulServices.DatosServicio.fecharegistro,
        //            Oficina = resulServices.DatosServicio.oficina,
        //            IdOrganizacionWs = resulServices.DatosServicio.IdCliente,
        //            OrganizacionContentWs = JsonConvert.SerializeObject(responseBusquedaCliente.resultados[0]),
        //            Nit = responseBusquedaCliente.resultados[0].NIT,
        //            CodigoServicioWs = resulServices.DatosServicio.Codigo,
        //            IdparamTipoServicio = 1,/*CERTIFICACION - RENOVACION*///no se tiene del servicio
        //            CodigoIafws = resulServices.DatosServicio.iaf_primario_codigo + " - " + resulServices.DatosServicio.iaf_primario_descripcion,
        //            NumeroAnios = 0,
        //            Estado = "INICIAL",
        //            UsuarioRegistro = pUsuario,
        //            FechaDesde = DateTime.Now,
        //            FechaHasta = null
        //        };
        //        string mode = objPrograma.IdparamArea == 38 ? "TCS" : "TCP";
        //        int cont = 0;
        //        responseDatosPropuestaDTO.ListaServicios.ForEach(x =>
        //        {
        //            if (!objPrograma.Praciclosprogauditoria.Any(yy => yy.Anio == (short)Convert.ToInt32(x.cod_anio)))
        //            {
        //                Praciclosprogauditorium ciclosprogauditorium = new Praciclosprogauditorium
        //                {
        //                    UsuarioRegistro = pUsuario,
        //                    FechaDesde = DateTime.Now,
        //                    FechaHasta = null,
        //                    Anio = (short)Convert.ToInt32(x.cod_anio),
        //                    Referencia = x.descripcion,
        //                    IdparamTipoAuditoria = 1,
        //                    NombreOrganizacionCertificado = existeCertificadosVigentes == true ? nomreClienteCertificado : responseBusquedaCliente.resultados[0].NombreRazon,
        //                    EstadoDescripcion = "SIN FECHA DE AUDITORIA"
        //                };

        //                ///TDO: TCP - Cert. de Productos 
        //                if (mode.Equals("TCP"))
        //                {
        //                    ciclosprogauditorium.Pradireccionespaproductos = new List<Pradireccionespaproducto>();
        //                    cont = 0;

        //                    resulServices.DatosServicio.ListaProducto.ForEach(dir =>
        //                    {
        //                        Pradireccionespaproducto objDirProd = new Pradireccionespaproducto
        //                        {
        //                            Nombre = existeCertificadosVigentes == true && cont < direcciones.Count ? direcciones[cont].Split('|')[0] : dir.nombre,
        //                            Direccion = existeCertificadosVigentes == true && cont < direcciones.Count ? direcciones[cont].Split('|')[2] : dir.direccion,
        //                            Marca = existeCertificadosVigentes == true && cont < direcciones.Count ? direcciones[cont].Split('|')[1] : dir.marca,
        //                            Sello = dir.nro_sello,
        //                            Ciudad = dir.ciudad,
        //                            Estado = dir.estado,
        //                            Pais = dir.pais,
        //                            Norma = dir.norma,
        //                            FechaEmisionPrimerCertificado = cont < certificadosValidos.Count ? fechaEmisionCertificado : null,//////////////////////////////
        //                            FechaVencimientoUltimoCertificado = cont < certificadosValidos.Count ? fechaVencimientoCertificado : null,
        //                            FechaVencimientoCertificado = cont < certificadosValidos.Count ? fechaVencimientoCertificado : null,
        //                            UsuarioRegistro = pUsuario,
        //                            FechaDesde = DateTime.Now,
        //                            FechaHasta = null,
        //                            NumeroDeCertificacion = existeCertificadosVigentes == true
        //                            && cont < certificadosValidos.Count ?
        //                            certificadosValidos[cont].IdCertificadoServicios : "",
        //                        };

        //                        ciclosprogauditorium.Pradireccionespaproductos.Add(objDirProd);
        //                        cont++;
        //                    });

        //                }

        //                ///TDO: TCS - Cert.Sistemas de Gestion
        //                if (mode.Equals("TCS"))
        //                {
        //                    cont = 0;
        //                    ///TDO: direcciones
        //                    ciclosprogauditorium.Pradireccionespasistemas = new List<Pradireccionespasistema>();
        //                    string norma = string.Empty;
        //                    resulServices.DatosServicio.ListaDireccion.ForEach(dir =>
        //                    {
        //                        Pradireccionespasistema objDirSis = new Pradireccionespasistema
        //                        {
        //                            Ciudad = dir.ciudad,
        //                            Departamento = dir.estado,
        //                            Dias = 0.00M,
        //                            Direccion = existeCertificadosVigentes == true && cont < direcciones.Count ? direcciones[cont] : dir.direccion,
        //                            FechaDesde = DateTime.Now,
        //                            FechaHasta = null,
        //                            Pais = dir.pais,
        //                            UsuarioRegistro = pUsuario,
        //                            Nombre = dir.nombre
        //                        };
        //                        norma = dir.norma;
        //                        ciclosprogauditorium.Pradireccionespasistemas.Add(objDirSis);
        //                        cont++;
        //                    });

        //                    ///TDO: normas
        //                    ciclosprogauditorium.Praciclonormassistemas = new List<Praciclonormassistema>();
        //                    ciclosprogauditorium.Praciclonormassistemas.Add(new Praciclonormassistema
        //                    {
        //                        Alcance = existeCertificadosVigentes == true ? alcance : resulServices.DatosServicio.alcance_propuesta,
        //                        IdparamNorma = null,
        //                        Norma = norma,
        //                        FechaDesde = DateTime.Now,
        //                        FechaEmisionPrimerCertificado = fechaEmisionCertificado,
        //                        FechaHasta = null,
        //                        FechaVencimientoUltimoCertificado = fechaVencimientoCertificado,
        //                        NumeroDeCertificacion = nroCertificado,
        //                        UsuarioRegistro = pUsuario
        //                    });
        //                }

        //                ///TDO: Cronograma 
        //                Praciclocronograma cronograma = new Praciclocronograma
        //                {
        //                    DiasPresupuesto = (decimal)Convert.ToDecimal(x.cantidad),
        //                    DiasInsitu = 0.00M,
        //                    DiasRemoto = 0.00M,
        //                    FechaDeFinDeEjecucionAuditoria = null,
        //                    FechaDesde = DateTime.Now,
        //                    FechaHasta = null,
        //                    FechaInicioDeEjecucionDeAuditoria = null,
        //                    MesProgramado = CalcularMesProgramado(mode, Convert.ToInt32(x.cod_anio), fechaVencimientoCertificado),
        //                    MesReprogramado = null,
        //                    UsuarioRegistro = pUsuario
        //                };
        //                ciclosprogauditorium.Praciclocronogramas = new List<Praciclocronograma>();
        //                ciclosprogauditorium.Praciclocronogramas.Add(cronograma);

        //                ///TDO: lista de personal
        //                ciclosprogauditorium.Pracicloparticipantes = new List<Pracicloparticipante>();
        //                responseDatosPropuestaDTO.ListaAuditores.ForEach(auditor =>
        //                {
        //                    if (Convert.ToInt32(auditor.cod_anio) == ciclosprogauditorium.Anio)
        //                    {
        //                        Pracicloparticipante participante = new Pracicloparticipante
        //                        {
        //                            FechaDesde = DateTime.Now,
        //                            CargoDetalleWs = JsonConvert.SerializeObject(auditor),
        //                            IdCargoWs = Convert.ToInt32(auditor.cod_tipoauditor),
        //                            IdParticipanteWs = null,
        //                            ParticipanteDetalleWs = null,
        //                            UsuarioRegistro = pUsuario
        //                        };
        //                        ciclosprogauditorium.Pracicloparticipantes.Add(participante);
        //                    }

        //                });
        //                objPrograma.Praciclosprogauditoria.Add(ciclosprogauditorium);
        //            }
        //        });

        //        ///insertamos el ciclo final como año 4 renovacion
        //        var cicloClonar = objPrograma.Praciclosprogauditoria.LastOrDefault();
        //        if (cicloClonar != null)
        //        {
        //            var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
        //            var serializeSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
        //            var cicloClonado = JsonConvert.DeserializeObject<Praciclosprogauditorium>(JsonConvert.SerializeObject(cicloClonar, serializeSettings), deserializeSettings);
        //            cicloClonado.Anio = 4;
        //            cicloClonado.Praciclocronogramas.First().MesProgramado = cicloClonado.Praciclocronogramas.First().MesProgramado?.AddYears(1);
        //            cicloClonado.Referencia = "Renovacion";
        //            objPrograma.Praciclosprogauditoria.Add(cicloClonado);
        //        }

        //        ///Inserta el programa de auditoria
        //        Entity<Praprogramasdeauditorium> entity = new Entity<Praprogramasdeauditorium> { EntityDB = objPrograma, stateEntity = StateEntity.add };
        //        repositoryMySql.SaveObject<Praprogramasdeauditorium>(entity);
        //        objPrograma.IdPrAprogramaAuditoria = entity.EntityDB.IdPrAprogramaAuditoria;
        //        resul.Object = objPrograma;

        //        resul.Object = resulDB[0];
        //        resul.Object.Praciclosprogauditoria = repositoryMySql.SimpleSelect<Praciclosprogauditorium>(x => x.IdPrAprogramaAuditoria == resul.Object.IdPrAprogramaAuditoria);
        //        List<Praciclosprogauditorium> lAuxiliar = resul.Object.Praciclosprogauditoria.ToList();
        //        lAuxiliar.ForEach(x =>
        //        {
        //            x.Praciclocronogramas = repositoryMySql.SimpleSelect<Praciclocronograma>(y => y.IdPrAcicloProgAuditoria == x.IdPrAcicloProgAuditoria);
        //            x.Praciclonormassistemas = repositoryMySql.SimpleSelect<Praciclonormassistema>(y => y.IdPrAcicloProgAuditoria == x.IdPrAcicloProgAuditoria);
        //            x.Pracicloparticipantes = repositoryMySql.SimpleSelect<Pracicloparticipante>(y => y.IdPrAcicloProgAuditoria == x.IdPrAcicloProgAuditoria);
        //            x.Pradireccionespaproductos = repositoryMySql.SimpleSelect<Pradireccionespaproducto>(y => y.IdPrAcicloProgAuditoria == x.IdPrAcicloProgAuditoria);
        //            x.Pradireccionespasistemas = repositoryMySql.SimpleSelect<Pradireccionespasistema>(y => y.IdPrAcicloProgAuditoria == x.IdPrAcicloProgAuditoria);
        //        });
        //        resul.Object.Praciclosprogauditoria = lAuxiliar;

        //        resul.State = ResponseType.Success;

        //    }
        //    catch (Exception ex)
        //    {
        //        ProcessError(ex, resul);
        //    }
        //    return resul;
        //}
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

                    ///TDO obtenemos de los certificados del cliente
                    RequestListaCertificadosxClienteyTipo requestListaCertificadosxClienteyTipoDTO = new RequestListaCertificadosxClienteyTipo { accion = "ListaCertificadosxClienteyTipo", sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES, IdCliente = requestBusquedaCliente.IdCliente, Tipo = resulServices.DatosServicio.area };
                    ResponseListaCertificadosxClienteyTipo responseListaCertificadosxClienteyTipoDTO = clientHelper.Consume<ResponseListaCertificadosxClienteyTipo>(Global.URIGLOBAL_SERVICES + Global.URI_CERTIFICADO, requestListaCertificadosxClienteyTipoDTO).Result;
                    if (!responseListaCertificadosxClienteyTipoDTO.estado)
                    {
                        resul.State = ResponseType.Warning;
                        resul.Message = $"Existe problemas al consumir el servicio de ibnorca (ListaCertificadosxClienteyTipoDTO ): {responseListaCertificadosxClienteyTipoDTO.mensaje}";
                        return resul;
                    }

                    #endregion

                    ///TDO verificamos que exista certificados vigentes 
                    ///TCS
                    bool? existeCertificadosVigentes = responseListaCertificadosxClienteyTipoDTO.ListaCertifcados?.Any(x => x.idEstado == "474");
                    string alcance = string.Empty;
                    DateTime? fechaVencimientoCertificado = null;
                    DateTime? fechaEmisionCertificado = null;
                    string nroCertificado = string.Empty;
                    string nomreClienteCertificado = string.Empty;
                    List<ListaCertifcado> certificadosValidos = new List<ListaCertifcado>();

                    List<string> direcciones = new List<string>();
                    if (existeCertificadosVigentes == true && resulServices.DatosServicio.area == "TCS")
                    {
                        alcance = responseListaCertificadosxClienteyTipoDTO.ListaCertifcados.First(x => x.idEstado == "474").Alcance;
                        direcciones = responseListaCertificadosxClienteyTipoDTO.ListaCertifcados.First(x => x.idEstado == "474").ProductoServicio.Split('|').ToList();
                        fechaVencimientoCertificado = Convert.ToDateTime(responseListaCertificadosxClienteyTipoDTO.ListaCertifcados.First(x => x.idEstado == "474").FechaValido);
                        fechaEmisionCertificado = Convert.ToDateTime(responseListaCertificadosxClienteyTipoDTO.ListaCertifcados.First(x => x.idEstado == "474").FechaEmision);
                        nroCertificado = responseListaCertificadosxClienteyTipoDTO.ListaCertifcados.First(x => x.idEstado == "474").IdCertificadoServicios;
                        nomreClienteCertificado = responseListaCertificadosxClienteyTipoDTO.ListaCertifcados.First(x => x.idEstado == "474").cliente.ToUpper();
                    }
                    ///TCP
                    if (existeCertificadosVigentes == true && resulServices.DatosServicio.area == "TCP")
                    {
                        alcance = string.Empty;
                        direcciones = responseListaCertificadosxClienteyTipoDTO.ListaCertifcados.Where(x => x.idEstado == "474").Select(x => x.ProductoServicio
                        .Replace("Al Producto:", "")
                        .Replace("Marca Comercial:", "")
                        .Replace("Lugar de Fabricación:", "")
                        ).ToList();
                        fechaVencimientoCertificado = Convert.ToDateTime(responseListaCertificadosxClienteyTipoDTO.ListaCertifcados.First(x => x.idEstado == "474").FechaValido);
                        fechaEmisionCertificado = Convert.ToDateTime(responseListaCertificadosxClienteyTipoDTO.ListaCertifcados.First(x => x.idEstado == "474").FechaEmision);
                        nroCertificado = responseListaCertificadosxClienteyTipoDTO.ListaCertifcados.First(x => x.idEstado == "474").IdCertificadoServicios;
                        nomreClienteCertificado = responseListaCertificadosxClienteyTipoDTO.ListaCertifcados.First(x => x.idEstado == "474").cliente.ToUpper();
                        certificadosValidos = responseListaCertificadosxClienteyTipoDTO.ListaCertifcados.Where(x => x.idEstado == "474").ToList();
                    }

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
                        CodigoIafws = resulServices.DatosServicio.iaf_primario_codigo + " - " + resulServices.DatosServicio.iaf_primario_descripcion,
                        NumeroAnios = 0,
                        Estado = "INICIAL",
                        UsuarioRegistro = pUsuario,
                        FechaDesde = DateTime.Now,
                        FechaHasta = null
                    };
                    string mode = objPrograma.IdparamArea == 38 ? "TCS" : "TCP";
                    int cont = 0;
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
                                NombreOrganizacionCertificado = existeCertificadosVigentes == true ? nomreClienteCertificado : responseBusquedaCliente.resultados[0].NombreRazon,
                                EstadoDescripcion = "SIN FECHA DE AUDITORIA"
                            };

                            ///TDO: TCP - Cert. de Productos 
                            if (mode.Equals("TCP"))
                            {
                                ciclosprogauditorium.Pradireccionespaproductos = new List<Pradireccionespaproducto>();
                                cont = 0;

                                resulServices.DatosServicio.ListaProducto.ForEach(dir =>
                                {
                                    Pradireccionespaproducto objDirProd = new Pradireccionespaproducto
                                    {
                                        Nombre = existeCertificadosVigentes == true && cont < direcciones.Count ? direcciones[cont].Split('|')[0] : dir.nombre,
                                        Direccion = existeCertificadosVigentes == true && cont < direcciones.Count ? direcciones[cont].Split('|')[2] : dir.direccion,
                                        Marca = existeCertificadosVigentes == true && cont < direcciones.Count ? direcciones[cont].Split('|')[1] : dir.marca,
                                        Sello = dir.nro_sello,
                                        Ciudad = dir.ciudad,
                                        Estado = dir.estado,
                                        Pais = dir.pais,
                                        Norma = dir.norma,
                                        FechaEmisionPrimerCertificado = cont < certificadosValidos.Count ? fechaEmisionCertificado : null,//////////////////////////////
                                        FechaVencimientoUltimoCertificado = cont < certificadosValidos.Count ? fechaVencimientoCertificado : null,
                                        FechaVencimientoCertificado = cont < certificadosValidos.Count ? fechaVencimientoCertificado : null,
                                        UsuarioRegistro = pUsuario,
                                        FechaDesde = DateTime.Now,
                                        FechaHasta = null,
                                        NumeroDeCertificacion = existeCertificadosVigentes == true
                                        && cont < certificadosValidos.Count ?
                                        certificadosValidos[cont].IdCertificadoServicios : "",
                                    };

                                    ciclosprogauditorium.Pradireccionespaproductos.Add(objDirProd);
                                    cont++;
                                });

                            }

                            ///TDO: TCS - Cert.Sistemas de Gestion
                            if (mode.Equals("TCS"))
                            {
                                cont = 0;
                                ///TDO: direcciones
                                ciclosprogauditorium.Pradireccionespasistemas = new List<Pradireccionespasistema>();
                                string norma = string.Empty;
                                resulServices.DatosServicio.ListaDireccion.ForEach(dir =>
                                {
                                    Pradireccionespasistema objDirSis = new Pradireccionespasistema
                                    {
                                        Ciudad = dir.ciudad,
                                        Departamento = dir.estado,
                                        Dias = 0.00M,
                                        Direccion = existeCertificadosVigentes == true && cont < direcciones.Count ? direcciones[cont] : dir.direccion,
                                        FechaDesde = DateTime.Now,
                                        FechaHasta = null,
                                        Pais = dir.pais,
                                        UsuarioRegistro = pUsuario,
                                        Nombre = dir.nombre
                                    };
                                    norma = dir.norma;
                                    ciclosprogauditorium.Pradireccionespasistemas.Add(objDirSis);
                                    cont++;
                                });

                                ///TDO: normas
                                ciclosprogauditorium.Praciclonormassistemas = new List<Praciclonormassistema>();
                                ciclosprogauditorium.Praciclonormassistemas.Add(new Praciclonormassistema
                                {
                                    Alcance = existeCertificadosVigentes == true ? alcance : resulServices.DatosServicio.alcance_propuesta,
                                    IdparamNorma = null,
                                    Norma = norma,
                                    FechaDesde = DateTime.Now,
                                    FechaEmisionPrimerCertificado = fechaEmisionCertificado,
                                    FechaHasta = null,
                                    FechaVencimientoUltimoCertificado = fechaVencimientoCertificado,
                                    NumeroDeCertificacion = nroCertificado,
                                    UsuarioRegistro = pUsuario
                                });
                            }

                            ///TDO: Cronograma 
                            Praciclocronograma cronograma = new Praciclocronograma
                            {
                                DiasPresupuesto = (decimal)Convert.ToDecimal(x.cantidad),
                                DiasInsitu = 0.00M,
                                DiasRemoto = 0.00M,
                                FechaDeFinDeEjecucionAuditoria = null,
                                FechaDesde = DateTime.Now,
                                FechaHasta = null,
                                FechaInicioDeEjecucionDeAuditoria = null,
                                MesProgramado = CalcularMesProgramado(mode, Convert.ToInt32(x.cod_anio), fechaVencimientoCertificado),
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

                    ///insertamos el ciclo final como año 4 renovacion
                    var cicloClonar = objPrograma.Praciclosprogauditoria.LastOrDefault();
                    if (cicloClonar != null)
                    {
                        var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
                        var serializeSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
                        var cicloClonado = JsonConvert.DeserializeObject<Praciclosprogauditorium>(JsonConvert.SerializeObject(cicloClonar, serializeSettings), deserializeSettings);
                        cicloClonado.Anio = 4;
                        cicloClonado.Praciclocronogramas.First().MesProgramado = cicloClonado.Praciclocronogramas.First().MesProgramado?.AddYears(1);
                        cicloClonado.Referencia = "Renovacion";
                        objPrograma.Praciclosprogauditoria.Add(cicloClonado);
                    }

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
        public ResponseQuery<NormaInternacional> BuscarNormasInternacionales(string Codigo)
        {
            ResponseQuery<NormaInternacional> response = new ResponseQuery<NormaInternacional> { Message = "Parametros obtenidos correctamente.", State = ResponseType.Success, ListEntities = new List<NormaInternacional>() };
            try
            {
                ClientHelper clientHelper = new ClientHelper();
                ///TDO: obtenemos los datos del servicio
                RequestBuscarNormaIntxCodigo requestDato = new RequestBuscarNormaIntxCodigo { accion = "BuscarNormaIntxCodigo", sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES, Codigo = Codigo };
                ResponseBuscarNormaIntxCodigo resulServices = clientHelper.Consume<ResponseBuscarNormaIntxCodigo>(Global.URIGLOBAL_SERVICES + Global.URI_NORMAS_INT, requestDato).Result;
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
                RequestBuscarPais requestDato = new RequestBuscarPais { accion = "BuscarPais", sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES, palabra = pais, TipoLista = "BuscarPais", };
                ResponseBuscarPais resulServices = clientHelper.Consume<ResponseBuscarPais>(Global.URIGLOBAL_CLASIFICADOR + Global.URI_PAISES, requestDato).Result;
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
                ResponseEstados resulServices = clientHelper.Consume<ResponseEstados>(Global.URIGLOBAL_CLASIFICADOR + Global.URI_PAISES, requestDato).Result;
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
                ResponseCiudades resulServices = clientHelper.Consume<ResponseCiudades>(Global.URIGLOBAL_CLASIFICADOR + Global.URI_PAISES, requestDato).Result;
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
        public Response GenerarDesignacion(int IdCiclo, string pathPlantilla)
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

                ///llenamos el reporte con la informacion de este ciclo
                RepDocDesignacion praDocDesignacion = new RepDocDesignacion
                {
                    FechadeAuditoria = praciclocronograma.Praciclocronogramas.First().FechaInicioDeEjecucionDeAuditoria?.ToString("dd/MM/yyyy"),
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
                    }).ToList()
                };


                /*string filePlantilla = Global.PATH_PLANTILLA_DESIGNACION + pathPlantilla;
                WordHelper generadorWord = new WordHelper(filePlantilla);
                praDocDesignacion.SitiosAAuditar = string.Empty;
                praciclocronograma.Pradireccionespasistemas?.ToList().ForEach(direccion =>
                {
                    praDocDesignacion.SitiosAAuditar += direccion.Direccion + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                praciclocronograma.Praciclonormassistemas?.ToList().ForEach(alcance =>
                {
                    praDocDesignacion.AlcanceDeCertificacion += alcance.Norma + " - " + alcance.Alcance + WordHelper.GetCodeKey(WordHelper.keys.enter);
                });

                //generamos el documento en word
                Dictionary<string, CellTitles[]> pTitles = new Dictionary<string, CellTitles[]>();
                CellTitles[] cellTitlesTitulo = new CellTitles[2];
                cellTitlesTitulo[0] = new CellTitles { Title = "Calificación", Visible = true, Width = "50" };
                cellTitlesTitulo[1] = new CellTitles { Title = "Auditor", Visible = true, Width = "50" };
                pTitles.Add("ListRepDesginacionParticipante", cellTitlesTitulo);

                string fileNameGenerado = generadorWord.GenerarDocumento(praDocDesignacion, pTitles, $"{Global.PATH_PLANTILLA_DESIGNACION}\\Salidas");

                ///Convertimos en PDF
                //using (var process = new Process())
                //{
                //    process.StartInfo.FileName = @"E:\ConvertPDF\ConvertExecute.exe"; // relative path. absolute path works too.
                //    process.StartInfo.Arguments = $"{fileNameGenerado}";
                //    process.StartInfo.CreateNoWindow = true;
                //    process.StartInfo.UseShellExecute = false;
                //    process.StartInfo.RedirectStandardOutput = true;
                //    process.StartInfo.RedirectStandardError = true;
                //    process.OutputDataReceived += (sender, data) => Console.WriteLine(data.Data);
                //    process.ErrorDataReceived += (sender, data) => Console.WriteLine(data.Data);
                //    process.Start();
                //    process.BeginOutputReadLine();
                //    process.BeginErrorReadLine();
                //    process.WaitForExit();     // (optional) wait up to 10 seconds                    
                //}
                //response.Message = fileNameGenerado.Replace(".doc", ".pdf");
                response.Message = fileNameGenerado;*/
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
        public ResponseQuery<Clasificador> BuscarOrganismosCertificadores(RequestBuscarClasificador req)
        {
            ResponseQuery<Clasificador> response = new ResponseQuery<Clasificador> { Message = "Organismos obtenidos correctamente.", State = ResponseType.Success, ListEntities = new List<Clasificador>() };
            try
            {
                ClientHelper clientHelper = new ClientHelper();
                ///TDO: obtenemos los datos del servicio
                RequestBuscarClasificador requestDato = new RequestBuscarClasificador { accion = "DatosContactoEmpresa", sIdentificador = Global.IDENTIFICADOR, sKey = Global.KEY_SERVICES, padre = req.padre };
                ResponseBuscarClasificador resulServices = clientHelper.Consume<ResponseBuscarClasificador>(Global.URIGLOBAL_CLASIFICADOR + Global.URI_CLASIFICADOR, requestDato).Result;
                if (!resulServices.estado)
                {
                    response.State = ResponseType.Warning;
                    response.Message = $"Existe problemas al consumir el servicio de ibnorca (BuscarOrganismosCertificadores): {resulServices.mensaje}";
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
        private DateTime? CalcularMesProgramado(string area, int año, DateTime? fechaVencimiento)
        {
            DateTime? resul = null;
            if (fechaVencimiento == null)
            {
                return resul;
            }

            switch (año)
            {
                case 0: break;
                case 1: break;
                case 2:
                    resul = area == "TCS" ? fechaVencimiento?.AddMonths(-26) : fechaVencimiento?.AddMonths(-27);
                    break;
                case 3:
                    resul = area == "TCS" ? fechaVencimiento?.AddMonths(-14) : fechaVencimiento?.AddMonths(-15);
                    break;
                case 4:
                    resul = area == "TCS" ? fechaVencimiento?.AddMonths(-4) : fechaVencimiento?.AddMonths(-3);
                    break;
                default:
                    break;
            }
            return resul;
        }

        public Response CargarSolicitudExcel(string pathExcel)
        {
            Response response = new Response() { Message = "Solicitud registrada correctamente", State = ResponseType.Success };
            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var stream = File.Open(pathExcel, FileMode.Open, FileAccess.Read))
                {
                    SolicitudCliente solicitudCliente = new SolicitudCliente();
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        DataSet dataset = reader.AsDataSet();
                        solicitudCliente.GetType().GetProperties().ToList().ForEach(x =>
                        {
                            if (x.PropertyType.GetInterface("IList") != null)
                            {
                                Rango rango = x.GetCustomAttributes(true).ToList().FirstOrDefault(y => y.GetType() == typeof(Rango)) as Rango;
                                Type innerType = null;
                                IList listReference = null;
                                if (x.PropertyType == typeof(List<Laboratorio>))
                                {
                                    listReference = new List<Laboratorio>();
                                    x.SetValue(solicitudCliente, listReference);
                                    innerType = typeof(Laboratorio);
                                }
                                else if (x.PropertyType == typeof(List<SolicitudProducto>))
                                {
                                    listReference = new List<SolicitudProducto>();
                                    x.SetValue(solicitudCliente, listReference);
                                    innerType = typeof(SolicitudProducto);
                                }
                                else if (x.PropertyType == typeof(List<ListSimple>))
                                {
                                    listReference = new List<ListSimple>();
                                    x.SetValue(solicitudCliente, listReference);
                                    innerType = typeof(ListSimple);
                                }

                                for (int i = rango.rowA; i <= rango.rowB; i++)
                                {
                                    object newInstance = Activator.CreateInstance(innerType);
                                    newInstance.GetType().GetProperties().ToList().ForEach(zz =>
                                    {
                                        Coordenada coordenada = zz.GetCustomAttributes(true).ToList().FirstOrDefault(y => y.GetType() == typeof(Coordenada)) as Coordenada;
                                        string valor = coordenada != null ? Convert.ToString(dataset.Tables[0].Rows[i][coordenada.column]) : "";
                                        zz.SetValue(newInstance, valor);
                                    });
                                    listReference.Add(newInstance);
                                }
                                x.SetValue(solicitudCliente, listReference);
                            }
                            else
                            {
                                Coordenada coordenada = x.GetCustomAttributes(true).ToList().FirstOrDefault(y => y.GetType() == typeof(Coordenada)) as Coordenada;
                                string valor = coordenada != null ? Convert.ToString(dataset.Tables[0].Rows[coordenada.row][coordenada.column]) : "";
                                x.SetValue(solicitudCliente, valor);
                            }

                        });

                    }
                    ///Guardamos el objeto en BD
                    Entity<Importacionsolicitud> entity = new Entity<Importacionsolicitud>() { EntityDB = new Importacionsolicitud(), stateEntity = StateEntity.add };
                    entity.EntityDB.Nit = solicitudCliente.nit;
                    entity.EntityDB.Cliente = solicitudCliente.razonSocial;
                    entity.EntityDB.Detalle = JsonConvert.SerializeObject(solicitudCliente);
                    entity.EntityDB.FechaRegistro = DateTime.Now;
                    repositoryMySql.SaveObject<Importacionsolicitud>(entity);
                    repositoryMySql.Commit();
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
    }
}
