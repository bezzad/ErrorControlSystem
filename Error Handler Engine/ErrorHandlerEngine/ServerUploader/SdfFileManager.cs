using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlServerCe;
using ErrorHandlerEngine.ModelObjecting;

namespace ErrorHandlerEngine.ServerUploader
{
    class SdfFileManager
    {
        private static string _connString;

        public static void CreateSdf(string filePath)
        {
            _connString = string.Format("DataSource=\"{0}.sdf\"; Encrypt Database=True;", filePath);

            if (File.Exists(filePath)) return;

            new SqlCeEngine(_connString).CreateDatabase();

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


            using (var sqlCon = new SqlCeConnection(_connString))
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

        public static void InsertToSdf(Error error)
        {
            using (var sqlConn = new SqlCeConnection(_connString))
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
                cmd.Parameters.AddWithValue("@LineCol", error.ErrorLineColumn.ToString());
                cmd.Parameters.AddWithValue("@Snapshot", error.GetSnapshot().ToBytes());
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

        public static Error GetFromSdf(int id)
        {
            using (var sqlConn = new SqlCeConnection(_connString))
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
            using (var sqlConn = new SqlCeConnection(_connString))
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

        public static Error[] GetErrorsFromSdf()
        {
            using (var sqlConn = new SqlCeConnection(_connString))
            using (var cmd = sqlConn.CreateCommand())
            {
                try
                {
                    cmd.CommandText = string.Format("Select * From ErrorLog");

                    sqlConn.Open();

                    return (Error[])cmd.ExecuteScalar();
                }
                finally
                {
                    sqlConn.Close();
                }
            }
        }
    }
}
