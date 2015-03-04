using System;
using System.Windows.Forms;
using ErrorHandlerEngine.ExceptionManager;

namespace ErrorHandlerEngine.ModelObjecting
{
    public class Error : IError, IDisposable, ICloneable, IEquatable<Error>
    {
        #region Properties

        public System.Drawing.Image Snapshot { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Get handled exception's by additional data.
        /// </summary>
        /// <param name="exception">>The occurrence raw error.</param>
        /// <param name="option">What preprocess must be doing on that exception's ?</param>
        public Error(Exception exception, ExceptionHandlerOption option = ExceptionHandlerOption.Default)
        {
            #region HResult [Exception Type Code]

            HResult = exception.HResult;

            #endregion

            #region Error Line Column

            LineColumn = new CodeLocation(exception);

            #endregion

            #region Id = HashCode
            Id = GetHashCode();
            #endregion

            #region Screen Capture

            // First initialize Snapshot of Error, because that's speed is important!
            if (/*!SdfFileManager.Contains(Id) && */ option.HasFlag(ExceptionHandlerOption.Snapshot))
            {
                Snapshot = option.HasFlag(ExceptionHandlerOption.ReSizeSnapshots)
                        ? ScreenCapture.Capture().ResizeImage(ScreenCapture.ReSizeAspectRatio.Width, ScreenCapture.ReSizeAspectRatio.Height)
                        : ScreenCapture.Capture();
            }

            #endregion

            #region StackTrace

            StackTrace = exception.InnerException != null
                ? exception.InnerException.StackTrace ?? ""
                : exception.StackTrace ?? "";

            #endregion

            #region Error Date Time

            ErrorDateTime = DateTime.Now;

            #endregion

            #region Server Date Time

            ServerDateTime = option.HasFlag(ExceptionHandlerOption.FetchServerDateTime)
                ? NetworkHelper.GetServerDateTime()
                : DateTime.Now;

            #endregion

            #region Current Culture

            CurrentCulture = String.Format("{0} ({1})",
                    InputLanguage.CurrentInputLanguage.Culture.NativeName,
                    InputLanguage.CurrentInputLanguage.Culture.Name);

            #endregion

            #region Message

            Message = exception.Message;

            #endregion

            #region Method

            Method = (exception.TargetSite != null && exception.TargetSite.ReflectedType != null) ?
                    exception.TargetSite.ReflectedType.FullName + "." + exception.TargetSite : "";

            #endregion

            #region Member Type

            MemberType = (exception.TargetSite != null)
                    ? exception.TargetSite.MemberType.ToString()
                    : "";

            #endregion

            #region Module Name

            ModuleName =
                    (exception.TargetSite != null) ? exception.TargetSite.Module.Name : "";

            #endregion

            #region User [Domain.UserName]

            User = Environment.UserDomainName + "\\" + Environment.UserName;

            #endregion

            #region Host [Machine Name]

            Host = Environment.MachineName;

            #endregion

            #region Operation System Information

            OS = new OperationSystemInfo(true).ToString();

            #endregion

            #region Application Name [Name  v#####]

            AppName = String.Format("{0}  v{1}",
                    AppDomain.CurrentDomain.FriendlyName.Replace(".vshost", ""),
                    Application.ProductVersion);

            #endregion

            #region Process Name String List

            Processes = new CurrentProcesses().ToString();

            #endregion

            #region Is Handled Error or UnHandled?

            IsHandled = option.HasFlag(ExceptionHandlerOption.IsHandled);

            #endregion

            #region Current Static Valid IPv4 Address

            IPv4Address = NetworkHelper.GetIpAddress();

            #endregion

            #region Network Physical Address [MAC HEX]

            MacAddress = NetworkHelper.GetMacAddress();

            #endregion

            #region Common Language Runtime Version [Major.Minor.Build.Revison]

            ClrVersion = Environment.Version.ToString();

            #endregion

            #region Error Type

            ErrorType = exception.GetType().Name;

            #endregion

            #region Source

            Source = exception.Source;

            #endregion
        }


        public Error() { }

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
