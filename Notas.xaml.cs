using MauiApp1.Clases;
using System.Net;
using MySqlConnector;

namespace MauiApp1;

public partial class Notas : ContentPage
{
    static string servidor = "127.0.0.1";
    static string bd = "mobile";
    static string user = "root";
    static string password = "root";
    static string puerto = "3306";

    string cadenaConexion = "Server=" + servidor + ";" + "Port=" + puerto + ";" + "User Id=" + user + ";" + "Password=" + password + ";" + "Database=" + bd + ";SSL Mode =None";
    MySqlConnection conexion;
    

    int alumnoDNI;
    int materiaId;
    int profeDNI;

    int notaTrimestre1;
    int notaTrimestre2;
    int notaTrimestre3;

    private INavigation navigation;

    public Notas(int alumnoDNI, int materiaId, int profeDNI, INavigation navigation)
	{
		InitializeComponent();
        this.alumnoDNI = alumnoDNI;
        this.materiaId =materiaId;
        this.profeDNI = profeDNI;

        this.navigation = navigation;


        ObtenerNotaDesdeBaseDeDatos(out notaTrimestre1, out notaTrimestre2, out notaTrimestre3);
        conexion = new MySqlConnection(cadenaConexion);
    }
    public class NotaAlumno
    {
        public int Trimestre1;
        public int Trimestre2;
        public int Trimestre3;
    }
    private void TrimestrePicker_SelectedIndexChanged(object sender, EventArgs e)
    {

        //DisplayAlert("valores", notaTrimestre1 + " " + notaTrimestre2 + " " + notaTrimestre3, "OK");
        int selectedIndex = trimestrePicker.SelectedIndex;

        switch (selectedIndex)
        {
            case 0:
                // Seleccionado Trimestre 1
                notaLabel.Text = $"La nota del Trimestre 1 es: {notaTrimestre1}";
                break;

            case 1:
                // Seleccionado Trimestre 2
                notaLabel.Text = $"La nota del Trimestre 2 es: {notaTrimestre2}";
                break;

            case 2:
                // Seleccionado Trimestre 3
                notaLabel.Text = $"La nota del Trimestre 3 es: {notaTrimestre3}";
                break;
        }
    }

    private void ObtenerNotaDesdeBaseDeDatos(out int notaTrimestre1, out int notaTrimestre2, out int notaTrimestre3)
    {
        notaTrimestre1 = 0;
        notaTrimestre2 = 0;
        notaTrimestre3 = 0;
        using (MySqlConnection connection = new MySqlConnection(cadenaConexion))
        {
            connection.Open();

            string query = $"SELECT alumno_has_materia.nota_trimestre1 as 'Trimestre 1', " +
                           $"alumno_has_materia.nota_trimestre2 as 'Trimestre 2', " +
                           $"alumno_has_materia.nota_trimestre3 as 'Trimestre 3' " +
                           $"FROM alumno_has_materia " +
                           $"WHERE alumno_has_materia.alumno_DNI = {alumnoDNI} AND alumno_has_materia.materia_id = {materiaId}";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Obtener los valores de las notas
                        notaTrimestre1 = reader.GetInt32("Trimestre 1");
                        notaTrimestre2 = reader.GetInt32("Trimestre 2");
                        notaTrimestre3 = reader.GetInt32("Trimestre 3");
                        
                        // Aquí puedes utilizar las variables notaTrimestre1, notaTrimestre2 y notaTrimestre3 según tus necesidades
                    }
                }
            }
        }
    }
    private void Guardar_Clicked(object sender, EventArgs e)
    {
        int notaSeleccionada;
        // Obtener el trimestre y el número seleccionados
        string trimestreSeleccionado = trimestrePicker.SelectedItem as string;
        try 
        {
            notaSeleccionada = (int)numeroPicker.SelectedItem;
            try
            {
                ActualizarNotaEnBaseDeDatos(notaSeleccionada, trimestreSeleccionado);
                DisplayAlert("Datos Guardados", $"Trimestre: {trimestreSeleccionado}\nNota: {notaSeleccionada}", "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "Error al guardar la nota.", "OK");
            }
        }
        catch(Exception ex)
        {

            DisplayAlert("Error", "Por favor selecciona la nota", "OK");
        }

        // Aquí puedes realizar las operaciones necesarias con los datos
        // Por ejemplo, puedes mostrar un mensaje con los datos seleccionados/modificados


        // Aquí deberías actualizar la base de datos con la nota seleccionada
        // Utiliza tu lógica específica para interactuar con la base de datos
        


    }
    private void ActualizarNotaEnBaseDeDatos(int nota, string trimestre)
    {
        using (MySqlConnection connection = new MySqlConnection(cadenaConexion))
        {
            connection.Open();

            string columnaTrimestre;
            switch (trimestre)
            {
                case "Trimestre 1":
                    columnaTrimestre = "nota_trimestre1";
                    break;
                case "Trimestre 2":
                    columnaTrimestre = "nota_trimestre2";
                    break;
                case "Trimestre 3":
                    columnaTrimestre = "nota_trimestre3";
                    break;
                default:
                    throw new ArgumentException("Trimestre no válido");
            }

            string query = $"UPDATE alumno_has_materia " +
                           $"SET {columnaTrimestre} = {nota} " +
                           $"WHERE alumno_DNI = {alumnoDNI} AND materia_id = {materiaId}";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }
        ObtenerNotaDesdeBaseDeDatos(out notaTrimestre1, out notaTrimestre2, out notaTrimestre3);
    }
    private void VolverAtras_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            // Maneja la excepción o imprime detalles para diagnosticar el problema
            DisplayAlert("Error", ex.Message, "OK");
        }
    }
}