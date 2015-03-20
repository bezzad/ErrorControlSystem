# [Error Control System](https://BehzadKhosravifar@bitbucket.org/BehzadKhosravifar/error-control-system.git)  README #
---------------------
### What Is This? ###

This is a C#.Net project's for manage __Exceptions__ of your .Net applications by handling and 
logging that. This is a .dll modules to provide error handling worker on your app background's.
The modules strive to be simple, well documented and
modification friendly, in order to help developers quickly learn their inner
workings.

NOTE: 
>     This program is only for exception handling of Windows-based applications (WinForms, WPF)
>     that are written by .NET languages such as C#,  VB, Visual F#  and  Visual C++


--------------------------------
### How To Use The Examples ###

To interact with this project, at first find main class of your project to 
add this module. Then, at the beginning of the instructions before any other 
user codes, enter the following command to invoke and run the module.

This is initialize codes of the module by `C# language`:
 
```
#!csharp

using System;
using System.Windows.Forms;
using ExceptionManager;

namespace TestApplication
{
    static class Program
    {
        [STAThread]
        private static void Main()
        {
			Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //
            //  ------------------ Initial Error Handler Engine --------------------------------
            //
            ExpHandlerEngine.Start(new DbConnectionsManager.Connection("localhost", "UsersManagements"),
                ErrorHandlerOption.Default & ~ErrorHandlerOption.ReSizeSnapshots);

            // Except 'NotImplementedException' from raise log
            ExceptionHandler.ExceptedExceptionTypes.Add(typeof(NotImplementedException));

            // Filter 'Exception' type from Snapshot capturing 
            ExceptionHandler.NonSnapshotExceptionTypes.Add(typeof(FormatException));

            // Add extra data for labeling exceptions
            ExceptionHandler.AttachExtraData.Add("TestWinFormDotNet45 v2.1.1.0", "beta version");
            //
            // ---------------------------------------------------------------------------------
            //

            Application.Run(new FormTest());
        }
    }
}
```

This is initialize codes of the module by `VB.NET language`:
 
```
#!vb

Imports ExceptionManager

Module MainModule
    Sub Main()

        ' ------------------ Initial Error Handler Engine --------------------------------

        ExpHandlerEngine.Start(New DbConnectionsManager.Connection("localhost", "UsersManagements"),
                ErrorHandlerOption.Default And Not ErrorHandlerOption.ReSizeSnapshots)

        'Except 'NotImplementedException' from raise log
        ExceptionHandler.ExceptedExceptionTypes.Add(GetType(NotImplementedException))

        'Filter 'Exception' type from Snapshot capturing 
        ExceptionHandler.NonSnapshotExceptionTypes.Add(GetType(FormatException))

        'Add extra data for labeling exceptions
        ExceptionHandler.AttachExtraData.Add("TestVBwinFormDotNet45 v2.1.1.0", "beta version")

        ' ---------------------------------------------------------------------------------
        
        Application.Run(New Form1())
    End Sub
End Module

```

For create a Sub Main method of MainModule.vb in VB winform apps, go to the Project Designer (go to Project Properties or double-click the My Project node in Solution Explorer)

![VB Solution.PNG](https://bitbucket.org/repo/7AAK6y/images/391053001-VB%20Solution.PNG)

After shown below form make uncheck the `Enable Application Framework` option on the Application tab:
![VB Application TAB.PNG](https://bitbucket.org/repo/7AAK6y/images/2287578620-VB%20Application%20TAB.PNG)

Change the __Startup object__ to `Sub Main`.


In the initialize code snippet you've seen that, for the `ExpHandlerEngine` method should be an option. This option is used to specify the error data, which includes the following values:

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
ExpHandlerEngine.Start(new DbConnectionsManager.Connection(@"localhost", "UsersManagements"),
                ErrorHandlerOption.Default & ~ErrorHandlerOption.ReSizeSnapshots);
```
Select all options by excepted `ReSizeSnapshots`

By adding the our module starter code to the beginning of your program code, you can raise all exceptions history, including __Handled__ or __UnHandled__ exceptions on the your database.

Note:
>     In the new version 2.1.1.0 a later, the option set in from database at runtime, 
>     and not necessary to set that from initial  Start  method


--------------------------
### How To Install The Modules ###

1. Install examples for developers (unpacking it to your disk, and go to
\ErrorControlSystem\Error Handler Engine\ErrorHandlerEngine\bin\Release directory if you're installing by hand, for example).

 Note:
>      If there is not any file in the Release folders, please open project .sln file on 
>      Visual Studio 2013 and debug/release that to compile native files.


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

Note:
>     In the new version database and tables automatically created by ErrorHandlerEngine


First open `SQL Server Management Studio` then connect to your Server instance.
Next step's, Create New Database by name __UsersManagements__ or any name of your choice.

Only be carefully, in order to launch the program, enter the database name. For example: 

```
#!csharp

ExpHandlerEngine.Start(new Connection("Server Name", "Database Name", "UserName", "Password", ...));
```

Now, we need to create two table by names __ErrorLog__ and __Snapshots__ to save exceptions by screen captures.

__ErrorLog Table:__

| Column Name      | Data Type             | 
|:----------------------- |:------------------------ |
| ErrorId                  |   `bigint`                |
| DateTime             |   `datetime`           |
| Host                     |  `varchar(200)`      |
| [User]                   | `varchar(200)`      |
| IsHandled            |  `bit`                      |
| Type                    |  `varchar(100)`      |
| AppName            |  `varchar(100)`      |
| Data                    |  `xml`                     |
| CurrentCulture    |  `nvarchar(100)`    |
| CLRVersion        |  `varchar(20)`        |
| Message             |  `nvarchar(MAX)`  |
| Source                |  `nvarchar(Max)`   |
| StackTrace          |  `nvarchar(Max)`  |
| ModuleName      |  `varchar(200)`      |
| MemberType       |  `varchar(50)`       |
| Method                |  `nvarchar(500)`   |
| Processes            |  `varchar(Max)`   |
| ErrorDateTime     |  `datetime`           |
| OS                       |  `varchar(1000)`   |
| IPv4Address        |  `varchar(15)`       |
| MACAddress       |  `varchar(50)`       |
| HResult               |  `int`                      |
| LineColumn         |  `varchar(50)`       |
| DuplicateNo        |  `int`                      |

__Snapshots Table:__

| Column Name      | Data Type             |
|:----------------------- |:------------------------ |
| ErrorLogId            |   `int`                     |
| ScreenCapture     |   `image`               |

For easy way please go ahead and try [Wiki](https://BehzadKhosravifar@bitbucket.org/BehzadKhosravifar/error-control-system.git/wiki) to use SQL Queries.
There are __SQL Scripts__ to create tables and queries.


--------------------------
### Mirroring the Error Control System Repository to Your Git Repository ###

If you want to mirror this repository in another location, including getting updates from the original, you can clone a mirror and periodically push the changes.

```
#!command-line

$ git clone --mirror https://BehzadKhosravifar@bitbucket.org/BehzadKhosravifar/error-control-system.git
# Make a bare mirrored clone of the repository

$ cd error-control-system.git
$ git remote set-url --push origin https://github.com/ExampleUser/mirrored
# Set the push location to your mirror
```

As with a bare clone, a mirrored clone includes all remote branches and tags, but all local references will be overwritten each time you fetch, so it will always be the same as the original repository. Setting the URL for pushes simplifies pushing to your mirror. To update your mirror, fetch updates and push, which could be automated by running a cron job.

```
#!command-line

$ git fetch -p origin
$ git push --mirror
```


--------------------------
### LICENSE INFORMATION ###

Error Handler Engine 3.0.0.2
This Class Library creates a way of handling structured exception handling,
inheriting from the Exception class gives us access to many method
we wouldn't otherwise have access to
                  
Copyright (C) 2015  [Shoniz](http://shoniz.com/) corporation
[Behzad Khosravifar](mailto: Behzad.Khosravifar@Gmail.com)

This program published by the Free Software Foundation,
either version 3 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.