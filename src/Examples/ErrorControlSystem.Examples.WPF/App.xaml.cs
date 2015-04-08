using System.Windows;
using ErrorControlSystem;

namespace ErrorControlSystem.Examples.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ExceptionHandler.Engine.Start(".", "UsersManagements");
        }
    }
}
