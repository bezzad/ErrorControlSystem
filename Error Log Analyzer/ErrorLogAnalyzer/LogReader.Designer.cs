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
            this.refreshAlert = new System.Windows.Forms.ErrorProvider(this.components);
            this.pictureBox_viewer = new Shoniz.Windows.Forms.ImageBox();
            this.prgCacheSize = new Shoniz.Windows.Forms.ProgressBar();
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
            this.dgv_ErrorsViewer.Size = new System.Drawing.Size(631, 611);
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
            this.splitContainer1.Location = new System.Drawing.Point(12, 57);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgv_ErrorsViewer);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox_viewer);
            this.splitContainer1.Size = new System.Drawing.Size(1020, 611);
            this.splitContainer1.SplitterDistance = 631;
            this.splitContainer1.TabIndex = 10;
            // 
            // refreshAlert
            // 
            this.refreshAlert.ContainerControl = this;
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
            this.pictureBox_viewer.Size = new System.Drawing.Size(385, 611);
            this.pictureBox_viewer.TabIndex = 0;
            // 
            // prgCacheSize
            // 
            this.prgCacheSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prgCacheSize.CustomText = null;
            this.prgCacheSize.DisplayStyle = Shoniz.Windows.Forms.ProgressBarDisplayText.Percentage;
            this.prgCacheSize.Location = new System.Drawing.Point(647, 11);
            this.prgCacheSize.Name = "prgCacheSize";
            this.prgCacheSize.Size = new System.Drawing.Size(385, 40);
            this.prgCacheSize.TabIndex = 11;
            this.prgCacheSize.TextColor = System.Drawing.Color.Tan;
            this.prgCacheSize.TextFont = new System.Drawing.Font("Times New Roman", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // LogReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 680);
            this.Controls.Add(this.prgCacheSize);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btnRefreshGridView);
            this.Controls.Add(this.btnQuit);
            this.Name = "LogReader";
            this.Text = "Error Log Analyzer";
            this.Load += new System.EventHandler(this.LogReader_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ErrorsViewer)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.refreshAlert)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.DataGridView dgv_ErrorsViewer;
        private System.Windows.Forms.Button btnRefreshGridView;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Shoniz.Windows.Forms.ImageBox pictureBox_viewer;
        private ErrorProvider refreshAlert;
        private Shoniz.Windows.Forms.ProgressBar prgCacheSize;
    }
}

