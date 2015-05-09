using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ErrorControlSystem.Examples.WinForms.SomeNamespace
{
    public partial class FormTest : Form
    {
        private readonly List<Action> _exps;

        public FormTest()
        {
            InitializeComponent();

            _exps = new List<Action>
            {
                () => { throw new FormatException("Test"); },
                () => { throw new Exception("Test"); },
                () => { throw new InvalidExpressionException("Test"); },
                () => { throw new ApplicationException("Test"); },
                () => { throw new SystemException("Test"); },
                () => { throw new Exception("Test"); },
                () => { throw new FormatException("Test"); },
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
                () => { throw new FormatException("Test"); },
                () => { throw new ApplicationException("Test"); },
                () => { throw new SystemException("Test"); },
                () => { throw new Exception("Test"); },
                () => { throw new InvalidExpressionException("Test"); },
                () => { throw new FormatException("Test"); },
                () => { throw new SystemException("Test"); },
                () => { throw new ArithmeticException("Test"); },
                () => { throw new Exception("Test"); },
                () => { throw new FormatException("Test"); },
                () => { throw new ApplicationException("Test"); },
                () => { throw new FormatException("Test"); },
                () => { throw new Exception("Test"); },
                () => { throw new InvalidExpressionException("Test"); },
                () => { throw new ApplicationException("Test"); },
                () => { throw new SystemException("Test"); },
                () => { throw new Exception("Test"); },
                () => { throw new InvalidExpressionException("Test"); },
                () => { throw new ApplicationException("Test"); },
                () => { throw new SystemException("Test"); },
                () => { throw new Exception("Test"); },
                () => { throw new FormatException("Test"); },
                () => { throw new FormatException("Test"); },
                () => { throw new SystemException("Test"); },
                () => { throw new FormatException("Test"); },
                () => { throw new InvalidExpressionException("Test"); },
                () => { throw new ApplicationException("Test"); },
                () => { throw new SystemException("Test"); },
                () => { throw new Exception("Test"); },
                () => { throw new InvalidExpressionException("Test"); },
                () => { throw new ApplicationException("Test"); },
                () => { throw new FormatException("Test"); }
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

        private void btnDataException_Click(object sender, EventArgs e)
        {
            try
            {
                var exp = new XExp();
                exp.Data.Add("test data Key", "string Value");
                exp.Data.Add("test data Key2", 25156);
                throw exp;
            }
            catch { }
        }

        private void btnThrowMultiExps_Click(object sender, EventArgs e)
        {
            Parallel.For(0, 8, (i) =>
            {
                try
                {
                    _exps[new Random().Next(0, _exps.Count - 1)]();
                }
                catch { }
            });
        }

        private void btnExemptedMethodException_Click(object sender, EventArgs e)
        {
            new ExpThrower().throwExp();
        }

        private void btnViolation_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Debugger.Break();
            this.AccessViolation();
        }

        private unsafe void AccessViolation()
        {
            var b = *(byte*)8762765876;
        }

        private void btnTestExceptionHandlerSpeed_Click(object sender, EventArgs e)
        {
            const long oneMinute = 60000;

            Task.Factory.StartNew(() =>
            {
                int counter = 0;
                var firstTime = Environment.TickCount;

                while (Environment.TickCount - firstTime <= oneMinute)
                {
                    try
                    {
                        throw new InvalidCastException("Speed Test Exception");
                    }
                    catch
                    {
                        counter++;
                    }
                }

                MessageBox.Show("Number of exceptions that handling in one minute is: " + counter);
            });
        }

        private void cmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(cmbLanguage.SelectedValue.ToString());
        }

        private void FormTest_Load(object sender, EventArgs e)
        {
            var tblData = new DataTable();
            tblData.Columns.Add("Culture", typeof(String));
            tblData.Columns.Add("EnglishName", typeof(String));

            tblData.Rows.Add("ar", "Arabic");
            tblData.Rows.Add("az", "Azerbaijan");
            tblData.Rows.Add("zh-CN", "Chinese");
            tblData.Rows.Add("en", "English");
            tblData.Rows.Add("fr-FR", "French");
            tblData.Rows.Add("de", "German");
            tblData.Rows.Add("he", "Hebrew");
            tblData.Rows.Add("fa-IR", "Persian");
            tblData.Rows.Add("ru-RU", "Russian");
            tblData.Rows.Add("tr-TR", "Turkish");

            cmbLanguage.DataSource = tblData;
            cmbLanguage.DisplayMember = "EnglishName";
            cmbLanguage.ValueMember = "Culture";


            this.cmbLanguage.SelectedIndexChanged += new System.EventHandler(this.cmbLanguage_SelectedIndexChanged);

            cmbLanguage.SelectedValue = CultureInfo.CurrentCulture.Name;
        }
    }

    public class TestTable
    {
        public string EnglishName
        {
            get;
            set;
        }

        public string Culture
        {
            get;
            set;
        }

        public TestTable(string name, string culture)
        {
            EnglishName = name;
            Culture = culture;
        }
    }



    class XExp : Exception
    {
        public string MyPublicData1 { get; set; }


        public string MyPublicData2 { get; set; }

        public string MyPublicData3 { get; set; }

        public string MyPublicData4 { get; set; }

        public string MyPublicData5 { get; set; }



        public XExp()
        {
            MyPublicData1 = "I am Public Property 1";
            MyPublicData2 = "I am Public Property 2";
            MyPublicData3 = "I am Public Property 3";
            MyPublicData4 = "I am Public Property 4";
            MyPublicData5 = "I am Public Property 5";
        }
    }



    class ExpThrower
    {
        public void throwExp()
        {
            throw new IOException("Exempted Method Exception");
        }
    }
}
