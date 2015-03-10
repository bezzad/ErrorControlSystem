Declare @logPath nvarchar(256),
        @dataPath nvarchar(256),
        @dbName nvarchar(256);
                     
SET @dbName = '#DatabaseName'
                     
SET @logPath = (select 
                    LEFT(physical_name, LEN(physical_name) - CHARINDEX('\', REVERSE(physical_name)) + 1) 
                from sys.master_files 
                where name = 'modellog') + @dbName + '.ldf'
                     
SET @dataPath = (select 
                    LEFT(physical_name, LEN(physical_name)  - CHARINDEX('\', REVERSE(physical_name)) + 1) 
                    from sys.master_files 
                    where name = 'modeldev') + @dbName + '_log.mdf'
                     
                     
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = @dbName)
BEGIN
	DECLARE @createDatabase NVARCHAR(MAX)
	SET @createDatabase = 'CREATE DATABASE [' + @dbName + '] ON  PRIMARY 
	    ( NAME = N''' + @dbName + ''', FILENAME = N''' + @dataPath + ''', SIZE = 51200KB , FILEGROWTH = 10240KB )
	    LOG ON 
	    ( NAME = N''' + @dbName + '' + '_log'' , FILENAME = N''' + @logPath + ''' , SIZE = 5120KB , FILEGROWTH = 5120KB )'
	                 
	EXEC sp_executesql @createDatabase
END