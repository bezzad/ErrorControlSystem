using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using ConnectionsManager;
using ErrorHandlerEngine.ServerUploader;

namespace ErrorHandlerEngine.ModelObjecting
{
    public static class NetworkHelper
    {
        #region Get Current PC MAC Address
        /// <summary>
        /// Gets the MAC address of the current PC.
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
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
        public static string GetIpAddress()
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
        public static bool IsNetworkAvailable()
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

        public static DateTime GetServerDateTime()
        {
            return ConnectionManager.GetDefaultConnection().IsReady
                ? DynamicStoredProcedures.FetchServerDataTimeTsql()
                : DateTime.Now;
        }

        #endregion
    }
}
