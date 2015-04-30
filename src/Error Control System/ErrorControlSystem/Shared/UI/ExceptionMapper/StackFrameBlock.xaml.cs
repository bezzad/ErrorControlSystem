using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace ErrorControlSystem.Shared.UI.ExceptionMapper
{
    /// <summary>
    /// Interaction logic for StackFrame.xaml
    /// </summary>
    public partial class StackFrameBlock : UserControl
    {
        #region Fields

        private CodeScope _frameCodeScope;

        #endregion

        #region Constructors

        public StackFrameBlock()
        {
            this.InitializeComponent();

            this.DataContext = this;
            LayoutRoot.DataContext = this;
        }

        #endregion

        #region Properties

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [Bindable(true), Category("Common"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public String Text
        {
            get { return lblText.Text; }
            set { lblText.Text = value; }
        }


        public CodeScope FrameCodeScope
        {
            get { return _frameCodeScope; }
            set
            {
                _frameCodeScope = value;

                Text = value.ToString(CodeScopeStringFormat.Normal);

                if (!String.IsNullOrEmpty(value.FilePath))
                {
                    ToolTip = string.Format("Click on frame to go to exception location in {0}  {1}",
                        value.FilePath, value.ToString(CodeScopeStringFormat.JustLineColumn));

                    this.Cursor = Cursors.Hand;
                }
            }
        }



        #endregion

        #region Events

        public CodeMapEventHandler Clicked = delegate(object sender, CodeMapEventArgs args) { };

        #endregion

        #region Methods

        public void RaiseClick()
        {
            Clicked(this, new CodeMapEventArgs(FrameCodeScope));
        }

        #endregion
    }
}