using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using ErrorHandlerEngine.CacheErrors;
using ErrorHandlerEngine.DbConnectionManager;
using ErrorHandlerEngine.Resources;
using ErrorHandlerEngine.Shared;

namespace ErrorHandlerEngine.ServerController
{
    public static class ServerTransmitter
    {
        #region Properties

        // maybe the network have exception then dead loop occurred,
        // so this variable closed that
        public static volatile bool CanToSent = true;

        public static TransformBlock<ProxyError, Tuple<ProxyError, bool>> ErrorListenerTransformBlock;

        #endregion

        #region Constructor

        public static async Task InitialTransmitterAsync()
        {
        CheckDatabase:
            await ConnectionManager.GetDefaultConnection().CheckDbConnectionAsync();

            var cm = ConnectionManager.GetDefaultConnection();

            if (cm.IsReady)
            {
                await CreateTablesAndStoredProceduresAsync();
            }
            else if (await cm.IsServerOnlineAsync())
            {
                await CreateDatabaseAsync();

                goto CheckDatabase;
            }

            ErrorListenerTransformBlock = new TransformBlock<ProxyError, Tuple<ProxyError, bool>>(
                async (e) =>
                {
                    if (CanToSent) // Server Connector to online or offline ?
                    {
                        try
                        {
                            await InsertErrorAsync(e);
                        }
                        catch (Exception)
                        {
                            CanToSent = false;
                        }
                        finally
                        {
                            CacheController.AreErrorsInSendState = false;
                        }
                    }
                    else ErrorListenerTransformBlock.Complete();
                    //
                    // Post to Acknowledge Action Block:
                    return new Tuple<ProxyError, bool>(e, CanToSent);
                },
                new ExecutionDataflowBlockOptions()
                {
                    MaxMessagesPerTask = 1,
                    MaxDegreeOfParallelism = 1
                });

            ErrorListenerTransformBlock.LinkTo(CacheController.AcknowledgeActionBlock);
        }

        #endregion

        #region Methods

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

                cmd.CommandText = EmbeddedAssembly.GetFromResources("DatabaseCreatorQuery.sql").Replace("#DatabaseName", databaseName);
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

        public static async Task InsertErrorAsync(ProxyError error)
        {
            // Create a command object identifying the stored procedure
            using (var cmd = new SqlCommand("sp_InsertErrorLog"))
            {
                //
                // Set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;
                //
                // Add parameters to command, which will be passed to the stored procedure
                if (error.Snapshot.Value != null)
                    cmd.Parameters.AddWithValue("@ScreenCapture", error.Snapshot.Value.ToBytes());


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
                cmd.Parameters.AddWithValue("@LineCol", error.LineColumn.ToString());
                cmd.Parameters.AddWithValue("@Duplicate", error.Duplicate);
                cmd.Parameters.AddWithValue("@Data", error.Data);
                //
                // execute the command

                await ConnectionManager.GetDefaultConnection().ExecuteNonQueryAsync(cmd);
            }
        }

        public static DateTime FetchServerDataTime()
        {
            //
            // execute the command
            return ConnectionManager.GetDefaultConnection().ExecuteScalar<DateTime>("SELECT GETDATE()", CommandType.Text);
        }

        public static async Task<DateTime> FetchServerDataTimeAsync()
        {
            //
            // execute the command
            return await ConnectionManager.GetDefaultConnection().ExecuteScalarAsync<DateTime>("SELECT GETDATE()", CommandType.Text);
        }

        public static async Task<ErrorHandlingOptions> GetErrorHandlingOptionsAsync()
        {
            //
            // execute the command
            var optInt = await ConnectionManager.GetDefaultConnection().ExecuteScalarAsync<int>("SELECT dbo.GetErrorHandlingOptions()", CommandType.Text);
            return (ErrorHandlingOptions)optInt;
        }

        

        #endregion
    }
}
