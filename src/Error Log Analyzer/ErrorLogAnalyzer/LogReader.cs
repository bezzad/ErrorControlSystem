using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Windows.Forms;
using ErrorControlSystem;
using ErrorControlSystem.CachErrors;
using ErrorControlSystem.DbConnectionManager;
using ErrorControlSystem.Shared;

namespace ErrorLogAnalyzer
{
    public partial class LogReader : BaseForm
    {
        private List<ProxyError> _errors = new List<ProxyError>();

        public LogReader()
        {
            InitializeComponent();

            _errors = new List<ProxyError>();
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

        void cmbDatabaseName_TextChanged(object sender, EventArgs e)
        {
            ConnectionManager.Add(new Connection(cmbServerName.Text, cmbDatabaseName.Text), "UM");
            ConnectionManager.SetToDefaultConnection("um");
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

        private void btnQuit_Click(object sender, EventArgs e)
        {
            // End the program.
            Close();
        }


        private void btnRefreshGridView_Click(object sender, EventArgs e)
        {
            timer.Start();

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
        }

        private async void cmbServerName_DropDown(object sender, EventArgs e)
        {
            if (cmbServerName.Items.Count == 0)
            {
                await InvokeAsync(() =>
                {
                    // Exit form this method if reopen the combo box
                    if (cmbServerName.Items.Count > 0) return;

                    cmbServerName.Invoke(new Action(() =>
                    {
                        // add local host in first time
                        cmbServerName.Items.Add("localhost");
                    }));

                    // Find any servers in network
                    var servers = GetServersAsync().GetAwaiter().GetResult();

                    cmbServerName.Invoke(new Action(() =>
                    {
                        // Fill server names combo
                        cmbServerName.Items.AddRange(servers);
                    }));
                });

                cmbServerName.DroppedDown = true;
            }
        }

        private void LogReader_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Stop();
        }

        private async void cmbServerName_TextChanged(object sender, EventArgs e)
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



    }
}