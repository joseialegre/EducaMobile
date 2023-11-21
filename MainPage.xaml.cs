using MauiApp1.Clases;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls.PlatformConfiguration;

using MySqlConnector;


namespace MauiApp1;

public partial class MainPage : ContentPage
{
    //static string servidor = "b4rvajc57svembdlstfn-mysql.services.clever-cloud.com";
    //static string bd = "b4rvajc57svembdlstfn";
    //static string user = "u2jtqd59omfyrxxf";
    //static string password = "IFQEz4NITM6m2Zc8gHNi";
    //static string puerto = "3306";



    static string servidor = "127.0.0.1";
    static string bd = "mobile";
    static string user = "root";
    static string password = "root";
    static string puerto = "3306";

    string cadenaConexion = "Server=" + servidor + ";" + "Port=" + puerto + ";" + "User Id=" + user + ";" + "Password=" + password + ";" + "Database=" + bd + ";SSL Mode =None";
    MySqlConnection conexion = new MySqlConnection();
    
	public MainPage()
    {
		InitializeComponent();
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
    }
    private void OnIniciarSesionClicked(object sender, EventArgs e)
    {
        
        // Acceder a las entradas utilizando los nombres asignados
        string username = entryUsername.Text;
        string password = entryPassword.Text;
        string rol = string.Empty;
        int DNI = 0;



        using var conexion = new MySqlConnection(Conexion.cadenaConexion);
        {
            conexion.Open();

            string query = "SELECT * FROM docente WHERE username = @username AND password = @password";

            using (MySqlCommand command = new MySqlCommand(query, conexion))
            {
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        DNI = reader.GetInt32("DNI");

                        // Cerrar el DataReader de usuario_usuario antes de abrir otro DataReader
                        reader.Close();
                        Navigation.PushAsync(new MenuProfesor(conexion, DNI));
                    }
                    else
                    {
                        // Las credenciales no son válidas, muestra un mensaje de error o toma otra acción
                        ShowNotification("Usuario o contraseña incorrecta / Acceso solo para docentes.");
                    }
                }
            }
        }
        
    }

    private void ShowNotification(string message)
    {
        Device.BeginInvokeOnMainThread(async () =>
        {
            await App.Current.MainPage.DisplayAlert("Notificación", message, "OK");
        });
    }
    public bool TestDatabaseConnection(string cadenaConexion)
    {
        string connectionString = cadenaConexion;

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                return true; // La conexión se estableció con éxito
            }
        }
        catch (MySqlException ex)
        {
            // Manejo de errores: aquí puedes registrar el error o realizar otras acciones necesarias.
            // También puedes mostrar un mensaje de error si lo deseas.
            ShowNotification("Error al conectar a la base de datos: " + ex.Message);
            return false; // La conexión falló
        }
    }
    public void testerconnection(object sender, EventArgs e)
    {
        bool isConnected = TestDatabaseConnection(cadenaConexion);

        if (isConnected)
        {
            ShowNotification("todo ok");
        }
        else
        {
            ShowNotification("todo mal");
        }
    }
    public void changetheme(object sender, EventArgs e)
    {
        
    }
}

