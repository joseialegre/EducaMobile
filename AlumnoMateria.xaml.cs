
using System.Net;
using MySqlConnector;

namespace MauiApp1;

public partial class AlumnoMateria : ContentPage
{
    static string servidor = "127.0.0.1";
    static string bd = "mobile";
    static string user = "root";
    static string password = "root";
    static string puerto = "3306";

    string cadenaConexion = "Server=" + servidor + ";" + "Port=" + puerto + ";" + "User Id=" + user + ";" + "Password=" + password + ";" + "Database=" + bd + ";SSL Mode =None";


    Notas notasPage;
    Notas2xaml notasPage2;

    //MySqlConnection conexion;
    int materiaId;
    int profeDNI;
    public AlumnoMateria(int materiaId, int profeDNI)
    {
        InitializeComponent();
        //this.conexion = conexion;
        this.materiaId = materiaId;

        var alumnos = ObtenerAlumnosPorMateria(materiaId);
        alumnosListView.ItemsSource = alumnos;
        this.profeDNI = profeDNI;
    }

    public class AlumnoenMateria
    {
        public long MateriaId { get; set; }
        public int AlumnoDNI { get; set; }
        public int AlumnoLegajo { get; set; }
        public string AlumnoNombre { get; set; }
        public string AlumnoApellido { get; set; }

    }

    public List<AlumnoenMateria> ObtenerAlumnosPorMateria(int materiaId)
    {
        List<AlumnoenMateria> alumnosPorMateria = new List<AlumnoenMateria>();

        using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
        {
            string query = "SELECT alumno.Nombre, alumno.Apellido, alumno.Legajo, alumno.DNI " +
                "FROM alumno RIGHT JOIN alumno_has_materia ON alumno_has_materia.alumno_DNI = alumno.DNI " +
                "WHERE alumno_has_materia.materia_id = @materiaId";

            using (MySqlCommand command2 = new MySqlCommand(query, conexion))
            {
                command2.Parameters.AddWithValue("@materiaId", materiaId);

                conexion.Open();  // Abre la conexión antes de ejecutar la consulta

                using (MySqlDataReader reader2 = command2.ExecuteReader())
                {
                    while (reader2.Read())
                    {
                        var AlumnoenMateria = new AlumnoenMateria
                        {
                            AlumnoDNI = reader2.GetInt32("DNI"),
                            AlumnoLegajo = reader2.GetInt32("legajo"),
                            AlumnoNombre = reader2.GetString("nombre"),
                            AlumnoApellido = reader2.GetString("apellido")
                        };

                        alumnosPorMateria.Add(AlumnoenMateria);
                    }
                }  // La conexión se cerrará automáticamente al salir del bloque using MySqlDataReader
            }  // La conexión se cerrará automáticamente al salir del bloque using MySqlCommand
            conexion.Close();
        }  // La conexión se cerrará automáticamente al salir del bloque using MySqlConnection
        
        return alumnosPorMateria;
    }

    private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
            return;

        // Desactiva la selección del elemento
        ((ListView)sender).SelectedItem = null;

        // Obtiene el elemento seleccionado del ListView
        var selectedAlumno = (AlumnoenMateria)e.SelectedItem;
        int dnialumno = selectedAlumno.AlumnoDNI;

        // Navega a la nueva página pasando el objeto seleccionado como parámetro
        
        Navigation.PushAsync(new Notas(dnialumno, materiaId, profeDNI, Navigation));
        
    }
    private void irOtraPagina(object sender, EventArgs e)
    {
        notasPage2 = new Notas2xaml();
        Navigation.PushAsync(notasPage2);
        
    }

}