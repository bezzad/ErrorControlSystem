using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ConnectionsManager;
using ErrorHandlerEngine.ExceptionManager;
using ErrorHandlerEngine.ModelObjecting;

namespace ErrorHandlerEngine.ServerUploader
{
    public static class DynamicStoredProcedures
    {
        

        public static async Task InsertErrorStoredProcedureAsync(ProxyError error)
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


                cmd.Parameters.AddWithValue("@DateTime", error.ServerDateTime);
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
                try
                {
                    ExceptionHandler.IsSelfException = true;
                    await ConnectionManager.GetDefaultConnection().ExecuteNonQueryAsync(cmd);
                }
                finally
                {
                    ExceptionHandler.IsSelfException = false;
                }
            }
        }

        public static DateTime FetchServerDataTimeTsql()
        {
            //
            // execute the command
            try
            {
                ExceptionHandler.IsSelfException = true;
                return ConnectionManager.GetDefaultConnection().ExecuteScalar<DateTime>("SELECT GETDATE()", CommandType.Text);
            }
            finally
            {
                ExceptionHandler.IsSelfException = false;
            }
        }

        public static async Task<DateTime> FetchServerDataTimeTsqlAsync()
        {
            //
            // execute the command
            try
            {
                ExceptionHandler.IsSelfException = true;
                return await ConnectionManager.GetDefaultConnection().ExecuteScalarAsync<DateTime>("SELECT GETDATE()", CommandType.Text);
            }
            finally
            {
                ExceptionHandler.IsSelfException = false;
            }
        }

    }
}
