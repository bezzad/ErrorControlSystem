using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WaitSplash;

namespace ErrorLogAnalyzer
{
    public class BaseForm : Form
    {
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
            this.SuspendLayout();
            // 
            // BaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 210);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "BaseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BaseForm";
            this.ResumeLayout(false);

        }

        #endregion
        #endregion


        #region Properties

        public Splash WaitSplash;

        #endregion


        #region Methods
        
        public BaseForm()
        {
            InitializeComponent();

            WaitSplash = new Splash(this);

            Application.Idle += Application_Idle;
        }

        async void Application_Idle(object sender, EventArgs e)
        {
            Application.Idle -= Application_Idle;

            await InvokeAsync(OnFormLoad);
        }

        public async Task InvokeAsync(Action doSomething)
        {
            WaitSplash.Start();

            if (doSomething != null)
            {
                await Task.Run(() =>
                {
                    var invokeThread = new Thread(new ThreadStart(doSomething));
                    invokeThread.SetApartmentState(ApartmentState.STA);
                    invokeThread.Start();
                    invokeThread.Join();
                });
            }

            WaitSplash.Stop();

            this.Focus();
        }

        public virtual void OnFormLoad()
        {
            
        }
        #endregion
    }
}
