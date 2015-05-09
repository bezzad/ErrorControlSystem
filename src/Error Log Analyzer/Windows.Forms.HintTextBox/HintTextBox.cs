using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Globalization;
using System.Numerics;
using System.Windows.Forms;

namespace Windows.Forms
{
    [ProvideProperty("HintTextBox", typeof(Control))]
    public sealed class HintTextBox : TextBox
    {
        #region Members
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Hint text, when text box is empty this value be superseded."), Category("Appearance")]
        [Localizable(true)]
        [Editor(typeof(MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string HintValue
        {
            set
            {
                this._hintValue = value;
                Text = value;
                ForeColor = HintColor;
            }
            get { return _hintValue; }
        }
        private string _hintValue;


        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Hint value text's fore color."), Category("Appearance")]
        public Color HintColor
        {
            set
            {
                if (this.ForeColor == this._hintColor || Text == string.Empty || Text == this.HintValue)
                {
                    this.ForeColor = value;
                }
                _hintColor = value;
            }
            get { return this._hintColor; }
        }
        private Color _hintColor;


        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Main text's fore color."), Category("Appearance")]
        public Color TextForeColor { get; set; }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Is Numerical TextBox? if value is true then just typed numbers, and if false then typed any chars."), Category("Behavior")]
        [DefaultValue(false)]
        public bool IsNumerical
        {
            get { return _isNumerical; }
            set
            {
                _isNumerical = value;

                if (!value)
                {
                    this.ThousandsSeparator = false;
                    this.AcceptMathChars = false;
                }
            }
        }
        private bool _isNumerical;

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Show Thousands Separator in TextBox? if value is true then split any 3 numerical digits by char ',' .\n\rNote: IsNumerical must be 'true' for runes this behavior."), Category("Behavior")]
        [DisplayName("Thousands Separator"), DefaultValue(false)]
        public bool ThousandsSeparator
        {
            get { return _thousandsSeparator; }
            set
            {
                _thousandsSeparator = value;
                if (value)
                {
                    IsNumerical = true;
                    AcceptMathChars = false;
                }
            }
        }
        private bool _thousandsSeparator;

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Convert Enter key press to TAB and focus next controls."), Category("Behavior")]
        [DisplayName("Enter key To Tab")]
        public bool EnterToTab { get; set; }



        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Accepting mathematical operators such as + , - , * and /"), Category("Behavior")]
        [DefaultValue(false)]
        public bool AcceptMathChars
        {
            get { return _acceptMathChars; }
            set
            {
                _acceptMathChars = value;
                if (value)
                {
                    IsNumerical = true;
                    ThousandsSeparator = false;
                }
            }
        }
        private bool _acceptMathChars;

        public string MathParserResult { get; private set; }

        private MathParser mathParser = new MathParser();

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("The real text of text box"), Category("Appearance")]
        [Localizable(true)]
        [Editor(typeof(MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string Value
        {
            get
            {
                return (_hintValue != null && Text == HintValue) ? string.Empty : Text;
            }
            set
            {
                if (value != string.Empty && value != HintValue)
                    ForeColor = TextForeColor;

                Text = value;
            }
        }

        #endregion

        #region Constructors
        public HintTextBox()
        {
            InitializeComponent();

            TextForeColor = Color.Black;
            this.HintColor = Color.Gray;
            this.HintValue = "Hint Value";

            this.HandleCreated += (s, e) => { if (string.IsNullOrEmpty(Value)) Text = HintValue; };
        }

        public HintTextBox(IContainer container)
            : this() { container.Add(this); }

        #endregion

        #region PreInitialized Events

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            //
            if (Text == this.HintValue)
            {
                // Clean hint text
                Text = string.Empty;
                this.ForeColor = TextForeColor;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            //
            if (Text == string.Empty)
            {
                // Set to hint text
                this.ForeColor = this.HintColor;
                Text = this.HintValue;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            //
            if (Text == string.Empty)
            {
                // Set to hint text
                this.ForeColor = this.HintColor;
                Text = this.HintValue;
            }

            if (ThousandsSeparator && !AcceptMathChars)
            {
                var indexSelectionBuffer = this.SelectionStart;
                if (!string.IsNullOrEmpty(Text) && e.KeyData != Keys.Left && e.KeyData != Keys.Right)
                {
                    BigInteger valueBefore;
                    // Parse currency value using en-GB culture. 
                    // value = "�1,097.63";
                    // Displays:  
                    //       Converted '�1,097.63' to 1097.63
                    var style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
                    var culture = CultureInfo.CreateSpecificCulture("en-US");
                    if (BigInteger.TryParse(Text, style, culture, out valueBefore))
                    {
                        Text = String.Format(culture, "{0:N0}", valueBefore);
                        if (e.KeyData != Keys.Delete && e.KeyData != Keys.Back) this.Select(Text.Length, 0);
                        else this.Select(indexSelectionBuffer, 0);
                    }
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            //
            if (e.KeyCode == Keys.Enter && EnterToTab) SendKeys.Send("{TAB}");
            else if (Text == this.HintValue)
            {
                // Clean hint text
                Text = string.Empty;
                this.ForeColor = TextForeColor;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            //
            if (!char.IsDigit(e.KeyChar) && this.IsNumerical)
            {
                int charValue = e.KeyChar;
                const int backKeyCharValue = 8; // 8 or '\b'
                const int deleteKeyCharValue = 13; // 13 or '\d'

                if (charValue == backKeyCharValue || charValue == deleteKeyCharValue)
                {
                    e.Handled = false;
                    return;
                }
                else if (AcceptMathChars && !ThousandsSeparator)
                {
                    if (e.KeyChar == '+' || e.KeyChar == '-' ||
                        e.KeyChar == '*' || e.KeyChar == '/' ||
                        e.KeyChar == '(' || e.KeyChar == ')')
                    {
                        e.Handled = false;
                        return;
                    }
                }

                e.Handled = true;
                return;
            }
            else
            {
                e.Handled = false;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            //
            if (IsNumerical && AcceptMathChars && !string.IsNullOrEmpty(Text))
            {
                try
                {
                    MathParserResult = mathParser.Calculate(Text).ToString(CultureInfo.InvariantCulture);
                }
                catch { MathParserResult = ""; }
            }
            else MathParserResult = "";
        }


        #endregion

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion
        #endregion
    }
}
