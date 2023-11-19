namespace MauiApp1;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(MenuProfesor), typeof(MenuProfesor));
        Routing.RegisterRoute(nameof(MateriasProfesor), typeof(MateriasProfesor));
        
    }
}
