using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace ErrorControlSystem.Shared.UI.ExceptionMapper
{
    /// <summary>
    /// Interaction logic for MessageBlock.xaml
    /// </summary>
    public partial class MessageBlock : UserControl
    {
        public MessageBlock()
        {
            this.InitializeComponent();

            this.DataContext = this;
            LayoutRoot.DataContext = this;
        }

        #region Properties

        #region Text

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [Bindable(true), Category("Common"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public String Text
        {
            get { return lblText.Text; }
            set
            {
                lblText.Text = value;
                //
                // 40 Characters = 430px width + (10px corner)
                //Width = (value.Length * 450 / 40) + 10;
            }
        }

        #endregion

        #endregion
    }
}