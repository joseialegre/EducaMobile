using MySqlConnector;
using Microsoft.Maui.Controls;
namespace MauiApp1;

public partial class MenuProfesor : ContentPage
{
	public MenuProfesor(MySqlConnection conexion, int userId)
	{
		InitializeComponent();

        MateriasProfesor MateriasProfesor = new MateriasProfesor(conexion, userId);
        ShellContent Materias = new ShellContent
        {
            Title = "Materias",
            Content = MateriasProfesor,
            Route = "MateriasProfesor"
        };
        Shell.Current.Items.Add(Materias);

    }

}