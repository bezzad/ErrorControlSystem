namespace ErrorLogAnalyzer
{
    partial class DatabaseConfigurationForm
    {
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
            this.grbConnectToServer = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.picDbState = new System.Windows.Forms.PictureBox();
            this.txtConnName = new Windows.Forms.HintTextBox(this.components);
            this.txtPassword = new Windows.Forms.HintTextBox(this.components);
            this.txtPort = new Windows.Forms.HintTextBox(this.components);
            this.txtTimeout = new Windows.Forms.HintTextBox(this.components);
            this.txtUsername = new Windows.Forms.HintTextBox(this.components);
            this.cmbDatabaseName = new System.Windows.Forms.ComboBox();
            this.cmbServerName = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grbConnStr = new System.Windows.Forms.GroupBox();
            this.txtConnectionString = new Windows.Forms.HintTextBox(this.components);
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grbConnectToServer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDbState)).BeginInit();
            this.grbConnStr.SuspendLayout();
            this.SuspendLayout();
            // 
            // grbConnectToServer
            // 
            this.grbConnectToServer.Controls.Add(this.splitContainer1);
            this.grbConnectToServer.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbConnectToServer.ForeColor = System.Drawing.Color.Gray;
            this.grbConnectToServer.Location = new System.Drawing.Point(15, 10);
            this.grbConnectToServer.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.grbConnectToServer.Name = "grbConnectToServer";
            this.grbConnectToServer.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.grbConnectToServer.Size = new System.Drawing.Size(501, 222);
            this.grbConnectToServer.TabIndex = 0;
            this.grbConnectToServer.TabStop = false;
            this.grbConnectToServer.Text = "Connect to Server";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(2, 24);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pictureBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.picDbState);
            this.splitContainer1.Panel2.Controls.Add(this.txtConnName);
            this.splitContainer1.Panel2.Controls.Add(this.txtPassword);
            this.splitContainer1.Panel2.Controls.Add(this.txtPort);
            this.splitContainer1.Panel2.Controls.Add(this.txtTimeout);
            this.splitContainer1.Panel2.Controls.Add(this.txtUsername);
            this.splitContainer1.Panel2.Controls.Add(this.cmbDatabaseName);
            this.splitContainer1.Panel2.Controls.Add(this.cmbServerName);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(498, 195);
            this.splitContainer1.SplitterDistance = 135;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::ErrorLogAnalyzer.Properties.Resources.MicrosoftSQLServerLogo;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(135, 195);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // picDbState
            // 
            this.picDbState.Image = global::ErrorLogAnalyzer.Properties.Resources.Disable;
            this.picDbState.Location = new System.Drawing.Point(309, 115);
            this.picDbState.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.picDbState.Name = "picDbState";
            this.picDbState.Size = new System.Drawing.Size(38, 41);
            this.picDbState.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picDbState.TabIndex = 5;
            this.picDbState.TabStop = false;
            // 
            // txtConnName
            // 
            this.txtConnName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtConnName.DefaultValue = "Connection Name";
            this.txtConnName.DefaultValueColor = System.Drawing.Color.Gray;
            this.txtConnName.EnterToTab = false;
            this.txtConnName.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.txtConnName.ForeColor = System.Drawing.Color.Gray;
            this.txtConnName.Location = new System.Drawing.Point(15, 161);
            this.txtConnName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtConnName.Name = "txtConnName";
            this.txtConnName.Size = new System.Drawing.Size(333, 24);
            this.txtConnName.TabIndex = 4;
            this.txtConnName.Text = "Connection Name";
            this.txtConnName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtConnName.TextForeColor = System.Drawing.Color.Black;
            this.txtConnName.Value = "";
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtPassword.DefaultValue = "Password";
            this.txtPassword.DefaultValueColor = System.Drawing.Color.Gray;
            this.txtPassword.EnterToTab = false;
            this.txtPassword.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.txtPassword.ForeColor = System.Drawing.Color.Gray;
            this.txtPassword.Location = new System.Drawing.Point(15, 80);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(211, 24);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.Text = "Password";
            this.txtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPassword.TextForeColor = System.Drawing.Color.Black;
            this.txtPassword.Value = "";
            // 
            // txtPort
            // 
            this.txtPort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtPort.DefaultValue = "Port = 1433";
            this.txtPort.DefaultValueColor = System.Drawing.Color.Gray;
            this.txtPort.EnterToTab = false;
            this.txtPort.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.txtPort.ForeColor = System.Drawing.Color.Gray;
            this.txtPort.IsNumerical = true;
            this.txtPort.Location = new System.Drawing.Point(229, 51);
            this.txtPort.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(120, 24);
            this.txtPort.TabIndex = 4;
            this.txtPort.Text = "Port = 1433";
            this.txtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPort.TextForeColor = System.Drawing.Color.Black;
            this.txtPort.Value = "";
            // 
            // txtTimeout
            // 
            this.txtTimeout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtTimeout.DefaultValue = "Timeout = 30";
            this.txtTimeout.DefaultValueColor = System.Drawing.Color.Gray;
            this.txtTimeout.EnterToTab = false;
            this.txtTimeout.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.txtTimeout.ForeColor = System.Drawing.Color.Gray;
            this.txtTimeout.IsNumerical = true;
            this.txtTimeout.Location = new System.Drawing.Point(229, 80);
            this.txtTimeout.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtTimeout.Name = "txtTimeout";
            this.txtTimeout.Size = new System.Drawing.Size(120, 24);
            this.txtTimeout.TabIndex = 4;
            this.txtTimeout.Text = "Timeout = 30";
            this.txtTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTimeout.TextForeColor = System.Drawing.Color.Black;
            this.txtTimeout.Value = "";
            // 
            // txtUsername
            // 
            this.txtUsername.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtUsername.DefaultValue = "Username";
            this.txtUsername.DefaultValueColor = System.Drawing.Color.Gray;
            this.txtUsername.EnterToTab = false;
            this.txtUsername.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.txtUsername.ForeColor = System.Drawing.Color.Gray;
            this.txtUsername.Location = new System.Drawing.Point(15, 51);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(211, 24);
            this.txtUsername.TabIndex = 3;
            this.txtUsername.Text = "Username";
            this.txtUsername.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtUsername.TextForeColor = System.Drawing.Color.Black;
            this.txtUsername.Value = "";
            // 
            // cmbDatabaseName
            // 
            this.cmbDatabaseName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.cmbDatabaseName.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.cmbDatabaseName.FormattingEnabled = true;
            this.cmbDatabaseName.Location = new System.Drawing.Point(121, 125);
            this.cmbDatabaseName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbDatabaseName.Name = "cmbDatabaseName";
            this.cmbDatabaseName.Size = new System.Drawing.Size(186, 25);
            this.cmbDatabaseName.TabIndex = 2;
            // 
            // cmbServerName
            // 
            this.cmbServerName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.cmbServerName.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.cmbServerName.FormattingEnabled = true;
            this.cmbServerName.Location = new System.Drawing.Point(99, 16);
            this.cmbServerName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbServerName.Name = "cmbServerName";
            this.cmbServerName.Size = new System.Drawing.Size(249, 25);
            this.cmbServerName.TabIndex = 2;
            this.cmbServerName.DropDown += new System.EventHandler(this.cmbServerName_DropDown);
            this.cmbServerName.TextChanged += new System.EventHandler(this.cmbServerName_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(12, 128);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Database Name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Server Name:";
            // 
            // grbConnStr
            // 
            this.grbConnStr.Controls.Add(this.txtConnectionString);
            this.grbConnStr.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbConnStr.ForeColor = System.Drawing.Color.Gray;
            this.grbConnStr.Location = new System.Drawing.Point(15, 236);
            this.grbConnStr.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.grbConnStr.Name = "grbConnStr";
            this.grbConnStr.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.grbConnStr.Size = new System.Drawing.Size(501, 97);
            this.grbConnStr.TabIndex = 0;
            this.grbConnStr.TabStop = false;
            this.grbConnStr.Text = "Connection String";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtConnectionString.DefaultValue = "\r\nConnection String";
            this.txtConnectionString.DefaultValueColor = System.Drawing.Color.Gray;
            this.txtConnectionString.EnterToTab = false;
            this.txtConnectionString.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.txtConnectionString.ForeColor = System.Drawing.Color.Gray;
            this.txtConnectionString.Location = new System.Drawing.Point(17, 27);
            this.txtConnectionString.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtConnectionString.Multiline = true;
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(469, 57);
            this.txtConnectionString.TabIndex = 5;
            this.txtConnectionString.Text = "\r\nConnection String";
            this.txtConnectionString.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtConnectionString.TextForeColor = System.Drawing.Color.Black;
            this.txtConnectionString.Value = "";
            // 
            // btnSave
            // 
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.btnSave.Location = new System.Drawing.Point(155, 351);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(150, 35);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "&Save Connection";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.btnCancel.Location = new System.Drawing.Point(17, 351);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 35);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // DatabaseConfigurationForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Azure;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(528, 396);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grbConnStr);
            this.Controls.Add(this.grbConnectToServer);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(546, 439);
            this.MinimizeBox = false;
            this.Name = "DatabaseConfigurationForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Database Configuration";
            this.grbConnectToServer.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDbState)).EndInit();
            this.grbConnStr.ResumeLayout(false);
            this.grbConnStr.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grbConnectToServer;
        private System.Windows.Forms.GroupBox grbConnStr;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ComboBox cmbServerName;
        private System.Windows.Forms.Label label1;
        private Windows.Forms.HintTextBox txtUsername;
        private System.Windows.Forms.ComboBox cmbDatabaseName;
        private System.Windows.Forms.Label label2;
        private Windows.Forms.HintTextBox txtConnName;
        private Windows.Forms.HintTextBox txtPassword;
        private Windows.Forms.HintTextBox txtPort;
        private Windows.Forms.HintTextBox txtTimeout;
        private System.Windows.Forms.PictureBox picDbState;
        private Windows.Forms.HintTextBox txtConnectionString;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}