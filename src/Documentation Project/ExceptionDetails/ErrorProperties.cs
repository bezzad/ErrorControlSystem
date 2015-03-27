using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Tools.Excel;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;

namespace ExceptionDetails
{
    public partial class ErrorProperties
    {
        private void Sheet1_Startup(object sender, System.EventArgs e)
        {
            MessageBox.Show("This excel file is information of Error object properties in project and database",
                "About this Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Sheet1_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(Sheet1_Startup);
            this.Shutdown += new System.EventHandler(Sheet1_Shutdown);
        }

        #endregion

    }
}
