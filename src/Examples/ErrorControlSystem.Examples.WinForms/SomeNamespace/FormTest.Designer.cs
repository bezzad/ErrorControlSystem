namespace ErrorControlSystem.Examples.WinForms.SomeNamespace
{
    partial class FormTest
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
            this.btnTestHandledFirstExp = new System.Windows.Forms.Button();
            this.btnTestUnHandledUIExp = new System.Windows.Forms.Button();
            this.btnTestUnHandledThreadExp = new System.Windows.Forms.Button();
            this.btnTestUnhandledTaskExp = new System.Windows.Forms.Button();
            this.btnThrowExceptExceptions = new System.Windows.Forms.Button();
            this.btnDataException = new System.Windows.Forms.Button();
            this.btnThrowMultiExps = new System.Windows.Forms.Button();
            this.btnExemptedMethodException = new System.Windows.Forms.Button();
            this.btnViolation = new System.Windows.Forms.Button();
            this.btnTestExceptionHandlerSpeed = new System.Windows.Forms.Button();
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnQuit
            // 
            this.btnQuit.Location = new System.Drawing.Point(68, 639);
            this.btnQuit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(317, 47);
            this.btnQuit.TabIndex = 1;
            this.btnQuit.Text = "Quit";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // btnTestHandledFirstExp
            // 
            this.btnTestHandledFirstExp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTestHandledFirstExp.Location = new System.Drawing.Point(68, 15);
            this.btnTestHandledFirstExp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTestHandledFirstExp.Name = "btnTestHandledFirstExp";
            this.btnTestHandledFirstExp.Size = new System.Drawing.Size(317, 52);
            this.btnTestHandledFirstExp.TabIndex = 6;
            this.btnTestHandledFirstExp.Text = "ThrowTry/Catch Exception";
            this.btnTestHandledFirstExp.UseVisualStyleBackColor = true;
            this.btnTestHandledFirstExp.Click += new System.EventHandler(this.btnTestHandledFirstExp_Click);
            // 
            // btnTestUnHandledUIExp
            // 
            this.btnTestUnHandledUIExp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTestUnHandledUIExp.Location = new System.Drawing.Point(68, 74);
            this.btnTestUnHandledUIExp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTestUnHandledUIExp.Name = "btnTestUnHandledUIExp";
            this.btnTestUnHandledUIExp.Size = new System.Drawing.Size(317, 52);
            this.btnTestUnHandledUIExp.TabIndex = 6;
            this.btnTestUnHandledUIExp.Text = "Throw UnHandled UI Exception";
            this.btnTestUnHandledUIExp.UseVisualStyleBackColor = true;
            this.btnTestUnHandledUIExp.Click += new System.EventHandler(this.btnTestUnHandledUIExp_Click);
            // 
            // btnTestUnHandledThreadExp
            // 
            this.btnTestUnHandledThreadExp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTestUnHandledThreadExp.Location = new System.Drawing.Point(68, 133);
            this.btnTestUnHandledThreadExp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTestUnHandledThreadExp.Name = "btnTestUnHandledThreadExp";
            this.btnTestUnHandledThreadExp.Size = new System.Drawing.Size(317, 52);
            this.btnTestUnHandledThreadExp.TabIndex = 7;
            this.btnTestUnHandledThreadExp.Text = "Throw UnHandled Thread Exception";
            this.btnTestUnHandledThreadExp.UseVisualStyleBackColor = true;
            this.btnTestUnHandledThreadExp.Click += new System.EventHandler(this.btnTestUnHandledThreadExp_Click);
            // 
            // btnTestUnhandledTaskExp
            // 
            this.btnTestUnhandledTaskExp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTestUnhandledTaskExp.Location = new System.Drawing.Point(68, 192);
            this.btnTestUnhandledTaskExp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTestUnhandledTaskExp.Name = "btnTestUnhandledTaskExp";
            this.btnTestUnhandledTaskExp.Size = new System.Drawing.Size(317, 52);
            this.btnTestUnhandledTaskExp.TabIndex = 8;
            this.btnTestUnhandledTaskExp.Text = "Throw UnHandled Task Exception";
            this.btnTestUnhandledTaskExp.UseVisualStyleBackColor = true;
            this.btnTestUnhandledTaskExp.Click += new System.EventHandler(this.btnTestUnhandledTaskExp_Click);
            // 
            // btnThrowExceptExceptions
            // 
            this.btnThrowExceptExceptions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThrowExceptExceptions.Location = new System.Drawing.Point(68, 251);
            this.btnThrowExceptExceptions.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnThrowExceptExceptions.Name = "btnThrowExceptExceptions";
            this.btnThrowExceptExceptions.Size = new System.Drawing.Size(317, 52);
            this.btnThrowExceptExceptions.TabIndex = 9;
            this.btnThrowExceptExceptions.Text = "Throw Except Exception";
            this.btnThrowExceptExceptions.UseVisualStyleBackColor = true;
            this.btnThrowExceptExceptions.Click += new System.EventHandler(this.btnThrowExceptExceptions_Click);
            // 
            // btnDataException
            // 
            this.btnDataException.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDataException.Location = new System.Drawing.Point(68, 310);
            this.btnDataException.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDataException.Name = "btnDataException";
            this.btnDataException.Size = new System.Drawing.Size(317, 52);
            this.btnDataException.TabIndex = 10;
            this.btnDataException.Text = "Throw Data Exception";
            this.btnDataException.UseVisualStyleBackColor = true;
            this.btnDataException.Click += new System.EventHandler(this.btnDataException_Click);
            // 
            // btnThrowMultiExps
            // 
            this.btnThrowMultiExps.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThrowMultiExps.Location = new System.Drawing.Point(68, 369);
            this.btnThrowMultiExps.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnThrowMultiExps.Name = "btnThrowMultiExps";
            this.btnThrowMultiExps.Size = new System.Drawing.Size(317, 52);
            this.btnThrowMultiExps.TabIndex = 11;
            this.btnThrowMultiExps.Text = "Throw Multiple Exceptions";
            this.btnThrowMultiExps.UseVisualStyleBackColor = true;
            this.btnThrowMultiExps.Click += new System.EventHandler(this.btnThrowMultiExps_Click);
            // 
            // btnExemptedMethodException
            // 
            this.btnExemptedMethodException.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExemptedMethodException.Location = new System.Drawing.Point(68, 428);
            this.btnExemptedMethodException.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExemptedMethodException.Name = "btnExemptedMethodException";
            this.btnExemptedMethodException.Size = new System.Drawing.Size(317, 52);
            this.btnExemptedMethodException.TabIndex = 11;
            this.btnExemptedMethodException.Text = "Exempted Method Exception";
            this.btnExemptedMethodException.UseVisualStyleBackColor = true;
            this.btnExemptedMethodException.Click += new System.EventHandler(this.btnExemptedMethodException_Click);
            // 
            // btnViolation
            // 
            this.btnViolation.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnViolation.Location = new System.Drawing.Point(68, 487);
            this.btnViolation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnViolation.Name = "btnViolation";
            this.btnViolation.Size = new System.Drawing.Size(317, 52);
            this.btnViolation.TabIndex = 11;
            this.btnViolation.Text = "Violation Access Exception";
            this.btnViolation.UseVisualStyleBackColor = true;
            this.btnViolation.Click += new System.EventHandler(this.btnViolation_Click);
            // 
            // btnTestExceptionHandlerSpeed
            // 
            this.btnTestExceptionHandlerSpeed.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTestExceptionHandlerSpeed.Location = new System.Drawing.Point(68, 546);
            this.btnTestExceptionHandlerSpeed.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTestExceptionHandlerSpeed.Name = "btnTestExceptionHandlerSpeed";
            this.btnTestExceptionHandlerSpeed.Size = new System.Drawing.Size(317, 52);
            this.btnTestExceptionHandlerSpeed.TabIndex = 11;
            this.btnTestExceptionHandlerSpeed.Text = "Test Exception Handler Speed";
            this.btnTestExceptionHandlerSpeed.UseVisualStyleBackColor = true;
            this.btnTestExceptionHandlerSpeed.Click += new System.EventHandler(this.btnTestExceptionHandlerSpeed_Click);
            // 
            // cmbLanguage
            // 
            this.cmbLanguage.FormattingEnabled = true;
            this.cmbLanguage.Location = new System.Drawing.Point(68, 719);
            this.cmbLanguage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbLanguage.Name = "cmbLanguage";
            this.cmbLanguage.Size = new System.Drawing.Size(316, 24);
            this.cmbLanguage.TabIndex = 12;
            // 
            // FormTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 796);
            this.Controls.Add(this.cmbLanguage);
            this.Controls.Add(this.btnTestExceptionHandlerSpeed);
            this.Controls.Add(this.btnViolation);
            this.Controls.Add(this.btnExemptedMethodException);
            this.Controls.Add(this.btnThrowMultiExps);
            this.Controls.Add(this.btnDataException);
            this.Controls.Add(this.btnThrowExceptExceptions);
            this.Controls.Add(this.btnTestUnhandledTaskExp);
            this.Controls.Add(this.btnTestUnHandledThreadExp);
            this.Controls.Add(this.btnTestUnHandledUIExp);
            this.Controls.Add(this.btnTestHandledFirstExp);
            this.Controls.Add(this.btnQuit);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormTest";
            this.Text = "Test by Throw Exceptions";
            this.Load += new System.EventHandler(this.FormTest_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.Button btnTestHandledFirstExp;
        private System.Windows.Forms.Button btnTestUnHandledUIExp;
        private System.Windows.Forms.Button btnTestUnHandledThreadExp;
        private System.Windows.Forms.Button btnTestUnhandledTaskExp;
        private System.Windows.Forms.Button btnThrowExceptExceptions;
        private System.Windows.Forms.Button btnDataException;
        private System.Windows.Forms.Button btnThrowMultiExps;
        private System.Windows.Forms.Button btnExemptedMethodException;
        private System.Windows.Forms.Button btnViolation;
        private System.Windows.Forms.Button btnTestExceptionHandlerSpeed;
        private System.Windows.Forms.ComboBox cmbLanguage;
    }
}

