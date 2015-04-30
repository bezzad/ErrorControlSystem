using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace ErrorControlSystem.Shared.UI.ExceptionMapper
{
    /// <summary>
    /// Interaction logic for HeaderBlock.xaml
    /// </summary>
    public partial class HeaderBlock : UserControl
    {
        public HeaderBlock()
        {
            InitializeComponent();

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
            set { lblText.Text = value; }
        }

        #endregion

        #endregion
    }
}
