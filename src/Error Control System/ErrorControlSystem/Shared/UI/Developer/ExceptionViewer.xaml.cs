using System;
using System.Windows;

namespace ErrorControlSystem.Shared.UI.Developer
{
    /// <summary>
    /// Interaction logic for ExceptionViewer.xaml
    /// </summary>
    public partial class ExceptionViewer : Window
    {
        public ExceptionViewer()
        {
            InitializeComponent();

            BtnContinue.Content = Properties.Localization.BtnContinue;
            BtnExit.Content = Properties.Localization.BtnExit;
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


        public ProcessFlow ShowDialog(Exception exp)
        {
            ExpMapper.Add(exp);

            var dialogResult = this.ShowDialog();
            return (dialogResult != null && (bool)dialogResult) ? ProcessFlow.Continue : ProcessFlow.Exit;
        }
    }
}
