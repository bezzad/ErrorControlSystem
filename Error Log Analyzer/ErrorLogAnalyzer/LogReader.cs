
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ErrorHandlerEngine.ServerUploader;
using System;
using System.Windows.Forms;
using ErrorHandlerEngine.ModelObjecting;

namespace ErrorLogAnalyzer
{
    public partial class LogReader : Form
    {
        private List<ProxyError> errors;

        public LogReader()
        {
            InitializeComponent();

            dataGridView1.CreateColumns(typeof(ProxyError));

            errors = new List<ProxyError>();

            // LocalApplicationData: "C:\Users\[UserName]\AppData\Local"
            var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            var ofd = new OpenFileDialog
            {
                AutoUpgradeEnabled = true,
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "Log Files *.Log",
                FileName = "Error.Log",
                Multiselect = false
            };
            ofd.CustomPlaces.Add(appDataDir);

            if (ofd.ShowDialog() == DialogResult.OK)
                SdfFileManager.SetConnectionString(ofd.FileName);

            dataGridView1.SelectionChanged += (sender, args) => JustRunEventByUser(dataGridView1_SelectionChanged);
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            // End the program.
            Close();
        }

        private void dataGridView1_SelectionChanged()
        {
            int index = dataGridView1.CurrentRow != null ? dataGridView1.CurrentRow.Index : 0;
            if (index < errors.Count && index >= 0)
                pictureBox1.Image = errors[index].Snapshot.Value;
        }



        private void btnRefreshGridView_Click(object sender, EventArgs e)
        {
            errors = SdfFileManager.GetErrors().ToList();

            dataGridView1.Rows.Clear();
            foreach (var item in errors)
            {
                dataGridView1.AddRow(item);
            }
        }

        public void JustRunEventByUser(Action method)
        {
            if (!new StackTrace().GetFrames().Skip(3).Any(x => x.GetMethod().DeclaringType.Name == this.Name))
                method();
        }
    }
}
