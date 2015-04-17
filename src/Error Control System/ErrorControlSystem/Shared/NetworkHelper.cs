using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using ErrorControlSystem.DbConnectionManager;
using ErrorControlSystem.ServerController;

namespace ErrorControlSystem.Shared
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
            if (!IsNetworkAvailable()) return "Network Not Available";

            var host = Dns.GetHostEntry(Dns.GetHostName());

            return host.AddressList.Last(ip => ip.AddressFamily.ToString() == "InterNetwork").ToString();
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

        #region Get Server Instances

        /// <summary>
        /// Gets the servers asynchronous.
        /// </summary>
        public static async Task<string[]> GetServersAsync()
        {
            return await Task.Run(() =>
            {
                var dt = SqlDataSourceEnumerator.Instance.GetDataSources();


                if (dt.Rows.Count == 0)
                {
                    return null;
                }

                var sqlServers = new string[dt.Rows.Count];

                var f = -1;

                foreach (DataRow r in dt.Rows)
                {
                    var sqlServer = r["ServerName"].ToString();
                    var instance = r["InstanceName"].ToString();

                    if (!string.IsNullOrEmpty(instance))
                    {
                        sqlServer += "\\" + instance;
                    }

                    sqlServers[++f] = sqlServer;
                }

                Array.Sort(sqlServers);

                return sqlServers;
            });
        }

        #endregion

        #region Get SQL Databases

        /// <summary>
        /// Gets the SQL databases asynchronous.
        /// </summary>
        public static async Task<string[]> GetSqlDatabasesAsync(Connection cm)
        {
            var databases = new List<String>();

            //create connection
            using (var sqlConn = new ConnectionManager(cm).SqlConn)
            {
                try
                {
                    //open connection
                    await sqlConn.OpenAsync();

                    //get databases
                    var tblDatabases = sqlConn.GetSchema("Databases");

                    //add to list
                    databases.AddRange(from DataRow row in tblDatabases.Rows select row["database_name"].ToString());
                }
                catch
                {
                    return null;
                }
                finally
                {
                    //close connection
                    sqlConn.Close();
                }

                return databases.ToArray();
            }
        }

        #endregion

        #region Get Server Date Time

        public static DateTime GetServerDateTime()
        {
            return ConnectionManager.GetDefaultConnection().IsReady
                ? ServerTransmitter.FetchServerDataTime()
                : DateTime.Now;
        }

        #endregion
    }
}
