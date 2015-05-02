namespace ErrorControlSystem.Shared.UI.ExceptionMapper
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;


    /// <summary>
    /// Interaction logic for ExceptionView.xaml
    /// </summary>
    public partial class TreeMep : UserControl
    {
        public TreeMep()
        {
            InitializeComponent();

            this.DataContext = this;
        }


        #region Properties

        private Brush _exceptionLineBackground = Brushes.LightCoral;
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [Bindable(true), Category("Brush"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Brush ExceptionLineBackground
        {
            get { return _exceptionLineBackground; }
            set { _exceptionLineBackground = value; }
        }



        private double _codeFontSize = 12;
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [Bindable(true), Category("Common"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public double CodeFontSize
        {
            get { return _codeFontSize; }
            set { _codeFontSize = value; }
        }



        private double _exceptionCodeFontSize = 16;
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [Bindable(true), Category("Common"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public double ExceptionCodeFontSize
        {
            get { return _exceptionCodeFontSize; }
            set { _exceptionCodeFontSize = value; }
        }


        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [Bindable(true), Category("Common"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool TextWrapping { get; set; }

        #endregion


        #region Methods


        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="exp">The exception item.</param>
        /// <returns>index of added item at list of controls</returns>
        public int Add(Exception exp)
        {
            return Add(new TreeMapItem(exp));
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>index of added item at list of controls</returns>
        public int Add(TreeMapItem item)
        {
            item.OnShowCodeMapEventHandler += OnShowFrameCodeMap;
            var index = ExceptionTree.Items.Add(item);

            item.IsExpanded = true;
            if (item.StackTrace != null) item.StackTrace.IsExpanded = true;

            return index;
        }



        private void ExceptionTree_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            var expStackFrame = e.NewValue as StackFrameBlock;

            if (expStackFrame != null) expStackFrame.RaiseClick();
        }

        private async void OnShowFrameCodeMap(object sender, CodeMapEventArgs e)
        {
            var lines = await e.GetCodesAsync();

            // Create a FlowDocument
            var mcFlowDoc = new FlowDocument();

            if (lines == null || !lines.Any())
            {
                mcFlowDoc.Blocks.Add(new Paragraph(new Bold(new Run("File Not Found !!!"))) { FontSize = 50, TextAlignment = TextAlignment.Center, Background = ExceptionLineBackground });
                TxtCodes.Document = mcFlowDoc;
                LblAddress.Text = "File Not Found !!!";

                return;
            }

            LblAddress.Text = "File Path:  " + e.CodeAddress.FilePath;
            LblAddress.ToolTip = LblAddress.Text;


            // Add paragraphs to the FlowDocument.
            mcFlowDoc.Blocks.Add(new Paragraph(new Run(string.Join(Environment.NewLine, lines.Take(e.CodeAddress.Line - 1)))));
            mcFlowDoc.Blocks.Add(new Paragraph(new Bold(new Run(lines[e.CodeAddress.Line - 1]))) { FontSize = ExceptionCodeFontSize, Background = ExceptionLineBackground });
            mcFlowDoc.Blocks.Add(new Paragraph(new Run(string.Join(Environment.NewLine, lines.Skip(e.CodeAddress.Line)))));
            TxtCodes.Document = mcFlowDoc;

            if (!TextWrapping) TxtCodes.Document.PageWidth = lines.Max(x => x.Length) * TxtCodes.FontSize;


            var offset = (e.CodeAddress.Line * (TxtCodes.FontSize + 1.96)) - TxtCodes.ActualHeight / 2;


            TxtCodes.ScrollToVerticalOffset(offset);

        }


        #endregion
    }
}
