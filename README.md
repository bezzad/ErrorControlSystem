# [Error Control System](https://github.com/Behzadkhosravifar/ErrorControlSystem)  README #
---------------------
[![Build status](https://ci.appveyor.com/api/projects/status/9qdlpbpf3gyfuvdu?svg=true)](https://ci.appveyor.com/project/Behzadkhosravifar/errorcontrolsystem)

[![Error Control System.png](https://raw.githubusercontent.com/Behzadkhosravifar/ErrorControlSystem/master/Images/Error%20Control%20System.png)](https://github.com/Behzadkhosravifar/ErrorControlSystem)

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
 
```csharp

using System;
using System.Windows.Forms;
using ErrorHandlerEngine;

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
            ExceptionHandler.Engine.Start(
                new ErrorHandlerEngine.DbConnectionManager.Connection("localhost", "UsersManagements"),
                   ErrorHandlingOptions.Default & ~ErrorHandlingOptions.ReSizeSnapshots);
            //
            // Or this new version(3.0.0.6 or later) model:
            // ExceptionHandler.Engine.Start("localhost", "UsersManagements");
            //

            // Except 'NotImplementedException' from raise log
            ExceptionHandler.Filter.ExemptedExceptionTypes.Add(typeof(NotImplementedException));

            // Filter 'Exception' type from Snapshot capturing 
            ExceptionHandler.Filter.NonSnapshotExceptionTypes.Add(typeof(FormatException));

            // Add extra data for labeling exceptions
            ExceptionHandler.Filter.AttachExtraData.Add("TestWinFormDotNet45 v3", "beta version");

            // Filter a method of a specific class in my assembly from raise unhanded exceptions log
            ExceptionHandler.Filter.ExemptedCodeScopes.Add(
                new CodeScope("TestWinFormDotNet45", "FormTest", "btnExemptedMethodException_Click"));

            // The just raise error from 'TestWinFormDotNet45'.
            // Do not raise any exception in other code places.
            ExceptionHandler.Filter.JustRaiseErrorCodeScopes.Add(
                new CodeScope("TestWinFormDotNet45", null, null));
            //
            // ---------------------------------------------------------------------------------
            //

            Application.Run(new FormTest());
        }
    }
}
```

This is initialize codes of the module by `VB.NET language`:
 
```vb

Imports ErrorHandlerEngine

Module MainModule
    Sub Main()

        ' ------------------ Initial Error Handler Engine ------------------
		
		ExceptionHandler.Engine.Start("localhost", "UsersManagements")

        ' ------------------------------------------------------------------
        
        Application.Run(New Form1())
    End Sub
End Module

```

For create a Sub Main method of MainModule.vb in VB winform apps, go to the Project Designer (go to Project Properties or double-click the My Project node in Solution Explorer)

![VB Solution Project.PNG](https://github.com/Behzadkhosravifar/ErrorControlSystem/blob/master/Images/VB%20Solution%20Project.png)

After shown below form make uncheck the `Enable Application Framework` option on the Application tab:
![VB Application Properties.PNG](https://raw.githubusercontent.com/Behzadkhosravifar/ErrorControlSystem/master/Images/VB%20Application%20Properties.png)

Change the __Startup object__ to `Sub Main`.


In the initialize code snippet you've seen that, for the `ExceptionHandler.Engine` method should be an option. This option is used to specify the error data, which includes the following values:

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

ExceptionHandler.Engine.Start("localhost", "UsersManagements",
                ErrorHandlingOptions.Default & ~ErrorHandlingOptions.ReSizeSnapshots);
```
Select all options by excepted `ReSizeSnapshots`

By adding the our module starter code to the beginning of your program code, you can raise all exceptions history, including __Handled__ or __UnHandled__ exceptions on the your database.

Note:
>     In the new version 2.1.1.0 and later, the option set in from database at runtime, 
>     and not necessary to set that from initial  Start  method


--------------------------
### How To Install The Modules ###

1. Install examples for developers (unpacking it to your disk, and go to
\ErrorControlSystem\out\Error Handler Engines\... directory if you're installing by hand, for example).

 Note:
>      If there is not any file in the out folders, 
>      run the buildx86.bat (or buildx64 on 64bit Operation System) file 
>      to compile project and create executable files.
>      Or 
>      please open project .sln file on Visual Studio 2013 
>      and debug/release that to compile native files.


2. In your project reference add this module dll file:  
   References > Add Reference... > Browse to above path > Select ErrorHandlerEngine.dll

3. Rebuild access permissions if you are prompted to.

Now you can read the code and its comments and see the result, experiment with
it, and hopefully quickly grasp how things work.

If you find a problem, incorrect comment, obsolete or improper code or such,
please search for an issue about it at [ECS Issue](https://github.com/Behzadkhosravifar/ErrorControlSystem/issues)
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

```csharp

ExceptionHandler.Engine.Start(new Connection("Server Name", "Database Name", "UserName", "Password", ...));

// or simpler mode:
ExceptionHandler.Engine.Start("Server Name", "Database Name", "UserName", "Password");

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
| Type                    |  `varchar(200)`      |
| AppName            |  `varchar(200)`      |
| Data                    |  `xml`                     |
| CurrentCulture    |  `nvarchar(200)`    |
| CLRVersion        |  `varchar(100)`        |
| Message             |  `nvarchar(MAX)`  |
| Source                |  `nvarchar(MAX)`   |
| StackTrace          |  `nvarchar(MAX)`  |
| ModuleName      |  `varchar(200)`      |
| MemberType       |  `varchar(200)`       |
| Method                |  `nvarchar(500)`   |
| Processes            |  `varchar(MAX)`   |
| ErrorDateTime     |  `datetime`           |
| OS                       |  `varchar(1000)`   |
| IPv4Address        |  `varchar(50)`       |
| MACAddress       |  `varchar(50)`       |
| HResult               |  `int`                      |
| LineColumn         |  `varchar(50)`       |
| DuplicateNo        |  `int`                      |

__Snapshots Table:__

| Column Name      | Data Type             |
|:----------------------- |:------------------------ |
| ErrorLogId            |   `int`                     |
| ScreenCapture     |   `image`               |

For easy way please go ahead and try [Wiki](https://github.com/Behzadkhosravifar/ErrorControlSystem/wiki) to use SQL Queries.
There are __SQL Scripts__ to create tables and queries.


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
