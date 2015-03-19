using System.Windows.Forms;

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
            this.btnQuit = new System.Windows.Forms.Button();
            this.dgv_ErrorsViewer = new System.Windows.Forms.DataGridView();
            this.btnRefreshGridView = new System.Windows.Forms.Button();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.pictureBox_viewer = new Shoniz.Windows.Forms.ImageBox();
            this.refreshAlert = new System.Windows.Forms.ErrorProvider(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.lblRecordsNum = new System.Windows.Forms.Label();
            this.prgCacheSize = new Shoniz.Windows.Forms.ProgressBar();
            this.cmbServerName = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbDatabaseName = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.picServerState = new System.Windows.Forms.PictureBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ErrorsViewer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.refreshAlert)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picServerState)).BeginInit();
            this.SuspendLayout();
            // 
            // btnQuit
            // 
            this.btnQuit.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.btnQuit.Location = new System.Drawing.Point(12, 12);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(112, 39);
            this.btnQuit.TabIndex = 1;
            this.btnQuit.Text = "Quit";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // dgv_ErrorsViewer
            // 
            this.dgv_ErrorsViewer.AllowUserToAddRows = false;
            this.dgv_ErrorsViewer.AllowUserToDeleteRows = false;
            this.dgv_ErrorsViewer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_ErrorsViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_ErrorsViewer.Location = new System.Drawing.Point(0, 0);
            this.dgv_ErrorsViewer.Name = "dgv_ErrorsViewer";
            this.dgv_ErrorsViewer.ReadOnly = true;
            this.dgv_ErrorsViewer.RowHeadersVisible = false;
            this.dgv_ErrorsViewer.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_ErrorsViewer.Size = new System.Drawing.Size(626, 530);
            this.dgv_ErrorsViewer.TabIndex = 4;
            // 
            // btnRefreshGridView
            // 
            this.btnRefreshGridView.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefreshGridView.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnRefreshGridView.Location = new System.Drawing.Point(130, 12);
            this.btnRefreshGridView.Name = "btnRefreshGridView";
            this.btnRefreshGridView.Size = new System.Drawing.Size(112, 39);
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
            this.splitContainerMain.Location = new System.Drawing.Point(12, 139);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.dgv_ErrorsViewer);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.pictureBox_viewer);
            this.splitContainerMain.Size = new System.Drawing.Size(1016, 530);
            this.splitContainerMain.SplitterDistance = 626;
            this.splitContainerMain.TabIndex = 10;
            // 
            // pictureBox_viewer
            // 
            this.pictureBox_viewer.AllowDoubleClick = true;
            this.pictureBox_viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_viewer.GridCellSize = 20;
            this.pictureBox_viewer.GridColor = System.Drawing.Color.Transparent;
            this.pictureBox_viewer.GridColorAlternate = System.Drawing.Color.Transparent;
            this.pictureBox_viewer.GridDisplayMode = Shoniz.Windows.Forms.ImageBoxGridDisplayMode.Image;
            this.pictureBox_viewer.GridScale = Shoniz.Windows.Forms.ImageBoxGridScale.Tiny;
            this.pictureBox_viewer.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            this.pictureBox_viewer.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_viewer.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox_viewer.Name = "pictureBox_viewer";
            this.pictureBox_viewer.Size = new System.Drawing.Size(386, 530);
            this.pictureBox_viewer.TabIndex = 0;
            // 
            // refreshAlert
            // 
            this.refreshAlert.ContainerControl = this;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(282, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(225, 20);
            this.label1.TabIndex = 12;
            this.label1.Text = "Number of Database Records:";
            // 
            // lblRecordsNum
            // 
            this.lblRecordsNum.AutoSize = true;
            this.lblRecordsNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblRecordsNum.ForeColor = System.Drawing.Color.Chocolate;
            this.lblRecordsNum.Location = new System.Drawing.Point(513, 61);
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
            this.prgCacheSize.DisplayStyle = Shoniz.Windows.Forms.ProgressBarDisplayText.Percentage;
            this.prgCacheSize.Location = new System.Drawing.Point(12, 99);
            this.prgCacheSize.Name = "prgCacheSize";
            this.prgCacheSize.Size = new System.Drawing.Size(1016, 34);
            this.prgCacheSize.TabIndex = 11;
            this.prgCacheSize.TextColor = System.Drawing.Color.Tan;
            this.prgCacheSize.TextFont = new System.Drawing.Font("Times New Roman", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // cmbServerName
            // 
            this.cmbServerName.FormattingEnabled = true;
            this.cmbServerName.Location = new System.Drawing.Point(379, 21);
            this.cmbServerName.Name = "cmbServerName";
            this.cmbServerName.Size = new System.Drawing.Size(222, 21);
            this.cmbServerName.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(283, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Server\\Instance :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbDatabaseName
            // 
            this.cmbDatabaseName.FormattingEnabled = true;
            this.cmbDatabaseName.Location = new System.Drawing.Point(703, 21);
            this.cmbDatabaseName.Name = "cmbDatabaseName";
            this.cmbDatabaseName.Size = new System.Drawing.Size(205, 21);
            this.cmbDatabaseName.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(638, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Database :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picServerState
            // 
            this.picServerState.Image = global::ErrorLogAnalyzer.Properties.Resources.Disable;
            this.picServerState.Location = new System.Drawing.Point(929, 12);
            this.picServerState.Name = "picServerState";
            this.picServerState.Size = new System.Drawing.Size(40, 37);
            this.picServerState.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picServerState.TabIndex = 15;
            this.picServerState.TabStop = false;
            // 
            // LogReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1040, 681);
            this.Controls.Add(this.picServerState);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbDatabaseName);
            this.Controls.Add(this.cmbServerName);
            this.Controls.Add(this.lblRecordsNum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.prgCacheSize);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.btnRefreshGridView);
            this.Controls.Add(this.btnQuit);
            this.MinimumSize = new System.Drawing.Size(879, 43);
            this.Name = "LogReader";
            this.Text = "Error Log Analyzer";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ErrorsViewer)).EndInit();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.refreshAlert)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picServerState)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.DataGridView dgv_ErrorsViewer;
        private System.Windows.Forms.Button btnRefreshGridView;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private Shoniz.Windows.Forms.ImageBox pictureBox_viewer;
        private ErrorProvider refreshAlert;
        private Shoniz.Windows.Forms.ProgressBar prgCacheSize;
        private Label lblRecordsNum;
        private Label label1;
        private ComboBox cmbServerName;
        private Label label2;
        private ComboBox cmbDatabaseName;
        private Label label3;
        private PictureBox picServerState;
        private ToolTip toolTip;
    }
}

