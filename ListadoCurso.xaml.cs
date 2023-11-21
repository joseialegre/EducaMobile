using System.Net;
using MauiApp1.Clases;
using MySqlConnector;
namespace MauiApp1;

public partial class ListadoCurso : ContentPage
{
    static string servidor = "127.0.0.1";
    static string bd = "mobile";
    static string user = "root";
    static string password = "root";
    static string puerto = "3306";

    string cadenaConexion = "Server=" + servidor + ";" + "Port=" + puerto + ";" + "User Id=" + user + ";" + "Password=" + password + ";" + "Database=" + bd + ";SSL Mode =None";

    int DocenteDNI;

    public ListadoCurso(int DoncenteDNI)
	{
		InitializeComponent();
        this.DocenteDNI = DoncenteDNI;

        var cursos = ObtenerMateriasPorDocente(DoncenteDNI);
        cursosListView.ItemsSource = cursos;

    }
    public class DocenteCurso
    {
        public int CursoId { get; set; }
        public int DoncenteDNI { get; set; }
        public string NombreCurso { get; set; }
        public string DescripcionCurso { get; set; }
    }
    public List<DocenteCurso> ObtenerMateriasPorDocente(int DNI)
    {
        List<DocenteCurso> CursosPorDocente = new List<DocenteCurso>();
        using (MySqlConnection conexion = new MySqlConnection(Conexion.cadenaConexion))
        {
            conexion.Open();

            string query = "Select id_curso, nombre, descripcion FROM curso";


            using (MySqlCommand command = new MySqlCommand(query, conexion))
            {

                //command.Parameters.Add(new MySqlParameter("@DNI", MySqlDbType.Int32) { Value = DNI });

                using (MySqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var DocenteCurso = new DocenteCurso
                        {
                            CursoId = reader.GetInt32("id_curso"),
                            NombreCurso = reader.GetString("nombre"),
                            DescripcionCurso = reader.GetString("descripcion")
                        };

                        CursosPorDocente.Add(DocenteCurso);
                    }
                }
            }
        }

        return CursosPorDocente;
    }
    int CursoId;
    private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
            return;

        // Desactiva la selección del elemento
        ((ListView)sender).SelectedItem = null;

        // Obtiene el elemento seleccionado del ListView
        var selectedCurso = (DocenteCurso)e.SelectedItem;
        CursoId = selectedCurso.CursoId;
        // Navega a la nueva página pasando el objeto seleccionado como parámetro


        //Navigation.PushAsync(new AlumnoMateria2(idmateria,DNI));

        Navigation.PushAsync(new ListadoMateria(CursoId,DocenteDNI ));


    }
}