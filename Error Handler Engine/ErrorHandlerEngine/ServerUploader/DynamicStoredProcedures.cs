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
        #region Insert Stored Procedure on SQL Database
        /*
        USE [UsersManagements]
        GO
        -- Object:  StoredProcedure [dbo].[sp_InsertErrorLog]    Script Date: 11/26/2014 09:45:56
        SET ANSI_NULLS ON
        GO
        SET QUOTED_IDENTIFIER ON
        GO
        -- =============================================
        -- Author:		<Author: Behzad Khosravifar>
        -- Create date: <Create Date: 1393-08-29>
        -- Description:	<Description: Error Control System Stored Procedure>
        -- =============================================
        ALTER PROCEDURE [dbo].[sp_InsertErrorLog]
            @DateTime DATETIME ,
            @Host VARCHAR(200) ,
            @User VARCHAR(200) ,
            @IsHandled BIT ,
            @Type VARCHAR(100) ,
            @AppName VARCHAR(100) ,
            @ScreenCapture IMAGE ,
            @CurrentCulture NVARCHAR(100) ,
            @CLRVersion VARCHAR(20) ,
            @Message NVARCHAR(MAX) ,
            @Source NVARCHAR(MAX) ,
            @StackTrace NVARCHAR(MAX) ,
            @ModuleName VARCHAR(200) ,
            @MemberType VARCHAR(50) ,
            @Method VARCHAR(500) ,
            @Processes VARCHAR(MAX) ,
            @ErrorDateTime DATETIME ,
            @OS VARCHAR(1000) ,
            @IPv4Address VARCHAR(15) ,
            @MACAddress VARCHAR(50) ,
            @HResult INT ,
            @LineCol VARCHAR(50) ,
            @Duplicate INT
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
            			         
            			           
			        -- Insert error object into ErrorLog table
                    INSERT  INTO [ErrorLog]
					           ([DateTime]
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
					           ,[DuplicateNo])
                    VALUES  ( @DateTime
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
                            )
                    -- Set AutoId of ErrorLog table to @ErrorLogID for use in Snapshots table        
			        SELECT @ErrorLogID = SCOPE_IDENTITY()	
					
			        -- Get Free Space Of Database Drive
			        Insert Into @TempTable
			        exec  xp_fixeddrives
			        Select @MyDrive = SUBSTRING(physical_name,1,1) from sys.database_files
			        Select @FreeSpace = MBfree from @TempTable where Drive = @MyDrive
			        If @FreeSpace > 10240 -- If grater than 10GB
				        Begin	-- Save snapshot image		
					        -- Insert into error image into Snapshot								
					        if @ScreenCapture is not null
						        INSERT INTO [Snapshots]
							           ([ErrorLogID]
							           ,[ScreenCapture])
						         VALUES
							           (@ErrorLogID
							           ,@ScreenCapture)
				        End            
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
                                ( [Type] ,
                                  [Data]
		                        )
                        VALUES  ( 'SqlException' ,
                                  ( SELECT  @ERROR_LINE LineNumber ,
                                            @ERROR_MESSAGE [Message] ,
                                            @ERROR_NUMBER Number ,
                                            @ERROR_PROCEDURE [Procedure]
                                  FOR
                                    XML PATH('') ,
                                        ROOT('Error')
                                  )
                                )
		
                    RAISERROR(@ERROR_MESSAGE, 18, 255)
                END CATCH
            END
         */
        #endregion

        public static void InsertErrorStoredProcedure(ProxyError error)
        {
            // Create a command object identifying the stored procedure
            using (var cmd = new SqlCommand("sp_InsertErrorLog"))
            {
                //
                // Set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;
                //
                // Add parameters to command, which will be passed to the stored procedure
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
                cmd.Parameters.AddWithValue("@Duplicate", error.Duplicate);
                //
                // execute the command
                try
                {
                    ExceptionHandler.IsSelfException = true;
                    ConnectionManager.GetDefaultConnection().ExecuteNonQuery(cmd);
                }
                finally
                {
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
                cmd.Parameters.AddWithValue("@DateTime", error.ServerDateTime);
                cmd.Parameters.AddWithValue("@Host", error.Host);
                cmd.Parameters.AddWithValue("@User", error.User);
                cmd.Parameters.AddWithValue("@IsHandled", error.IsHandled);
                cmd.Parameters.AddWithValue("@Type", error.ErrorType);
                cmd.Parameters.AddWithValue("@AppName", error.AppName);
                cmd.Parameters.AddWithValue("@ScreenCapture", error.Snapshot.Value.ToBytes());
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
