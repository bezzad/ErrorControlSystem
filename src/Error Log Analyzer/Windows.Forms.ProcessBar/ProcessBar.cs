using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Windows.Forms
{
    // Shoniz ProgressBar
    // Copyright (c) 2010-2014 
    // http://com

    // If you use this control in your applications, attribution, donations or contributions are welcome.

    /// <summary>
    ///   Component for displaying percent of process with support for show percent on label text's.
    /// </summary>
    public class ProgressBar : System.Windows.Forms.ProgressBar
    {
        #region Fields

        private string _customText;
        private Color _textColor;
        private Font _textFont;
        private ProgressBarDisplayText _displayStyle;

        #endregion

        #region Properties

        //Property to set to decide whether to print a % or Text
        public ProgressBarDisplayText DisplayStyle
        {
            get { return _displayStyle; }
            set
            {
                _displayStyle = value;
                this.Invalidate();
            }
        }

        //Property to hold the custom text
        public String CustomText
        {
            get { return _customText; }
            set
            {
                _customText = value;
                this.Invalidate();
            }
        }

        public Color TextColor
        {
            get { return _textColor; }
            set
            {
                _textColor = value;
                this.Invalidate();
            }
        }

        public Font TextFont
        {
            get { return _textFont; }
            set
            {
                _textFont = value;
                this.Invalidate();
            }
        }

        #endregion

        #region Constructor

        public ProgressBar()
        {
            // Modify the ControlStyles flags
            //http://msdn.microsoft.com/en-us/library/system.windows.forms.controlstyles.aspx
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);

            TextColor = Color.Tan;
            TextFont = new System.Drawing.Font("Times New Roman", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Size = new Size(200, 40);
        }

        #endregion

        #region Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rect = ClientRectangle;
            using (Graphics g = e.Graphics)
            {
                ProgressBarRenderer.DrawHorizontalBar(g, rect);
                rect.Inflate(-3, -3);
                if (Value > 0)
                {
                    // As we doing this ourselves we need to draw the chunks on the progress bar
                    var clip = new Rectangle(rect.X, rect.Y, (int)Math.Round(((float)Value / Maximum) * rect.Width), rect.Height);
                    ProgressBarRenderer.DrawHorizontalChunks(g, clip);
                }

                // Set the Display text (Either a % amount or our custom text
                var text = DisplayStyle == ProgressBarDisplayText.Percentage ? Value.ToString(CultureInfo.InvariantCulture) + '%' : CustomText;


                SizeF len = g.MeasureString(text, TextFont);
                // Calculate the location of the text (the middle of progress bar)
                // Point location = new Point(Convert.ToInt32((rect.Width / 2) - (len.Width / 2)), Convert.ToInt32((rect.Height / 2) - (len.Height / 2)));
                var location = new Point(Convert.ToInt32((Width / 2) - len.Width / 2), Convert.ToInt32((Height / 2) - len.Height / 2));
                // The commented-out code will centre the text into the highlighted area only. This will centre the text regardless of the highlighted area.
                // Draw the custom text
                g.DrawString(text, TextFont, new SolidBrush(TextColor), location);
            }
        }

        #endregion
    }
}
