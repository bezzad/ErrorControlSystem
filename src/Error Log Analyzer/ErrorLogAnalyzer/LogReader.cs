using System.Threading.Tasks;

namespace ErrorLogAnalyzer
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System;
    using System.Windows.Forms;

    using ErrorControlSystem;
    using ErrorControlSystem.CacheErrors;
    using ErrorControlSystem.DbConnectionManager;
    using ErrorControlSystem.Shared;


    public partial class LogReader : BaseForm
    {
        private List<ProxyError> _errors = new List<ProxyError>();

        #region Constructors

        public LogReader()
        {
            InitializeComponent();

            LoadConfigFile();

            _errors = new List<ProxyError>();
        }


        #endregion


        #region Methods

        public void LoadConfigFile()
        {
            Task.Run(async () =>
            {
                if (File.Exists(DatabaseConfigurationForm.ConfigPath))
                {
                    var config = await DatabaseConfigurationForm.ReadTextAsync(DatabaseConfigurationForm.ConfigPath);
                    var conn = Connection.Parse(config);
                    txtConnStr.Text = conn.ConnectionString;

                    ConnectionManager.Add(conn, conn.Name);
                    ConnectionManager.SetToDefaultConnection(conn.Name);

                    await ConnectionManager.GetDefaultConnection().CheckDbConnectionAsync();
                }
            });
        }

        public override void OnFormLoad()
        {
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

            bool canNotReadCacheFile;
            do
            {
                canNotReadCacheFile = false;

                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    if (this.IsHandleCreated)
                        this.Invoke(new Action(Close));
                    return;
                }

                var extension = Path.GetExtension(ofd.FileName);
                if (extension != null && !extension.Equals(".log", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Can not read your cache file");
                    canNotReadCacheFile = true;
                }

            } while (canNotReadCacheFile); // till the user path to correct cache file

            txtCacheFilePath.Invoke(new Action(() =>
                txtCacheFilePath.Text = ofd.FileName));

            this.Invoke(new Action(() =>
                refreshAlert.SetError(btnRefreshGridView, "Click on Refresh button to show cache data")));
        }

        private void CountDatabaseRecords()
        {
            try
            {
                var conn = ConnectionManager.GetDefaultConnection();

                if (ConnectionManager.GetDefaultConnection() == null || !conn.IsReady)
                {
                    lblRecordsNum.Text = "Database Connection Corrupted";
                    return;
                }

                var num = conn.ExecuteScalar<int>("SELECT SUM (DuplicateNo + 1)  FROM ErrorLog", CommandType.Text);

                lblRecordsNum.Text = num.ToString(CultureInfo.InvariantCulture);
                SetDatabaseConnectionState(true);
            }
            catch
            {
                lblRecordsNum.Text = "Database Connection Corrupted";
                SetDatabaseConnectionState(false);
            }
        }

        private void CountCacheRecords()
        {
            try
            {
                lblCacheRecords.Text = _errors.Sum(x => x.Duplicate + 1).ToString(CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                lblCacheRecords.Text = "Cache Connection Corrupted";
            }
        }

        private void SetCacheSizeViewer()
        {
            var dirSize = new DirectoryInfo(Path.GetDirectoryName(txtCacheFilePath.Text)).GetDirectorySize();
            var limitSize = ErrorHandlingOption.CacheLimitSize;

            var percent = unchecked((int)(dirSize * 100 / limitSize));
            prgCacheSize.Value = percent > 100 ? 100 : percent;
        }

        public void JustRunEventByUser(Action method)
        {
            if (!new StackTrace().GetFrames().Skip(3).Any(x => x.GetMethod().DeclaringType.Name == this.Name))
                method();
        }

        private void SetDatabaseConnectionState(bool success)
        {
            btnSetConnection.Invoke(new Action(() =>
            {
                btnSetConnection.BackgroundImage = (success)
                    ? Properties.Resources.Enable // Connected Successful
                    : Properties.Resources.Disable;
            }));
        }


        #endregion


        #region Designer Event Listener

        private void btnQuit_Click(object sender, EventArgs e)
        {
            // End the program.
            Close();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            SqlCompactEditionManager.SetConnectionString(txtCacheFilePath.Text);

            var newErrors = SqlCompactEditionManager.GetErrors().ToList();

            //
            // Update old errors
            foreach (var item in newErrors.Union(_errors))
            {
                DynamicDgv.UpdateRow(item);
            }
            //
            // Add new errors
            foreach (var item in newErrors.Except(_errors))
            {
                _errors.Add(item);

                DynamicDgv.AddRow(item);
            }
            //
            // Remove sent errors in old list and data grid view
            foreach (var item in _errors.Except(newErrors))
            {
                DynamicDgv.RemoveRow(item);
            }

            _errors = newErrors;

            refreshAlert.Clear();

            SetCacheSizeViewer();

            CountDatabaseRecords();

            CountCacheRecords();

            if (DynamicDgv.RowCount > 0 && DynamicDgv.SelectedRows[0].Index == 0)
                dgv_ErrorsViewer_SelectionChanged(sender, e);
        }

        private void btnRefreshGridView_Click(object sender, EventArgs e)
        {
            timer.Start();

            btnRefreshGridView.Enabled = false;
        }
        private void LogReader_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Stop();
        }

        private void pictureBox_viewer_MouseEnter(object sender, EventArgs e)
        {
            pictureBox_viewer.Focus();
        }

        private void dgv_ErrorsViewer_SelectionChanged(object sender, EventArgs e)
        {
            JustRunEventByUser(() =>
            {
                var currentRow = DynamicDgv.GetCurrentRow();
                if (currentRow != null)
                    pictureBox_viewer.Image = currentRow.Snapshot.Value ?? Properties.Resources._null;
            });
        }



        private void btnSetConnection_Click(object sender, EventArgs e)
        {
            var dcf = new DatabaseConfigurationForm();
            var result = dcf.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                var cm = ConnectionManager.GetDefaultConnection();

                txtConnStr.Text = cm.ConnectionString;
                SetDatabaseConnectionState(cm.IsReady);
            }
        }

        #endregion

    }
}