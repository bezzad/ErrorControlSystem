
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using ConnectionsManager;
using ErrorHandlerEngine.ServerUploader;
using System;
using System.Windows.Forms;
using ErrorHandlerEngine.ModelObjecting;

namespace ErrorLogAnalyzer
{
    public partial class LogReader : Form
    {
        private List<ProxyError> _errors;
        private DirectoryInfo _cacheDir;

        public LogReader()
        {
            InitializeComponent();

            pictureBox_viewer.MouseEnter += (s, ea) => pictureBox_viewer.Focus();

            dgv_ErrorsViewer.SelectionChanged += (sender, args) => JustRunEventByUser(dgvErrorsViewer_SelectionChanged);

            dgv_ErrorsViewer.CreateColumns(typeof(IError));

            _errors = new List<ProxyError>();

            ConnectionManager.Add(new Connection("localhost", "UsersManagements"), "UM");
            ConnectionManager.SetToDefaultConnection("um");

            Application.Idle += Application_Idle;
        }

        void Application_Idle(object sender, EventArgs e)
        {
            Application.Idle -= Application_Idle;

            // LocalApplicationData: "C:\Users\[UserName]\AppData\Local"
            var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            var ofd = new OpenFileDialog
            {
                AutoUpgradeEnabled = true,
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "Log Files  |*.Log",
                FileName = "Error.Log",
                Title = "Path of local cache file",
                InitialDirectory = appDataDir,
                Multiselect = false
            };
            ofd.CustomPlaces.Add(appDataDir);

            if (ofd.ShowDialog() != DialogResult.OK)
                this.Close();
            else
            {
                SdfFileManager.SetConnectionString(ofd.FileName);

                _cacheDir = new DirectoryInfo(Path.GetDirectoryName(ofd.FileName));

                refreshAlert.SetError(btnRefreshGridView, "Click on Refresh button to show cache data");
            }
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            // End the program.
            Close();
        }

        private void dgvErrorsViewer_SelectionChanged()
        {
            var index = dgv_ErrorsViewer.CurrentRow != null ? dgv_ErrorsViewer.CurrentRow.Index : 0;

            if (index < _errors.Count && index >= 0)
                pictureBox_viewer.Image = _errors[index].Snapshot.Value ?? Properties.Resources._null;
        }



        private void btnRefreshGridView_Click(object sender, EventArgs e)
        {
            _errors = SdfFileManager.GetErrors().ToList();

            dgv_ErrorsViewer.Rows.Clear();
            foreach (var item in _errors)
            {
                dgv_ErrorsViewer.AddRow(item);
            }

            refreshAlert.Clear();

            SetCacheSizeViewer();

            CountDatabaseRecords();
        }

        private void CountDatabaseRecords()
        {
            try
            {
            var num = ConnectionManager.GetDefaultConnection()
                .ExecuteScalar<int>("SELECT SUM (DuplicateNo + 1)  FROM ErrorLog", CommandType.Text);

            lblRecordsNum.Text = num.ToString(CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                lblRecordsNum.Text = "Database Connection Corrupted";
            }
        }

        private void SetCacheSizeViewer()
        {
            var dirSize = _cacheDir.GetDirectorySize();
            var limitSize = DiskHelper.CacheLimitSize;

            var percent = checked((int)(dirSize * 100 / limitSize));
            prgCacheSize.Value = percent > 100 ? 100 : percent;
        }

        public void JustRunEventByUser(Action method)
        {
            if (!new StackTrace().GetFrames().Skip(3).Any(x => x.GetMethod().DeclaringType.Name == this.Name))
                method();
        }
    }
}
