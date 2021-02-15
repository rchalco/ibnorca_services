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
    public class AperturaAuditoriaManager: BaseManager
    {
        public ResponseObject<ComplexProgramaAuditoria> RegisterProgramaAuditoria(ComplexProgramaAuditoria req)
        {
            ResponseObject<ComplexProgramaAuditoria> response = new ResponseObject<ComplexProgramaAuditoria>();
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

                Entity<Programasdeauditorium> entity = new Entity<Programasdeauditorium> { EntityDB = req.reqPrograma, stateEntity = StateEntity.add };
                /*if (req.reqPrograma.IdProgramaAuditoria != 0)
                {
                    entity.stateEntity = StateEntity.modify;
                }*/

                repositoryMySql.SaveObject<Programasdeauditorium>(entity);


                //if (response. == null)
                //{
                //}
                //ciclos 
                req.reqCiclosProg.IdProgramaAuditoria = req.reqPrograma.IdProgramaAuditoria;
                Entity<Ciclosprogauditorium> entityCiclos = new Entity<Ciclosprogauditorium> { EntityDB = req.reqCiclosProg, stateEntity = StateEntity.add };
                
                repositoryMySql.SaveObject<Ciclosprogauditorium>(entityCiclos);

                //Cronograma  
                req.reqCronograma.IdCicloProgAuditoria = req.reqCiclosProg.IdCicloProgAuditoria;
                Entity<Ciclocronograma> entityCronograma = new Entity<Ciclocronograma> { EntityDB = req.reqCronograma, stateEntity = StateEntity.add };
                if (req.reqPrograma.IdProgramaAuditoria != 0)
                {
                    entity.stateEntity = StateEntity.modify;
                }
                repositoryMySql.SaveObject<Ciclocronograma>(entityCronograma);

                // Participantes
                req.reqParticipante.IdCicloProgAuditoria = req.reqCiclosProg.IdCicloProgAuditoria;
                Entity<Cicloparticipante> entityParticipante = new Entity<Cicloparticipante> { EntityDB = req.reqParticipante, stateEntity = StateEntity.add };
                if (req.reqPrograma.IdProgramaAuditoria != 0)
                {
                    entity.stateEntity = StateEntity.modify;
                }
                repositoryMySql.SaveObject<Cicloparticipante>(entityParticipante);


                //PROUCTOS
                req.reqDireccionesProducto.IdCicloProgAuditoria = req.reqCiclosProg.IdCicloProgAuditoria;
                Entity<Direccionespaproducto> entityProd = new Entity<Direccionespaproducto> { EntityDB = req.reqDireccionesProducto, stateEntity = StateEntity.add };
                if (req.reqDireccionesProducto.IdDireccionPaproducto != 0)
                {
                    entity.stateEntity = StateEntity.modify;
                }
                repositoryMySql.SaveObject<Direccionespaproducto>(entityProd);


                Entity<Direccionespasistema> entitySis = new Entity<Direccionespasistema> { EntityDB = req.reqDireccionesPASistema, stateEntity = StateEntity.add };
                if (req.reqDireccionesProducto.IdDireccionPaproducto != 0)
                {
                    entity.stateEntity = StateEntity.modify;
                }
                
                repositoryMySql.SaveObject<Direccionespasistema>(entitySis);
                */

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
