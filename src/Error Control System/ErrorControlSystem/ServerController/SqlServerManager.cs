namespace ErrorControlSystem.ServerController
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Sql;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;

    using ErrorControlSystem.DbConnectionManager;
    using ErrorControlSystem.Resources;
    using ErrorControlSystem.Shared;

    public static partial class ServerTransmitter
    {
        public class SqlServerManager
        {
            public static async Task CreateDatabaseAsync()
            {
                var conn = ConnectionManager.GetDefaultConnection();

                var databaseName = conn.Connection.DatabaseName;
                var masterConn = conn.Connection.Clone() as Connection;
                masterConn.DatabaseName = "master";

                var sqlConn = new SqlConnection(masterConn.ConnectionString);

                // Create a command object identifying the stored procedure
                using (var cmd = sqlConn.CreateCommand())
                {
                    //
                    // Set the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText =
                        EmbeddedAssembly.GetFromResources("DatabaseCreatorQuery.sql")
                            .Replace("#DatabaseName", databaseName);
                    //
                    // execute the command
                    try
                    {
                        await sqlConn.OpenAsync();

                        await cmd.ExecuteNonQueryAsync();
                    }
                    finally
                    {
                        sqlConn.Close();
                    }
                }
            }

            public static async Task CreateTablesAndStoredProceduresAsync()
            {
                var conn = ConnectionManager.GetDefaultConnection();

                // Create a command object identifying the stored procedure
                using (var cmd = conn.CreateCommand())
                {
                    //
                    // Set the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = EmbeddedAssembly.GetFromResources("TablesAndSPsCreatorQuery.sql");
                    //
                    // execute the command
                    try
                    {
                        await conn.OpenAsync();

                        await cmd.ExecuteNonQueryAsync();
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            public static async Task InsertErrorAsync(IError error)
            {
                // Create a command object identifying the stored procedure
                using (var cmd = new SqlCommand("sp_InsertErrorLog"))
                {
                    //
                    // Set the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;
                    //
                    // Add parameters to command, which will be passed to the stored procedure
                    if (error.Snapshot != null)
                        cmd.Parameters.AddWithValue("@ScreenCapture", error.Snapshot.ToBytes());


                    cmd.Parameters.AddWithValue("@ServerDateTime", error.ServerDateTime);
                    cmd.Parameters.AddWithValue("@Host", error.Host);
                    cmd.Parameters.AddWithValue("@User", error.User);
                    cmd.Parameters.AddWithValue("@IsHandled", error.IsHandled);
                    cmd.Parameters.AddWithValue("@Type", error.ErrorType);
                    cmd.Parameters.AddWithValue("@AppName", error.AppName);
                    cmd.Parameters.AddWithValue("@CurrentCulture", error.CurrentCulture);
                    cmd.Parameters.AddWithValue("@CLRVersion", error.ClrVersion);
                    cmd.Parameters.AddWithValue("@Message", error.Message);
                    cmd.Parameters.AddWithValue("@Source", error.Source ?? "");
                    cmd.Parameters.AddWithValue("@StackTrace", error.StackTrace);
                    cmd.Parameters.AddWithValue("@ModuleName", error.ModuleName);
                    cmd.Parameters.AddWithValue("@MemberType", error.MemberType);
                    cmd.Parameters.AddWithValue("@Method", error.Method);
                    cmd.Parameters.AddWithValue("@Processes", error.Processes);
                    cmd.Parameters.AddWithValue("@ErrorDateTime", error.ErrorDateTime);
                    cmd.Parameters.AddWithValue("@OS", error.OS);
                    cmd.Parameters.AddWithValue("@IPv4Address", error.IPv4Address);
                    cmd.Parameters.AddWithValue("@MACAddress", error.MacAddress);
                    cmd.Parameters.AddWithValue("@HResult", error.HResult);
                    cmd.Parameters.AddWithValue("@Line", error.LineColumn.Line);
                    cmd.Parameters.AddWithValue("@Column", error.LineColumn.Column);
                    cmd.Parameters.AddWithValue("@Duplicate", error.Duplicate);
                    cmd.Parameters.AddWithValue("@Data", error.Data);
                    //
                    // execute the command

                    await ConnectionManager.GetDefaultConnection().ExecuteNonQueryAsync(cmd);
                }
            }

            public static DateTime GetServerDateTime()
            {
                var cm = ConnectionManager.GetDefaultConnection();

                //
                // execute the command
                try
                {
                    var serverDate = cm.IsReady
                            ? cm.ExecuteScalar<DateTime>("SELECT GETDATE()", CommandType.Text)
                            : DateTime.Now;

                    return serverDate;
                }
                catch
                {
                    ErrorHandlingOption.EnableNetworkSending = false;

                    return DateTime.Now;
                }
            }

            public static async Task<ErrorHandlingOptions> GetErrorHandlingOptionsAsync()
            {
                //
                // execute the command
                var optInt =
                    await
                        ConnectionManager.GetDefaultConnection()
                            .ExecuteScalarAsync<int>("SELECT dbo.GetErrorHandlingOptions()", CommandType.Text);
                return (ErrorHandlingOptions)optInt;
            }

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


            #region Get Server Instances

            /// <summary>
            /// Gets the servers asynchronous.
            /// </summary>
            public static async Task<string[]> GetSqlServersInstanceAsync()
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
        }
    }
}
