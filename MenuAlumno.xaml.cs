using System.Data;
using MauiApp1.Clases;
using MySqlConnector;

namespace MauiApp1;

public partial class MenuAlumno : ContentPage
{
	MySqlConnection conexion;
	int userId;
	public MenuAlumno(MySqlConnection conexion, int userId)
	{
		InitializeComponent();
		this.userId = userId;
		this.conexion = conexion;
        this.Title = "Bienvenido! "+obtenerNombre(conexion,userId);

        //agregamos todos los items del menu desplegable, para el rol del alumno
        addMenus();
        //////////
        
    }
    private void OnMaterias(object sender, EventArgs e)
	{
        Navigation.PushAsync(new MateriasAlumno(conexion, userId));
    }
    private string obtenerNombre(MySqlConnection conexion, int userId)
    {
        
        string nombre = string.Empty;
        string usernameQuery = "SELECT username FROM persona WHERE id = @userId";
        using(conexion)
        {
            conexion.Open();
            using (MySqlCommand usernameCommand = new MySqlCommand(usernameQuery, conexion))
            {
                usernameCommand.Parameters.AddWithValue("@userId", userId);
                using (MySqlDataReader usernameReader = usernameCommand.ExecuteReader())
                {
                    if (usernameReader.Read())
                    {
                        nombre = usernameReader.GetString("username");
                    }
                }
            }
        }
        

        return nombre;
    }
    private void addMenus()
    {
        //menu materias
        MateriasAlumno MateriasAlumno = new MateriasAlumno(conexion, userId);
        ShellContent Materias = new ShellContent
        {
            Title = "Materias",
            Content = MateriasAlumno,
            Route = "MateriasAlumno"
        };
        Shell.Current.Items.Add(Materias);
        //menu pagos
        PagosAlumno PagosAlumno = new PagosAlumno(conexion, userId);
        ShellContent Pagos = new ShellContent
        {
            Title = "Pagos",
            Content = PagosAlumno,
            Route = "PagosAlumno"
        };
        Shell.Current.Items.Add(Pagos);
    }

    

    
}