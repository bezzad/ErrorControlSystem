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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pictureBox_viewer = new Shoniz.Windows.Forms.ImageBox();
            this.refreshAlert = new System.Windows.Forms.ErrorProvider(this.components);
            this.prgCacheSize = new Shoniz.Windows.Forms.ProgressBar();
            this.lblCacheSize = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblRecordsNum = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ErrorsViewer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.refreshAlert)).BeginInit();
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
            this.dgv_ErrorsViewer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_ErrorsViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_ErrorsViewer.Location = new System.Drawing.Point(0, 0);
            this.dgv_ErrorsViewer.Name = "dgv_ErrorsViewer";
            this.dgv_ErrorsViewer.Size = new System.Drawing.Size(581, 567);
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
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 102);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgv_ErrorsViewer);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox_viewer);
            this.splitContainer1.Size = new System.Drawing.Size(940, 567);
            this.splitContainer1.SplitterDistance = 581;
            this.splitContainer1.TabIndex = 10;
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
            this.pictureBox_viewer.Size = new System.Drawing.Size(355, 567);
            this.pictureBox_viewer.TabIndex = 0;
            // 
            // refreshAlert
            // 
            this.refreshAlert.ContainerControl = this;
            // 
            // prgCacheSize
            // 
            this.prgCacheSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.prgCacheSize.CustomText = null;
            this.prgCacheSize.DisplayStyle = Shoniz.Windows.Forms.ProgressBarDisplayText.Percentage;
            this.prgCacheSize.Location = new System.Drawing.Point(454, 11);
            this.prgCacheSize.Name = "prgCacheSize";
            this.prgCacheSize.Size = new System.Drawing.Size(498, 40);
            this.prgCacheSize.TabIndex = 11;
            this.prgCacheSize.TextColor = System.Drawing.Color.Tan;
            this.prgCacheSize.TextFont = new System.Drawing.Font("Times New Roman", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // lblCacheSize
            // 
            this.lblCacheSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCacheSize.AutoSize = true;
            this.lblCacheSize.Font = new System.Drawing.Font("Mitra Bold Mazar", 12F);
            this.lblCacheSize.Location = new System.Drawing.Point(361, 19);
            this.lblCacheSize.Name = "lblCacheSize";
            this.lblCacheSize.Size = new System.Drawing.Size(87, 26);
            this.lblCacheSize.TabIndex = 12;
            this.lblCacheSize.Text = "Cache Size:";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Mitra Bold Mazar", 12F);
            this.label1.Location = new System.Drawing.Point(361, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 26);
            this.label1.TabIndex = 12;
            this.label1.Text = "Number of Database Records:";
            // 
            // lblRecordsNum
            // 
            this.lblRecordsNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRecordsNum.AutoSize = true;
            this.lblRecordsNum.Font = new System.Drawing.Font("Mitra Bold Mazar", 12F);
            this.lblRecordsNum.Location = new System.Drawing.Point(586, 59);
            this.lblRecordsNum.Name = "lblRecordsNum";
            this.lblRecordsNum.Size = new System.Drawing.Size(20, 26);
            this.lblRecordsNum.TabIndex = 12;
            this.lblRecordsNum.Text = "0";
            // 
            // LogReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 681);
            this.Controls.Add(this.lblRecordsNum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblCacheSize);
            this.Controls.Add(this.prgCacheSize);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btnRefreshGridView);
            this.Controls.Add(this.btnQuit);
            this.MinimumSize = new System.Drawing.Size(880, 0);
            this.Name = "LogReader";
            this.Text = "Error Log Analyzer";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ErrorsViewer)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.refreshAlert)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.DataGridView dgv_ErrorsViewer;
        private System.Windows.Forms.Button btnRefreshGridView;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Shoniz.Windows.Forms.ImageBox pictureBox_viewer;
        private ErrorProvider refreshAlert;
        private Shoniz.Windows.Forms.ProgressBar prgCacheSize;
        private Label lblCacheSize;
        private Label lblRecordsNum;
        private Label label1;
    }
}

