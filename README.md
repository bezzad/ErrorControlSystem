# [Error Control System](https://BehzadKhosravifar@bitbucket.org/BehzadKhosravifar/error-control-system.git)  README #
---------------------
### What Is This? ###

This is a C#.Net project's for manage __Exceptions__ of .Net applications by handling and 
logging that. This is a dll modules to provide error handling worker on your app background's.
The modules strive to be simple, well documented and
modification friendly, in order to help developers quickly learn their inner
workings.

NOTE: 
> This program is only for exception handling of Windows-based applications 
> that are written by .NET.


--------------------------------
### How To Use The Examples ###

To interact with this project, at first find main class of your project to 
add this module. Then, at the beginning of the instructions before any other 
user codes, enter the following command to invoke and run the module.

This is initializer codes of the module by `C# language`:
 
```csharp

using System;
using System.Windows.Forms;
using ConnectionsManager;
using ErrorControlSystem.ErrorHandlerEngine;

namespace TestApplication
{
    static class Program
    {
        [STAThread]
        private static void Main()
        {
               ExpHandlerEngine.Start(new Connection("localhost", "UsersManagements"), 
                                ExceptionHandlerOption.All & ~ExceptionHandlerOption.ReSizeSnapshots);

               Application.Run(new Form1());
        }
    }
}
```

In the above code snippet you've seen that, for the `ExpHandlerEngine` method should be an option. This option is used to specify the error data, which includes the following values:

-  All
-  AlertUnHandledError
-  Default (AlertUnHandledError + FetchServerDateTime + Snapshot + ReSizeSnapshots + SendCacheToServer)
-  FetchServerDateTime
-  IsHandled
-  None
-  ReSizeSnapshots
-  SendCacheToServer
-  Snapshot

For example in above codes, this code means is:
```csharp
ExpHandlerEngine.Start(new Connection("localhost", "UsersManagements"), 
                ExceptionHandlerOption.All & ~ExceptionHandlerOption.ReSizeSnapshots);
```
Select all options by excepted `ReSizeSnapshots`

By adding the our module starter code to the beginning of your program code, you can raise all exceptions history, including __Handled__ or __UnHandled__ exceptions on the your database.


--------------------------
### How To Install The Modules ###

1. Install Examples for Developers (unpacking it to your disk, and go to
\ErrorControlSystem\Error Handler Engine\ErrorHandlerEngine\bin\Release directory if you're installing by hand, for example).

  __Note:__
> If there is not any file in the Release folders, please open project .sln file on Visual Studio 2013 and debug/release that to compile native files.


2. In your project reference add this module dll file:  References > Add Reference... > Browse to above path > Select ErrorHandlerEngine.dll

3. Rebuild access permissions if you are prompted to.

Now you can read the code and its comments and see the result, experiment with
it, and hopefully quickly grasp how things work.

If you find a problem, incorrect comment, obsolete or improper code or such,
please search for an issue about it at [ECS Issue](https://bitbucket.org/BehzadKhosravifar/error-control-system/issues)
If there isn't already an issue for it, please create a new one.


--------------------------
### Creating SQL Server Database ###

The `ErrorHandlerEngine` project used from __UsersManagements __ database. So that is necessary for run this application.
For Creating __UsersManagements__ database on your server or pc do below steps:

First open `SQL Server Management Studio` then connect to your Server instance.
Next step's, Create New Database by name __UsersManagements__ or any name of your choice.

> Only be carefully, in order to launch the program, enter the database name. For example: 

```
#!c#

ExpHandlerEngine.Start(new Connection("Server Name", "Database Name", "UserName", "Password", ...));
```

Now, we need to create two table by names __ErrorLog__ and __Snapshots__ to save exceptions by screen captures.

__ErrorLog Table:__

| Column Name      | Data Type             || 
|:----------------------- |:------------------------ ||
| ErrorId                  |   `bigint`                ||
| DateTime             |   `datetime`            ||
| Host                     |  `varchar(200)`       ||
| [User]                   | `varchar(200)`       ||
| IsHandled            |  `bit`                       ||
| Type                    |  `varchar(100)`       ||
| AppName            |  `varchar(100)`      ||
| Data                    |  `xml`                      ||
| CurrentCulture    |  `nvarchar(100)`     ||
| CLRVersion        |  `varchar(20)`         ||
| Message             |  `nvarchar(MAX)`   ||
| Source                |  `nvarchar(Max)`   ||
| StackTrace          |  `nvarchar(Max)`   ||
| ModuleName      |  `varchar(200)`       ||
| MemberType       |  `varchar(50)`        ||
| Method                |  `nvarchar(500)`    ||
| Processes            |  `varchar(Max)`     ||
| ErrorDateTime     |  `datetime`            ||
| OS                       |  `varchar(1000)`    ||
| IPv4Address        |  `varchar(15)`        ||
| MACAddress       |  `varchar(50)`        ||
| HResult               |  `int`                       ||
| LineColumn         |  `varchar(50)`        ||
| DuplicateNo        |  `int`                       ||

__Snapshots Table:__

| Column Name      | Data Type             || 
|:----------------------- |:------------------------ ||
| ErrorLogId            |   `int`                     ||
| ScreenCapture     |   `image`               ||

For easy way please go ahead and try [Wiki](https://BehzadKhosravifar@bitbucket.org/BehzadKhosravifar/error-control-system.git/wiki) to use SQL Queries.
There are __SQL Scripts__ to create tables and queries.

--------------------------
### LICENSE INFORMATION ###

Error Handler Engine 1.0.0.2
This Class Library creates a way of handling structured exception handling,
inheriting from the Exception class gives us access to many method
we wouldn't otherwise have access to
                  
Copyright (C) 2015  [Shoniz](http://shoniz.com/) corporation
[Behzad Khosravifar](mailto: Behzad.Khosravifar@Gmail.com)

This program published by the Free Software Foundation,
either version 2 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.