using MauiApp1.Clases;
using MySqlConnector;
//using Windows.System;

namespace MauiApp1;

public partial class ListadoAlumno : ContentPage
{
    static string servidor = "127.0.0.1";
    static string bd = "mobile";
    static string user = "root";
    static string password = "root";
    static string puerto = "3306";

    string cadenaConexion = "Server=" + servidor + ";" + "Port=" + puerto + ";" + "User Id=" + user + ";" + "Password=" + password + ";" + "Database=" + bd + ";SSL Mode =None";

    int CursoId;
    int MateriaId;

    public ListadoAlumno(int CursoId, int MateriaId)
	{
		InitializeComponent();
        this.CursoId = CursoId;
        this.MateriaId = MateriaId;

        var alumnos = ObtenerAlumnosPorMateria(CursoId, MateriaId);

        alumnosListView.ItemsSource = alumnos;
    }

    public class MateriaAlumno
    {
        public int AlumnoDNI { get; set; }
        public int LegajoAlumno { get; set; }
        public string NombreAlumno { get; set; }
        public string ApellidoAlumno { get; set; }
        public string EmailAlumno { get; set; }
    }
    public List<MateriaAlumno> ObtenerAlumnosPorMateria(int CursoId, int MateriaId)
    {
        List<MateriaAlumno> materiasPorAlumno = new List<MateriaAlumno>();
        using (MySqlConnection conexion = new MySqlConnection(Conexion.cadenaConexion))
        {
            conexion.Open();

            string query = "SELECT  a.Nombre, a.Apellido, a.Legajo, a.DNI, a.Email " +
                "FROM alumno as a " +
                "JOIN alumno_has_materia1 as ahm ON ahm.alumno_DNI = a.DNI " +
                "WHERE ahm.curso_id_curso = @CursoId AND ahm.materia_id = @MateriaId";

            using (MySqlCommand command = new MySqlCommand(query, conexion))
            {
                
                command.Parameters.AddWithValue("@CursoId",CursoId);
                command.Parameters.AddWithValue("@MateriaId", MateriaId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var MateriaAlumno = new MateriaAlumno
                        {
                            NombreAlumno = reader.GetString(0),
                            ApellidoAlumno = reader.GetString(1),
                            LegajoAlumno = reader.GetInt32(2),
                            AlumnoDNI = reader.GetInt32(3),
                            EmailAlumno = reader.GetString(4),

                        };

                        materiasPorAlumno.Add(MateriaAlumno);
                    }
                }
            }
        }

        return materiasPorAlumno;
    }


}