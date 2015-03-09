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

                #region Command Text
                cmd.CommandText =
                    @"Declare @logpath nvarchar(256),
                              @datapath nvarchar(256),
                              @name nvarchar(256),
                              @namelog nvarchar(256);
                     
                      set @name = '" + databaseName + @"'
                     
                      SET @logpath = (select 
                                          LEFT(physical_name, LEN(physical_name) - CHARINDEX('\', REVERSE(physical_name)) + 1) 
                                      from sys.master_files 
                                      where name = 'modellog') + @name + '.ldf'
                     
                      SET @datapath = (select 
                                          LEFT(physical_name, LEN(physical_name)  - CHARINDEX('\', REVERSE(physical_name)) + 1) 
                                          from sys.master_files 
                                          where name = 'modeldev') + @name + '_log.mdf'
                     
                     
                      IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = @name)
                      BEGIN
	                      DECLARE @createDatabase NVARCHAR(MAX)
	                      SET @createDatabase = 'CREATE DATABASE [' + @name + '] ON  PRIMARY 
	                          ( NAME = N''' + @name + ''', FILENAME = N''' + @datapath + ''', SIZE = 51200KB , FILEGROWTH = 10240KB )
	                          LOG ON 
	                          ( NAME = N''' + @name + '' + '_log'' , FILENAME = N''' + @logpath + ''' , SIZE = 5120KB , FILEGROWTH = 5120KB )'
	                 
	                      EXEC sp_executesql @createDatabase
                      END";

                #endregion
                //
                // execute the command
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

        public static async Task CreateTablesAndStoredProceduresAsync()
        {
            var conn = ConnectionManager.GetDefaultConnection();

            // Create a command object identifying the stored procedure
            using (var cmd = conn.CreateCommand())
            {
                //
                // Set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.Text;

                #region Command Text
                cmd.CommandText =
                    "-- Object:  Table [dbo].[ErrorLog]    Script Date: 03/08/2015 12:22:27 \n"
                    + "IF NOT  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ErrorLog]') AND type in (N'U')) \n"
                    + "BEGIN \n"
                    + "	DECLARE @ErrorLogTable NVARCHAR(MAX) \n"
                    + "	SET @ErrorLogTable =  \n"
                    + "	'CREATE TABLE [dbo].[ErrorLog]( \n"
                    + "	[ErrorId] [bigint] IDENTITY(1,1) NOT NULL, \n"
                    + "	[ServerDateTime] [datetime] NULL, \n"
                    + "	[Host] [varchar](200) NULL, \n"
                    + "	[User] [varchar](200) NULL, \n"
                    + "	[IsHandled] [bit] NULL, \n"
                    + "	[Type] [varchar](100) NULL, \n"
                    + "	[AppName] [varchar](100) NULL, \n"
                    + "	[Data] [xml] NULL, \n"
                    + "	[CurrentCulture] [nvarchar](100) NULL, \n"
                    + "	[CLRVersion] [varchar](20) NULL, \n"
                    + "	[Message] [nvarchar](max) NULL, \n"
                    + "	[Source] [nvarchar](max) NULL, \n"
                    + "	[StackTrace] [nvarchar](max) NULL, \n"
                    + "	[ModuleName] [varchar](200) NULL, \n"
                    + "	[MemberType] [varchar](50) NULL, \n"
                    + "	[Method] [varchar](500) NULL, \n"
                    + "	[Processes] [varchar](max) NULL, \n"
                    + "	[ErrorDateTime] [datetime] NULL, \n"
                    + "	[OS] [varchar](1000) NULL, \n"
                    + "	[IPv4Address] [varchar](15) NULL, \n"
                    + "	[MACAddress] [varchar](50) NULL, \n"
                    + "	[HResult] [int] NULL, \n"
                    + "	[LineColumn] [varchar](50) NULL, \n"
                    + "	[DuplicateNo] [int] NULL, \n"
                    + " CONSTRAINT [PK_ErrorLog] PRIMARY KEY CLUSTERED  \n"
                    + "( \n"
                    + "	[ErrorId] ASC \n"
                    + ")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] \n"
                    + ") ON [PRIMARY] \n"
                    + "	SET ANSI_PADDING OFF \n"
                    + "	 \n"
                    + "	EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''The Error Identifier'' , @level0type=N''SCHEMA'',@level0name=N''dbo'', @level1type=N''TABLE'',@level1name=N''ErrorLog'', @level2type=N''COLUMN'',@level2name=N''ErrorId'' \n"
                    + "	 \n"
                    + "	EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''The Server DataTime in Exception Occurrence Time'' , @level0type=N''SCHEMA'',@level0name=N''dbo'', @level1type=N''TABLE'',@level1name=N''ErrorLog'', @level2type=N''COLUMN'',@level2name=N''ServerDateTime'' \n"
                    + "	 \n"
                    + "	EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''The Client Machine Name'' , @level0type=N''SCHEMA'',@level0name=N''dbo'', @level1type=N''TABLE'',@level1name=N''ErrorLog'', @level2type=N''COLUMN'',@level2name=N''Host'' \n"
                    + "	 \n"
                    + "	EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''The Client DomainName\\UserName'' , @level0type=N''SCHEMA'',@level0name=N''dbo'', @level1type=N''TABLE'',@level1name=N''ErrorLog'', @level2type=N''COLUMN'',@level2name=N''User'' \n"
                    + "	 \n"
                    + "	EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Is Handled?'' , @level0type=N''SCHEMA'',@level0name=N''dbo'', @level1type=N''TABLE'',@level1name=N''ErrorLog'', @level2type=N''COLUMN'',@level2name=N''IsHandled'' \n"
                    + "	 \n"
                    + "	EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Exception Type'' , @level0type=N''SCHEMA'',@level0name=N''dbo'', @level1type=N''TABLE'',@level1name=N''ErrorLog'', @level2type=N''COLUMN'',@level2name=N''Type'' \n"
                    + "	 \n"
                    + "	EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Application Name'' , @level0type=N''SCHEMA'',@level0name=N''dbo'', @level1type=N''TABLE'',@level1name=N''ErrorLog'', @level2type=N''COLUMN'',@level2name=N''AppName'' \n"
                    + "	 \n"
                    + "	EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Extra Properties and Exception Data'' , @level0type=N''SCHEMA'',@level0name=N''dbo'', @level1type=N''TABLE'',@level1name=N''ErrorLog'', @level2type=N''COLUMN'',@level2name=N''Data'' \n"
                    + "	 \n"
                    + "	EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Error log Table for store applications exceptions data'' , @level0type=N''SCHEMA'',@level0name=N''dbo'', @level1type=N''TABLE'',@level1name=N''ErrorLog'' \n"
                    + "	 \n"
                    + "	-- Object:  StoredProcedure [dbo].[sp_InsertErrorLog]    Script Date: 03/08/2015 12:22:28 \n"
                    + "	SET ANSI_NULLS ON \n"
                    + "	 \n"
                    + "	SET QUOTED_IDENTIFIER ON' \n"
                    + "	 \n"
                    + "	EXEC (@ErrorLogTable)			 \n"
                    + "END \n"
                    + " \n"
                    + "-- Object:  Table [dbo].[Snapshots]    Script Date: 03/08/2015 12:22:27 \n"
                    + "IF NOT  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Snapshots]') AND type in (N'U')) \n"
                    + "BEGIN \n"
                    + "	DECLARE @SnapshotsTable NVARCHAR(Max) \n"
                    + "	SET @SnapshotsTable = ' \n"
                    + "	CREATE TABLE [dbo].[Snapshots]( \n"
                    + "		[ErrorLogID] [int] NOT NULL, \n"
                    + "		[ScreenCapture] [image] NOT NULL, \n"
                    + "	 CONSTRAINT [PK_Snapshots] PRIMARY KEY CLUSTERED  \n"
                    + "	( \n"
                    + "		[ErrorLogID] ASC \n"
                    + "	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] \n"
                    + "	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] \n"
                    + "	' \n"
                    + "	EXEC (@SnapshotsTable) \n"
                    + "END \n"
                    + " \n"
                    + "-- ============================================= \n"
                    + "-- Author:		<Author: Behzad Khosravifar> \n"
                    + "-- Create date: <Create Date: 1393-08-29> \n"
                    + "-- Description:	<Description: Error Control System Stored Procedure> \n"
                    + "-- ============================================= \n"
                    + "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_InsertErrorLog]') AND type in (N'P', N'PC')) \n"
                    + "BEGIN	 \n"
                    + "	DECLARE @sp_InsertErrorLog NVARCHAR(MAX) \n"
                    + "	SET @sp_InsertErrorLog = ' \n"
                    + "	CREATE PROCEDURE [dbo].[sp_InsertErrorLog] \n"
                    + "	@ServerDateTime DATETIME, \n"
                    + "	@Host VARCHAR(200)  = NULL, \n"
                    + "	@User VARCHAR(200) = NULL , \n"
                    + "	@IsHandled BIT = 0, \n"
                    + "	@Type VARCHAR(100)  = NULL, \n"
                    + "	@AppName VARCHAR(100)  = NULL, \n"
                    + "	@ScreenCapture IMAGE = NULL, \n"
                    + "	@CurrentCulture NVARCHAR(100)  = NULL, \n"
                    + "	@CLRVersion VARCHAR(20) = NULL, \n"
                    + "	@Message NVARCHAR(MAX)  = NULL, \n"
                    + "	@Source NVARCHAR(MAX)  = NULL, \n"
                    + "	@StackTrace NVARCHAR(MAX)  = NULL, \n"
                    + "	@ModuleName VARCHAR(200)  = NULL, \n"
                    + "	@MemberType VARCHAR(50)  = NULL, \n"
                    + "	@Method VARCHAR(500)  = NULL, \n"
                    + "	@Processes VARCHAR(MAX)  = NULL, \n"
                    + "	@ErrorDateTime DATETIME, \n"
                    + "	@OS VARCHAR(1000)  = NULL, \n"
                    + "	@IPv4Address VARCHAR(15)  = NULL, \n"
                    + "	@MACAddress VARCHAR(50)  = NULL, \n"
                    + "	@HResult INT, \n"
                    + "	@LineCol VARCHAR(50)  = NULL, \n"
                    + "	@Duplicate INT, \n"
                    + "	@Data XML = NULL \n"
                    + "	AS \n"
                    + "	DECLARE @ErrorLogID INT = 0 \n"
                    + "	Declare @TempTable table \n"
                    + "	( \n"
                    + "		Drive nvarchar(1) \n"
                    + "		, MBfree bigint \n"
                    + "	) \n"
                    + "	Declare @MyDrive nvarchar(1) \n"
                    + "	Declare @FreeSpace bigint \n"
                    + "	BEGIN  \n"
                    + "		BEGIN TRY	 \n"
                    + "			BEGIN TRANSACTION \n"
                    + "				-- Check the error exist or not? if exist then just update that \n"
                    + "				IF ( Select COUNT(ErrorId) FROM [ErrorLog]  \n"
                    + "					 WHERE HResult = @HResult AND  \n"
                    + "						   LineColumn = @LineCol AND  \n"
                    + "						   Method = @Method AND \n"
                    + "						   [User] = @User) > 0 \n"
                    + "					-- Update error object from ErrorLog table \n"
                    + "					UPDATE dbo.ErrorLog SET DuplicateNo += 1  \n"
                    + "						WHERE HResult = @HResult AND  \n"
                    + "							  LineColumn = @LineCol AND  \n"
                    + "							  Method = @Method AND \n"
                    + "							  [User] = @User \n"
                    + "				ELSE \n"
                    + "					BEGIN	            \n"
                    + "						-- Insert error object into ErrorLog table \n"
                    + "						INSERT  INTO [ErrorLog] \n"
                    + "								   ([ServerDateTime] \n"
                    + "								   ,[Host] \n"
                    + "								   ,[User] \n"
                    + "								   ,[IsHandled] \n"
                    + "								   ,[Type] \n"
                    + "								   ,[AppName] \n"
                    + "								   ,[CurrentCulture] \n"
                    + "								   ,[CLRVersion] \n"
                    + "								   ,[Message] \n"
                    + "								   ,[Source] \n"
                    + "								   ,[StackTrace] \n"
                    + "								   ,[ModuleName] \n"
                    + "								   ,[MemberType] \n"
                    + "								   ,[Method] \n"
                    + "								   ,[Processes] \n"
                    + "								   ,[ErrorDateTime] \n"
                    + "								   ,[OS] \n"
                    + "								   ,[IPv4Address] \n"
                    + "								   ,[MACAddress] \n"
                    + "								   ,[HResult] \n"
                    + "								   ,[LineColumn] \n"
                    + "								   ,[DuplicateNo] \n"
                    + "								   ,[Data]) \n"
                    + "						VALUES  (	@ServerDateTime \n"
                    + "								   ,@Host \n"
                    + "								   ,@User \n"
                    + "								   ,@IsHandled \n"
                    + "								   ,@Type \n"
                    + "								   ,@AppName \n"
                    + "								   ,@CurrentCulture \n"
                    + "								   ,@CLRVersion \n"
                    + "								   ,@Message \n"
                    + "								   ,@Source \n"
                    + "								   ,@StackTrace \n"
                    + "								   ,@ModuleName \n"
                    + "								   ,@MemberType \n"
                    + "								   ,@Method \n"
                    + "								   ,@Processes \n"
                    + "								   ,@ErrorDateTime \n"
                    + "								   ,@OS \n"
                    + "								   ,@IPv4Address \n"
                    + "								   ,@MACAddress \n"
                    + "								   ,@HResult \n"
                    + "								   ,@LineCol \n"
                    + "								   ,@Duplicate \n"
                    + "								   ,@Data \n"
                    + "								) \n"
                    + "						-- Set AutoId of ErrorLog table to @ErrorLogID for use in Snapshots table         \n"
                    + "						SELECT @ErrorLogID = SCOPE_IDENTITY()	 \n"
                    + " \n"
                    + "						-- Get Free Space Of Database Drive \n"
                    + "						Insert Into @TempTable \n"
                    + "						exec  xp_fixeddrives \n"
                    + "						Select @MyDrive = SUBSTRING(physical_name,1,1) from sys.database_files \n"
                    + "						Select @FreeSpace = MBfree from @TempTable where Drive = @MyDrive \n"
                    + "						If @FreeSpace > 10240 -- If grater than 10GB \n"
                    + "							-- Save snapshot image		 \n"
                    + "							-- Insert into error image into Snapshot								 \n"
                    + "							if @ScreenCapture is not null \n"
                    + "								INSERT INTO [Snapshots] \n"
                    + "									   ([ErrorLogID] \n"
                    + "									   ,[ScreenCapture]) \n"
                    + "									VALUES \n"
                    + "									   (@ErrorLogID \n"
                    + "									   ,@ScreenCapture) \n"
                    + "					END	 \n"
                    + "			COMMIT TRANSACTION \n"
                    + "		END TRY \n"
                    + "		BEGIN CATCH \n"
                    + "			ROLLBACK TRANSACTION \n"
                    + " \n"
                    + "			DECLARE @ERROR_NUMBER INT , \n"
                    + "				@ERROR_STATE INT , \n"
                    + "				@ERROR_LINE INT , \n"
                    + "				@ERROR_PROCEDURE NVARCHAR(126) , \n"
                    + "				@ERROR_MESSAGE NVARCHAR(4000) \n"
                    + " \n"
                    + "			SELECT  @ERROR_NUMBER = ERROR_NUMBER() , \n"
                    + "					@ERROR_PROCEDURE = ERROR_PROCEDURE() , \n"
                    + "					@ERROR_LINE = ERROR_LINE() , \n"
                    + "					@ERROR_MESSAGE = ERROR_MESSAGE() , \n"
                    + "					@ERROR_STATE = ERROR_STATE() \n"
                    + " \n"
                    + "			IF @ERROR_NUMBER <> 50000 \n"
                    + "				INSERT  INTO UsersManagements.dbo.ErrorLog \n"
                    + "						( [Type], \n"
                    + "						  [LineColumn], \n"
                    + "						  [Message], \n"
                    + "						  [HResult], \n"
                    + "						  [Data] \n"
                    + "						) \n"
                    + "				VALUES  (  ''SqlException'' , \n"
                    + "						   @ERROR_LINE, \n"
                    + "						   @ERROR_MESSAGE, \n"
                    + "						   @ERROR_NUMBER, \n"
                    + "						   ( SELECT @ERROR_PROCEDURE [Procedure] \n"
                    + "						  FOR \n"
                    + "							XML PATH('''') , \n"
                    + "								ROOT(''Error'') \n"
                    + "						  ) \n"
                    + "						) \n"
                    + " \n"
                    + "			RAISERROR(@ERROR_MESSAGE, 18, 255) \n"
                    + "		END CATCH \n"
                    + "	END \n"
                    + " \n"
                    + "	-- Object:  Default [DF_ErrorLog_ErrTime]    Script Date: 03/08/2015 12:22:27 \n"
                    + "	ALTER TABLE [dbo].[ErrorLog] ADD  CONSTRAINT [DF_ErrorLog_ErrTime]  DEFAULT (getdate()) FOR [ServerDateTime] \n"
                    + "	 \n"
                    + "	-- Object:  Default [DF_ErrorLog_ErrorHost]    Script Date: 03/08/2015 12:22:27 \n"
                    + "	ALTER TABLE [dbo].[ErrorLog] ADD  CONSTRAINT [DF_ErrorLog_ErrorHost]  DEFAULT (lower(host_name())) FOR [Host] \n"
                    + "	 \n"
                    + "	-- Object:  Default [DF_ErrorLog_ErrorSystemUser]    Script Date: 03/08/2015 12:22:27 \n"
                    + "	ALTER TABLE [dbo].[ErrorLog] ADD  CONSTRAINT [DF_ErrorLog_ErrorSystemUser]  DEFAULT (lower(suser_sname())) FOR [User] \n"
                    + "	 \n"
                    + "	-- Object:  Default [DF_ErrorLog_IsHandled]    Script Date: 03/08/2015 12:22:27 \n"
                    + "	ALTER TABLE [dbo].[ErrorLog] ADD  CONSTRAINT [DF_ErrorLog_IsHandled]  DEFAULT ((0)) FOR [IsHandled] \n"
                    + "	 \n"
                    + "	-- Object:  Default [DF_ErrorLog_Type]    Script Date: 03/08/2015 12:22:27 \n"
                    + "	ALTER TABLE [dbo].[ErrorLog] ADD  CONSTRAINT [DF_ErrorLog_Type]  DEFAULT (''Exception'') FOR [Type] \n"
                    + "	 \n"
                    + "	-- Object:  Default [DF_ErrorLog_AppName]    Script Date: 03/08/2015 12:22:27 \n"
                    + "	ALTER TABLE [dbo].[ErrorLog] ADD  CONSTRAINT [DF_ErrorLog_AppName]  DEFAULT (app_name()) FOR [AppName]	' \n"
                    + "	Exec (@sp_InsertErrorLog) \n"
                    + "END";

                #endregion
                //
                // execute the command
                try
                {
                    ExceptionHandler.IsSelfException = true;
                    await conn.OpenAsync();

                    await cmd.ExecuteNonQueryAsync();
                }
                finally
                {
                    conn.Close();
                    ExceptionHandler.IsSelfException = false;
                }
            }
        }

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
