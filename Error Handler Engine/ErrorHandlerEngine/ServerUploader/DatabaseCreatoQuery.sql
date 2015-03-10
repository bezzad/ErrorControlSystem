Declare @logpath nvarchar(256),
        @datapath nvarchar(256),
        @name nvarchar(256);
                     
-- set @name = '" + databaseName + @"'
                     
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
END