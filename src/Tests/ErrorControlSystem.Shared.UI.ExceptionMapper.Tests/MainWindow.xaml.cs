using System;
using System.Windows;

namespace ErrorControlSystem.Shared.UI.ExceptionMapper.Tests
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnThrowException_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                TestClassA.A1();
            }
            catch (Exception exp)
            {
                ExceptionViewer.Add(exp);
            }
        }
    }



}
