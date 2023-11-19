using MySqlConnector;

namespace MauiApp1;

public partial class PagosAlumno : ContentPage
{
	public PagosAlumno(MySqlConnection conexion, int userId)
	{
		InitializeComponent();
	}
}