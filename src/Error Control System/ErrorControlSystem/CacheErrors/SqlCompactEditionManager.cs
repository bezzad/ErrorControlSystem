namespace ErrorControlSystem.CacheErrors
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlServerCe;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using ErrorControlSystem.Shared;



    public class SqlCompactEditionManager
    {
        #region Properties

        private const int Max = 3999;

        public string ConnectionString { get; private set; }

        public Dictionary<int, bool> ErrorIds { get; set; }

        public String FilePath { get; protected set; }

        #endregion

        #region Constructors

        public SqlCompactEditionManager(string filePath)
        {
            ErrorIds = new Dictionary<int, bool>();

            this.FilePath = filePath;

            SetConnectionString(filePath);
        }

        #endregion

        #region Methods

        private void SetConnectionString(string filePath)
        {
            ConnectionString = string.Format("DataSource=\"{0}\"; Persist Security Info = false;", filePath);
        }

        public async Task CreateSdfAsync()
        {
            if (File.Exists(FilePath))
            {
                CheckSdf(FilePath);
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
	                                [StackTrace] [ntext] NULL,
	                                [ModuleName] [nvarchar](200) NULL,
	                                [MemberType] [nvarchar](200) NULL,
	                                [Method] [nvarchar](500) NULL,
	                                [Processes] [ntext] NULL,
	                                [ErrorDateTime] [datetime] NULL,
	                                [OS] [nvarchar](1000) NULL,
	                                [IPv4Address] [nvarchar](50) NULL,
	                                [MACAddress] [nvarchar](50) NULL,
	                                [HResult] [int] NULL,
	                                [Line] [int] NULL,
                                    [Column] [int] NULL,
                                    [ScreenCapture] [image] NULL,
	                                [DuplicateNo] [int] NULL, 
                                    [Data] [ntext] NULL) ";


            using (var sqlCon = new SqlCeConnection(ConnectionString))
            using (var cmd = sqlCon.CreateCommand())
            {
                try
                {
                    cmd.CommandText = createErrorLogTable;

                    await sqlCon.OpenAsync();

                    await cmd.ExecuteNonQueryAsync();
                }
                finally
                {
                    sqlCon.Close();
                }
            }
        }

        public void CheckSdf(string filePath)
        {
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
                CreateSdfAsync().GetAwaiter().GetResult();
            }
        }

        public async Task InsertAsync(Error error)
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
					   ,[Line]
                       ,[Column]
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
					   ,@Line
                       ,@Column
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
                cmd.Parameters.AddWithValue("@StackTrace", SubLimitString(error.StackTrace));
                cmd.Parameters.AddWithValue("@ModuleName", error.ModuleName);
                cmd.Parameters.AddWithValue("@MemberType", error.MemberType);
                cmd.Parameters.AddWithValue("@Method", error.Method);
                cmd.Parameters.AddWithValue("@Processes", SubLimitString(error.Processes));
                cmd.Parameters.AddWithValue("@ErrorDateTime", error.ErrorDateTime);
                cmd.Parameters.AddWithValue("@OS", error.OS);
                cmd.Parameters.AddWithValue("@IPv4Address", error.IPv4Address);
                cmd.Parameters.AddWithValue("@MACAddress", error.MacAddress);
                cmd.Parameters.AddWithValue("@HResult", error.HResult);
                cmd.Parameters.AddWithValue("@Line", error.LineColumn.Line);
                cmd.Parameters.AddWithValue("@Column", error.LineColumn.Column);
                cmd.Parameters.AddWithValue("@Duplicate", error.Duplicate);
                cmd.Parameters.AddWithValue("@Data", SubLimitString(error.Data));
                if (error.Snapshot == null) cmd.Parameters.AddWithValue("@Snapshot", DBNull.Value);
                else cmd.Parameters.AddWithValue("@Snapshot", error.Snapshot.ToBytes());

                try
                {
                    await sqlConn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    ErrorIds.Add(error.Id, error.IsHandled);
                }
                finally
                {
                    sqlConn.Close();
                }
            }
        }

        public async Task UpdateAsync(Error error)
        {
            // If error is not exist in list or error state is now at unhandled exception state then exit
            if (!ErrorIds.ContainsKey(error.Id) || !ErrorIds[error.Id]) return;

            using (var sqlConn = new SqlCeConnection(ConnectionString))
            using (var cmd = sqlConn.CreateCommand())
            {
                cmd.CommandText =
                    string.Format("UPDATE [ErrorLog] SET [DuplicateNo] = [DuplicateNo] + 1 {0} WHERE ErrorId = @id",
                        error.IsHandled ? "" : ", [IsHandled] = @isHandled, [StackTrace] = @stackTrace");

                //
                // Add parameters to command, which will be passed to the stored procedure
                cmd.Parameters.AddWithValue("@id", error.Id);
                if (!error.IsHandled) // Just in Unhandled Exceptions
                {
                    cmd.Parameters.AddWithValue("@isHandled", error.IsHandled);
                    cmd.Parameters.AddWithValue("@stackTrace", SubLimitString(error.StackTrace));
                }

                try
                {
                    await sqlConn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    ErrorIds[error.Id] = error.IsHandled;
                }
                finally
                {
                    sqlConn.Close();
                }
            }
        }

        public async Task<bool> InsertOrUpdateAsync(Error error)
        {
            if (ErrorIds.ContainsKey(error.Id))
            {
                await UpdateAsync(error);
                return false; // In update state not necessary to check cache size
            }

            await InsertAsync(error);
            return true; // New error added to database, so need to check cache size
        }

        public async Task<ProxyError> GetErrorAsync(int id)
        {
            using (var sqlConn = new SqlCeConnection(ConnectionString))
            using (var cmd = sqlConn.CreateCommand())
            {
                try
                {
                    cmd.CommandText = string.Format("Select * From ErrorLog Where ErrorId = {0}", id);

                    await sqlConn.OpenAsync();

                    return await cmd.ExecuteScalarAsync() as ProxyError;
                }
                finally
                {
                    sqlConn.Close();
                }
            }
        }

        public IEnumerable<ProxyError> GetErrors()
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
                                                            ,[Line]
                                                            ,[Column]
                                                            ,[DuplicateNo]
                                                            ,[Data]
                                                        FROM ErrorLog");

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
                                LineColumn = new CodeScope((int)error["Line"], (int)error["Column"]),
                                Duplicate = (int)error["DuplicateNo"],
                                Data = (string)error["Data"]
                            });
                }
                finally
                {
                    sqlConn.Close();
                }
            }
        }

        public Dictionary<int, bool> GetErrorsId()
        {
            using (var sqlConn = new SqlCeConnection(ConnectionString))
            using (var cmd = sqlConn.CreateCommand())
            {
                try
                {
                    cmd.CommandText = string.Format(@"SELECT [ErrorId]                                                            
                                                            ,[IsHandled]                                                            
                                                        FROM ErrorLog");

                    sqlConn.Open();

                    var adapter = new SqlCeDataAdapter(cmd);
                    var dt = new DataTable();
                    adapter.Fill(dt);

                    return dt.Rows.Cast<DataRow>()
                        .ToDictionary(row => (int)row["ErrorId"], row => (bool)row["IsHandled"]);
                }
                finally
                {
                    sqlConn.Close();
                }
            }
        }

        public Image GetSnapshot(int id)
        {
            using (var sqlConn = new SqlCeConnection(ConnectionString))
            using (var cmd = sqlConn.CreateCommand())
            {
                try
                {
                    cmd.CommandText = string.Format("Select [ScreenCapture] From ErrorLog Where ErrorId = {0}", id);

                    sqlConn.Open();

                    var result = cmd.ExecuteScalar();

                    return (result is DBNull) ? null : ((byte[])result).ToImage();
                }
                finally
                {
                    sqlConn.Close();
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            // If error is not exist in list then exit
            if (!ErrorIds.ContainsKey(id)) return;

            using (var sqlConn = new SqlCeConnection(ConnectionString))
            using (var cmd = sqlConn.CreateCommand())
            {
                try
                {
                    cmd.CommandText = string.Format("Delete From ErrorLog Where ErrorId = {0}", id);

                    await sqlConn.OpenAsync();

                    await cmd.ExecuteNonQueryAsync();

                    ErrorIds.Remove(id);
                }
                finally
                {
                    sqlConn.Close();
                }
            }
        }

        public async Task<int> CountAsync()
        {
            using (var sqlConn = new SqlCeConnection(ConnectionString))
            using (var cmd = sqlConn.CreateCommand())
            {
                try
                {
                    cmd.CommandText = string.Format("Select Count(ErrorId) From ErrorLog");

                    await sqlConn.OpenAsync();

                    return await cmd.ExecuteScalarAsync() as int? ?? 0;
                }
                finally
                {
                    sqlConn.Close();
                }
            }
        }

        public async Task<int> GetTheFirstErrorHoursAsync()
        {
            using (var sqlConn = new SqlCeConnection(ConnectionString))
            using (var cmd = sqlConn.CreateCommand())
            {
                try
                {
                    cmd.CommandText = string.Format("SELECT ABS(DATEDIFF(HH, Min([ErrorDateTime]), GETDATE())) FROM ErrorLog");

                    await sqlConn.OpenAsync();

                    return await cmd.ExecuteScalarAsync() as int? ?? 0;
                }
                finally
                {
                    sqlConn.Close();
                }
            }
        }

        private static string SubLimitString(string item)
        {
            return item.Length > Max ? item.Substring(0, Max) : item;
        }

        public void LoadCacheIds()
        {
            ErrorIds = GetErrorsId();
        }

        #endregion
    }
}
