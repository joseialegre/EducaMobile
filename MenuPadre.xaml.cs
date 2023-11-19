using MySqlConnector;

namespace MauiApp1;

public partial class MenuPadre : ContentPage
{
	public MenuPadre(MySqlConnection conexion, int userId)
	{
		InitializeComponent();

	}
}