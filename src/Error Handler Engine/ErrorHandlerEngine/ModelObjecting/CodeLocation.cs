using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;

namespace ModelObjecting
{
    [Serializable]
    [System.Runtime.InteropServices.ComVisible(true)]
    public struct CodeLocation
    {
        #region Properties

        /// <devdoc>
        ///    Creates a new instance of the <see cref='CodeLocation'/> class
        ///    with member data left uninitialized.
        /// </devdoc>
        public static readonly CodeLocation Empty = new CodeLocation();

        /// <devdoc>
        ///    Gets the line-coordinate of this <see cref='CodeLocation'/>.
        /// </devdoc>
        public int Line { get; set; }


        /// <devdoc>
        ///    <para>
        ///       Gets the column-coordinate of this <see cref='CodeLocation'/>.
        ///    </para>
        /// </devdoc>
        public int Column { get; set; }


        #endregion


        #region Constructors

        /// <devdoc>
        ///    Initializes a new instance of the <see cref='CodeLocation'/> class
        ///    with the specified coordinates.
        /// </devdoc>
        public CodeLocation(int line, int column)
            : this()
        {
            Line = line;
            Column = column;
        }

        /// <devdoc>
        ///    Initializes a new instance of the <see cref='CodeLocation'/> class
        ///    with the specified exception data to file code line and column from that stack trace.
        /// </devdoc>
        public CodeLocation(Exception exp)
            : this()
        {
            var frame = new StackTrace(exp, true).GetFrame(0);

            if (frame != null)
            {
                Line = frame.GetFileLineNumber();
                Column = frame.GetFileColumnNumber();
            }
        }

        #endregion


        #region Methods

        public static explicit operator CodeLocation(string value)
        {
            return Parse(value);
        }


        /// <devdoc>
        ///    <para>
        ///       Creates a <see cref='System.Drawing.Point'/> with the coordinates of the specified
        ///    <see cref='CodeLocation'/> 
        ///    .
        /// </para>
        /// </devdoc>
        public static implicit operator Point(CodeLocation p)
        {
            return new Point(p.Line, p.Column);
        }


        /// <devdoc>
        ///    <para>
        ///       Translates a <see cref='CodeLocation'/> by a given <see cref='System.Drawing.Point'/> .
        ///    </para>
        /// </devdoc>        
        public static CodeLocation operator +(CodeLocation cl, Point p)
        {
            return Add(cl, p);
        }

        /// <devdoc>
        ///    <para>
        ///       Translates a <see cref='CodeLocation'/> by the negative of a given <see cref='System.Drawing.Point'/> .
        ///    </para>
        /// </devdoc>        
        public static CodeLocation operator -(CodeLocation cl, Point p)
        {
            return Subtract(cl, p);
        }

        /// <devdoc>
        ///    <para>
        ///       Compares two <see cref='CodeLocation'/> objects. The result specifies
        ///       whether the values of the <see cref='CodeLocation.Line'/> and <see cref='CodeLocation.Column'/> properties of the two <see cref='CodeLocation'/>
        ///       objects are equal.
        ///    </para>
        /// </devdoc>
        public static bool operator ==(CodeLocation left, CodeLocation right)
        {
            return left.Line == right.Line && left.Column == right.Column;
        }

        /// <devdoc>
        ///    <para>
        ///       Compares two <see cref='CodeLocation'/> objects. The result specifies whether the values
        ///       of the <see cref='CodeLocation.Line'/> or <see cref='CodeLocation.Column'/> properties of the two
        ///    <see cref='CodeLocation'/> 
        ///    objects are unequal.
        /// </para>
        /// </devdoc>
        public static bool operator !=(CodeLocation left, CodeLocation right)
        {
            return !(left == right);
        }

        /// <devdoc>
        ///    <para>
        ///       Translates a <see cref='CodeLocation'/> by a given <see cref='System.Drawing.Point'/> .
        ///    </para>
        /// </devdoc>        
        public static CodeLocation Add(CodeLocation cl, Point p)
        {
            return new CodeLocation(cl.Line + p.X, cl.Column + p.Y);
        }

        /// <devdoc>
        ///    <para>
        ///       Translates a <see cref='CodeLocation'/> by the negative of a given <see cref='System.Drawing.Point'/> .
        ///    </para>
        /// </devdoc>        
        public static CodeLocation Subtract(CodeLocation cl, Point p)
        {
            return new CodeLocation(cl.Line - p.X, cl.Column - p.Y);
        }

        /// <devdoc>
        ///    <para>
        ///       Specifies whether this <see cref='CodeLocation'/> contains
        ///       the same coordinates as the specified <see cref='System.Object'/>.
        ///    </para>
        /// </devdoc>
        public override bool Equals(object obj)
        {
            if (!(obj is CodeLocation)) return false;
            var comp = (CodeLocation)obj;
            // Note value types can't have derived classes, so we don't need 
            // to check the types of the objects here.  -- [....], 2/21/2001
            return comp.Line == Line && comp.Column == Column;
        }

        /// <devdoc>
        ///    <para>
        ///       Returns a hash code.
        ///    </para>
        /// </devdoc>
        public override int GetHashCode()
        {
            return unchecked((Line * 1000) ^ Column);
        }

        /// <devdoc>
        ///    <para>
        ///       Converts this <see cref='CodeLocation'/>
        ///       to a human readable
        ///       string.
        ///    </para>
        /// </devdoc>
        public override string ToString()
        {
            return "{Line:" + Line.ToString(CultureInfo.CurrentCulture) + ",Column:" + Column.ToString(CultureInfo.CurrentCulture) + "}";
        }

        /// <devdoc>
        ///    <para>
        ///       Converts a human readable
        ///       string to this <see cref='CodeLocation'/>
        ///    </para>
        /// </devdoc>
        public static CodeLocation Parse(string value)
        {
            //
            // Example: {Line:177, Column:39}
            //
            var indexOfLine = value.IndexOf("{Line:", StringComparison.Ordinal);
            if (indexOfLine < 0) throw new InvalidCastException("Can't to find Line");
            indexOfLine += 6; // index at first of Line No. After ':'

            var indexOfLineNoLast = value.IndexOf(",", StringComparison.Ordinal); // index at last of Line No. Before ','
            if (indexOfLineNoLast < 0) throw new InvalidCastException("Can't to find Line");

            var strline = value.Substring(indexOfLine, indexOfLineNoLast - indexOfLine);

            //-----------------------------------------------------------------------------------------

            var indexOfCol = value.IndexOf("Column:", StringComparison.Ordinal);
            if (indexOfCol < 0) throw new InvalidCastException("Can't to find Column");
            indexOfCol += 7; // index at first of Column No. After ':'

            var indexOfColNoLast = value.IndexOf("}", StringComparison.Ordinal); // index at last of Column No. Before '}'
            if (indexOfColNoLast < 0) throw new InvalidCastException("Can't to find Column");

            var strCol = value.Substring(indexOfCol, indexOfColNoLast - indexOfCol);

            // -------------------------------------------------------------------------------------------

            var line = int.Parse(strline);
            var col = int.Parse(strCol);

            return new CodeLocation(line, col);
        }

        /// <devdoc>
        ///    <para>
        ///       Try to Converts a human readable
        ///       string to this <see cref='CodeLocation'/>
        ///    </para>
        /// </devdoc>
        public static bool TryParse(string value, out CodeLocation cl)
        {
            //
            // Example: {Line:177, Column:39}
            //
            cl = new CodeLocation();
            try
            {
                var indexOfLine = value.IndexOf("{Line:", StringComparison.Ordinal);
                if (indexOfLine < 0) throw new InvalidCastException("Can't to find Line");
                indexOfLine += 6; // index at first of Line No. After ':'

                var indexOfLineNoLast = value.IndexOf(",", StringComparison.Ordinal); // index at last of Line No. Before ','
                if (indexOfLineNoLast < 0) throw new InvalidCastException("Can't to find Line");

                var strline = value.Substring(indexOfLine, indexOfLineNoLast - indexOfLine);

                //-----------------------------------------------------------------------------------------

                var indexOfCol = value.IndexOf("Column:", StringComparison.Ordinal);
                if (indexOfCol < 0) throw new InvalidCastException("Can't to find Column");
                indexOfCol += 7; // index at first of Column No. After ':'

                var indexOfColNoLast = value.IndexOf("}", StringComparison.Ordinal); // index at last of Column No. Before '}'
                if (indexOfColNoLast < 0) throw new InvalidCastException("Can't to find Column");

                var strCol = value.Substring(indexOfCol, indexOfColNoLast - indexOfCol);
                // -------------------------------------------------------------------------------------------

                int line;
                if (!int.TryParse(strline, out line)) return false;

                int col;
                if (!int.TryParse(strCol, out col)) return false;

                cl = new CodeLocation(line, col);
            }
            catch (Exception)
            {
                return false;
            }


            return true;
        }

        #endregion
    }
}
