using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Clases
{
    internal class Conexion
    {
        static string servidor = "bm5hvpmvn9suu8rwxkgz-mysql.services.clever-cloud.com";
        static string bd = "bm5hvpmvn9suu8rwxkgz";
        static string user = "ubswm7wxsk3fx6yb";
        static string password = "i9xI3WitbzQZ9sTaN2CH";
        static string puerto = "3306";

        public static string cadenaConexion = "Server=" + servidor + ";" + "Port=" + puerto + ";" + "User Id=" + user + ";" + "Password=" + password + ";" + "Database=" + bd + ";SSL Mode =None";


    }
}
