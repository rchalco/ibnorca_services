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
