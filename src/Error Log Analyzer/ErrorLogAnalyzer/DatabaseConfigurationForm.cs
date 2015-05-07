using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ErrorControlSystem.DbConnectionManager;
using ErrorControlSystem.ServerController;
using ErrorControlSystem.Shared;

namespace ErrorLogAnalyzer
{
    public partial class DatabaseConfigurationForm : Form
    {
        public static string ConfigPath = Path.Combine(Environment.CurrentDirectory, "Connections.config");
        private Connection currentConnection;

        public DatabaseConfigurationForm()
        {
            InitializeComponent();

            currentConnection = new Connection
            {
                AppName = Connection.GetRunningAppNameVersion()
            };

            InitializeControlsValue();
        }

        private void InitializeControlsValue()
        {
            cmbDatabaseName.TextChanged += (s, e) => txtConnectionString.Value = CreateConnectionStringByControlsValue();
            txtUsername.TextChanged += (s, e) => txtConnectionString.Value = CreateConnectionStringByControlsValue();
            txtPassword.TextChanged += (s, e) => txtConnectionString.Value = CreateConnectionStringByControlsValue();
            txtPort.TextChanged += (s, e) => txtConnectionString.Value = CreateConnectionStringByControlsValue();
            txtTimeout.TextChanged += (s, e) => txtConnectionString.Value = CreateConnectionStringByControlsValue();
            txtConnName.TextChanged += (s, e) => txtConnectionString.Value = CreateConnectionStringByControlsValue();
        }

        private string CreateConnectionStringByControlsValue()
        {
            var port = string.IsNullOrEmpty(txtPort.Value) ? 0 : int.Parse(txtPort.Value);

            var timeOut = string.IsNullOrEmpty(txtTimeout.Value) ? 0 : int.Parse(txtTimeout.Value);

            currentConnection.Name = txtConnName.Value;
            currentConnection.Server = cmbServerName.Text;
            currentConnection.DatabaseName = cmbDatabaseName.Text;
            currentConnection.UserId = txtUsername.Value;
            currentConnection.Password = txtPassword.Value;
            currentConnection.PortNumber = port;
            currentConnection.TimeOut = timeOut;

            return currentConnection.ConnectionString;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ConnectionManager.Add(currentConnection, currentConnection.Name);
            ConnectionManager.SetToDefaultConnection(currentConnection.Name);

            Task.Run(async () =>
            {
                await WriteTextToDiskAsync(ConfigPath, currentConnection.ToString(true));

                await ConnectionManager.GetDefaultConnection().CheckDbConnectionAsync();
            });

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private async void cmbServerName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                txtConnectionString.Value = CreateConnectionStringByControlsValue();

                // Clear old server databases
                cmbDatabaseName.Items.Clear();

                // Set Database names of selected server
                var dbs = await ServerTransmitter.SqlServerManager.GetSqlDatabasesAsync(currentConnection);

                SetDatabaseConnectionState(dbs != null);

                if (dbs == null)
                {
                    MessageBox.Show("Not found any database for this server!");
                    return;
                }

                // Add database names to combo box items
                cmbDatabaseName.Items.AddRange(items: dbs);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private async void cmbServerName_DropDown(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Exit form this method if reopen the combo box
                if (cmbServerName.Items.Count > 0) return;

                // add local host in first time
                cmbServerName.Items.Add("localhost");

                // Find any servers in network
                string[] servers = await ServerTransmitter.SqlServerManager.GetSqlServersInstanceAsync();

                // Fill server names combo
                cmbServerName.Items.AddRange(servers);

                cmbServerName.DroppedDown = true;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }

        private void SetDatabaseConnectionState(bool success)
        {
            picDbState.Image = (success)
                ? Properties.Resources.Enable // Connected Successful
                : Properties.Resources.Disable;
        }



        public static async Task WriteTextToDiskAsync(string filePath, string text)
        {
            var encodedText = Encoding.Unicode.GetBytes(text);

            using (var sourceStream = new FileStream(filePath,
                FileMode.Create, FileAccess.Write, FileShare.ReadWrite,
                bufferSize: 4096, useAsync: true))
            {

                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);

                sourceStream.Close();
            }

        }

        public static async Task<string> ReadTextAsync(string filePath)
        {
            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Open, FileAccess.Read, FileShare.Read,
                bufferSize: 4096, useAsync: true))
            {
                StringBuilder sb = new StringBuilder();

                byte[] buffer = new byte[0x1000];
                int numRead;
                while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    string text = Encoding.Unicode.GetString(buffer, 0, numRead);
                    sb.Append(text);
                }

                return sb.ToString();
            }
        }
    }
}
