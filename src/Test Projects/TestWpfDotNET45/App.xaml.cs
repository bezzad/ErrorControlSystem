
using System.Windows;
using ErrorHandlerEngine.DbConnectionManager;
using ErrorHandlerEngine.ExceptionManager;

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

            ExpHandlerEngine.Start(new Connection(".", "UsersManagements"));
        }
    }
}
