namespace TestErrorHandlerBySelf
{
    partial class Form1
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnTestHandledFirstExp = new System.Windows.Forms.Button();
            this.btnTestUnHandledUIExp = new System.Windows.Forms.Button();
            this.btnTestUnHandledThreadExp = new System.Windows.Forms.Button();
            this.btnTestUnhandledTaskExp = new System.Windows.Forms.Button();
            this.btnRefreshGridView = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnQuit
            // 
            this.btnQuit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuit.Location = new System.Drawing.Point(954, 12);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(75, 23);
            this.btnQuit.TabIndex = 1;
            this.btnQuit.Text = "Quit";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(616, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(401, 405);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(607, 405);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.09804F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.90196F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 41);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1020, 411);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // btnTestHandledFirstExp
            // 
            this.btnTestHandledFirstExp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTestHandledFirstExp.Location = new System.Drawing.Point(12, 12);
            this.btnTestHandledFirstExp.Name = "btnTestHandledFirstExp";
            this.btnTestHandledFirstExp.Size = new System.Drawing.Size(172, 23);
            this.btnTestHandledFirstExp.TabIndex = 6;
            this.btnTestHandledFirstExp.Text = "ThrowTry/Catch Exception";
            this.btnTestHandledFirstExp.UseVisualStyleBackColor = true;
            this.btnTestHandledFirstExp.Click += new System.EventHandler(this.btnTestHandledFirstExp_Click);
            // 
            // btnTestUnHandledUIExp
            // 
            this.btnTestUnHandledUIExp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTestUnHandledUIExp.Location = new System.Drawing.Point(190, 12);
            this.btnTestUnHandledUIExp.Name = "btnTestUnHandledUIExp";
            this.btnTestUnHandledUIExp.Size = new System.Drawing.Size(172, 23);
            this.btnTestUnHandledUIExp.TabIndex = 6;
            this.btnTestUnHandledUIExp.Text = "Throw UnHandled UI Exception";
            this.btnTestUnHandledUIExp.UseVisualStyleBackColor = true;
            this.btnTestUnHandledUIExp.Click += new System.EventHandler(this.btnTestUnHandledUIExp_Click);
            // 
            // btnTestUnHandledThreadExp
            // 
            this.btnTestUnHandledThreadExp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTestUnHandledThreadExp.Location = new System.Drawing.Point(368, 12);
            this.btnTestUnHandledThreadExp.Name = "btnTestUnHandledThreadExp";
            this.btnTestUnHandledThreadExp.Size = new System.Drawing.Size(172, 23);
            this.btnTestUnHandledThreadExp.TabIndex = 7;
            this.btnTestUnHandledThreadExp.Text = "Throw UnHandled Thread Exception";
            this.btnTestUnHandledThreadExp.UseVisualStyleBackColor = true;
            this.btnTestUnHandledThreadExp.Click += new System.EventHandler(this.btnTestUnHandledThreadExp_Click);
            // 
            // btnTestUnhandledTaskExp
            // 
            this.btnTestUnhandledTaskExp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTestUnhandledTaskExp.Location = new System.Drawing.Point(546, 12);
            this.btnTestUnhandledTaskExp.Name = "btnTestUnhandledTaskExp";
            this.btnTestUnhandledTaskExp.Size = new System.Drawing.Size(172, 23);
            this.btnTestUnhandledTaskExp.TabIndex = 8;
            this.btnTestUnhandledTaskExp.Text = "Throw UnHandled Task Exception";
            this.btnTestUnhandledTaskExp.UseVisualStyleBackColor = true;
            this.btnTestUnhandledTaskExp.Click += new System.EventHandler(this.btnTestUnhandledTaskExp_Click);
            // 
            // btnRefreshGridView
            // 
            this.btnRefreshGridView.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefreshGridView.Location = new System.Drawing.Point(724, 12);
            this.btnRefreshGridView.Name = "btnRefreshGridView";
            this.btnRefreshGridView.Size = new System.Drawing.Size(132, 23);
            this.btnRefreshGridView.TabIndex = 9;
            this.btnRefreshGridView.Text = "Refresh Grid View";
            this.btnRefreshGridView.UseVisualStyleBackColor = true;
            this.btnRefreshGridView.Click += new System.EventHandler(this.btnRefreshGridView_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 464);
            this.Controls.Add(this.btnRefreshGridView);
            this.Controls.Add(this.btnTestUnhandledTaskExp);
            this.Controls.Add(this.btnTestUnHandledThreadExp);
            this.Controls.Add(this.btnTestUnHandledUIExp);
            this.Controls.Add(this.btnTestHandledFirstExp);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnQuit);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnTestHandledFirstExp;
        private System.Windows.Forms.Button btnTestUnHandledUIExp;
        private System.Windows.Forms.Button btnTestUnHandledThreadExp;
        private System.Windows.Forms.Button btnTestUnhandledTaskExp;
        private System.Windows.Forms.Button btnRefreshGridView;
    }
}

