using Business.Main.Base;
using Business.Main.IbnorcaContext;
using Business.Main.Modules.ApeeturaAuditoria.Domain;
using CoreAccesLayer.Wraper;
using Domain.Main.AperturaAuditoria;
using Domain.Main.Wraper;
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

        public ResponseObject<Praprogramasdeauditorium> ObtenerProgramaAuditoria(int IdServicios)
        {
            ResponseObject<Praprogramasdeauditorium> resul = new ResponseObject<Praprogramasdeauditorium> { Object = new Praprogramasdeauditorium(), Code = "000", Message = "Programa obtenido correctamente", State = ResponseType.Success };
            try
            {
                var resulDB = repositoryMySql.GetDataByProcedure<Praprogramasdeauditorium>("spGetProgramaAuditoriaByIdServicio", IdServicios);
                if (resulDB.Count == 0)
                {
                    //aqui llenamos los datos con ws para llenar la primera instancia y guardar en la BD
                    AperturaAuditoriaManager objProgramaAudi = new AperturaAuditoriaManager();
                    ComplexProgramaAuditoria objComplex = new ComplexProgramaAuditoria();
                    Praprogramasdeauditorium objPrograma = new Praprogramasdeauditorium
                    {
                        IdpArea = 1, // /*SISTEMA - PRODUCTO*/
                        Nit = "123456",
                        Gestion = 2021,
                        IdpPais = 1,
                        IdpDepartamento = 1,
                        IdOrganizacionWs = "5",
                        OrganizacionContentWs = "{\"NOmbre\":\"ruben\"}",
                        CodigoServicioWs = "REG-PROG-XXXXXXX",
                        DetalleServicio = "{\"NOmbre\":\"ruben\"}",
                        IdpTipoServicio = 1,/*CERTIFICACION - RENOVACION*/
                        IdCodigoDeServicioCodigoIafWs = "{\"NOmbre\":\"ruben\"}",
                        NumeroAnos = 1,
                        IdpEstadosProgAuditoria = 2, /*'Sin fecha de auditoría' -  Con  - audi realizada*/
                        UsuarioRegistro = "ivan.vilela",
                        FechaDesde = DateTime.Now,
                        FechaHasta = null
                    };

                    Praciclosprogauditorium ciclosprogauditorium = new Praciclosprogauditorium
                    {
                        UsuarioRegistro = "ivan.vilela",
                        FechaDesde = DateTime.Now,
                        FechaHasta = null,
                        Ano = 2000,
                        IdpTipoAuditoria = 1,
                        NombreOrganizacionCertificado = "nombre que va en el certificado"

                    };
                    objPrograma.Praciclosprogauditoria.Add(ciclosprogauditorium);

                    ciclosprogauditorium = new Praciclosprogauditorium
                    {
                        UsuarioRegistro = "ivan.vilela",
                        FechaDesde = DateTime.Now,
                        FechaHasta = null,
                        Ano = 2001,
                        IdpTipoAuditoria = 1,
                        NombreOrganizacionCertificado = "nombre que va en el certificado"

                    };
                    objPrograma.Praciclosprogauditoria.Add(ciclosprogauditorium);
                    ciclosprogauditorium = new Praciclosprogauditorium
                    {
                        UsuarioRegistro = "ivan.vilela",
                        FechaDesde = DateTime.Now,
                        FechaHasta = null,
                        Ano = 2002,
                        IdpTipoAuditoria = 1,
                        NombreOrganizacionCertificado = "nombre que va en el certificado"

                    };


                    Praciclocronograma objCicloCrono = new Praciclocronograma
                    {
                        IdPrAcicloProgAuditoria = 1,
                        CantidadDeDiasTotal = 5,
                        MesProgramado = 1,
                        MesReprogramado = 3,
                        FechaInicioDeEjecucionDeAuditoria = DateTime.Now,
                        FechaDeFinDeEjecucionAuditoria = DateTime.Now,
                        UsuarioRegistro = "ivan.vilela",
                        FechaDesde = DateTime.Now,
                        FechaHasta = null

                    }; //doble tipo de auditoria, se repite en la cabecera por sis ac los dos

                    ciclosprogauditorium.Praciclocronogramas.Add(objCicloCrono);
                    //Participante
                    Pracicloparticipante objParticipante = new Pracicloparticipante
                    {
                        IdPrAcicloProgAuditoria = 1,
                        IdParticipanteWs = "1WS",
                        ParticipanteContextWs = "{\"NOmbre\":\"ruben\"}",
                        UsuarioRegistro = "ivan.vilela",
                        FechaDesde = DateTime.Now,
                        IdpEstadoParticipante = 1 ///baja  - vigente
                    };
                    ciclosprogauditorium.Pracicloparticipantes.Add(objParticipante);

                    //Productos
                    Pradireccionespaproducto objDirProd = new Pradireccionespaproducto
                    {
                        IdPrAcicloProgAuditoria = 1,
                        Nombre = "cemento Portland",
                        Direccion = "Planta industrial Viacha",
                        Marca = "NB",
                        Sello = 1,
                        IdPais = 1,
                        IdDepartamento = 1,
                        Ciudad = "el alto",
                        FechaEmisionPrimerCertificado = DateTime.Now,
                        FechaVencimientoUltimoCertificado = DateTime.Now,
                        FechaVencimientoCertificado = DateTime.Now,
                        UsuarioRegistro = "ivan.vilela",
                        FechaDesde = DateTime.Now,
                        FechaHasta = null
                    };
                    ciclosprogauditorium.Pradireccionespaproductos.Add(objDirProd);

                    objPrograma.Praciclosprogauditoria.Add(ciclosprogauditorium);

                    ///Inserta el programa de auditoria
                    Entity<Praprogramasdeauditorium> entity = new Entity<Praprogramasdeauditorium> { EntityDB = objPrograma, stateEntity = StateEntity.add };
                    repositoryMySql.SaveObject<Praprogramasdeauditorium>(entity);
                    objPrograma.IdPrAprogramaAuditoria = entity.EntityDB.IdPrAprogramaAuditoria;
                    resul.Object = objPrograma;
                    //objComplex.reqPrograma = objPrograma;

                    //var resul = objProgramaAudi.RegisterProgramaAuditoria(objPrograma);
                    //Assert.AreEqual(resul.State, ResponseType.Success);

                }
                else
                {
                    resul.Object = resulDB[0];
                    resul.Object.Praciclosprogauditoria = repositoryMySql.SimpleSelect<Praciclosprogauditorium>(("IdPrAprogramaAuditoria", resul.Object.IdPrAprogramaAuditoria));
                    List<Praciclosprogauditorium> lAuxiliar = resul.Object.Praciclosprogauditoria.ToList();
                    lAuxiliar.ForEach(x => { 
                        x.Praciclocronogramas = repositoryMySql.SimpleSelect<Praciclocronograma>(("IdPrAcicloProgAuditoria", x.IdPrAcicloProgAuditoria));                        
                        x.Praciclonormassistemas = repositoryMySql.SimpleSelect<Praciclonormassistema>(("IdPrAcicloProgAuditoria", x.IdPrAcicloProgAuditoria));
                        x.Pracicloparticipantes = repositoryMySql.SimpleSelect<Pracicloparticipante>(("IdPrAcicloProgAuditoria", x.IdPrAcicloProgAuditoria));
                        x.Pradireccionespaproductos = repositoryMySql.SimpleSelect<Pradireccionespaproducto>(("IdPrAcicloProgAuditoria", x.IdPrAcicloProgAuditoria));
                        x.Pradireccionespasistemas = repositoryMySql.SimpleSelect<Pradireccionespasistema>(("IdPrAcicloProgAuditoria", x.IdPrAcicloProgAuditoria));
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

        public ResponseObject<ComplexParametricas> GetParametricas(ComplexParametricas req)
        {
            ResponseObject<ComplexParametricas> response = new ResponseObject<ComplexParametricas> { Message = "Parametros obtenidos correctamente.", State = ResponseType.Success, Object = new ComplexParametricas() };
            try
            {
                response.Object.ListCargosParticipante = repositoryMySql.GetDataByProcedure<Pcargosparticipante>("spGetCargosParticipante", 1);
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }
    }
}
