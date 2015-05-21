
-- ==================================================================================
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
		[Host] [nvarchar](200) NULL, 
		[User] [nvarchar](200) NULL, 
		[IsHandled] [bit] NULL, 
		[Type] [varchar](200) NULL, 
		[AppName] [nvarchar](200) NULL, 
		[Data] [xml] NULL, 
		[CurrentCulture] [nvarchar](200) NULL, 
		[CLRVersion] [varchar](100) NULL, 
		[Message] [nvarchar](max) NULL, 
		[Source] [nvarchar](max) NULL, 
		[StackTrace] [nvarchar](max) NULL, 
		[ModuleName] [varchar](200) NULL, 
		[MemberType] [varchar](200) NULL, 
		[Method] [varchar](500) NULL, 
		[Processes] [nvarchar](max) NULL, 
		[ErrorDateTime] [datetime] NULL, 
		[OS] [varchar](1000) NULL, 
		[IPv4Address] [varchar](50) NULL, 
		[MACAddress] [varchar](50) NULL, 
		[HResult] [int] NULL, 
		[Line] [int] NULL, 
		[Column] [int] NULL,
		[DuplicateNo] [int] NULL, 
    	
		 CONSTRAINT [PK_ErrorLog] PRIMARY KEY CLUSTERED  
		( 
			[ErrorId] ASC 
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] 
		) ON [PRIMARY] 
		                    	           	 
		-- Object:  Default [DF_ErrorLog_ErrTime]    Script Date: 03/08/2015 12:22:27 
		ALTER TABLE [dbo].[ErrorLog] ADD  CONSTRAINT [DF_ErrorLog_ErrTime]  DEFAULT (getdate()) FOR [ServerDateTime] 
    	 
		-- Object:  Default [DF_ErrorLog_ErrorHost]    Script Date: 03/08/2015 12:22:27 
		ALTER TABLE [dbo].[ErrorLog] ADD  CONSTRAINT [DF_ErrorLog_ErrorHost]  DEFAULT (host_name()) FOR [Host] 
    	 
		-- Object:  Default [DF_ErrorLog_ErrorSystemUser]    Script Date: 03/08/2015 12:22:27 
		ALTER TABLE [dbo].[ErrorLog] ADD  CONSTRAINT [DF_ErrorLog_ErrorSystemUser]  DEFAULT (suser_sname()) FOR [User] 
    	 
		-- Object:  Default [DF_ErrorLog_IsHandled]    Script Date: 03/08/2015 12:22:27 
		ALTER TABLE [dbo].[ErrorLog] ADD  CONSTRAINT [DF_ErrorLog_IsHandled]  DEFAULT ((0)) FOR [IsHandled] 
    	 
		-- Object:  Default [DF_ErrorLog_Type]    Script Date: 03/08/2015 12:22:27 
		ALTER TABLE [dbo].[ErrorLog] ADD  CONSTRAINT [DF_ErrorLog_Type]  DEFAULT (''Exception'') FOR [Type] 
    	 
		-- Object:  Default [DF_ErrorLog_AppName]    Script Date: 03/08/2015 12:22:27 
		ALTER TABLE [dbo].[ErrorLog] ADD  CONSTRAINT [DF_ErrorLog_AppName]  DEFAULT (app_name()) FOR [AppName]
	             	
		-- Object:  Default [DF_ErrorLog_IPv4Address]    Script Date: 03/10/2015 12:44:27 
		ALTER TABLE [dbo].[ErrorLog] ADD  CONSTRAINT [DF_ErrorLog_IPv4Address]  DEFAULT (''Network Not Available'') FOR [IPv4Address]

		-- Object:  Default [DF_ErrorLog_MACAddress]    Script Date: 03/10/2015 12:45:27
		ALTER TABLE [dbo].[ErrorLog] ADD  CONSTRAINT [DF_ErrorLog_MACAddress]  DEFAULT (''Network Not Available'') FOR [MACAddress] '

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
-- Create StoredProcedure [dbo].[sp_CatchError]       Script Date: 05/16/2015 22:02:01 
---------------------------------------------------------------------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CatchError]') AND type in (N'P', N'PC')) 
BEGIN	 
	DECLARE @sp_CatchError NVARCHAR(MAX) 
	SET @sp_CatchError = 
		'CREATE PROCEDURE [dbo].[sp_CatchError] 
		@RaisError bit,
		@ExtraData NVARCHAR(max) = NULL,
		@ErrorId BIGINT OUTPUT
		AS
		BEGIN			
			DECLARE 
				@DatabaseName NVARCHAR(max) = IsNull(Original_DB_NAME(), DB_NAME()),
				@ERROR_NUMBER INT = ERROR_NUMBER() , -- @@ERROR
				@ERROR_STATE INT = ERROR_STATE() ,
				@ERROR_SEVERITY INT = ERROR_SEVERITY(),
				@ERROR_LINE INT = ERROR_LINE() , 
				@ERROR_Column INT = 0,
				@ERROR_PROCEDURE SysName = ISNULL(ERROR_PROCEDURE() ,  ''New Query''),
				@ERROR_MESSAGE NVARCHAR(max) = ERROR_MESSAGE(),			
				@Source NVARCHAR(1024) = @@SERVERNAME,
				@IP_Address SysName = (SELECT client_net_address FROM SYS.DM_EXEC_CONNECTIONS WHERE SESSION_ID = @@SPID),
				@MAC_Address SysName = (SELECT net_address from sys.sysprocesses where spid = @@SPID),
				@Culture SysName = @@LANGUAGE,
				@OS NVARCHAR(max) = @@Version,
				@ClrVersion SysName = (SELECT CONVERT(sysname, SERVERPROPERTY(''BuildClrVersion''))),
				@ErrorDate DateTime = GetDate(),
				@IsHandled bit = 1,
				@ErrorType SysName = ''SqlException'',
				@UserName SysName = suser_sname(),
				@MemberType SysName = ''Stored Procedure'';

			IF @ERROR_NUMBER <> 50000 
				-- Check the error exist or not? if exist then only update that 
				IF ( Select 1 FROM [ErrorLog]  
						WHERE HResult = @ERROR_NUMBER AND  
							Line = @ERROR_LINE AND
							Method = @ERROR_PROCEDURE AND 
							[User] = @UserName) > 0 
					-- Update error object from ErrorLog table 
					UPDATE dbo.ErrorLog 
					SET DuplicateNo += 1, @ErrorId = ErrorId	  
						WHERE 
							HResult = @ERROR_NUMBER AND  
							Line = @ERROR_LINE AND
							Method = @ERROR_PROCEDURE AND 
							[User] = @UserName;	
				ELSE 
					Begin					
						INSERT  INTO [ErrorLog]
								(  
									[OS],
									[User],
									[CLRVersion],
									[ErrorDateTime],
									[IsHandled],
									[Type], 
									[Line], 
									[Column],
									[Message], 
									[HResult], 
									[Source],
									[Method],
									[ModuleName],
									[IPv4Address],
									[MACAddress],
									[MemberType],
									[CurrentCulture],
									[DuplicateNo],
									[Data] 
								) 
						VALUES  (  
									@OS,
									@UserName,
									@ClrVersion,
									@ErrorDate,
									@IsHandled,
									@ErrorType, 
									@ERROR_LINE, 
									@ERROR_Column,
									@ERROR_MESSAGE, 
									@ERROR_NUMBER,
									@Source, 
									@ERROR_PROCEDURE,
									@DatabaseName,
									@IP_Address,
									@MAC_Address,
									@MemberType,
									@Culture,
									0,
									( 
										SELECT 
											@ExtraData [ExtraData],
											@ERROR_SEVERITY [SEVERITY],
											@ERROR_STATE [STATE]
										FOR 
										XML PATH('''') ,
											ROOT(''Error'') 
									) 
								)
						-- Set AutoId of ErrorLog table to @ErrorLogID for use in Snapshots table         
						SELECT @ErrorId = SCOPE_IDENTITY()	
					END

				If @RaisError = 1
					RAISERROR(@ERROR_MESSAGE, 18, 255) 
			
			RETURN
		END'
		
	Exec (@sp_CatchError) 
END



--------------------------------------------------------------------------------------------------------------------------------------------------- 
-- Create StoredProcedure [dbo].[sp_InsertErrorLog]       Script Date: 05/16/2015 22:02:02
---------------------------------------------------------------------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_InsertErrorLog]') AND type in (N'P', N'PC')) 
BEGIN	 
	DECLARE @sp_InsertErrorLog NVARCHAR(MAX) 
	SET @sp_InsertErrorLog = 
		'CREATE PROCEDURE [dbo].[sp_InsertErrorLog]
		@ServerDateTime DATETIME,
		@Host SYSNAME,
		@User SYSNAME,
		@IsHandled BIT,
		@Type VARCHAR(200),
		@AppName VARCHAR(200),
		@ScreenCapture IMAGE = NULL,
		@CurrentCulture SYSNAME = NULL,
		@CLRVersion SYSNAME = NULL,
		@Message NVARCHAR(MAX) = NULL,
		@Source NVARCHAR(MAX) = NULL,
		@StackTrace NVARCHAR(MAX) = NULL,
		@ModuleName VARCHAR(200) = NULL,
		@MemberType SYSNAME = NULL,
		@Method VARCHAR(500) = NULL,
		@Processes VARCHAR(MAX) = NULL,
		@ErrorDateTime DATETIME,
		@OS VARCHAR(1000) = NULL,
		@IPv4Address SYSNAME = NULL,
		@MACAddress SYSNAME = NULL,
		@HResult INT,
		@Line INT,
		@Column INT,
		@Duplicate INT,
		@Data XML = NULL
	AS
		DECLARE @ErrorLogID INT = 0 
		DECLARE @TempTable TABLE 
				(Drive NVARCHAR(1), MBfree BIGINT)
	
		DECLARE @MyDrive NVARCHAR(1) 
		DECLARE @FreeSpace BIGINT 
		BEGIN
			BEGIN TRY
				BEGIN TRANSACTION
				-- Check the error exist or not? if exist then only update that 
				IF (
						SELECT COUNT(ErrorId)
						FROM   [ErrorLog]
						WHERE  HResult        = @HResult
								AND Line       = @Line
								AND Method     = @Method
								AND [User]     = @User
					) > 0
					-- Update error object from ErrorLog table 
					UPDATE dbo.ErrorLog
					SET    DuplicateNo += 1
					WHERE  HResult = @HResult
							AND Line = @Line
							AND Method = @Method
							AND [User] = @User
				ELSE
				BEGIN
					-- Insert error object into ErrorLog table 
					INSERT INTO [ErrorLog]
						(
						[ServerDateTime],
						[Host],
						[User],
						[IsHandled],
						[Type],
						[AppName],
						[CurrentCulture],
						[CLRVersion],
						[Message],
						[Source],
						[StackTrace],
						[ModuleName],
						[MemberType],
						[Method],
						[Processes],
						[ErrorDateTime],
						[OS],
						[IPv4Address],
						[MACAddress],
						[HResult],
						[Line],
						[Column],
						[DuplicateNo],
						[Data]
						)
					VALUES
						(
						@ServerDateTime,
						@Host,
						@User,
						@IsHandled,
						@Type,
						@AppName,
						@CurrentCulture,
						@CLRVersion,
						@Message,
						@Source,
						@StackTrace,
						@ModuleName,
						@MemberType,
						@Method,
						@Processes,
						@ErrorDateTime,
						@OS,
						@IPv4Address,
						@MACAddress,
						@HResult,
						@Line,
						@Column,
						@Duplicate,
						@Data
						) 
					-- Set AutoId of ErrorLog table to @ErrorLogID for use in Snapshots table         
					SELECT @ErrorLogID = SCOPE_IDENTITY() 
			    
					-- Get Free Space Of Database Drive 
					INSERT INTO @TempTable
					EXEC xp_fixeddrives
			    
					SELECT @MyDrive = SUBSTRING(physical_name, 1, 1)
					FROM   sys.database_files
			    
					SELECT @FreeSpace = MBfree
					FROM   @TempTable
					WHERE  Drive = @MyDrive
			    
					IF @FreeSpace > 2048
						-- If grater than 2GB
						-- Save snapshot image
						-- Insert into error image into Snapshot								 
						IF @ScreenCapture IS NOT NULL
							INSERT INTO [Snapshots]
								(
								[ErrorLogID],
								[ScreenCapture]
								)
							VALUES
								(
								@ErrorLogID,
								@ScreenCapture
								)
				END 
				COMMIT TRANSACTION
			END TRY 
			BEGIN CATCH
				ROLLBACK TRANSACTION 
				EXEC UsersManagements.dbo.sp_CatchError
					@RaisError = 0, -- Do not Raiserror again to client
					@ExtraData = null,
					@ErrorId = null
			END CATCH
		END'
		
	Exec (@sp_InsertErrorLog) 
END



--------------------------------------------------------------------------------------------------------------------------------------------------- 
-- Create UserDefinedFunction [dbo].[GetErrorHandlingOptions]      Script Date: 03/12/2015 17:25:26 
---------------------------------------------------------------------------------------------------------------------------------------------------
IF Not EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetErrorHandlingOptions]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DECLARE @fnCreator nvarchar(MAX) =
		'CREATE	FUNCTION [dbo].[GetErrorHandlingOptions] ( )
		RETURNS INT As

		BEGIN
			DECLARE @AppName NVARCHAR(200) = APP_NAME();
			DECLARE @UserName NVARCHAR(200) = SYSTEM_USER;

			DECLARE @None INT = 0,
					@All INT = 0xFFFF,
					@DisplayUnhandledExceptions INT = 1,
					@ReportHandledExceptions INT = 2,
					@Snapshot INT = 4,
					@FetchServerDateTime INT = 8,
					@ResizeSnapshots INT = 16,
					@EnableNetworkSending INT = 32,
					@FilterExceptions INT = 64,
					@ExitApplicationImmediately INT = 128,
					@HandleProcessCorruptedStateExceptions INT = 256,
					@Default INT,
					@DisplayDeveloperUI INT = 512;
					
			SET @Default =  (@All - @ExitApplicationImmediately - @HandleProcessCorruptedStateExceptions);

			IF(CHARINDEX(''TestWinFormDotNet45'', @AppName) = 1 AND @UserName = ''DBITABRIZ\khosravifar.b'')
				RETURN (@Default)

			IF(CHARINDEX(''TestWinFormDotNet45'', @AppName) = 1)
				RETURN (@Default)
				
			RETURN @None
		END'
	EXEC (@fnCreator);
END