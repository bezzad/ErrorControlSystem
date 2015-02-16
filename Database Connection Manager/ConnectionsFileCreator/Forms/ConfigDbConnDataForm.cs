using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConnectionsManager;

namespace ConnectionsFileCreator.Forms
{
    public class ConfigDbConnDataForm : Form
    {
        private Connection _currentConnection;

        #region Constructor

        public ConfigDbConnDataForm()
        {
            InitializeComponent();


            InitializeControlsValue();
        }


        #endregion


        #region Event Handlers

        private async void btnCreate_Click(object sender, EventArgs e)
        {
            await WriteTextToDiskAsync(txtFilePath.Value, _currentConnection.ToString());
        }

        private async void cmbServerName_DropDown(object sender, EventArgs e)
        {
            // Exit form this method if reopen the combo box
            if (cmbServerName.Items.Count > 0) return;

            // add local host in first time
            cmbServerName.Items.Add("localhost");

            // Find any servers in network
            var servers = await GetServersAsync();

            // Fill server names combo
            cmbServerName.Items.AddRange(servers);
        }

        private async void cmbDbName_DropDown(object sender, EventArgs e)
        {
            // Clear old server databases
            cmbDbName.Items.Clear();

            // Set Database names of selected server
            var dbs = await GetSqlDatabasesAsync(CreateConnectionStringByControlsValue());

            // Add database names to combo box items
            cmbDbName.Items.AddRange(items: dbs);
        }



        #endregion


        #region Methods

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


        public async Task<string[]> GetSqlDatabasesAsync(string connString)
        {
            return await Task.Run(() =>
            {
                var databases = new List<String>();

                //create connection
                var sqlConn = new SqlConnection(connString);

                try
                {
                    //open connection
                    sqlConn.Open();

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
            });
        }


        private void InitializeControlsValue()
        {
            this.Icon = global::ConnectionsFileCreator.Properties.Resources.DatabaseConnection;

            cmbServerName.TextChanged += (s, e) => txtConnectionString.Value = CreateConnectionStringByControlsValue();
            cmbDbName.TextChanged += (s, e) => txtConnectionString.Value = CreateConnectionStringByControlsValue();
            txtUsername.TextChanged += (s, e) => txtConnectionString.Value = CreateConnectionStringByControlsValue();
            txtPassword.TextChanged += (s, e) => txtConnectionString.Value = CreateConnectionStringByControlsValue();
            txtPort.TextChanged += (s, e) => txtConnectionString.Value = CreateConnectionStringByControlsValue();
            txtTimeout.TextChanged += (s, e) => txtConnectionString.Value = CreateConnectionStringByControlsValue();
            txtConnName.TextChanged += (s, e) => txtConnectionString.Value = CreateConnectionStringByControlsValue();

            btnCreate.Click += btnCreate_Click;

            txtFilePath.Value = Path.Combine(Environment.CurrentDirectory, "Connections.config");
        }




        private string CreateConnectionStringByControlsValue()
        {
            var port = string.IsNullOrEmpty(txtPort.Value) ? 0 : uint.Parse(txtPort.Value);

            var timeOut = string.IsNullOrEmpty(txtTimeout.Value) ? 0 : int.Parse(txtTimeout.Value);

            _currentConnection = new Connection(txtConnName.Value)
            {
                Server = cmbServerName.Text,
                DatabaseName = cmbDbName.Text,
                UserId = txtUsername.Value,
                Password = txtPassword.Value,
                PortNumber = port,
                TimeOut = timeOut
            };

            return _currentConnection.ConnectionString;
        }



        private void SetDatabaseConnectionState(Exception exp)
        {
            CheckForIllegalCrossThreadCalls = false;

            picServerState.Invoke(new Action(() =>
                picServerState.Image = (exp == null)
                ? Properties.Resources.Enable // Connected Successful
                : Properties.Resources.Disable));


            this.toolTip.SetToolTip(this.picServerState,
                (exp == null)
                    ? string.Format(
                        "The {0} is available to connect.",
                        string.IsNullOrEmpty(cmbServerName.Text) ? Environment.MachineName : cmbServerName.Text)
                    : string.Format(
                        "The {0} is not available to connect.{1}The user id or password maybe is not correct",
                        string.IsNullOrEmpty(cmbServerName.Text) ? Environment.MachineName : cmbServerName.Text,
                        Environment.NewLine));


            this.toolTip.ToolTipIcon = (exp == null) ? ToolTipIcon.Info : ToolTipIcon.Error;

            CheckForIllegalCrossThreadCalls = true;
        }

        public async Task WriteTextToDiskAsync(string filePath, string text)
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
        #endregion






        #region Designer

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.grbInputConnData = new System.Windows.Forms.GroupBox();
            this.picServerState = new System.Windows.Forms.PictureBox();
            this.txtConnName = new DVTextBox.DVTextBox(this.components);
            this.txtPassword = new DVTextBox.DVTextBox(this.components);
            this.txtUsername = new DVTextBox.DVTextBox(this.components);
            this.txtTimeout = new DVTextBox.DVTextBox(this.components);
            this.txtPort = new DVTextBox.DVTextBox(this.components);
            this.lblDbName = new System.Windows.Forms.Label();
            this.cmbDbName = new System.Windows.Forms.ComboBox();
            this.lblServerName = new System.Windows.Forms.Label();
            this.cmbServerName = new System.Windows.Forms.ComboBox();
            this.picSqlLogo = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtConnectionString = new DVTextBox.DVTextBox(this.components);
            this.btnAddConnStr = new System.Windows.Forms.Button();
            this.dgvConnections = new System.Windows.Forms.DataGridView();
            this.grbConnList = new System.Windows.Forms.GroupBox();
            this.txtFilePath = new DVTextBox.DVTextBox(this.components);
            this.btnCreate = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.grbInputConnData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picServerState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSqlLogo)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConnections)).BeginInit();
            this.grbConnList.SuspendLayout();
            this.SuspendLayout();
            // 
            // grbInputConnData
            // 
            this.grbInputConnData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbInputConnData.Controls.Add(this.picServerState);
            this.grbInputConnData.Controls.Add(this.txtConnName);
            this.grbInputConnData.Controls.Add(this.txtPassword);
            this.grbInputConnData.Controls.Add(this.txtUsername);
            this.grbInputConnData.Controls.Add(this.txtTimeout);
            this.grbInputConnData.Controls.Add(this.txtPort);
            this.grbInputConnData.Controls.Add(this.lblDbName);
            this.grbInputConnData.Controls.Add(this.cmbDbName);
            this.grbInputConnData.Controls.Add(this.lblServerName);
            this.grbInputConnData.Controls.Add(this.cmbServerName);
            this.grbInputConnData.Controls.Add(this.picSqlLogo);
            this.grbInputConnData.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.grbInputConnData.Location = new System.Drawing.Point(12, 12);
            this.grbInputConnData.Name = "grbInputConnData";
            this.grbInputConnData.Size = new System.Drawing.Size(585, 189);
            this.grbInputConnData.TabIndex = 0;
            this.grbInputConnData.TabStop = false;
            this.grbInputConnData.Text = "Connect to Server";
            // 
            // picServerState
            // 
            this.picServerState.Image = global::ConnectionsFileCreator.Properties.Resources.Enable;
            this.picServerState.Location = new System.Drawing.Point(530, 117);
            this.picServerState.Name = "picServerState";
            this.picServerState.Size = new System.Drawing.Size(40, 37);
            this.picServerState.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picServerState.TabIndex = 7;
            this.picServerState.TabStop = false;
            this.toolTip.SetToolTip(this.picServerState, "The localhost is available to connect.");
            // 
            // txtConnName
            // 
            this.txtConnName.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtConnName.DefaultValue = "Connection Name";
            this.txtConnName.DefaultValueColor = System.Drawing.Color.Gray;
            this.txtConnName.EnterToTab = false;
            this.txtConnName.Font = new System.Drawing.Font("Lucida Fax", 10F);
            this.txtConnName.ForeColor = System.Drawing.Color.Gray;
            this.txtConnName.Location = new System.Drawing.Point(188, 160);
            this.txtConnName.Name = "txtConnName";
            this.txtConnName.Size = new System.Drawing.Size(382, 23);
            this.txtConnName.TabIndex = 6;
            this.txtConnName.Text = "Connection Name";
            this.txtConnName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtConnName.TextForeColor = System.Drawing.Color.Black;
            this.txtConnName.Value = "";
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtPassword.DefaultValue = "Password";
            this.txtPassword.DefaultValueColor = System.Drawing.Color.Gray;
            this.txtPassword.EnterToTab = false;
            this.txtPassword.Font = new System.Drawing.Font("Lucida Fax", 10F);
            this.txtPassword.ForeColor = System.Drawing.Color.Gray;
            this.txtPassword.Location = new System.Drawing.Point(188, 88);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(247, 23);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.Text = "Password";
            this.txtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPassword.TextForeColor = System.Drawing.Color.Black;
            this.txtPassword.Value = "";
            // 
            // txtUsername
            // 
            this.txtUsername.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtUsername.DefaultValue = "Login";
            this.txtUsername.DefaultValueColor = System.Drawing.Color.Gray;
            this.txtUsername.EnterToTab = false;
            this.txtUsername.Font = new System.Drawing.Font("Lucida Fax", 10F);
            this.txtUsername.ForeColor = System.Drawing.Color.Gray;
            this.txtUsername.Location = new System.Drawing.Point(188, 59);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(247, 23);
            this.txtUsername.TabIndex = 1;
            this.txtUsername.Text = "Login";
            this.txtUsername.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtUsername.TextForeColor = System.Drawing.Color.Black;
            this.txtUsername.Value = "";
            // 
            // txtTimeout
            // 
            this.txtTimeout.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtTimeout.DefaultValue = "Timeout = 30";
            this.txtTimeout.DefaultValueColor = System.Drawing.Color.Gray;
            this.txtTimeout.EnterToTab = false;
            this.txtTimeout.Font = new System.Drawing.Font("Lucida Fax", 10F);
            this.txtTimeout.ForeColor = System.Drawing.Color.Gray;
            this.txtTimeout.IsNumerical = true;
            this.txtTimeout.Location = new System.Drawing.Point(441, 88);
            this.txtTimeout.Name = "txtTimeout";
            this.txtTimeout.Size = new System.Drawing.Size(129, 23);
            this.txtTimeout.TabIndex = 4;
            this.txtTimeout.Text = "Timeout = 30";
            this.txtTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTimeout.TextForeColor = System.Drawing.Color.Black;
            this.txtTimeout.Value = "";
            // 
            // txtPort
            // 
            this.txtPort.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtPort.DefaultValue = "Port = 1433";
            this.txtPort.DefaultValueColor = System.Drawing.Color.Gray;
            this.txtPort.EnterToTab = false;
            this.txtPort.Font = new System.Drawing.Font("Lucida Fax", 10F);
            this.txtPort.ForeColor = System.Drawing.Color.Gray;
            this.txtPort.IsNumerical = true;
            this.txtPort.Location = new System.Drawing.Point(441, 59);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(129, 23);
            this.txtPort.TabIndex = 2;
            this.txtPort.Text = "Port = 1433";
            this.txtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPort.TextForeColor = System.Drawing.Color.Black;
            this.txtPort.Value = "";
            // 
            // lblDbName
            // 
            this.lblDbName.AutoSize = true;
            this.lblDbName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDbName.Location = new System.Drawing.Point(185, 132);
            this.lblDbName.Name = "lblDbName";
            this.lblDbName.Size = new System.Drawing.Size(93, 15);
            this.lblDbName.TabIndex = 4;
            this.lblDbName.Text = "Database Name:";
            // 
            // cmbDbName
            // 
            this.cmbDbName.BackColor = System.Drawing.Color.LemonChiffon;
            this.cmbDbName.FormattingEnabled = true;
            this.cmbDbName.Location = new System.Drawing.Point(284, 125);
            this.cmbDbName.Name = "cmbDbName";
            this.cmbDbName.Size = new System.Drawing.Size(242, 28);
            this.cmbDbName.TabIndex = 5;
            this.cmbDbName.DropDown += new System.EventHandler(this.cmbDbName_DropDown);
            // 
            // lblServerName
            // 
            this.lblServerName.AutoSize = true;
            this.lblServerName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblServerName.Location = new System.Drawing.Point(185, 32);
            this.lblServerName.Name = "lblServerName";
            this.lblServerName.Size = new System.Drawing.Size(77, 15);
            this.lblServerName.TabIndex = 3;
            this.lblServerName.Text = "Server Name:";
            // 
            // cmbServerName
            // 
            this.cmbServerName.BackColor = System.Drawing.Color.LemonChiffon;
            this.cmbServerName.FormattingEnabled = true;
            this.cmbServerName.Location = new System.Drawing.Point(268, 25);
            this.cmbServerName.Name = "cmbServerName";
            this.cmbServerName.Size = new System.Drawing.Size(302, 28);
            this.cmbServerName.TabIndex = 0;
            this.cmbServerName.DropDown += new System.EventHandler(this.cmbServerName_DropDown);
            // 
            // picSqlLogo
            // 
            this.picSqlLogo.Image = global::ConnectionsFileCreator.Properties.Resources.MicrosoftSQLServerLogo;
            this.picSqlLogo.Location = new System.Drawing.Point(8, 22);
            this.picSqlLogo.Name = "picSqlLogo";
            this.picSqlLogo.Size = new System.Drawing.Size(153, 161);
            this.picSqlLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picSqlLogo.TabIndex = 1;
            this.picSqlLogo.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtConnectionString);
            this.groupBox1.Controls.Add(this.btnAddConnStr);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.groupBox1.Location = new System.Drawing.Point(12, 207);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(585, 92);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection String";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConnectionString.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtConnectionString.DefaultValue = "\r\nConnection String";
            this.txtConnectionString.DefaultValueColor = System.Drawing.Color.Gray;
            this.txtConnectionString.EnterToTab = false;
            this.txtConnectionString.Font = new System.Drawing.Font("Lucida Fax", 10F);
            this.txtConnectionString.ForeColor = System.Drawing.Color.Gray;
            this.txtConnectionString.Location = new System.Drawing.Point(8, 24);
            this.txtConnectionString.Multiline = true;
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(474, 61);
            this.txtConnectionString.TabIndex = 14;
            this.txtConnectionString.Text = "\r\nConnection String";
            this.txtConnectionString.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtConnectionString.TextForeColor = System.Drawing.Color.Black;
            this.txtConnectionString.Value = "";
            // 
            // btnAddConnStr
            // 
            this.btnAddConnStr.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAddConnStr.Location = new System.Drawing.Point(488, 24);
            this.btnAddConnStr.Name = "btnAddConnStr";
            this.btnAddConnStr.Size = new System.Drawing.Size(89, 61);
            this.btnAddConnStr.TabIndex = 0;
            this.btnAddConnStr.Text = "Add Connection String";
            this.btnAddConnStr.UseVisualStyleBackColor = true;
            // 
            // dgvConnections
            // 
            this.dgvConnections.AllowUserToAddRows = false;
            this.dgvConnections.AllowUserToOrderColumns = true;
            this.dgvConnections.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvConnections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvConnections.Location = new System.Drawing.Point(3, 23);
            this.dgvConnections.Name = "dgvConnections";
            this.dgvConnections.Size = new System.Drawing.Size(579, 173);
            this.dgvConnections.TabIndex = 2;
            this.dgvConnections.TabStop = false;
            // 
            // grbConnList
            // 
            this.grbConnList.Controls.Add(this.dgvConnections);
            this.grbConnList.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbConnList.Location = new System.Drawing.Point(12, 305);
            this.grbConnList.Name = "grbConnList";
            this.grbConnList.Size = new System.Drawing.Size(585, 199);
            this.grbConnList.TabIndex = 4;
            this.grbConnList.TabStop = false;
            this.grbConnList.Text = "Connections";
            // 
            // txtFilePath
            // 
            this.txtFilePath.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtFilePath.DefaultValue = "Save Path";
            this.txtFilePath.DefaultValueColor = System.Drawing.Color.Gray;
            this.txtFilePath.EnterToTab = false;
            this.txtFilePath.Font = new System.Drawing.Font("Lucida Fax", 10F);
            this.txtFilePath.ForeColor = System.Drawing.Color.Gray;
            this.txtFilePath.Location = new System.Drawing.Point(12, 510);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(482, 23);
            this.txtFilePath.TabIndex = 14;
            this.txtFilePath.Text = "Save Path";
            this.txtFilePath.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtFilePath.TextForeColor = System.Drawing.Color.Black;
            this.txtFilePath.Value = "";
            // 
            // btnCreate
            // 
            this.btnCreate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnCreate.Location = new System.Drawing.Point(508, 510);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(89, 23);
            this.btnCreate.TabIndex = 2;
            this.btnCreate.Text = "Create File";
            this.btnCreate.UseVisualStyleBackColor = true;
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 10;
            this.toolTip.IsBalloon = true;
            this.toolTip.ReshowDelay = 10;
            this.toolTip.ShowAlways = true;
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip.ToolTipTitle = "Server Connection State";
            // 
            // ConfigDbConnDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Azure;
            this.ClientSize = new System.Drawing.Size(609, 545);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.grbConnList);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grbInputConnData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ConfigDbConnDataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Config Database Connection Data";
            this.grbInputConnData.ResumeLayout(false);
            this.grbInputConnData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picServerState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSqlLogo)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConnections)).EndInit();
            this.grbConnList.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ToolTip toolTip;
        private PictureBox picServerState;
        private DVTextBox.DVTextBox txtFilePath;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.GroupBox grbInputConnData;
        private System.Windows.Forms.PictureBox picSqlLogo;
        private System.Windows.Forms.Label lblServerName;
        private System.Windows.Forms.ComboBox cmbServerName;
        private System.Windows.Forms.Label lblDbName;
        private System.Windows.Forms.ComboBox cmbDbName;
        private DVTextBox.DVTextBox txtPort;
        private DVTextBox.DVTextBox txtTimeout;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnAddConnStr;
        private System.Windows.Forms.DataGridView dgvConnections;
        private System.Windows.Forms.GroupBox grbConnList;
        private DVTextBox.DVTextBox txtPassword;
        private DVTextBox.DVTextBox txtUsername;
        private DVTextBox.DVTextBox txtConnName;
        private DVTextBox.DVTextBox txtConnectionString;

        #endregion

    }
}
