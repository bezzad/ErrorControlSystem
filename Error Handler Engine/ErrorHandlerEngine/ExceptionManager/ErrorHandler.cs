//**********************************************************************************//
//                           LICENSE INFORMATION                                    //
//**********************************************************************************//
//   Error Handler Engine 1.0.0.0                                                   //
//   ThisClass Library creates a way of handling structured exception handling,     //
//   inheriting from the Exception class gives us access to many method             //
//   we wouldn't otherwise have access to                                           //
//                                                                                  //
//   Copyright (C) 2014                                                             //
//   Behzad Khosravifar                                                             //
//   Email: Behzad.Khosravifar@Gmail.com                                            //
//                                                                                  //
//   This program is free software: you can redistribute it and/or modify           //
//   it under the terms of the GNU General Public License as published by           //
//   the Free Software Foundation, either version 3 of the License, or              //
//   (at your option) any later version.                                            //
//                                                                                  //
//   This program is distributed in the hope that it will be useful,                //
//   but WITHOUT ANY WARRANTY; without even the implied warranty of                 //
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the                  //
//   GNU General Public License for more details.                                   //
//                                                                                  //
//   You should have received a copy of the GNU General Public License              //
//   along with this program.                                                       //
//**********************************************************************************//

using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using ErrorHandlerEngine.ModelObjecting;
using ErrorHandlerEngine.ServerUploader;

namespace ErrorHandlerEngine.ExceptionManager
{
    /// <summary>
    /// Additional Data attached to exception object.
    /// </summary>
    public static class ErrorHandler
    {
        #region Events

        public static EventHandler OnErrorRaised = delegate { };

        #endregion

        #region Properties
        public static Size MaxScreenShotSize = new Size(800, 600); // set to aspect 800×600
        #endregion

        #region Public Methods
        #region Raise Error Log
        public static Error RaiseLog(this Exception exp, ErrorHandlingOption option = ErrorHandlingOption.Default, String errorTitle = "UnHandled Exception")
        {
            if (Kernel.IsSelfException)
            {
                Kernel.IsSelfException = false;
                return null;
            }

            var error = exp.HandleError(option);

            if (option.HasFlag(ErrorHandlingOption.AlertUnHandledError) && !option.HasFlag(ErrorHandlingOption.IsHandled)) // Alert Unhandled Error 
            {
                MessageBox.Show(exp.Message,
                    errorTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }


            if (OnErrorRaised != null)
                OnErrorRaised(error, new EventArgs());

            return error;
        }
        #endregion

        #endregion

        #region Private Methods

        #region Get Handled Error Object
        private static Error HandleError(this Exception exception, ErrorHandlingOption option = ErrorHandlingOption.Default)
        {
            // Create Empty Error object
            var error = new Error();

            #region Initialize Exception Additional Data


            #region Screen Capture
            // First initialize Snapshot of Error, because that's speed is important!
            error.SetSnapshot(option.HasFlag(ErrorHandlingOption.Snapshot) ?
                ScreenCapture.CaptureScreen().ResizeImage(MaxScreenShotSize.Width, MaxScreenShotSize.Height) // Resize and reduce size of screen shot image file
                : null);

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

            error.ServerDateTime = option.HasFlag(ErrorHandlingOption.FetchServerDateTime) ? GetServerDateTime() : DateTime.Now;

            #endregion

            #region Current Culture

            error.CurrentCulture = string.Format("{0} ({1})",
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

            error.AppName = string.Format("{0}  v{1}",
                    AppDomain.CurrentDomain.FriendlyName.Replace(".vshost", ""),
                    Application.ProductVersion);

            #endregion

            #region Process Name String List

            error.Processes = new CurrentProcesses().ToString();

            #endregion

            #region Is Handled Error or UnHandled?

            error.IsHandled = option.HasFlag(ErrorHandlingOption.IsHandled);

            #endregion

            #region Screen Capture Address

            error.SnapshotAddress = "";

            #endregion

            #region Current Static Valid IPv4 Address

            error.IPv4Address = GetIpAddress();

            #endregion

            #region Network Physical Address [MAC HEX]

            error.MacAddress = GetMacAddress();

            #endregion

            #region Common Language Runtime Version [Major.Minor.Build.Revison.]

            error.ClrVersion = Environment.Version.ToString();

            #endregion

            #region Error Type

            error.ErrorType = exception.GetType().Name;

            #endregion

            #region Source

            error.Source = exception.Source;

            #endregion

            #region HResult [Exception Type Code]

            error.HResult = exception.HResult;

            #endregion

            #region Error Line Column

            error.ErrorLineColumn = new CodeLocation(exception);
            #endregion

            #region Id = HashCode
            error.Id = error.GetHashCode();
            #endregion

            #endregion

            return error;
        }

        #endregion

        #region Get Current PC MAC Address
        /// <summary>
        /// Gets the MAC address of the current PC.
        /// </summary>
        /// <returns></returns>
        private static string GetMacAddress()
        {
            // only recognizes changes related to Internet adapters
            if (IsNetworkAvailable())
            {
                foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    // Only consider Ethernet network interfaces
                    if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                        nic.OperationalStatus == OperationalStatus.Up)
                    {
                        return nic.GetPhysicalAddress().ToString();
                    }
                }
            }

            return "Network Not Available";
        }
        #endregion

        #region Get Current PC IP Address
        /// <summary>
        /// Gets the IP address of the current PC.
        /// </summary>
        /// <returns></returns>
        private static string GetIpAddress()
        {
            if (IsNetworkAvailable())
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());

                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily.ToString() == "InterNetwork")
                    {
                        return ip.ToString();
                    }
                }
            }

            return "Network Not Available";
        }
        #endregion

        #region Check Network Availability
        /// <summary>
        /// Indicates whether any network connection is available
        /// </summary>
        /// <returns>
        ///     <c>true</c> if a network connection is available; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsNetworkAvailable()
        {
            // only recognizes changes related to Internet adapters
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                // however, this will include all adapters
                return 
                    NetworkInterface.GetAllNetworkInterfaces().
                    Where(face => face.OperationalStatus == OperationalStatus.Up).
                    Any(face => 
                        (face.NetworkInterfaceType != NetworkInterfaceType.Tunnel) && 
                        (face.NetworkInterfaceType != NetworkInterfaceType.Loopback));
            }

            return false;
        }
        #endregion

        #region Get Server Date Time
        private static DateTime GetServerDateTime()
        {
            if (Kernel.Conn.IsReady)
                return DynamicStoredProcedures.FetchServerDataTimeTsql(Kernel.Conn);

            return DateTime.Now;
        }
        #endregion

        #endregion
    }
}
