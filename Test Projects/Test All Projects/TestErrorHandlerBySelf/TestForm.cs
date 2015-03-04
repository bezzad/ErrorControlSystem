using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestErrorHandlerBySelf
{
    public partial class FormTest : Form
    {
        private readonly List<Action> _exps;

        public FormTest()
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
                () => { throw new SystemException("Test"); }
            };

        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            // End the program.
            Close();
        }


        private void btnTestHandledFirstExp_Click(object sender, EventArgs e)
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

        private void btnTestUnHandledUIExp_Click(object sender, EventArgs e)
        {
            Program.Exp();
        }

        private void btnTestUnHandledThreadExp_Click(object sender, EventArgs e)
        {
            throw new Exception("Test UnHandled Thread Exception");
        }

        private void btnTestUnhandledTaskExp_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            { throw new Exception("Test UnHandled Task Exception"); });
        }

        private void btnThrowExceptExceptions_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException("Test Except Exceptions");
        }

    }
}
