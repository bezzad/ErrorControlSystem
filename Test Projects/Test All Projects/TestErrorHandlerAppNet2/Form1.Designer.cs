namespace TestErrorHandlerAppNet2
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
            this.btnThrowExp = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnThrowExp
            // 
            this.btnThrowExp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnThrowExp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThrowExp.Location = new System.Drawing.Point(12, 12);
            this.btnThrowExp.Name = "btnThrowExp";
            this.btnThrowExp.Size = new System.Drawing.Size(309, 103);
            this.btnThrowExp.TabIndex = 0;
            this.btnThrowExp.Text = "Throw new random exception";
            this.btnThrowExp.UseVisualStyleBackColor = true;
            this.btnThrowExp.Click += new System.EventHandler(this.btnThrowExp_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 123);
            this.Controls.Add(this.btnThrowExp);
            this.Name = "Form1";
            this.Text = "Test Error Handler";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnThrowExp;
    }
}

