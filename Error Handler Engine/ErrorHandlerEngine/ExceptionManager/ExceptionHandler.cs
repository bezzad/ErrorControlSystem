//**********************************************************************************//
//                           LICENSE INFORMATION                                    //
//**********************************************************************************//
//   Error Handler Engine 1.0.0.2                                                   //
//   This Class Library creates a way of handling structured exception handling,     //
//   inheriting from the Exception class gives us access to many method             //
//   we wouldn't otherwise have access to                                           //
//                                                                                  //
//   Copyright (C) 2015                                                             //
//   Behzad Khosravifar                                                             //
//   Email: Behzad.Khosravifar@Gmail.com                                            //
//                                                                                  //
//   This program published by the Free Software Foundation,                        //
//   either version 2 of the License, or (at your option) any later version.        //
//                                                                                  //
//   This program is distributed in the hope that it will be useful,                //
//   but WITHOUT ANY WARRANTY; without even the implied warranty of                 //
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.                           //
//                                                                                  //
//**********************************************************************************//

using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using ErrorHandlerEngine.CacheHandledErrors;
using ErrorHandlerEngine.ModelObjecting;
using ErrorHandlerEngine.ServerUploader;

namespace ErrorHandlerEngine.ExceptionManager
{
    /// <summary>
    /// Additional Data attached to exception object.
    /// </summary>
    public static class ExceptionHandler
    {
        #region Properties

        public static Size ScreenShotReSizeAspectRatio = new Size(1024, 768); // set to aspect 1024×768

        public static volatile bool IsSelfException = false;

        #endregion


        #region Methods

        /// <summary>
        /// Raise log of handled error's.
        /// </summary>
        /// <param name="exp">The Error object.</param>
        /// <param name="option">The option to select what jobs must be doing and stored in Error object's.</param>
        /// <param name="errorTitle">Determine the mode of occurrence of an error in the program.</param>
        /// <returns></returns>
        public static Error RaiseLog(this Exception exp, ExceptionHandlerOption option = ExceptionHandlerOption.Default, String errorTitle = "UnHandled Exception")
        {
            if (IsSelfException)
            {
                IsSelfException = false;
                return null;
            }

            // initial the error object by additional data 
            var error = exp.CreateError(option);

            if (option.HasFlag(ExceptionHandlerOption.AlertUnHandledError) && !option.HasFlag(ExceptionHandlerOption.IsHandled)) // Alert Unhandled Error 
            {
                MessageBox.Show(exp.Message,
                    errorTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }

            CacheController.ErrorSaverActionBlock.Post(error);

            return error;
        }

        /// <summary>
        /// Get handled exception's by additional data.
        /// </summary>
        /// <param name="exception">The occurrence raw error.</param>
        /// <param name="option">What preprocess must be doing on that exception's ?</param>
        /// <returns>The ripe error object's.</returns>
        private static Error CreateError(this Exception exception, ExceptionHandlerOption option = ExceptionHandlerOption.Default)
        {
            // Create Empty Error object
            var error = new Error();

            #region Initialize Exception Additional Data

            #region HResult [Exception Type Code]

            error.HResult = exception.HResult;

            #endregion

            #region Error Line Column

            error.LineColumn = new CodeLocation(exception);

            #endregion

            #region Id = HashCode
            error.Id = error.GetHashCode();
            #endregion

            #region Screen Capture

            // First initialize Snapshot of Error, because that's speed is important!
            if (!SdfFileManager.Contains(error.Id) && option.HasFlag(ExceptionHandlerOption.Snapshot))
            {
                error.Snapshot = option.HasFlag(ExceptionHandlerOption.ReSizeSnapshots)
                        ? ScreenCapture.Capture().ResizeImage(ScreenShotReSizeAspectRatio.Width, ScreenShotReSizeAspectRatio.Height)
                        : ScreenCapture.Capture();
            }

            #endregion

            #region StackTrace

            error.StackTrace = exception.InnerException != null
                ? exception.InnerException.StackTrace ?? ""
                : exception.StackTrace ?? "";

            #endregion

            #region Error Date Time

            error.ErrorDateTime = DateTime.Now;

            #endregion

            #region Server Date Time

            error.ServerDateTime = option.HasFlag(ExceptionHandlerOption.FetchServerDateTime)
                ? NetworkHelper.GetServerDateTime()
                : DateTime.Now;

            #endregion

            #region Current Culture

            error.CurrentCulture = String.Format("{0} ({1})",
                    CultureInfo.CurrentCulture.NativeName,
                    CultureInfo.CurrentCulture.Name);

            #endregion

            #region Message

            error.Message = exception.Message;

            #endregion

            #region Method

            error.Method = (exception.TargetSite != null && exception.TargetSite.ReflectedType != null) ?
                    exception.TargetSite.ReflectedType.FullName + "." + exception.TargetSite : "";

            #endregion

            #region Member Type

            error.MemberType = (exception.TargetSite != null)
                    ? exception.TargetSite.MemberType.ToString()
                    : "";

            #endregion

            #region Module Name

            error.ModuleName =
                    (exception.TargetSite != null) ? exception.TargetSite.Module.Name : "";

            #endregion

            #region User [Domain.UserName]

            error.User = Environment.UserDomainName + "\\" + Environment.UserName;

            #endregion

            #region Host [Machine Name]

            error.Host = Environment.MachineName;

            #endregion

            #region Operation System Information

            error.OS = new OperationSystemInfo(true).ToString();

            #endregion

            #region Application Name [Name  v#####]

            error.AppName = String.Format("{0}  v{1}",
                    AppDomain.CurrentDomain.FriendlyName.Replace(".vshost", ""),
                    Application.ProductVersion);

            #endregion

            #region Process Name String List

            error.Processes = new CurrentProcesses().ToString();

            #endregion

            #region Is Handled Error or UnHandled?

            error.IsHandled = option.HasFlag(ExceptionHandlerOption.IsHandled);

            #endregion

            #region Current Static Valid IPv4 Address

            error.IPv4Address = NetworkHelper.GetIpAddress();

            #endregion

            #region Network Physical Address [MAC HEX]

            error.MacAddress = NetworkHelper.GetMacAddress();

            #endregion

            #region Common Language Runtime Version [Major.Minor.Build.Revison]

            error.ClrVersion = Environment.Version.ToString();

            #endregion

            #region Error Type

            error.ErrorType = exception.GetType().Name;

            #endregion

            #region Source

            error.Source = exception.Source;

            #endregion

            #endregion

            return error;
        }

        #endregion
    }
}
