using MauiApp1.Clases;

using MySqlConnector;

namespace MauiApp1;

public partial class MateriasProfesor : ContentPage
{
    MySqlConnection conexion;
    int DNI;

    public MateriasProfesor(MySqlConnection conexion, int DNI)
	{
		InitializeComponent();

        this.DNI = DNI;
        this.conexion = conexion;

        var materias = ObtenerMateriasPorDocente(DNI);

        materiasListView.ItemsSource = materias;
        //materiasListView.ItemTapped = 
    }
    public class DocenteMateria
    {
        public long Id { get; set; }
        public long DoncenteDNI { get; set; }
        public int MateriaId { get; set; }
        //public DateTime FechaInscripcion { get; set; }

        // Agrega las propiedades de las materias que necesitas aquí
        public string NombreMateria { get; set; }
        public string DescripcionMateria { get; set; }
        // Agrega otras propiedades de materia según tus necesidades
    }
    public List<DocenteMateria> ObtenerMateriasPorDocente(int DNI)
    {
        List<DocenteMateria> materiasPorDocente = new List<DocenteMateria>();
        using (conexion)
        {

            string query = "SELECT materia.id, materia.nombre, materia.descripcion " +
                "FROM materia " +
                "INNER JOIN docente_has_materia ON docente_has_materia.materia_id = materia.id " +
                "WHERE docente_has_materia.docente_dni = @DNI";


            using (MySqlCommand command = new MySqlCommand(query, conexion))
            {

                command.Parameters.Add(new MySqlParameter("@DNI", MySqlDbType.Int32) { Value = DNI });

                using (MySqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var DocenteMateria = new DocenteMateria
                        {
                            MateriaId = reader.GetInt32("id"),
                            NombreMateria = reader.GetString("nombre"),
                            DescripcionMateria = reader.GetString("descripcion"),

                            // Agrega otras propiedades de materia según tus necesidades

                        };

                        materiasPorDocente.Add(DocenteMateria);
                    }
                }
            }
        }

        return materiasPorDocente;
    }

    private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
            return;

        // Desactiva la selección del elemento
        ((ListView)sender).SelectedItem = null;

        // Obtiene el elemento seleccionado del ListView
        var selectedMateria = (DocenteMateria)e.SelectedItem;
        int idmateria = selectedMateria.MateriaId;
        // Navega a la nueva página pasando el objeto seleccionado como parámetro
        Navigation.PushAsync(new AlumnoMateria(idmateria,DNI));

        //Navigation.PushAsync(new MenuProfesor(conexion, DNI));
    }
}