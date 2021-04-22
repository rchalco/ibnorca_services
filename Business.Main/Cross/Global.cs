using PlumbingProps.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Cross
{
    public static class Global
    {
        public static string URIGLOBAL_SERVICES = ConfigManager.GetConfiguration().GetSection("URIGLOBAL_SERVICES").Value;
        public static string URIGLOBAL_CLASIFICADOR = ConfigManager.GetConfiguration().GetSection("URIGLOBAL_CLASFICADOR").Value;
        public static string URI_CLIENTE = "ws-au-cliente.php";
        public static string URI_SERVICIO = "ws-au-servicio.php";
        public static string URI_CLIENTE_CONTACTO = "ws-au-cliente-contacto.php";
        public static string URI_SIMULACION = "ws-au-simulacion-servicio.php";
        public static string URI_CERTIFICADO = "ws-au-certificado.php";
        public static string URI_PERSONAL = "ws-au-calificado.php";
        public static string URI_PAISES = "ws-paises.php";        
        public static string URI_NORMAS = "ws-au-catalogo-nal.php";
        public static string URI_NORMAS_INT = "ws-au-catalogo-int.php";
        public static string URI_CARGOS = "ws-au-calificado.php";
        public static string URI_CLASIFICADOR = "ws-clasificador-post.php";
        public static string KEY_SERVICES = "f5bebe5def9bfe88eb8932f9da7d34cd";

        
        public static string IDENTIFICADOR = "iauditoria";

        ///Seccion de plantillas        
        public static string PATH_PLANTILLAS= ConfigManager.GetConfiguration().GetSection("PLANTILLAS__PATH").Value; 
        //para cadena de conexion  
        //optionsBuilder.UseMySql(ConfigManager.GetConfiguration().GetSection("conexionString").Value, Microsoft.EntityFrameworkCore.ServerVersion.FromString("8.0.22-mysql")); 

    }
}
