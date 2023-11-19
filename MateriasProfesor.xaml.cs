using MauiApp1.Clases;
using Microsoft.Maui.Controls.Shapes;
using MySqlConnector;

namespace MauiApp1;

public partial class MateriasProfesor : ContentPage
{
    MySqlConnection conexion;
    long DNI;

    public MateriasProfesor(MySqlConnection conexion, long DNI)
	{
		InitializeComponent();

        this.DNI = DNI;
        this.conexion = conexion;

        var materias = ObtenerMateriasPorDocente(DNI);

        materiasListView.ItemsSource = materias;
    }
    public class DocenteMateria
    {
        public long Id { get; set; }
        public long DoncenteDNI { get; set; }
        public long MateriaId { get; set; }
        //public DateTime FechaInscripcion { get; set; }

        // Agrega las propiedades de las materias que necesitas aquí
        public string NombreMateria { get; set; }
        public string DescripcionMateria { get; set; }
        // Agrega otras propiedades de materia según tus necesidades
    }
    public List<DocenteMateria> ObtenerMateriasPorDocente(long personaId)
    {
        List<DocenteMateria> materiasPorDocente = new List<DocenteMateria>();
        using (conexion)
        {

            string query = "SELECT materia.id, materia.nombre, materia.descripcion " +
                "FROM materia " +
                "INNER JOIN docente_has_materia ON docente_has_materia.materia_id = materia.id " +
                "WHERE docente_has_materia.docente_dni = 111";


            using (MySqlCommand command = new MySqlCommand(query, conexion))
            {

                command.Parameters.Add(new MySqlParameter("@personaId", MySqlDbType.Int64) { Value = personaId });

                using (MySqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var DocenteMateria = new DocenteMateria
                        {
                            MateriaId = reader.GetInt64("id"),
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
}