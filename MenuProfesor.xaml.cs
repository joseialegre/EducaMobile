using MySqlConnector;
using Microsoft.Maui.Controls;
namespace MauiApp1;

public partial class MenuProfesor : ContentPage
{
	public MenuProfesor(MySqlConnection conexion, int userId)
	{
		InitializeComponent();

        MateriasProfesor MateriasProfesor = new MateriasProfesor(userId);
        ListadoCurso listadoCurso = new ListadoCurso(userId);
        ShellContent Materias = new ShellContent
        {
            Title = "Notas",
            Content = MateriasProfesor,
            Route = "MateriasProfesor"
        };
        Shell.Current.Items.Add(Materias);
        ShellContent Cursos = new ShellContent
        {
            Title = "Cursos",
            Content = listadoCurso,
            Route = "ListadoCurso"
        };
        Shell.Current.Items.Add(Cursos);

    }

}