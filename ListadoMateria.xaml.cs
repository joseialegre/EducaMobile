using System.Net;
using MauiApp1.Clases;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Maui.Controls;
using MySqlConnector;
namespace MauiApp1;

public partial class ListadoMateria : ContentPage
{
    static string servidor = "127.0.0.1";
    static string bd = "mobile";
    static string user = "root";
    static string password = "root";
    static string puerto = "3306";

    string cadenaConexion = "Server=" + servidor + ";" + "Port=" + puerto + ";" + "User Id=" + user + ";" + "Password=" + password + ";" + "Database=" + bd + ";SSL Mode =None";

    int CursoId;
    int DocenteDNI;
    public ListadoMateria(int CursoId, int DocenteDNI)
	{
		InitializeComponent();

        this.CursoId = CursoId;
        this.DocenteDNI = DocenteDNI;
        var materias = ObtenerMateriasPorDocente(DocenteDNI);

        materiasListView.ItemsSource = materias;
    }
    public class CursoMateria
    {
        public int MateriaId { get; set; }
        public string NombreMateria { get; set; }
        public string DescripcionMateria { get; set; }
        public string HorarioMateria { get; set; }
        public string AulaMateria { get; set; }
    }
    public List<CursoMateria> ObtenerMateriasPorDocente(int DNI)
    {
        List<CursoMateria> materiasPorCurso = new List<CursoMateria>();
        using (MySqlConnection conexion = new MySqlConnection(Conexion.cadenaConexion))
        {
            conexion.Open();

            string query = "SELECT id, Nombre, Descripcion, Horarios, Aula FROM materia join docente_has_materia ON docente_has_materia.Materia_id = materia.id WHERE docente_has_materia.Docente_DNI = @DocenteDNI";


            using (MySqlCommand command = new MySqlCommand(query, conexion))
            {

                command.Parameters.Add(new MySqlParameter("@DocenteDNI", MySqlDbType.Int32) { Value = DNI });

                using (MySqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var CursoMateria = new CursoMateria
                        {
                            MateriaId = reader.GetInt32("id"),
                            NombreMateria = reader.GetString("Nombre"),
                            DescripcionMateria = reader.GetString("Descripcion"),
                            HorarioMateria = reader.GetString("Horarios"),
                            AulaMateria = reader.GetString("Aula"),

                            // Agrega otras propiedades de materia según tus necesidades

                        };

                        materiasPorCurso.Add(CursoMateria);
                    }
                }
            }
        }

        return materiasPorCurso;
    }
    int MateriaId;
    private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
            return;

        // Desactiva la selección del elemento
        ((ListView)sender).SelectedItem = null;

        // Obtiene el elemento seleccionado del ListView
        var selectedMateria = (CursoMateria)e.SelectedItem;
        MateriaId = selectedMateria.MateriaId;
        // Navega a la nueva página pasando el objeto seleccionado como parámetro


        //COMPLETAR
        Navigation.PushAsync(new ListadoAlumno(CursoId, MateriaId));


    }
}