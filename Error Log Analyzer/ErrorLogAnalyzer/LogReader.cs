using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CacheErrors;
using System;
using System.Windows.Forms;
using DbConnectionsManager;
using ModelObjecting;

namespace ErrorLogAnalyzer
{
    public partial class LogReader : BaseForm
    {
        private List<ProxyError> _errors = new List<ProxyError>();
        private DirectoryInfo _cacheDir;
        private readonly Timer _timer;
        private string _filePath;

        public LogReader()
        {
            InitializeComponent();

            _timer = new Timer { Interval = 1000 };
            _timer.Tick += _timer_Tick;


            cmbServerName.TextChanged += (sender, ev) => CreateConnectionStringByControlsValue();

            cmbDatabaseName.TextChanged += cmbDatabaseName_TextChanged;

            pictureBox_viewer.MouseEnter += (s, ea) => pictureBox_viewer.Focus();

            dgv_ErrorsViewer.SelectionChanged += (sender, args) => JustRunEventByUser(dgvErrorsViewer_SelectionChanged);

            dgv_ErrorsViewer.CreateColumns(typeof(IError));

            _errors = new List<ProxyError>();
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            SdfFileManager.SetConnectionString(_filePath);

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
        }

        void cmbDatabaseName_TextChanged(object sender, EventArgs e)
        {
            ConnectionManager.Add(new Connection(cmbServerName.Text, cmbDatabaseName.Text), "UM");
            ConnectionManager.SetToDefaultConnection("um");
        }

        public override async void OnStartup(object sender, EventArgs e)
        {
            base.OnStartup(sender, e);

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
                Close();
            else
            {
                _filePath = ofd.FileName;

                _cacheDir = new DirectoryInfo(Path.GetDirectoryName(ofd.FileName));

                refreshAlert.SetError(btnRefreshGridView, "Click on Refresh button to show cache data");
            }


            // Exit form this method if reopen the combo box
            if (cmbServerName.Items.Count > 0) return;

            // add local host in first time
            cmbServerName.Items.Add("localhost");

            // Find any servers in network
            var servers = await GetServersAsync();

            // Fill server names combo
            cmbServerName.Items.AddRange(servers);
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
                SetDatabaseConnectionState(null);
            }
            catch (Exception exp)
            {
                lblRecordsNum.Text = "Database Connection Corrupted";
                SetDatabaseConnectionState(exp);
            }
        }

        private void SetCacheSizeViewer()
        {
            var dirSize = _cacheDir.GetDirectorySize();
            var limitSize = DiskHelper.CacheLimitSize;

            var percent = unchecked((int)(dirSize * 100 / limitSize));
            prgCacheSize.Value = percent > 100 ? 100 : percent;
        }

        public void JustRunEventByUser(Action method)
        {
            if (!new StackTrace().GetFrames().Skip(3).Any(x => x.GetMethod().DeclaringType.Name == this.Name))
                method();
        }

        public async Task<string[]> GetServersAsync()
        {
            return await Task.Run(() =>
            {
                var dt = SqlDataSourceEnumerator.Instance.GetDataSources();


                if (dt.Rows.Count == 0)
                {
                    return null;
                }

                var sqlServers = new string[dt.Rows.Count];

                var f = -1;

                foreach (DataRow r in dt.Rows)
                {
                    var sqlServer = r["ServerName"].ToString();
                    var instance = r["InstanceName"].ToString();

                    if (!string.IsNullOrEmpty(instance))
                    {
                        sqlServer += "\\" + instance;
                    }

                    sqlServers[++f] = sqlServer;
                }

                Array.Sort(sqlServers);

                return sqlServers;
            });
        }


        public async Task<string[]> GetSqlDatabasesAsync()
        {
            var databases = new List<String>();

            //create connection
            using (var sqlConn = ConnectionManager.GetDefaultConnection().SqlConn)
            {

                try
                {
                    //open connection
                    await sqlConn.OpenAsync();

                    //get databases
                    var tblDatabases = sqlConn.GetSchema("Databases");

                    //set database state
                    SetDatabaseConnectionState(null);

                    //add to list
                    databases.AddRange(from DataRow row in tblDatabases.Rows select row["database_name"].ToString());
                }
                catch (Exception exp)
                {
                    SetDatabaseConnectionState(exp);
                }
                finally
                {
                    //close connection
                    sqlConn.Close();
                }

                return databases.ToArray();
            }
        }

        private void SetDatabaseConnectionState(Exception exp)
        {
            CheckForIllegalCrossThreadCalls = false;

            picServerState.Invoke(new Action(() =>
            {
                picServerState.Image = (exp == null)
                    ? Properties.Resources.Enable // Connected Successful
                    : Properties.Resources.Disable;

                this.toolTip.SetToolTip(this.picServerState,
                    (exp == null)
                        ? string.Format(
                            "The {0} is available to connect.",
                            string.IsNullOrEmpty(cmbServerName.Text) ? Environment.MachineName : cmbServerName.Text)
                        : string.Format(
                            "The {0} is not available to connect.{1}The user id or password maybe is not correct",
                            string.IsNullOrEmpty(cmbServerName.Text) ? Environment.MachineName : cmbServerName.Text,
                            Environment.NewLine));
            }));


            this.toolTip.ToolTipIcon = (exp == null) ? ToolTipIcon.Info : ToolTipIcon.Error;

            CheckForIllegalCrossThreadCalls = true;
        }


        private async void CreateConnectionStringByControlsValue()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                ConnectionManager.Add(new Connection(cmbServerName.Text, cmbDatabaseName.Text), "UM");
                ConnectionManager.SetToDefaultConnection("um");

                // Clear old server databases
                cmbDatabaseName.Items.Clear();

                // Set Database names of selected server
                var dbs = await GetSqlDatabasesAsync();

                // Add database names to combo box items
                cmbDatabaseName.Items.AddRange(items: dbs);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }
}