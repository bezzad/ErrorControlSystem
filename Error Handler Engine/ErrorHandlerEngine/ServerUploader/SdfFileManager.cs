using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlServerCe;
using ErrorHandlerEngine.ModelObjecting;

namespace ErrorHandlerEngine.ServerUploader
{
    public static class SdfFileManager
    {
        public static string ConnectionString { get; set; }



        public static void CreateSdf(string filePath)
        {
            ConnectionString = string.Format("DataSource=\"{0}.sdf\"; Encrypt Database=True;", filePath);

            if (File.Exists(filePath)) return;

            new SqlCeEngine(ConnectionString).CreateDatabase();

            const string createErrorLogTable = @"CREATE TABLE [ErrorLog](
	                                [ErrorId] [bigint] NOT NULL CONSTRAINT PK_ErrorLog PRIMARY KEY,
	                                [ServerDateTime] [datetime] NULL,
	                                [Host] [nvarchar](200) NULL,
	                                [User] [nvarchar](200) NULL,
	                                [IsHandled] [bit] NOT NULL,
	                                [Type] [nvarchar](200) NULL,
	                                [AppName] [nvarchar](200) NULL,
	                                [CurrentCulture] [nvarchar](200) NULL,
	                                [CLRVersion] [nvarchar](50) NULL,
	                                [Message] [nvarchar](1000) NULL,
	                                [Source] [nvarchar](200) NULL,
	                                [StackTrace] [nvarchar](2000) NULL,
	                                [ModuleName] [nvarchar](200) NULL,
	                                [MemberType] [nvarchar](50) NULL,
	                                [Method] [nvarchar](200) NULL,
	                                [Processes] [nvarchar](3000) NULL,
	                                [ErrorDateTime] [datetime] NULL,
	                                [OS] [nvarchar](100) NULL,
	                                [IPv4Address] [nvarchar](20) NULL,
	                                [MACAddress] [nvarchar](50) NULL,
	                                [HResult] [int] NULL,
	                                [LineColumn] [nvarchar](40) NULL,
                                    [ScreenCapture] [image] NULL,
	                                [DuplicateNo] [int] NULL ) ";


            using (var sqlCon = new SqlCeConnection(ConnectionString))
            using (var comError = new SqlCeCommand(createErrorLogTable))
            using (comError.Connection = sqlCon)
            {
                try
                {
                    sqlCon.Open();

                    comError.ExecuteNonQuery();
                }
                finally
                {
                    sqlCon.Close();
                }
            }
        }

        public static void Insert(Error error)
        {
            using (var sqlConn = new SqlCeConnection(ConnectionString))
            using (var cmd = sqlConn.CreateCommand())
            {
                cmd.CommandText = @"INSERT  INTO [ErrorLog]
					   ([ServerDateTime]
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
					   ,[DuplicateNo])
            VALUES  ( 
`                       @ServerDateTime
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
                    )";

                //
                // Add parameters to command, which will be passed to the stored procedure
                cmd.Parameters.AddWithValue("@ServerDateTime", error.ServerDateTime);
                cmd.Parameters.AddWithValue("@Host", error.Host);
                cmd.Parameters.AddWithValue("@User", error.User);
                cmd.Parameters.AddWithValue("@IsHandled", error.IsHandled);
                cmd.Parameters.AddWithValue("@Type", error.ErrorType);
                cmd.Parameters.AddWithValue("@AppName", error.AppName);
                cmd.Parameters.AddWithValue("@CurrentCulture", error.CurrentCulture);
                cmd.Parameters.AddWithValue("@CLRVersion", error.ClrVersion);
                cmd.Parameters.AddWithValue("@Message", error.Message);
                cmd.Parameters.AddWithValue("@Source", error.Source);
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
                cmd.Parameters.AddWithValue("@Snapshot", error.Snapshot().ToBytes());
                cmd.Parameters.AddWithValue("@Duplicate", error.Duplicate);

                try
                {
                    sqlConn.Open();
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    sqlConn.Close();
                }
            }
        }

        public static void Update(Error error)
        {
            using (var sqlConn = new SqlCeConnection(ConnectionString))
            using (var cmd = sqlConn.CreateCommand())
            {
                cmd.CommandText = @"UPDATE [ErrorLog]
                                    SET [DuplicateNo] +=1,
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
                    sqlConn.Open();
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    sqlConn.Close();
                }
            }
        }

        public static void InsertOrUpdate(Error error)
        {
            if (Contains(error.Id))
                UpdateSdf(error);
            else
                InsertToSdf(error);
        }

        public static ProxyError GetErrorById(int id)
        {
            using (var sqlConn = new SqlCeConnection(ConnectionString))
            using (var cmd = sqlConn.CreateCommand())
            {
                try
                {
                    cmd.CommandText = string.Format("Select * From ErrorLog Where ErrorId = {0}", id);

                    sqlConn.Open();

                    return (Error)cmd.ExecuteScalar();
                }
                finally
                {
                    sqlConn.Close();
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
                    cmd.CommandText = string.Format("Select Count(ErrorId) From ErrorLog Where ErrorId = {0}", id);

                    sqlConn.Open();

                    return (Int32)cmd.ExecuteScalar() > 0;
                }
                finally
                {
                    sqlConn.Close();
                }
            }
        }

        public static ProxyError[] GetErrors()
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
                                                        FROM ErrorLog");

                    sqlConn.Open();

                    return (ProxyError[])cmd.ExecuteScalar();
                }
                finally
                {
                    sqlConn.Close();
                }
            }
        }

        public static Image GetSnapshotById(int id)
        {
            using (var sqlConn = new SqlCeConnection(ConnectionString))
            using (var cmd = sqlConn.CreateCommand())
            {
                try
                {
                    cmd.CommandText = string.Format("Select [ScreenCapture] From ErrorLog Where ErrorId = {0}", id);

                    sqlConn.Open();

                    return (Image)cmd.ExecuteScalar();
                }
                finally
                {
                    sqlConn.Close();
                }
            }
        }

        public static void Delete(int id)
        {
            using (var sqlConn = new SqlCeConnection(ConnectionString))
            using (var cmd = sqlConn.CreateCommand())
            {
                try
                {
                    cmd.CommandText = string.Format("Delete From ErrorLog Where ErrorId = {0}", id);

                    sqlConn.Open();

                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    sqlConn.Close();
                }
            }
        }

        public static int Count()
        {
            using (var sqlConn = new SqlCeConnection(ConnectionString))
            using (var cmd = sqlConn.CreateCommand())
            {
                try
                {
                    cmd.CommandText = string.Format("Select Count(ErrorId) From ErrorLog");

                    sqlConn.Open();

                    return (Int32)cmd.ExecuteScalar();
                }
                finally
                {
                    sqlConn.Close();
                }
            }
        }
    }
}
