using Business.Main.Base;
using Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.ListarAuditoresxCargoCalificadoDTO;
using Business.Main.Modules.TomaDecision.DTO;
using Domain.Main.Wraper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.TomaDecision
{
    public class TomaDecisionManager : BaseManager
    {
       

        public ResponseObject<int> DevuelveCorrelativoDocAuditoria(long idElaAuditoria, int gestion, int idTipoDocumento)
        {

            ResponseObject<int> response = new ResponseObject<int> { Message = "Cargos obtenidos obtenido correctamente.", State = ResponseType.Success };
            try
            {

                ///TDO: obtenemos los datos del servicio
                var resultBd = repositoryMySql.GetDataByProcedure<Dto_spTmdConsecutivoDocAudi>("spTmdConsecutivoDocAudi", idElaAuditoria, gestion, idTipoDocumento);
                response.Object = resultBd[0].ConsecutivoDocAudi;

            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
            
            /*
            string connStr = "server=192.168.0.106;user=usrIbnorca;database=ibnorca;port=3306;password=admin.123";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                string sql = "select fTmdConsecutivoDocAudi(1,2021,2);";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine(rdr[0] + " -- " + rdr[1]);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
            Console.WriteLine("Done.");
            return 1;
            /*
            oCn = da.GetConnection();

                int res;

                if (oCn == null)
                {
                    oCn.Open();
                }

                sInsProcName = "fLogin_Check";
                insertcommand = new MySqlCommand(sInsProcName, oCn);
                insertcommand.CommandType = CommandType.StoredProcedure;
                insertcommand.Parameters.Add(new MySqlParameter("mRes", MySqlDbType.Int32, 0));
                insertcommand.Parameters["mRes"].Direction = ParameterDirection.ReturnValue;
                insertcommand.Parameters.Add("mUserName", MySqlDbType.VarChar, 50, mUserName);
                insertcommand.Parameters.Add("mUserPass", MySqlDbType.VarChar, 40, mPass);
                insertcommand.Parameters.Add("mUserKey", MySqlDbType.VarChar, 40);
                insertcommand.Parameters["mUserKey"].Value = mKey;

                res = insertcommand.ExecuteNonQuery();
                //res = int.Parse(insertcommand.Parameters["mRes"].Value.ToString());

                return (res);

                oCn.Close();*/
            
        }
    }
}
