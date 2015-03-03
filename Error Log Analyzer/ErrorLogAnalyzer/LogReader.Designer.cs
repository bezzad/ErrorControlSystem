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
            this.btnQuit = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnRefreshGridView = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pictureBox_viewer = new Shoniz.Windows.Forms.ImageBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnQuit
            // 
            this.btnQuit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuit.Location = new System.Drawing.Point(1272, 15);
            this.btnQuit.Margin = new System.Windows.Forms.Padding(4);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(100, 28);
            this.btnQuit.TabIndex = 1;
            this.btnQuit.Text = "Quit";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(842, 772);
            this.dataGridView1.TabIndex = 4;
            // 
            // btnRefreshGridView
            // 
            this.btnRefreshGridView.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefreshGridView.Location = new System.Drawing.Point(20, 15);
            this.btnRefreshGridView.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefreshGridView.Name = "btnRefreshGridView";
            this.btnRefreshGridView.Size = new System.Drawing.Size(176, 28);
            this.btnRefreshGridView.TabIndex = 9;
            this.btnRefreshGridView.Text = "Refresh Grid View";
            this.btnRefreshGridView.UseVisualStyleBackColor = true;
            this.btnRefreshGridView.Click += new System.EventHandler(this.btnRefreshGridView_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(16, 50);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox_viewer);
            this.splitContainer1.Size = new System.Drawing.Size(1360, 772);
            this.splitContainer1.SplitterDistance = 842;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 10;
            // 
            // pictureBox_viewer
            // 
            this.pictureBox_viewer.AllowDoubleClick = true;
            this.pictureBox_viewer.AutoSize = true;
            this.pictureBox_viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_viewer.GridCellSize = 20;
            this.pictureBox_viewer.GridColor = System.Drawing.Color.Transparent;
            this.pictureBox_viewer.GridColorAlternate = System.Drawing.Color.Transparent;
            this.pictureBox_viewer.GridDisplayMode = Shoniz.Windows.Forms.ImageBoxGridDisplayMode.Image;
            this.pictureBox_viewer.GridScale = Shoniz.Windows.Forms.ImageBoxGridScale.Tiny;
            this.pictureBox_viewer.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            this.pictureBox_viewer.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_viewer.Name = "pictureBox_viewer";
            this.pictureBox_viewer.Size = new System.Drawing.Size(513, 772);
            this.pictureBox_viewer.TabIndex = 0;
            // 
            // LogReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1392, 837);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btnRefreshGridView);
            this.Controls.Add(this.btnQuit);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LogReader";
            this.Text = "Error Log Analyzer";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnRefreshGridView;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Shoniz.Windows.Forms.ImageBox pictureBox_viewer;
    }
}

