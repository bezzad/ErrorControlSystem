using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using ConnectionsManager;
using ErrorHandlerEngine.ExceptionManager;
using ErrorHandlerEngine.ModelObjecting;

namespace ErrorHandlerEngine.ServerUploader
{
    public static class DataAccessLayer
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
                              @name nvarchar(256);
                     
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
                    @"-- ==================================================================================
-- Author:		<Author: Behzad Khosravifar> 
-- Create date: <Create Date: 1393-08-29> 
-- Description:	<Description: Error Control System Tables and Stored Procedures>
-- ==================================================================================
---------------------------------------------------------------------------------------------------------------------------------------------------
-- Create Table [dbo].[ErrorLog]    Script Date: 03/08/2015 12:22:27 ------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------
IF NOT  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ErrorLog]') AND type in (N'U')) 
BEGIN 
	DECLARE @ErrorLogTable NVARCHAR(MAX) 
	SET @ErrorLogTable =  
		'CREATE TABLE [dbo].[ErrorLog]( 
		[ErrorId] [bigint] IDENTITY(1,1) NOT NULL, 
		[ServerDateTime] [datetime] NULL, 
		[Host] [varchar](200) NULL, 
		[User] [varchar](200) NULL, 
		[IsHandled] [bit] NULL, 
		[Type] [varchar](100) NULL, 
		[AppName] [varchar](100) NULL, 
		[Data] [xml] NULL, 
		[CurrentCulture] [nvarchar](100) NULL, 
		[CLRVersion] [varchar](20) NULL, 
		[Message] [nvarchar](max) NULL, 
		[Source] [nvarchar](max) NULL, 
		[StackTrace] [nvarchar](max) NULL, 
		[ModuleName] [varchar](200) NULL, 
		[MemberType] [varchar](50) NULL, 
		[Method] [varchar](500) NULL, 
		[Processes] [varchar](max) NULL, 
		[ErrorDateTime] [datetime] NULL, 
		[OS] [varchar](1000) NULL, 
		[IPv4Address] [varchar](15) NULL, 
		[MACAddress] [varchar](50) NULL, 
		[HResult] [int] NULL, 
		[LineColumn] [varchar](50) NULL, 
		[DuplicateNo] [int] NULL, 
    	
		 CONSTRAINT [PK_ErrorLog] PRIMARY KEY CLUSTERED  
		( 
			[ErrorId] ASC 
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] 
		) ON [PRIMARY] 
		                    	           	 
		-- Object:  Default [DF_ErrorLog_ErrTime]    Script Date: 03/08/2015 12:22:27 
		ALTER TABLE [dbo].[ErrorLog] ADD  CONSTRAINT [DF_ErrorLog_ErrTime]  DEFAULT (getdate()) FOR [ServerDateTime] 
    	 
		-- Object:  Default [DF_ErrorLog_ErrorHost]    Script Date: 03/08/2015 12:22:27 
		ALTER TABLE [dbo].[ErrorLog] ADD  CONSTRAINT [DF_ErrorLog_ErrorHost]  DEFAULT (lower(host_name())) FOR [Host] 
    	 
		-- Object:  Default [DF_ErrorLog_ErrorSystemUser]    Script Date: 03/08/2015 12:22:27 
		ALTER TABLE [dbo].[ErrorLog] ADD  CONSTRAINT [DF_ErrorLog_ErrorSystemUser]  DEFAULT (lower(suser_sname())) FOR [User] 
    	 
		-- Object:  Default [DF_ErrorLog_IsHandled]    Script Date: 03/08/2015 12:22:27 
		ALTER TABLE [dbo].[ErrorLog] ADD  CONSTRAINT [DF_ErrorLog_IsHandled]  DEFAULT ((0)) FOR [IsHandled] 
    	 
		-- Object:  Default [DF_ErrorLog_Type]    Script Date: 03/08/2015 12:22:27 
		ALTER TABLE [dbo].[ErrorLog] ADD  CONSTRAINT [DF_ErrorLog_Type]  DEFAULT (''Exception'') FOR [Type] 
    	 
		-- Object:  Default [DF_ErrorLog_AppName]    Script Date: 03/08/2015 12:22:27 
		ALTER TABLE [dbo].[ErrorLog] ADD  CONSTRAINT [DF_ErrorLog_AppName]  DEFAULT (app_name()) FOR [AppName]' 
	             	
	 EXEC (@ErrorLogTable)			 
END 



---------------------------------------------------------------------------------------------------------------------------------------------------
-- Create Table [dbo].[Snapshots]    Script Date: 03/08/2015 12:22:27 
---------------------------------------------------------------------------------------------------------------------------------------------------
IF NOT  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Snapshots]') AND type in (N'U')) 
BEGIN 
	DECLARE @SnapshotsTable NVARCHAR(Max) 
	SET @SnapshotsTable =
		'CREATE TABLE [dbo].[Snapshots]( 
			[ErrorLogID] [int] NOT NULL, 
			[ScreenCapture] [image] NOT NULL, 
			 CONSTRAINT [PK_Snapshots] PRIMARY KEY CLUSTERED  
			( 
				[ErrorLogID] ASC 
			)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] 
			) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] ' 
		
	EXEC (@SnapshotsTable) 
END 
 
 
 
--------------------------------------------------------------------------------------------------------------------------------------------------- 
-- Create Error Control System Stored Procedure       Script Date: 03/08/2015 12:22:27 
---------------------------------------------------------------------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_InsertErrorLog]') AND type in (N'P', N'PC')) 
BEGIN	 
	DECLARE @sp_InsertErrorLog NVARCHAR(MAX) 
	SET @sp_InsertErrorLog = 
		'CREATE PROCEDURE [dbo].[sp_InsertErrorLog] 
		@ServerDateTime DATETIME, 
		@Host VARCHAR(200), 
		@User VARCHAR(200), 
		@IsHandled BIT, 
		@Type VARCHAR(100), 
		@AppName VARCHAR(100), 
		@ScreenCapture IMAGE = NULL, 
		@CurrentCulture NVARCHAR(100)  = NULL, 
		@CLRVersion VARCHAR(20) = NULL, 
		@Message NVARCHAR(MAX)  = NULL, 
		@Source NVARCHAR(MAX)  = NULL, 
		@StackTrace NVARCHAR(MAX)  = NULL, 
		@ModuleName VARCHAR(200)  = NULL, 
		@MemberType VARCHAR(50)  = NULL, 
		@Method VARCHAR(500)  = NULL, 
		@Processes VARCHAR(MAX)  = NULL, 
		@ErrorDateTime DATETIME, 
		@OS VARCHAR(1000)  = NULL, 
		@IPv4Address VARCHAR(15)  = NULL, 
		@MACAddress VARCHAR(50)  = NULL, 
		@HResult INT, 
		@LineCol VARCHAR(50)  = NULL, 
		@Duplicate INT, 
		@Data XML = NULL 
		AS 
		DECLARE @ErrorLogID INT = 0 
		Declare @TempTable table 
		( 
			Drive nvarchar(1) 
			, MBfree bigint 
		) 
		Declare @MyDrive nvarchar(1) 
		Declare @FreeSpace bigint 
		BEGIN  
			BEGIN TRY	 
				BEGIN TRANSACTION 
					-- Check the error exist or not? if exist then just update that 
					IF ( Select COUNT(ErrorId) FROM [ErrorLog]  
						 WHERE HResult = @HResult AND  
							   LineColumn = @LineCol AND  
							   Method = @Method AND 
							   [User] = @User) > 0 
						-- Update error object from ErrorLog table 
						UPDATE dbo.ErrorLog SET DuplicateNo += 1  
							WHERE HResult = @HResult AND  
								  LineColumn = @LineCol AND  
								  Method = @Method AND 
								  [User] = @User 
					ELSE 
						BEGIN	            
							-- Insert error object into ErrorLog table 
							INSERT  INTO [ErrorLog] 
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
									   ,[DuplicateNo] 
									   ,[Data]) 
							VALUES  (	@ServerDateTime 
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
									   ,@Duplicate 
									   ,@Data 
									) 
							-- Set AutoId of ErrorLog table to @ErrorLogID for use in Snapshots table         
							SELECT @ErrorLogID = SCOPE_IDENTITY()	 
	 
							-- Get Free Space Of Database Drive 
							Insert Into @TempTable 
							exec  xp_fixeddrives 
							Select @MyDrive = SUBSTRING(physical_name,1,1) from sys.database_files 
							Select @FreeSpace = MBfree from @TempTable where Drive = @MyDrive 
							If @FreeSpace > 10240 -- If grater than 10GB 
								-- Save snapshot image		 
								-- Insert into error image into Snapshot								 
								if @ScreenCapture is not null 
									INSERT INTO [Snapshots] 
										   ([ErrorLogID] 
										   ,[ScreenCapture]) 
										VALUES 
										   (@ErrorLogID 
										   ,@ScreenCapture) 
						END	 
				COMMIT TRANSACTION 
			END TRY 
			BEGIN CATCH 
				ROLLBACK TRANSACTION 
	 
				DECLARE @ERROR_NUMBER INT , 
					@ERROR_STATE INT , 
					@ERROR_LINE INT , 
					@ERROR_PROCEDURE NVARCHAR(126) , 
					@ERROR_MESSAGE NVARCHAR(4000) 
	 
				SELECT  @ERROR_NUMBER = ERROR_NUMBER() , 
						@ERROR_PROCEDURE = ERROR_PROCEDURE() , 
						@ERROR_LINE = ERROR_LINE() , 
						@ERROR_MESSAGE = ERROR_MESSAGE() , 
						@ERROR_STATE = ERROR_STATE() 
	 
				IF @ERROR_NUMBER <> 50000 
					INSERT  INTO UsersManagements.dbo.ErrorLog 
							( [Type], 
							  [LineColumn], 
							  [Message], 
							  [HResult], 
							  [Data] 
							) 
					VALUES  (  ''SqlException'' , 
							   @ERROR_LINE, 
							   @ERROR_MESSAGE, 
							   @ERROR_NUMBER, 
							   ( SELECT @ERROR_PROCEDURE [Procedure] 
							  FOR 
								XML PATH('''') , 
									ROOT(''Error'') 
							  ) 
							) 
	 
				RAISERROR(@ERROR_MESSAGE, 18, 255) 
			END CATCH 
		END'
		
	Exec (@sp_InsertErrorLog) 
END";

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

        internal static string GetFromResources(string resourceName)
        {
            var assem = Assembly.GetExecutingAssembly();

            using (var stream = assem.GetManifestResourceStream(assem.GetName().Name + '.' + resourceName))
            {
                if (stream == null) return string.Empty;

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
