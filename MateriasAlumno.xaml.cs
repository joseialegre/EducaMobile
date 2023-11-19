using System.Data;
using MauiApp1.Clases;
using MySqlConnector;

namespace MauiApp1;

public partial class MateriasAlumno : ContentPage
{
    MySqlConnection conexion;
    int userId;

    public MateriasAlumno(MySqlConnection conexion, int userId)
	{
        
		InitializeComponent();

        this.userId = userId;
        this.conexion = conexion;

        var materias = ObtenerMateriasPorPersona(userId);

        materiasListView.ItemsSource = materias;
    }
    public class PersonaMateria
    {
        public long Id { get; set; }
        public long PersonaId { get; set; }
        public long MateriaId { get; set; }
        public DateTime FechaInscripcion { get; set; }

        // Agrega las propiedades de las materias que necesitas aquí
        public string NombreMateria { get; set; }
        public string DescripcionMateria { get; set; }
        // Agrega otras propiedades de materia según tus necesidades
    }
    public List<PersonaMateria> ObtenerMateriasPorPersona(long personaId)
    {
        List<PersonaMateria> materiasPorPersona = new List<PersonaMateria>();
        using (conexion)
        {
            

            string query = @"
            SELECT 
                phm.id,
                phm.persona_id,
                phm.materia_id,
                phm.fecha_inscripcion,
                m.nombre AS nombre_materia,
                m.descripcion AS descripcion_materia
            FROM persona_has_materia phm
            JOIN materia m ON phm.materia_id = m.id
            WHERE phm.persona_id = @personaId"
            ;

            using (MySqlCommand command = new MySqlCommand(query, conexion))
            {
                command.Parameters.Add(new MySqlParameter("@personaId", MySqlDbType.Int64) { Value = personaId });

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var personaMateria = new PersonaMateria
                        {
                            Id = reader.GetInt64(0),
                            PersonaId = reader.GetInt64(1),
                            MateriaId = reader.GetInt64(2),
                            FechaInscripcion = reader.GetDateTime(3),
                            NombreMateria = reader.GetString(4),
                            DescripcionMateria = reader.GetString(5)
                            // Agrega otras propiedades de materia según tus necesidades
                        };

                        materiasPorPersona.Add(personaMateria);
                    }
                }
            }
        }

        return materiasPorPersona;
    }
}