using System;
using System.Windows;

namespace ErrorControlSystem.Shared.UI.Developer
{
    /// <summary>
    /// Interaction logic for ExceptionViewer.xaml
    /// </summary>
    public partial class ExceptionViewer : Window
    {
        public ExceptionViewer(Exception exp)
        {
            InitializeComponent();

            ExpMapper.Add(exp);
        }

        private void btnExit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }

        private void btnContinue_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DialogResult = true;
            Close();
        }
    }
}
