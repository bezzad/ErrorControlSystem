using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestErrorHandlerBySelf
{
    public partial class FormTest : Form
    {
        private List<Action> Exps;

        public FormTest()
        {
            InitializeComponent();

            Exps = new List<Action>
            {
                () => { int a = 0, b = 10, c = b/a; },
                () => { throw new ArithmeticException(); },
                () => { throw new Exception("TEst"); },
                () => { throw new InvalidExpressionException(); },
                () => { throw new ApplicationException(); },
                () => { throw new SystemException(); }
            };

        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            // End the program.
            Close();
        }


        private void btnTestHandledFirstExp_Click(object sender, EventArgs e)
        {
            var item = Exps[new Random().Next(0, Exps.Count - 1)];
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
            Program.UIExp();
        }

        private void btnTestUnHandledThreadExp_Click(object sender, EventArgs e)
        {

            int a = 0, b = 10, c = b / a;
        }

        private void btnTestUnhandledTaskExp_Click(object sender, EventArgs e)
        {

            Task.Run(() =>
            { int a = 0, b = 10, c = b / a; });

            Task.Run(() =>
            { int a = 0, b = 8, c = b / a; });
        }

    }
}
