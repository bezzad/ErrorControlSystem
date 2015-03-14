
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using ConnectionsManager;
using CacheErrors;
using System;
using System.Windows.Forms;
using ModelObjecting;

namespace ErrorLogAnalyzer
{
    public partial class LogReader : Form
    {
        private List<ProxyError> _errors = new List<ProxyError>();
        private DirectoryInfo _cacheDir;
        private readonly Timer _timer;

        public LogReader()
        {
            InitializeComponent();

            _timer = new Timer { Interval = 1000 };
            _timer.Tick += (s, e) =>
            {
                var newErrors = SdfFileManager.GetErrors().ToList();

                //
                // Update old errors
                foreach (var item in newErrors.Union(_errors))
                {
                    dgv_ErrorsViewer.UpdateRow(item, (row, o) => (int)row.Cells["Id"].Value == ((ProxyError)o).Id);
                }
                //
                // Add new errors
                foreach (var item in newErrors.Except(_errors))
                {
                    _errors.Add(item);

                    dgv_ErrorsViewer.AddRow(item);
                }
                //
                // Remove sent errors in old list and data grid view
                foreach (var item in _errors.Except(newErrors))
                {
                    dgv_ErrorsViewer.RemoveRowByCondition(item, (row, o) => (int)row.Cells["Id"].Value == ((ProxyError)o).Id);
                }

                _errors = newErrors;

                refreshAlert.Clear();

                SetCacheSizeViewer();

                CountDatabaseRecords();
            };

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
            _timer.Start();

            btnRefreshGridView.Enabled = false;
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
