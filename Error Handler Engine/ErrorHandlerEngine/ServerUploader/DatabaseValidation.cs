using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectionsManager;
using ErrorHandlerEngine.ExceptionManager;

namespace ErrorHandlerEngine.ServerUploader
{
    public class DatabaseValidation
    {
        public static void DropAndCreateInsertStoreProcedure()
        {
            var conn = ConnectionManager.GetDefaultConnection();
         
            // Create a command object identifying the stored procedure
            using (var cmd = conn.CreateSqlCommand())
            {
                //
                // Set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.Text;

                #region Command Text
                cmd.CommandText = @"-- Object:  StoredProcedure [dbo].[sp_InsertErrorLog]    Script Date: 03/05/2015 16:39:51 
                                    IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_InsertErrorLog]') AND type in (N'P', N'PC'))
                                    DROP PROCEDURE [dbo].[sp_InsertErrorLog]
                                    GO

                                    USE [UsersManagements]
                                    GO

                                    -- Object:  StoredProcedure [dbo].[sp_InsertErrorLog]    Script Date: 03/05/2015 16:39:51
                                    SET ANSI_NULLS ON
                                    GO

                                    SET QUOTED_IDENTIFIER ON
                                    GO



                                    -- =============================================
                                    -- Author:		<Author: Behzad Khosravifar>
                                    -- Create date: <Create Date: 1393-08-29>
                                    -- Description:	<Description: Error Control System Stored Procedure>
                                    -- =============================================
                                    CREATE PROCEDURE [dbo].[sp_InsertErrorLog]
                                        @DateTime DATETIME,
                                        @Host VARCHAR(200)  = NULL,
                                        @User VARCHAR(200) = NULL ,
                                        @IsHandled BIT = 0,
                                        @Type VARCHAR(100)  = NULL,
                                        @AppName VARCHAR(100)  = NULL,
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
								                                       ,[DuplicateNo]
                                                                       ,[Data])
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
                                                    VALUES  ( 'SqlException' ,
                                                               @ERROR_LINE,
                                                               @ERROR_MESSAGE,
                                                               @ERROR_NUMBER,
                                                               ( SELECT @ERROR_PROCEDURE [Procedure] ,
                                                                        @ERROR_STATE [State]
                                                              FOR
                                                                XML PATH('') ,
                                                                    ROOT('Error')
                                                              )
                                                            )
		
                                                RAISERROR(@ERROR_MESSAGE, 18, 255)
                                            END CATCH
                                        END
                                    GO
                                ";

                #endregion
                //
                // execute the command
                try
                {
                    ExceptionHandler.IsSelfException = true;
                    conn.Open();

                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    conn.Close();
                    ExceptionHandler.IsSelfException = false;
                }
            }
        }
    }
}
