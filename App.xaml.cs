using Microsoft.Maui.ApplicationModel;

namespace MauiApp1;

public partial class App : Application
{
	public App()
	{
        InitializeComponent();
       
        // Primera Shell (por defecto)
        MainPage = new AppShell();

        
    }
}
