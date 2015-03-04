
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ErrorHandlerEngine.CacheHandledErrors;
using ErrorHandlerEngine.ServerUploader;
using System;
using System.Windows.Forms;
using ErrorHandlerEngine.ModelObjecting;

namespace ErrorLogAnalyzer
{
    public partial class LogReader : Form
    {
        private List<ProxyError> _errors;
        private DirectoryInfo cacheDir;

        public LogReader()
        {
            InitializeComponent();

            pictureBox_viewer.MouseEnter += (s, ea) => pictureBox_viewer.Focus();

            dgv_ErrorsViewer.CreateColumns(typeof(IError));

            _errors = new List<ProxyError>();

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

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                SdfFileManager.SetConnectionString(ofd.FileName);

                cacheDir = new DirectoryInfo(Path.GetDirectoryName(ofd.FileName));
            }
            dgv_ErrorsViewer.SelectionChanged += (sender, args) => JustRunEventByUser(dgvErrorsViewer_SelectionChanged);
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
        }

        private void SetCacheSizeViewer()
        {
            var dirSize = cacheDir.GetDirectorySize();
            var limitSize = DiskHelper.CacheLimitSize;

            var percent = checked((int)(dirSize * 100 / limitSize));
            prgCacheSize.Value = percent > 100 ? 100 : percent;
        }

        public void JustRunEventByUser(Action method)
        {
            if (!new StackTrace().GetFrames().Skip(3).Any(x => x.GetMethod().DeclaringType.Name == this.Name))
                method();
        }

        private void LogReader_Load(object sender, EventArgs e)
        {
            refreshAlert.SetError(btnRefreshGridView, "Click on Refresh button to show cache data");
        }



    }
}
