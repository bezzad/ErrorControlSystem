//using System.Data;
//using ErrorLogAnalyzer;
using System;
using System.Windows.Forms;
using ErrorHandlerEngine.ModelObjecting;

namespace ErrorLogAnalyzer
{
    public partial class LogReader : Form
    {

        public LogReader()
        {
            InitializeComponent();

            dataGridView1.CreateColumns(typeof(ProxyError));
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            // End the program.
            Close();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            //int index = dataGridView1.CurrentRow != null ? dataGridView1.CurrentRow.Index : 0;
            //if (index < errors.Count && index >= 0)
            //    pictureBox1.Image = errors[index].Snapshot.Value;
        }



        private void btnRefreshGridView_Click(object sender, EventArgs e)
        {
            //ErrorLogReader.ReadAsync();
        }
    }
}
