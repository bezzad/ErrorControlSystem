
using System.Windows;
using ErrorControlSystem;

namespace TestWpfDotNET45
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ExceptionHandler.Engine.Start(".", "UsersManagements");
        }
    }
}
