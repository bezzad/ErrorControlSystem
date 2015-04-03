using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ErrorControlSystem.Shared
{
    public class CodeScope : IEquatable<CodeScope>
    {
        #region Properties

        /// <devdoc>
        ///    Creates a new instance of the <see cref='CodeScope'/> class
        ///    with member data left uninitialized.
        /// </devdoc>
        public static readonly CodeScope Empty = new CodeScope();

        public String Namespace = String.Empty;
        public String Class = String.Empty;
        public String Method = String.Empty;
        public String FilePath = String.Empty;

        /// <devdoc>
        ///    Gets the line-coordinate of this <see cref='CodeScope'/>.
        /// </devdoc>
        public int Line = 0;

        /// <devdoc>
        ///    <para>
        ///       Gets the column-coordinate of this <see cref='CodeScope'/>.
        ///    </para>
        /// </devdoc>
        public int Column = 0;

        #endregion

        #region Constructors

        protected CodeScope()
        {
            Namespace = String.Empty;
            Class = String.Empty;
            Method = String.Empty;
            FilePath = String.Empty;
            Line = 0;
            Column = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeScope"/> class
        /// with the specified exception data to file code line and column from that stack trace.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="methodName">Name of the method.</param>
        public CodeScope(string assemblyName, string className, string methodName)
        {
            this.Namespace = assemblyName;
            this.Class = className;
            this.Method = methodName;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CodeScope"/> class
        /// with the specified exception data to file code line and column from that stack trace.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="lineNo">The line no.</param>
        /// <param name="columnNo">The column no.</param>
        public CodeScope(string assemblyName, string className, string methodName, string filePath, int lineNo, int columnNo)
            : this(assemblyName, className, methodName)
        {
            FilePath = filePath;
            Line = lineNo;
            Column = columnNo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeScope"/> class
        /// with the specified exception data to file code line and column from that stack trace.
        /// </summary>
        /// <param name="frame">The stack frame to fetch assembly, class, method and etc names from that's.</param>
        public CodeScope(StackFrame frame)
        {
            if (frame == null) return;

            var sb = new System.Text.StringBuilder();
            MethodBase mb = frame.GetMethod();
            if (mb != null)
            {
                Type t = mb.DeclaringType;
                // if there is a type (non global method) print it
                if (t != null)
                {
                    this.Namespace = t.Namespace;
                    this.Class = t.Name;
                }

                sb.Append(mb.Name);

                // deal with the generic portion of the method
                if (mb is MethodInfo && mb.IsGenericMethod)
                {
                    Type[] typars = mb.GetGenericArguments();
                    sb.Append("[");
                    int k = 0;
                    bool fFirstTyParam = true;
                    while (k < typars.Length)
                    {
                        if (fFirstTyParam == false)
                            sb.Append(",");
                        else
                            fFirstTyParam = false;

                        sb.Append(typars[k].Name);
                        k++;
                    }
                    sb.Append("]");
                }

                // arguments printing
                sb.Append("(");
                ParameterInfo[] pi = mb.GetParameters();
                bool fFirstParam = true;
                for (int j = 0; j < pi.Length; j++)
                {
                    if (fFirstParam == false)
                        sb.Append(", ");
                    else
                        fFirstParam = false;

                    String typeName = "<UnknownType>";
                    if (pi[j].ParameterType != null)
                        typeName = pi[j].ParameterType.Name;
                    sb.Append(typeName + " " + pi[j].Name);
                }
                sb.Append(")");

                this.Method = sb.ToString();

                // source location printing
                if ((frame.GetILOffset() != -1))
                {
                    // If we don't have a PDB or PDB-reading is disabled for the module,
                    // then the file name will be null.
                    String fileName = null;

                    // Getting the filename from a StackFrame is a privileged operation - we won't want
                    // to disclose full path names to arbitrarily untrusted code.  Rather than just omit
                    // this we could probably trim to just the filename so it's still mostly usefull.
                    try
                    {
                        fileName = frame.GetFileName();
                    }
                    catch
                    {
                        // If the demand for displaying filenames fails, then it won't
                        // succeed later in the loop.  Avoid repeated exceptions by not trying again.
                        // displayFilenames = false;
                    }

                    if (fileName != null)
                    {
                        this.FilePath = fileName;
                        this.Line = frame.GetFileLineNumber();
                        this.Column = frame.GetFileColumnNumber();
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeScope"/> class
        /// with the specified exception data to file code line and column from that stack trace.
        /// </summary>
        /// <param name="exp">The <see cref="System.Exception"/> to fetch assembly, class, method and etc names from that's stack trace.</param>
        public CodeScope(Exception exp)
            : this(new StackTrace(exp, true).GetFrame(0))
        { }

        #endregion

        #region Methods

        /// <summary>
        /// Is call method from this code place.
        /// </summary>
        /// <param name="frames">The frames of call methods or exception stackTrace frames.</param>
        /// <returns><see cref="Boolean"/></returns>
        public bool IsCallFromThisPlace(IEnumerable<StackFrame> frames)
        {
            if (frames == null || !frames.Any()) return false;

            if (!string.IsNullOrEmpty(Namespace))
            {
                frames = frames.Where(x =>
                            Path.GetFileNameWithoutExtension(x.GetMethod().Module.Name)
                                .Equals(Namespace, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(Class))
            {
                frames = frames.Where(x =>
                            x.GetMethod().ReflectedType != null &&
                            x.GetMethod().ReflectedType.Name.Equals(Class, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(Method))
            {
                frames = frames.Where(x =>
                           x.GetMethod().Name.Equals(Method, StringComparison.OrdinalIgnoreCase));
            }

            return frames.Any();
        }


        /// <devdoc>
        ///    <para>
        ///       Specifies whether this <see cref='CodeScope'/> contains
        ///       the same coordinates as the specified <see cref='System.Object'/>.
        ///    </para>
        /// </devdoc>
        public override bool Equals(object obj)
        {
            if (!(obj is CodeScope)) return false;

            var comp = (CodeScope)obj;
            // Note value types can't have derived classes, so we don't need 
            // to check the types of the objects here.  -- [....], 2/21/2001
            return comp == this;
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
        ///       Converts this <see cref='CodeScope'/>
        ///       to a human readable
        ///       string.
        ///    </para>
        /// </devdoc>
        /// <param name="justLineColumn">Return just Line and Column value ?</param>
        public string ToString(bool justLineColumn)
        {
            return justLineColumn
                ? "{Line:" + Line.ToString(CultureInfo.CurrentCulture) + ",Column:" +
                        Column.ToString(CultureInfo.CurrentCulture) + "}"
                : ToString();
        }

        /// <devdoc>
        ///    <para>
        ///       Converts this <see cref='CodeScope'/>
        ///       to a human readable
        ///       string.
        ///    </para>
        /// </devdoc>
        public override string ToString()
        {
            return string.Format("{2}{3}{4}{5}{0}Line:{6},Column:{7}{1}",
                "{",
                "}",
                string.IsNullOrEmpty(Namespace) ? "" : "N{" + Namespace + "}.",
                string.IsNullOrEmpty(Class) ? "" : "C{" + Class + "}.",
                string.IsNullOrEmpty(Method) ? "" : "M{" + Method + "} ",
                string.IsNullOrEmpty(FilePath) ? "" : "@{" + FilePath + "}:",
                Line,
                Column
                );
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Parses the specified frame.
        /// </summary>
        /// <param name="frame">The stack frame to fetch assembly, class, method and etc names from that's.</param>
        /// <returns>Instance of the <see cref="CodeScope"/></returns>
        public static CodeScope Parse(StackFrame frame)
        {
            return new CodeScope(frame);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="StackFrame"/> to <see cref="CodeScope"/>.
        /// </summary>
        /// <param name="frame">The stack frame to fetch assembly, class, method and etc names from that's.</param>
        /// <returns>
        /// The result of the conversion is a instance of the <see cref="CodeScope"/>
        /// </returns>
        public static explicit operator CodeScope(StackFrame frame)
        {
            return Parse(frame);
        }

        /// <devdoc>
        ///    <para>
        ///       Compares two <see cref='CodeScope'/> objects. The result specifies
        ///       whether the values of the <see cref='CodeScope.Line'/> and 
        ///       <see cref='CodeScope.Column'/> and <see cref='CodeScope.Namespace'/> and
        ///       <see cref='CodeScope.Class'/> and <see cref='CodeScope.Method'/> properties of the two <see cref='CodeScope'/>
        ///       objects are equal.
        ///    </para>
        /// </devdoc>
        public static bool operator ==(CodeScope left, CodeScope right)
        {
            return left.Line == right.Line && left.Column == right.Column &&
                   left.Namespace == right.Namespace && left.Class == right.Class && left.Method == right.Method;
        }

        /// <devdoc>
        ///    <para>
        ///       Compares two <see cref='CodeScope'/> objects. The result specifies whether the values
        ///       of the <see cref='CodeScope.Line'/> or 
        ///       <see cref='CodeScope.Column'/> or <see cref='CodeScope.Namespace'/> or
        ///       <see cref='CodeScope.Class'/> or <see cref='CodeScope.Method'/> properties of the two
        ///    <see cref='CodeScope'/> 
        ///    objects are unequal.
        /// </para>
        /// </devdoc>
        public static bool operator !=(CodeScope left, CodeScope right)
        {
            return !(left == right);
        }

        /// <devdoc>
        ///    <para>
        ///       Converts a human readable
        ///       string to this <see cref='CodeScope'/>
        ///    </para>
        /// </devdoc>
        public static CodeScope Parse(string value)
        {
            CodeScope buffer;
            TryParse(value, out buffer);
            return buffer;
        }

        /// <devdoc>
        ///    <para>
        ///       Try to Converts a human readable
        ///       string to this <see cref='CodeScope'/>
        ///    </para>
        /// </devdoc>
        public static bool TryParse(string value, out CodeScope cl)
        {
            //
            // Example: {Line:177, Column:39}
            //
            cl = new CodeScope();
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

                cl = new CodeScope(null, null, null, null, line, col);
            }
            catch (Exception)
            {
                return false;
            }


            return true;
        }


        /// <summary>
        /// Convert stacks the frames to string.
        /// </summary>
        /// <devdoc>
        ///    <para>
        /// Reference: http://referencesource.microsoft.com/mscorlib/system/diagnostics/stacktrace.cs.html#f4bbb1fa2df8ea1f
        ///    </para>
        /// </devdoc>
        /// <param name="frames">The array of <see cref="System.Diagnostics.StackFrame"/></param>
        /// <returns><see cref="String"/></returns>
        public static string StackFramesToString(StackFrame[] frames)
        {
            bool displayFilenames = true;   // we'll try, but demand may fail
            String word_At = "->";
            String inFileLineColNum = "@{0}{2}{1}:{0}Line:{3},Column:{4}{1}";

            bool fFirstFrame = true;
            StringBuilder sb = new StringBuilder();
            for (int iFrameIndex = 0; iFrameIndex < frames.Count(); iFrameIndex++)
            {
                StackFrame sf = frames[iFrameIndex];
                MethodBase mb = sf.GetMethod();
                if (mb != null)
                {
                    // We want a newline at the end of every line except for the last
                    if (fFirstFrame)
                        fFirstFrame = false;
                    else
                        sb.Append(Environment.NewLine);

                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0} ", word_At);

                    Type t = mb.DeclaringType;
                    // if there is a type (non global method) print it
                    if (t != null)
                    {
                        if (t.Namespace != null)
                        {
                            sb.Append("N{" + t.Namespace.Replace('+', '.') + "}.");
                            sb.Append("C{" + t.Name + "}");
                        }
                        else
                            sb.Append(t.FullName.Replace('+', '.'));

                        sb.Append(".");
                    }

                    sb.Append("M{"); // Begin Method ---------------------
                    sb.Append(mb.Name);

                    // deal with the generic portion of the method
                    if (mb is MethodInfo && mb.IsGenericMethod)
                    {
                        Type[] typars = mb.GetGenericArguments();
                        sb.Append("[");
                        int k = 0;
                        bool fFirstTyParam = true;
                        while (k < typars.Length)
                        {
                            if (fFirstTyParam == false)
                                sb.Append(",");
                            else
                                fFirstTyParam = false;

                            sb.Append(typars[k].Name);
                            k++;
                        }
                        sb.Append("]");
                    }

                    // arguments printing
                    sb.Append("(");
                    ParameterInfo[] pi = mb.GetParameters();
                    bool fFirstParam = true;
                    for (int j = 0; j < pi.Length; j++)
                    {
                        if (fFirstParam == false)
                            sb.Append(", ");
                        else
                            fFirstParam = false;

                        String typeName = "<UnknownType>";
                        if (pi[j].ParameterType != null)
                            typeName = pi[j].ParameterType.Name;
                        sb.Append(typeName + " " + pi[j].Name);
                    }
                    sb.Append(")");
                    sb.Append("}"); // End Method ---------------------

                    // source location printing
                    if (displayFilenames && (sf.GetILOffset() != -1))
                    {
                        // If we don't have a PDB or PDB-reading is disabled for the module,
                        // then the file name will be null.
                        String fileName = null;

                        // Getting the filename from a StackFrame is a privileged operation - we won't want
                        // to disclose full path names to arbitrarily untrusted code.  Rather than just omit
                        // this we could probably trim to just the filename so it's still mostly usefull.
                        try
                        {
                            fileName = sf.GetFileName();
                        }
                        catch
                        {
                            // If the demand for displaying filenames fails, then it won't
                            // succeed later in the loop.  Avoid repeated exceptions by not trying again.
                            displayFilenames = false;
                        }

                        if (fileName != null)
                        {
                            // tack on " @{d:\Programming\temp\TestStackTrace\Form1.cs}:{Line:66,Column:13}"

                            sb.Append(' ');
                            sb.AppendFormat(CultureInfo.InvariantCulture, inFileLineColNum, "{", "}", fileName, sf.GetFileLineNumber(), sf.GetFileColumnNumber());
                        }
                    }
                }
            }

            return sb.ToString();
        }

        #endregion

        #region Implement IEquatable<CodeScope>

        /// <devdoc>
        ///    <para>
        ///       Specifies whether this <see cref='CodeScope'/> contains
        ///       the same coordinates as the specified <see cref='CodeScope'/>.
        ///    </para>
        /// </devdoc>
        public bool Equals(CodeScope other)
        {
            return other == this;
        }

        #endregion

    }
}
