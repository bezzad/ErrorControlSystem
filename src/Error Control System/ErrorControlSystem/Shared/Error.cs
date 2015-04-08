using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using ErrorControlSystem.CachErrors;
using ErrorControlSystem.DbConnectionManager;

namespace ErrorControlSystem.Shared
{
    public class Error : IError, IDisposable, ICloneable
    {
        #region Properties

        public System.Drawing.Image Snapshot { get; set; }

        /// <summary>
        /// Dictionary of key/value data that will be stored in exceptions as additional data.
        /// </summary>
        internal static Dictionary<string, string> DicExtraData = new Dictionary<string, string>();

        #endregion

        #region Constructors

        /// <summary>
        /// Get handled exception's by additional data.
        /// </summary>
        /// <param name="exp">>The occurrence raw error.</param>
        /// <param name="frames">The array of <see cref="System.Diagnostics.StackFrame"/> to changes by exception stackTrace</param>
        /// <param name="option">What preprocess must be doing on that exception's ?</param>
        public Error(Exception exp, StackFrame[] frames = null, ErrorHandlingOptions option = ErrorHandlingOptions.Default)
        {
            #region HResult [Exception Type Code]

            HResult = exp.HResult;

            #endregion

            #region Error Line Column

            LineColumn = new CodeScope(exp);

            #endregion

            #region Method

            Method = (exp.TargetSite != null && exp.TargetSite.ReflectedType != null) ?
                    exp.TargetSite.ReflectedType.FullName + "." + exp.TargetSite : "";

            #endregion

            #region Id = HashCode
            Id = GetHashCode();
            #endregion

            #region Screen Capture

            // First initialize Snapshot of Error, because that's speed is important!
            if (!SqlCompactEditionManager.Contains(Id) && option.HasFlag(ErrorHandlingOptions.Snapshot))
            {
                Snapshot = ScreenCapture.Capture();

                if (Snapshot != null && option.HasFlag(ErrorHandlingOptions.ReSizeSnapshots))
                    Snapshot.ResizeImage(ScreenCapture.ReSizeAspectRatio.Width, ScreenCapture.ReSizeAspectRatio.Height);
            }

            #endregion

            #region StackTrace

            StackTrace = CodeScope.StackFramesToString(frames) ??
                (exp.InnerException != null
                ? exp.InnerException.StackTrace ?? ""
                : exp.StackTrace ?? "");

            #endregion

            #region Error Date Time

            ErrorDateTime = DateTime.Now;

            #endregion

            #region Server Date Time

            ServerDateTime = option.HasFlag(ErrorHandlingOptions.FetchServerDateTime)
                ? NetworkHelper.GetServerDateTime()
                : DateTime.Now;

            #endregion

            #region Current Culture

            CurrentCulture = String.Format("{0} ({1})",
                    InputLanguage.CurrentInputLanguage.Culture.NativeName,
                    InputLanguage.CurrentInputLanguage.Culture.Name);

            #endregion

            #region Message

            Message = exp.Message;

            #endregion

            #region Member Type

            MemberType = (exp.TargetSite != null)
                    ? exp.TargetSite.MemberType.ToString()
                    : "";

            #endregion

            #region Module Name

            ModuleName =
                    (exp.TargetSite != null) ? exp.TargetSite.Module.Name : "";

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

            AppName = Connection.GetRunningAppNameVersion();

            #endregion

            #region Process Name String List

            Processes = new CurrentProcesses().ToString();

            #endregion

            #region Is Handled Error or UnHandled?

            IsHandled = option.HasFlag(ErrorHandlingOptions.IsHandled);

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

            ErrorType = exp.GetType().Name;

            #endregion

            #region Source

            Source = exp.Source;

            #endregion

            #region Data

            Data = DictionaryToXml(GetAdditionalData(exp), "ExtraProperties");

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
        public CodeScope LineColumn { get; set; }
        public int Duplicate { get; set; }
        public string Data { get; set; }
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
            LineColumn = CodeScope.Empty;
            Duplicate = 0;
            Data = string.Empty;
        }
        #endregion

        #region ICloneable
        object ICloneable.Clone()
        {
            return Clone(false);
        }

        public object Clone(bool lightCopy)
        {
            return lightCopy ? GetLightCopy() : this.MemberwiseClone();
        }

        public IError GetLightCopy()
        {
            var instance = new Error();

            foreach (var property in typeof(IError).GetProperties())
            {
                if (property.CanRead && property.CanWrite)
                    typeof(Error).GetProperty(property.Name)
                        .SetValue(instance, typeof(Error).GetProperty(property.Name).GetValue(this));
            }

            return instance;
        }
        #endregion

        #region IEquatable<IError> Implement

        public bool Equals(IError other)
        {
            if (other == null) return false;

            // Note value types can't have derived classes, so we don't need 
            return Id == other.Id;
        }

        public bool Equals(IError x, IError y)
        {
            // Note: value types can't have derived classes, so we don't need 
            return x != null && y != null && x.Equals(y);
        }

        /// <devdoc>
        ///    <para>
        ///       Specifies whether this <see cref='IError'/> contains
        ///       the same coordinates as the specified <see cref='System.Object'/>.
        ///    </para>
        /// </devdoc>
        public override bool Equals(object obj)
        {
            if (!(obj is IError)) return false;
            var comp = (IError)obj;
            // Note: value types can't have derived classes, so we don't need 
            // to check the types of the objects here.  -- [....], 2/21/2001
            return Equals(comp);
        }

        /// <devdoc>
        ///    <para>
        ///       Returns a hash code.
        ///    </para>
        /// </devdoc>
        public override sealed int GetHashCode()
        {
            // Unique ID  =  Line×1000   XOR   Column   XOR   |HResult|
            return unchecked(this.LineColumn.Line * 1000 ^ this.LineColumn.Column ^ Math.Abs(this.HResult) ^ Method.GetHashCode());
        }

        #endregion

        #region Methods

        protected Dictionary<string, object> GetAdditionalData(Exception exp)
        {
            // Read any declaring properties from custom exception object
            var data =
                (from property in exp.GetType().GetProperties()
                 where property.DeclaringType != null &&
                       property.DeclaringType != typeof(Exception) &&
                       property.DeclaringType == exp.GetType()
                 select property).ToDictionary(p => p.Name, p => p.GetValue(exp));
            //
            // Read Data dictionary of exception object
            foreach (DictionaryEntry item in exp.Data.Cast<DictionaryEntry>().Where(item => item.Key != null && item.Value != null))
            {
                data.Add(item.Key.ToString(), item.Value.ToString());
            }
            //
            // Read DicExtraData dictionary from global labeling data
            foreach (var item in DicExtraData.Where(item => item.Key != null && item.Value != null))
            {
                data.Add(item.Key, item.Value);
            }

            return data;
        }


        protected string DictionaryToXml(Dictionary<string, object> data, string rootName)
        {
            var root = new XElement(rootName);
            foreach (var pair in data)
            {
                root.Add(new XElement(pair.Key.Trim().Replace(' ', '_'), pair.Value));
            }

            return root.ToString(SaveOptions.None);
        }

        #endregion

    }
}
