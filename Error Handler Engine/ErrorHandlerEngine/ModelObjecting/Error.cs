using System;

namespace ErrorHandlerEngine.ModelObjecting
{
    public class Error : IError, IDisposable, ICloneable, IEquatable<Error>
    {
        #region Properties

        public System.Drawing.Image Snapshot { get; set; }

        #endregion

        #region IError Implement

        public int Id { get; set; }
        public bool IsHandled { get; set; }
        public DateTime ErrorDateTime { get; set; }
        public DateTime ServerDateTime { get; set; }
        public int HResult { get; set; }
        public string AppName { get; set; }
        public string ClrVersion { get; set; }
        public string CurrentCulture { get; set; }
        public string ErrorType { get; set; }
        public string Host { get; set; }
        public string IPv4Address { get; set; }
        public string MacAddress { get; set; }
        public string MemberType { get; set; }
        public string Message { get; set; }
        public string Method { get; set; }
        public string ModuleName { get; set; }
        public string OS { get; set; }
        public string Processes { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
        public string User { get; set; }
        public CodeLocation LineColumn { get; set; }
        public int Duplicate { get; set; }
        #endregion

        #region IDisposable Implement
        public void Dispose()
        {
            Id = -1;
            AppName = String.Empty;
            ClrVersion = String.Empty;
            CurrentCulture = String.Empty;
            ErrorDateTime = DateTime.MinValue;
            ErrorType = String.Empty;
            Host = String.Empty;
            HResult = 0;
            IPv4Address = String.Empty;
            IsHandled = false;
            MacAddress = String.Empty;
            MemberType = String.Empty;
            Message = String.Empty;
            Method = String.Empty;
            ModuleName = String.Empty;
            OS = null;
            Processes = null;
            ServerDateTime = DateTime.MinValue;
            Snapshot.Dispose();
            Source = String.Empty;
            StackTrace = String.Empty;
            User = String.Empty;
            LineColumn = CodeLocation.Empty;
            Duplicate = 0;
        }
        #endregion

        #region ICloneable
        object ICloneable.Clone()
        {
            return Clone(false);
        }

        public object Clone(bool lightCopy = true)
        {
            return lightCopy ? GetLightCopy() : this.MemberwiseClone();
        }

        public IError GetLightCopy()
        {
            var instance = new Error();

            foreach (var property in typeof(IError).GetProperties())
            {
                typeof(Error).GetProperty(property.Name)
                    .SetValue(instance, typeof(Error).GetProperty(property.Name).GetValue(this));
            }

            return instance;
        }
        #endregion

        #region IEquatable<Error> Implement

        public bool Equals(Error other)
        {
            if (other == null) return false;

            // Note value types can't have derived classes, so we don't need 
            return this.LineColumn == other.LineColumn &&
                   this.HResult == other.HResult;
        }

        public bool Equals(Error x, Error y)
        {
            if (x == null) return false;
            if (y == null) return false;

            // Note value types can't have derived classes, so we don't need 
            return x.LineColumn == y.LineColumn &&
                   x.HResult == y.HResult;
        }

        /// <devdoc>
        ///    <para>
        ///       Specifies whether this <see cref='Error'/> contains
        ///       the same coordinates as the specified <see cref='System.Object'/>.
        ///    </para>
        /// </devdoc>
        public override bool Equals(object obj)
        {
            if (!(obj is Error)) return false;
            var comp = (Error)obj;
            // Note value types can't have derived classes, so we don't need 
            // to check the types of the objects here.  -- [....], 2/21/2001
            return comp.LineColumn == this.LineColumn &&
                   comp.HResult == this.HResult;
        }

        /// <devdoc>
        ///    <para>
        ///       Returns a hash code.
        ///    </para>
        /// </devdoc>
        public override int GetHashCode()
        {
            // Unique ID  =  Line×1000   XOR   Column   XOR   |HResult|
            return unchecked(this.LineColumn.Line * 1000 ^ this.LineColumn.Column ^ Math.Abs(this.HResult));
        }

        #endregion
    }
}
