using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ErrorHandlerEngine.ExceptionManager;
using ErrorHandlerEngine.Shared;

namespace ErrorHandlerEngine.CacheErrors
{
    public static class SdfFileManager
    {
        public static string ConnectionString { get; private set; }

        public static void SetConnectionString(string filePath)
        {
            ConnectionString = string.Format("DataSource=\"{0}\"; Persist Security Info = false;", filePath);
        }

        public static async Task CreateSdfAsync(string filePath)
        {
            SetConnectionString(filePath);

            if (File.Exists(filePath))
            {
                CheckSdf(filePath);
                return;
            }

            new SqlCeEngine(ConnectionString).CreateDatabase();

            const string createErrorLogTable = @"CREATE TABLE [ErrorLog](
	                                [ErrorId] [int] NOT NULL CONSTRAINT PK_ErrorLog PRIMARY KEY,
	                                [ServerDateTime] [datetime] NULL,
	                                [Host] [nvarchar](200) NULL,
	                                [User] [nvarchar](200) NULL,
	                                [IsHandled] [bit] NOT NULL,
	                                [Type] [nvarchar](200) NULL,
	                                [AppName] [nvarchar](200) NULL,
	                                [CurrentCulture] [nvarchar](200) NULL,
	                                [CLRVersion] [nvarchar](100) NULL,
	                                [Message] [nvarchar](2000) NULL,
	                                [Source] [nvarchar](200) NULL,
	                                [StackTrace] [nvarchar](4000) NULL,
	                                [ModuleName] [nvarchar](200) NULL,
	                                [MemberType] [nvarchar](200) NULL,
	                                [Method] [nvarchar](500) NULL,
	                                [Processes] [nvarchar](4000) NULL,
	                                [ErrorDateTime] [datetime] NULL,
	                                [OS] [nvarchar](1000) NULL,
	                                [IPv4Address] [nvarchar](50) NULL,
	                                [MACAddress] [nvarchar](50) NULL,
	                                [HResult] [int] NULL,
	                                [LineColumn] [nvarchar](50) NULL,
                                    [ScreenCapture] [image] NULL,
	                                [DuplicateNo] [int] NULL, 
                                    [Data] [nvarchar] (4000) NULL) ";


            using (var sqlCon = new SqlCeConnection(ConnectionString))
            using (var cmd = sqlCon.CreateCommand())
            {
                try
                {
                    ExceptionHandler.IsSelfException = true;

                    cmd.CommandText = createErrorLogTable;

                    await sqlCon.OpenAsync();

                    await cmd.ExecuteNonQueryAsync();
                }
                finally
                {
                    sqlCon.Close();

                    ExceptionHandler.IsSelfException = false;
                }
            }
        }

        public static void CheckSdf(string filePath)
        {
            ExceptionHandler.IsSelfException = true;
            
            try
            {
                var testConn = new SqlCeConnection(ConnectionString);
                testConn.Open();

                testConn.Close();
            }
            catch (SqlCeException)
            {
                // fix the SDF file
                File.Delete(filePath);
                CreateSdfAsync(filePath).GetAwaiter().GetResult();
            }
            finally
            {
                ExceptionHandler.IsSelfException = false;
            }
        }

        public static async Task InsertAsync(Error error)
        {
            using (var sqlConn = new SqlCeConnection(ConnectionString))
            using (var cmd = sqlConn.CreateCommand())
            {
                cmd.CommandText = @"INSERT  INTO [ErrorLog]
					   ([ErrorId]
                       ,[ServerDateTime]
					   ,[Host]
					   ,[User]
					   ,[IsHandled]
					   ,[Type]
					   ,[AppName]
					   ,[CurrentCulture]
					   ,[CLRVersion]
					   ,[Message]
					   ,[Source]
					   ,[StackTrace]
					   ,[ModuleName]
					   ,[MemberType]
					   ,[Method]
					   ,[Processes]
					   ,[ErrorDateTime]
					   ,[OS]
					   ,[IPv4Address]
					   ,[MACAddress]
					   ,[HResult]
					   ,[LineColumn]
                       ,[ScreenCapture]
					   ,[DuplicateNo]
                       ,[Data])
            VALUES  ( 
                        @Id
                       ,@ServerDateTime
					   ,@Host
					   ,@User
					   ,@IsHandled
					   ,@Type
					   ,@AppName
					   ,@CurrentCulture
					   ,@CLRVersion
					   ,@Message
					   ,@Source
					   ,@StackTrace
					   ,@ModuleName
					   ,@MemberType
					   ,@Method
					   ,@Processes
					   ,@ErrorDateTime
					   ,@OS
					   ,@IPv4Address
					   ,@MACAddress
					   ,@HResult
					   ,@LineCol
                       ,@Snapshot
					   ,@Duplicate
                       ,@Data
                    )";

                //
                // Add parameters to command, which will be passed to the stored procedure
                cmd.Parameters.AddWithValue("@Id", error.Id);
                cmd.Parameters.AddWithValue("@ServerDateTime", error.ServerDateTime);
                cmd.Parameters.AddWithValue("@Host", error.Host);
                cmd.Parameters.AddWithValue("@User", error.User);
                cmd.Parameters.AddWithValue("@IsHandled", error.IsHandled);
                cmd.Parameters.AddWithValue("@Type", error.ErrorType);
                cmd.Parameters.AddWithValue("@AppName", error.AppName);
                cmd.Parameters.AddWithValue("@CurrentCulture", error.CurrentCulture);
                cmd.Parameters.AddWithValue("@CLRVersion", error.ClrVersion);
                cmd.Parameters.AddWithValue("@Message", error.Message);
                cmd.Parameters.AddWithValue("@Source", error.Source ?? "Source Not Found");
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
                if (error.Snapshot == null) cmd.Parameters.AddWithValue("@Snapshot", DBNull.Value);
                else cmd.Parameters.AddWithValue("@Snapshot", error.Snapshot.ToBytes());

                try
                {
                    ExceptionHandler.IsSelfException = true;
                    await sqlConn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
                finally
                {
                    sqlConn.Close();
                    ExceptionHandler.IsSelfException = false;
                }
            }
        }

        public static async Task UpdateAsync(Error error)
        {
            using (var sqlConn = new SqlCeConnection(ConnectionString))
            using (var cmd = sqlConn.CreateCommand())
            {
                cmd.CommandText = @"UPDATE [ErrorLog]
                                    SET [DuplicateNo] = [DuplicateNo] + 1,
                                        [IsHandled] = @isHandled,
                                        [StackTrace] = @stackTrace
                                    WHERE ErrorId = @id";

                //
                // Add parameters to command, which will be passed to the stored procedure
                cmd.Parameters.AddWithValue("@id", error.Id);
                cmd.Parameters.AddWithValue("@isHandled", error.IsHandled);
                cmd.Parameters.AddWithValue("@stackTrace", error.StackTrace);

                try
                {
                    ExceptionHandler.IsSelfException = true;

                    await sqlConn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
                finally
                {
                    sqlConn.Close();

                    ExceptionHandler.IsSelfException = false;
                }
            }
        }

        public static async Task InsertOrUpdateAsync(Error error)
        {
            if (Contains(error.Id))
                await UpdateAsync(error);
            else
                await InsertAsync(error);
        }

        public static async Task<ProxyError> GetErrorAsync(int id)
        {
            using (var sqlConn = new SqlCeConnection(ConnectionString))
            using (var cmd = sqlConn.CreateCommand())
            {
                try
                {
                    ExceptionHandler.IsSelfException = true;

                    cmd.CommandText = string.Format("Select * From ErrorLog Where ErrorId = {0}", id);

                    await sqlConn.OpenAsync();

                    return await cmd.ExecuteScalarAsync() as ProxyError;
                }
                finally
                {
                    sqlConn.Close();

                    ExceptionHandler.IsSelfException = false;
                }
            }
        }

        public static bool Contains(int id)
        {
            using (var sqlConn = new SqlCeConnection(ConnectionString))
            using (var cmd = sqlConn.CreateCommand())
            {
                try
                {
                    ExceptionHandler.IsSelfException = true;

                    cmd.CommandText = string.Format("Select Count(ErrorId) From ErrorLog Where ErrorId = {0}", id);

                    sqlConn.Open();

                    return cmd.ExecuteScalar() as int? > 0;
                }
                finally
                {
                    sqlConn.Close();

                    ExceptionHandler.IsSelfException = false;
                }
            }
        }

        public static IEnumerable<ProxyError> GetErrors()
        {
            using (var sqlConn = new SqlCeConnection(ConnectionString))
            using (var cmd = sqlConn.CreateCommand())
            {
                try
                {
                    cmd.CommandText = string.Format(@"SELECT [ErrorId]
                                                            ,[ServerDateTime]
                                                            ,[Host]
                                                            ,[User]
                                                            ,[IsHandled]
                                                            ,[Type]
                                                            ,[AppName]
                                                            ,[CurrentCulture]
                                                            ,[CLRVersion]
                                                            ,[Message]
                                                            ,[Source]
                                                            ,[StackTrace]
                                                            ,[ModuleName]
                                                            ,[MemberType]
                                                            ,[Method]
                                                            ,[Processes]
                                                            ,[ErrorDateTime]
                                                            ,[OS]
                                                            ,[IPv4Address]
                                                            ,[MACAddress]
                                                            ,[HResult]
                                                            ,[LineColumn]
                                                            ,[DuplicateNo]
                                                            ,[Data]
                                                        FROM ErrorLog");

                    ExceptionHandler.IsSelfException = true;

                    sqlConn.Open();

                    var adapter = new SqlCeDataAdapter(cmd);
                    var dt = new DataTable();
                    adapter.Fill(dt);

                    return (from DataRow error in dt.Rows
                            select new ProxyError
                            {
                                Id = unchecked((int)error["ErrorId"]),
                                ServerDateTime = (DateTime)error["ServerDateTime"],
                                Host = (string)error["Host"],
                                User = (string)error["User"],
                                IsHandled = (bool)error["IsHandled"],
                                ErrorType = (string)error["Type"],
                                AppName = (string)error["AppName"],
                                CurrentCulture = (string)error["CurrentCulture"],
                                ClrVersion = (string)error["CLRVersion"],
                                Message = (string)error["Message"],
                                Source = (string)error["Source"],
                                StackTrace = (string)error["StackTrace"],
                                ModuleName = (string)error["ModuleName"],
                                MemberType = (string)error["MemberType"],
                                Method = (string)error["Method"],
                                Processes = (string)error["Processes"],
                                ErrorDateTime = (DateTime)error["ErrorDateTime"],
                                OS = (string)error["OS"],
                                IPv4Address = (string)error["IPv4Address"],
                                MacAddress = (string)error["MACAddress"],
                                HResult = (int)error["HResult"],
                                LineColumn = CodeLocation.Parse(error["LineColumn"] as string),
                                Duplicate = (int)error["DuplicateNo"],
                                Data = (string)error["Data"]
                            });
                }
                finally
                {
                    sqlConn.Close();

                    ExceptionHandler.IsSelfException = false;
                }
            }
        }

        public static Image GetSnapshot(int id)
        {
            using (var sqlConn = new SqlCeConnection(ConnectionString))
            using (var cmd = sqlConn.CreateCommand())
            {
                try
                {
                    ExceptionHandler.IsSelfException = true;

                    cmd.CommandText = string.Format("Select [ScreenCapture] From ErrorLog Where ErrorId = {0}", id);

                    sqlConn.Open();
                    
                    var result = cmd.ExecuteScalar();

                    return (result is DBNull) ? null : ((byte[])result).ToImage();
                }
                finally
                {
                    sqlConn.Close();

                    ExceptionHandler.IsSelfException = true;
                }
            }
        }

        public static async Task DeleteAsync(int id)
        {
            using (var sqlConn = new SqlCeConnection(ConnectionString))
            using (var cmd = sqlConn.CreateCommand())
            {
                try
                {
                    ExceptionHandler.IsSelfException = true;

                    cmd.CommandText = string.Format("Delete From ErrorLog Where ErrorId = {0}", id);

                    await sqlConn.OpenAsync();

                    await cmd.ExecuteNonQueryAsync();
                }
                finally
                {
                    sqlConn.Close();

                    ExceptionHandler.IsSelfException = false;
                }
            }
        }

        public static async Task<int> CountAsync()
        {
            using (var sqlConn = new SqlCeConnection(ConnectionString))
            using (var cmd = sqlConn.CreateCommand())
            {
                try
                {
                    ExceptionHandler.IsSelfException = true;

                    cmd.CommandText = string.Format("Select Count(ErrorId) From ErrorLog");

                    await sqlConn.OpenAsync();

                    return await cmd.ExecuteScalarAsync() as int? ?? 0;
                }
                finally
                {
                    sqlConn.Close();

                    ExceptionHandler.IsSelfException = false;
                }
            }
        }

        public static async Task<int> GetTheFirstErrorHoursAsync()
        {
            using (var sqlConn = new SqlCeConnection(ConnectionString))
            using (var cmd = sqlConn.CreateCommand())
            {
                try
                {
                    ExceptionHandler.IsSelfException = true;

                    cmd.CommandText = string.Format("SELECT ABS(DATEDIFF(HH, Min([ErrorDateTime]), GETDATE())) FROM ErrorLog");

                    await sqlConn.OpenAsync();

                    return await cmd.ExecuteScalarAsync() as int? ?? 0;
                }
                finally
                {
                    sqlConn.Close();

                    ExceptionHandler.IsSelfException = false;
                }
            }
        }
    }
}
