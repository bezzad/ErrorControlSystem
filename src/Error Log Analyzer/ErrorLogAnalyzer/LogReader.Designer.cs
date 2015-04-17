using System.Drawing;
using System.Windows.Forms;
using ErrorControlSystem.Shared;

namespace ErrorLogAnalyzer
{
    partial class LogReader
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnQuit = new System.Windows.Forms.Button();
            this.DynamicDgv = new ErrorLogAnalyzer.ErrorDataGridView();
            this.btnRefreshGridView = new System.Windows.Forms.Button();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.pictureBox_viewer = new Windows.Forms.ImageBox();
            this.refreshAlert = new System.Windows.Forms.ErrorProvider(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.lblRecordsNum = new System.Windows.Forms.Label();
            this.prgCacheSize = new Windows.Forms.ProgressBar();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnSetConnection = new System.Windows.Forms.Button();
            this.txtCacheFilePath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblCacheRecords = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.txtConnStr = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.DynamicDgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.refreshAlert)).BeginInit();
            this.SuspendLayout();
            // 
            // btnQuit
            // 
            this.btnQuit.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.btnQuit.Location = new System.Drawing.Point(13, 13);
            this.btnQuit.Margin = new System.Windows.Forms.Padding(4);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(149, 94);
            this.btnQuit.TabIndex = 1;
            this.btnQuit.Text = "Quit";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // DynamicDgv
            // 
            this.DynamicDgv.AllowUserToAddRows = false;
            this.DynamicDgv.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.DynamicDgv.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DynamicDgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.DynamicDgv.ColumnHeadersHeight = 45;
            this.DynamicDgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DynamicDgv.Location = new System.Drawing.Point(0, 0);
            this.DynamicDgv.Margin = new System.Windows.Forms.Padding(4);
            this.DynamicDgv.Name = "DynamicDgv";
            this.DynamicDgv.ReadOnly = true;
            this.DynamicDgv.RowHeadersVisible = false;
            this.DynamicDgv.RowTemplate.Height = 40;
            this.DynamicDgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DynamicDgv.Size = new System.Drawing.Size(832, 582);
            this.DynamicDgv.TabIndex = 4;
            this.DynamicDgv.SelectionChanged += new System.EventHandler(this.dgv_ErrorsViewer_SelectionChanged);
            // 
            // btnRefreshGridView
            // 
            this.btnRefreshGridView.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefreshGridView.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnRefreshGridView.Location = new System.Drawing.Point(170, 13);
            this.btnRefreshGridView.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefreshGridView.Name = "btnRefreshGridView";
            this.btnRefreshGridView.Size = new System.Drawing.Size(149, 94);
            this.btnRefreshGridView.TabIndex = 9;
            this.btnRefreshGridView.Text = "&Refresh";
            this.btnRefreshGridView.UseVisualStyleBackColor = true;
            this.btnRefreshGridView.Click += new System.EventHandler(this.btnRefreshGridView_Click);
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerMain.Location = new System.Drawing.Point(16, 241);
            this.splitContainerMain.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.DynamicDgv);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.pictureBox_viewer);
            this.splitContainerMain.Size = new System.Drawing.Size(1355, 582);
            this.splitContainerMain.SplitterDistance = 832;
            this.splitContainerMain.SplitterWidth = 5;
            this.splitContainerMain.TabIndex = 10;
            // 
            // pictureBox_viewer
            // 
            this.pictureBox_viewer.AllowDoubleClick = true;
            this.pictureBox_viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_viewer.GridCellSize = 20;
            this.pictureBox_viewer.GridColor = System.Drawing.Color.Transparent;
            this.pictureBox_viewer.GridColorAlternate = System.Drawing.Color.Transparent;
            this.pictureBox_viewer.GridDisplayMode = Windows.Forms.ImageBoxGridDisplayMode.Image;
            this.pictureBox_viewer.GridScale = Windows.Forms.ImageBoxGridScale.Tiny;
            this.pictureBox_viewer.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            this.pictureBox_viewer.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_viewer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox_viewer.Name = "pictureBox_viewer";
            this.pictureBox_viewer.Size = new System.Drawing.Size(518, 582);
            this.pictureBox_viewer.TabIndex = 0;
            this.pictureBox_viewer.MouseEnter += new System.EventHandler(this.pictureBox_viewer_MouseEnter);
            // 
            // refreshAlert
            // 
            this.refreshAlert.ContainerControl = this;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F);
            this.label1.Location = new System.Drawing.Point(752, 117);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 20);
            this.label1.TabIndex = 12;
            this.label1.Text = "Number of Database Errors:";
            // 
            // lblRecordsNum
            // 
            this.lblRecordsNum.AutoSize = true;
            this.lblRecordsNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F);
            this.lblRecordsNum.ForeColor = System.Drawing.Color.Chocolate;
            this.lblRecordsNum.Location = new System.Drawing.Point(1006, 117);
            this.lblRecordsNum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRecordsNum.Name = "lblRecordsNum";
            this.lblRecordsNum.Size = new System.Drawing.Size(18, 20);
            this.lblRecordsNum.TabIndex = 12;
            this.lblRecordsNum.Text = "0";
            // 
            // prgCacheSize
            // 
            this.prgCacheSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prgCacheSize.CustomText = null;
            this.prgCacheSize.DisplayStyle = Windows.Forms.ProgressBarDisplayText.Percentage;
            this.prgCacheSize.Location = new System.Drawing.Point(15, 191);
            this.prgCacheSize.Margin = new System.Windows.Forms.Padding(4);
            this.prgCacheSize.Name = "prgCacheSize";
            this.prgCacheSize.Size = new System.Drawing.Size(1355, 42);
            this.prgCacheSize.TabIndex = 11;
            this.prgCacheSize.TextColor = System.Drawing.Color.Tan;
            this.prgCacheSize.TextFont = new System.Drawing.Font("Times New Roman", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // btnSetConnection
            // 
            this.btnSetConnection.BackgroundImage = global::ErrorLogAnalyzer.Properties.Resources.Disable;
            this.btnSetConnection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSetConnection.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSetConnection.Location = new System.Drawing.Point(1281, 12);
            this.btnSetConnection.Name = "btnSetConnection";
            this.btnSetConnection.Size = new System.Drawing.Size(90, 90);
            this.btnSetConnection.TabIndex = 21;
            this.toolTip.SetToolTip(this.btnSetConnection, "Click to set connection string configuration");
            this.btnSetConnection.UseVisualStyleBackColor = true;
            this.btnSetConnection.Click += new System.EventHandler(this.btnSetConnection_Click);
            // 
            // txtCacheFilePath
            // 
            this.txtCacheFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCacheFilePath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtCacheFilePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.txtCacheFilePath.Location = new System.Drawing.Point(516, 148);
            this.txtCacheFilePath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtCacheFilePath.Name = "txtCacheFilePath";
            this.txtCacheFilePath.ReadOnly = true;
            this.txtCacheFilePath.Size = new System.Drawing.Size(855, 28);
            this.txtCacheFilePath.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(360, 153);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 20);
            this.label4.TabIndex = 17;
            this.label4.Text = "Cache File Path:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(360, 117);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(197, 20);
            this.label5.TabIndex = 18;
            this.label5.Text = "Number of Cache Errors:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCacheRecords
            // 
            this.lblCacheRecords.AutoSize = true;
            this.lblCacheRecords.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F);
            this.lblCacheRecords.ForeColor = System.Drawing.Color.Chocolate;
            this.lblCacheRecords.Location = new System.Drawing.Point(588, 117);
            this.lblCacheRecords.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCacheRecords.Name = "lblCacheRecords";
            this.lblCacheRecords.Size = new System.Drawing.Size(18, 20);
            this.lblCacheRecords.TabIndex = 19;
            this.lblCacheRecords.Text = "0";
            // 
            // timer
            // 
            this.timer.Interval = 2000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // txtConnStr
            // 
            this.txtConnStr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConnStr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtConnStr.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.txtConnStr.Location = new System.Drawing.Point(364, 12);
            this.txtConnStr.Multiline = true;
            this.txtConnStr.Name = "txtConnStr";
            this.txtConnStr.ReadOnly = true;
            this.txtConnStr.Size = new System.Drawing.Size(900, 90);
            this.txtConnStr.TabIndex = 20;
            // 
            // LogReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1387, 838);
            this.Controls.Add(this.btnSetConnection);
            this.Controls.Add(this.txtConnStr);
            this.Controls.Add(this.lblCacheRecords);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtCacheFilePath);
            this.Controls.Add(this.lblRecordsNum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.prgCacheSize);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.btnRefreshGridView);
            this.Controls.Add(this.btnQuit);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MinimumSize = new System.Drawing.Size(1165, 44);
            this.Name = "LogReader";
            this.Text = "Error Log Analyzer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LogReader_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.DynamicDgv)).EndInit();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.refreshAlert)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnQuit;
        private ErrorDataGridView DynamicDgv;
        private System.Windows.Forms.Button btnRefreshGridView;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private Windows.Forms.ImageBox pictureBox_viewer;
        private ErrorProvider refreshAlert;
        private Windows.Forms.ProgressBar prgCacheSize;
        private Label lblRecordsNum;
        private Label label1;
        private ToolTip toolTip;
        private Label label4;
        private TextBox txtCacheFilePath;
        private Label lblCacheRecords;
        private Label label5;
        private Timer timer;
        private Button btnSetConnection;
        private TextBox txtConnStr;
    }
}

