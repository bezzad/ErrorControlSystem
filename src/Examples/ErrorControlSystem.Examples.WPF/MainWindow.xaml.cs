using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ErrorControlSystem.Examples.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<Action> _exps;

        public MainWindow()
        {
            InitializeComponent();


            _exps = new List<Action>
            {
                () => { throw new ArithmeticException("Test"); },
                () => { throw new Exception("Test"); },
                () => { throw new InvalidExpressionException("Test"); },
                () => { throw new ApplicationException("Test"); },
                () => { throw new SystemException("Test"); },
                () => { throw new Exception("Test"); },
                () => { throw new InvalidExpressionException("Test"); },
                () => { throw new ApplicationException("Test"); },
                () => { throw new SystemException("Test"); },
                () => { throw new Exception("Test"); },
                () => { throw new InvalidExpressionException("Test"); },
                () => { throw new ApplicationException("Test"); },
                () => { throw new SystemException("Test"); },
                () => { throw new Exception("Test"); },
                () => { throw new InvalidExpressionException("Test"); },
                () => { throw new ApplicationException("Test"); },
                () => { throw new SystemException("Test"); },
                () => { throw new Exception("Test"); },
                () => { throw new InvalidExpressionException("Test"); },
                () => { throw new ApplicationException("Test"); },
                () => { throw new SystemException("Test"); },
                () => { throw new Exception("Test"); },
                () => { throw new InvalidExpressionException("Test"); },
                () => { throw new ApplicationException("Test"); },
                () => { throw new SystemException("Test"); },
                () => { throw new ArithmeticException("Test"); },
                () => { throw new Exception("Test"); },
                () => { throw new InvalidExpressionException("Test"); },
                () => { throw new ApplicationException("Test"); },
                () => { throw new SystemException("Test"); },
                () => { throw new Exception("Test"); },
                () => { throw new InvalidExpressionException("Test"); },
                () => { throw new ApplicationException("Test"); },
                () => { throw new SystemException("Test"); },
                () => { throw new Exception("Test"); },
                () => { throw new InvalidExpressionException("Test"); },
                () => { throw new ApplicationException("Test"); },
                () => { throw new SystemException("Test"); },
                () => { throw new Exception("Test"); },
                () => { throw new InvalidExpressionException("Test"); },
                () => { throw new ApplicationException("Test"); },
                () => { throw new SystemException("Test"); },
                () => { throw new Exception("Test"); },
                () => { throw new InvalidExpressionException("Test"); },
                () => { throw new ApplicationException("Test"); },
                () => { throw new SystemException("Test"); },
                () => { throw new Exception("Test"); },
                () => { throw new InvalidExpressionException("Test"); },
                () => { throw new ApplicationException("Test"); },
                () => { throw new SystemException("Test"); }
            };
        }

        private void btnThrowHandledException_Click(object sender, RoutedEventArgs e)
        {
            var item = _exps[new Random().Next(0, _exps.Count - 1)];
            try
            {
                item();
            }
            catch (Exception ex)
            {
                ex.Data.Add("test1", "1");
                ex.Data.Add("test2", new Random().Next());
            }
        }

        private void btnThrowUnHandledExceptions_Click(object sender, RoutedEventArgs e)
        {
            throw new Exception("Test UnHandled Thread Exception");
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
