# [Error Control System](https://github.com/Behzadkhosravifar/ErrorControlSystem) #
---------------------
[![Build status](https://ci.appveyor.com/api/projects/status/lnjusej10c0451xw?svg=true)](https://ci.appveyor.com/project/Behzadkhosravifar/errorcontrolsystem)
[![Nuget count](http://img.shields.io/nuget/v/errorcontrolsystem.svg)](https://www.nuget.org/packages/errorcontrolsystem/)
[![Nuget downloads](http://img.shields.io/nuget/dt/errorcontrolsystem.svg)](https://www.nuget.org/packages/errorcontrolsystem/)
[![Issues open](http://img.shields.io/github/issues-raw/behzadkhosravifar/ErrorControlSystem.svg)](https://huboard.com/behzadkhosravifar/ErrorControlSystem)
[![Source Browser](https://img.shields.io/badge/Browse-Source-green.svg)](http://sourcebrowser.io/Browse/Behzadkhosravifar/ErrorControlSystem)

Check the official [project site](http://behzadkhosravifar.github.io/ErrorControlSystem/). There you will find information on how to contribute, our task board, definition of done, definition of ready, etc.

[![Error Control System.png](https://raw.githubusercontent.com/Behzadkhosravifar/ErrorControlSystem/master/src/Documentation/Readme/images/Error%20Control%20System.png)](https://www.nuget.org/packages/ErrorControlSystem)


---------------------
### What Is This?

This is a C#.Net project's for manage __Exceptions__ of your .Net applications by handling and 
logging that. This is a .dll modules to provide error handling worker on your app background's.
The modules strive to be simple, well documented and
modification friendly, in order to help developers quickly learn their inner
workings.

NOTE: 
>     This program is only for exception handling of 
>     Windows-based applications (WinForms, WPF, Console)
>     that are written by .NET languages such as C#, VB.NET, Visual F#


--------------------------------
### How To Use The Examples

To interact with this project, at first find main class of your project to 
add this module. Then, at the beginning of the instructions before any other 
user codes, enter the following command to invoke and run the module.

This is initialize codes of the module in `C# language` windows applications:
 
```C#

using System;
using System.Windows.Forms;
using ErrorControlSystem;
using ErrorControlSystem.Shared;

namespace TestApplication
{
    static class Program
    {
        [STAThread]
        private static void Main()
        {
			//
            // ------------------ Initial Error Control System ----------------------------
            //
            ExceptionHandler.Engine.Start("Server", "Database", "Username", "Password"
                ErrorHandlingOptions.Default &
                   ~ErrorHandlingOptions.ResizeSnapshots &
                   ~ErrorHandlingOptions.DisplayUnhandledExceptions);
            
            // Or Set Option this way:
            ErrorHandlingOption.ResizeSnapshots = false;
            ErrorHandlingOption.DisplayUnhandledExceptions = false;

            //
            // Some of the optional configuration items.
            //
            // Except 'NotImplementedException' from raise log
            ExceptionHandler.Filter.ExemptedExceptionTypes.Add(typeof(NotImplementedException));

            // Filter 'Exception' type from Snapshot capturing 
            ExceptionHandler.Filter.NonSnapshotExceptionTypes.Add(typeof(FormatException));

            // Add extra data for labeling exceptions
            ExceptionHandler.Filter.AttachExtraData.Add("WinForms", "beta version");

            // Filter a method of a specific class in my assembly 
			// from raise unhanded exceptions log
            ExceptionHandler.Filter.ExemptedCodeScopes.Add(
                new CodeScope("Assembly", "Namespace", "Class", "Method"));

            // Do not raise any exception in other code places.
            ExceptionHandler.Filter.JustRaiseErrorCodeScopes.Add(
                new CodeScope("Assembly", "Namespace", "Class", "Method"));
			//
            // Show unhandled exception message customized mode. 
            ExceptionHandler.OnShowUnhandledError += AlertUnhandledErrors;
            //
            // ----------------------------------------------------------------------------
            //

			Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormTest());
        }

		/// <summary>
        /// Show unhandled exception message customized mode.
        /// </summary>
        /// <param name="sender">Raw exception object</param>
        /// <param name="e">Compiled error object</param>
        public static void AlertUnhandledErrors(object sender, UnhandledErrorEventArgs e)
        {
            MessageBox.Show(e.ErrorObject.Message);
        }
    }
}
```

In VB.NET applications, open the solution window and select `My Project` 
then click on __Show All Files__ and go on this path:

* My Project
    * Application.myapp
	    * Application.Designer.vb  --> (open this file)

Add Startup events at this file, same below codes:

```VB

Partial Friend Class MyApplication
        
        <Global.System.Diagnostics.DebuggerStepThroughAttribute()>  _
        Public Sub New()
            MyBase.New(Global.Microsoft.VisualBasic.ApplicationServices.AuthenticationMode.Windows)
            Me.IsSingleInstance = True
            Me.EnableVisualStyles = True
            Me.SaveMySettingsOnExit = True
            Me.ShutDownStyle = Global.Microsoft.VisualBasic.ApplicationServices.ShutdownMode.AfterMainFormCloses
        End Sub

        <Global.System.Diagnostics.DebuggerStepThroughAttribute()> _
        Protected Overrides Sub OnCreateMainForm()
            Me.MainForm = Global.ErrorControlSystem.Examples.VisualBasicWinForms.Form1
        End Sub


        Private Sub MyApplication_Startup(sender As Object, e As ApplicationServices.StartupEventArgs) Handles Me.Startup
            ' ------------------ Initial Error Control System --------------------------------
            ExceptionHandler.Engine.Start("localhost", "UsersManagements")
            ' ---------------------------------------------------------------------------------
        End Sub
    End Class
```

Or create a Sub Main method like MainModule.vb, for this change must be to go on the Project Designer (go to Project 
Properties or double-click the My Project node in Solution Explorer) and then after shown properties form, uncheck 
the `Enable Application Framework` option on the Application tab, and at last change the __Startup object__ to `Sub Main`.


--------------------------------
### Settings

In the initialize code snippet you've seen that, for the `ExceptionHandler.Engine` method should be an option. 
This option is used to specify the error data, which includes the following values:

* None
* DisplayUnhandledExceptions
* ReportHandledExceptions
* Snapshot
* FetchServerDateTime
* ResizeSnapshots
* EnableNetworkSending
* FilterExceptions
* ExitApplicationImmediately
* HandleProcessCorruptedStateExceptions
* DisplayDeveloperUI
* All
* Default = All - (ExitApplicationImmediately, HandleProcessCorruptedStateExceptions)

For example in above codes, this code means is:

```C#
ExceptionHandler.Engine.Start("localhost", "UsersManagements",
                ErrorHandlingOptions.Default & ~ErrorHandlingOptions.ResizeSnapshots);
```
Select all options by excepted `ResizeSnapshots`

By adding the our module starter code to the beginning of your program code, you can raise all exceptions history, including __Handled__ or __UnHandled__ exceptions on the your database.

Note:
>     In the new version 2.1.1.0 and later, the option set in from database at runtime, 
>     and not necessary to set that from initial  Start  method


--------------------------
### How To Install The Modules

1. Install examples for developers (unpacking it to your disk, and go to
\ErrorControlSystem\out\ErrorControlSystem\... directory if you're installing by hand, for example).

 Note:
>      If there is not any file in the out folders, 
>      run the buildx86.bat (or buildx64 on 64bit Operation System) file 
>      to compile project and create executable files.
>      Or 
>      please open project .sln file on Visual Studio 2013 
>      and debug/release that to compile native files.


2. In your project reference add this module dll file:  
   References > Add Reference... > Browse to above path > Select ErrorControlSystem.dll

3. Rebuild access permissions if you are prompted to.

Now you can read the code and its comments and see the result, experiment with
it, and hopefully quickly grasp how things work.

If you find a problem, incorrect comment, obsolete or improper code or such,
please search for an issue about it at [ECS Issue](https://github.com/Behzadkhosravifar/ErrorControlSystem/issues)
If there isn't already an issue for it, please create a new one.


--------------------------
### Creating SQL Server Database Manually

The `ErrorControlSystem` project used from __UsersManagements __ database. So that is necessary for run this application.
For Creating __UsersManagements__ database on your server or pc do below steps:

Note:
>     In the new version database and tables automatically created by ErrorControlSystem


First open `SQL Server Management Studio` then connect to your Server instance.
Next step's, Create New Database by name __UsersManagements__ or any name of your choice.

Only be carefully, in order to launch the program, enter the database name. For example: 

```C#

ExceptionHandler.Engine.Start(new Connection("Server", "Database", "User", "Pass"));

// or simpler mode:
ExceptionHandler.Engine.Start("Server", "Database", "User", "Pass");

```

Now, we need to create two table by names __ErrorLog__ and __Snapshots__ to save exceptions by screen captures.

__ErrorLog Table:__

| Column Name      | Data Type           |  Description                                                  | Example																			|
|:---------------- |:-------------------:|:-------------------------------------------------------------:|:--------------------------------------------------------------------------------:|
| ErrorId          |  `bigint`           | The unique identity number for exceptions by that location.	 | 51																				|
| ServerDateTime   |  `datetime`         | Server system's date and time when the error occurred.		 | 2015-04-11 17:23:08.170															|
| Host             |  `varchar(200)`     | The client PC name.											 | KHOSRAVIFAR-B																	|
| User             |  `varchar(200)`     | User Domain Name \ User name in operation system				 | DBI\khosravifar.b																|
| IsHandled		   |  `bit`              | Determine this error whether handled or not ?				 | True																				|
| Type             |  `varchar(200)`     | Type of specify exceptions.									 | NullReferenceException															|
| AppName		   |  `varchar(200)`     | The application name, who that run this module on self.	 	 | Examples.WinForms v1.0.0															|
| Data             |  `xml`              | Provide additional user-defined information.					 | <ExtraProperties><Status>RequestCanceled</Status><Response /></ExtraProperties>  |
| CurrentCulture   |  `nvarchar(200)`    | Current keyboard language, may change over the app lifetime.  | English (United States) (en-US)													|
| CLRVersion	   |  `varchar(100)`     | Version of Common Language Runtime.							 | 4.0.30319.34014																	|
| Message          |  `nvarchar(MAX)`    | Message of Exceptions.										 | Object reference not set to an instance of an object.							|
| Source           |  `nvarchar(MAX)`    | Name of the application or the object that causes throw error.| mscorlib																			|
| StackTrace       |  `nvarchar(MAX)`    | Representation of the immediate frames on the call stack.	 | N{namespace}.C{class}.M{method(parameters)} -> ...								|
| ModuleName	   |  `varchar(200)`     | Name of the module that causes to throw exception.			 | Examples.WinForms.exe															|
| MemberType	   |  `varchar(200)`     | Type of the member, include: Method, constructor, and so on.	 | Method																			|
| Method           |  `nvarchar(500)`    | Name of the method that throws the exception.				 | Examples.WinForms.namespace.class.Void method(params)							|
| Processes        |  `varchar(MAX)`     | List of all running process when exception occurred.			 | vmware-hostd, Idle, ...															|
| ErrorDateTime	   |  `datetime`         | Client system's date and time when the error occurred.		 | 2015-04-11 17:23:07.800															|
| OS               |  `varchar(1000)`    | Information of the client operation system in `JSON` formats	 | Windows 8.1 64Bit v6.3.9600.0 - 64Bit Processor Architect - 32Bit Application	|
| IPv4Address	   |  `varchar(50)`      | Network Internal IP Address									 | 192.168.30.40																	|
| MACAddress	   |  `varchar(50)`      | Network Physical Address (MAC)								 | 74D435F250A0																		|
| HResult          |  `int`              | Error Code - Use GetType().Name for define exception model.   | -2147467261																		|
| Line			   |  `int`              | Line of exception occurrence code in file.					 | 125																				|
| Column		   |  `int`              | Column of exception occurrence code in file.					 | 17																				|
| DuplicateNo	   |  `int`              | Number of duplication for one exception's.					 | 5																				|

__Snapshots Table:__

| Column Name      | Data Type             |
|:---------------- |:--------------------- |
| ErrorLogId       |   `int`               |
| ScreenCapture    |   `image`             |

For easy way please go ahead and try [Wiki](https://github.com/Behzadkhosravifar/ErrorControlSystem/wiki) to use SQL Queries.
There are __SQL Scripts__ to create tables and queries.



--------------------------
### Get it on NuGet       [![Nuget count](http://img.shields.io/nuget/v/errorcontrolsystem.svg)](https://www.nuget.org/packages/errorcontrolsystem/)

You may use ErrorControlSystem as a library that you install with [Nuget](https://www.nuget.org/packages/ErrorControlSystem/) into your project or as a Visual Studio extension. 
The way you want to use it depends on the scenario you are working on. You most likely want the [Nuget](https://www.nuget.org/packages/ErrorControlSystem/) package.

To install Error Control System from [Nuget](https://www.nuget.org/packages/ErrorControlSystem/), 
run the following command in the [Package Manager Console](http://docs.nuget.org/consume/package-manager-console)

```powershell
PM> Install-Package ErrorControlSystem
```


--------------------------
### Contributing

Questions, comments, bug reports, and pull requests are all welcome.
Bug reports that include steps-to-reproduce (including code) are
preferred. Even better, make them in the form of pull requests.
Before you start to work on an existing issue, check if it is not assigned
to anyone yet, and if it is, talk to that person.
Also check the project [board](http://huboard.com/BehzadKhosravifar/ErrorControlSystem/board)
and verify it is not being worked on (it will be tagged with the `Working` tag).
If it is not being worked on, before you start check if the item is `Ready`.
If the issue has the `Working` tag (working swimlane on Huboard) and has no Assignee
then it is not being worked on by somebody on the core team. Check the issue's
description to find out who it is (if it is not there it has to be on the comments).


--------------------------
### Issues and task board       [![Issues open](http://img.shields.io/github/issues-raw/behzadkhosravifar/ErrorControlSystem.svg)](https://huboard.com/behzadkhosravifar/ErrorControlSystem)

* The task board is at [Huboard](http://huboard.com/BehzadKhosravifar/ErrorControlSystem/).
* You can also check the [Github backlog](https://github.com/BehzadKhosravifar/ErrorControlSystem/issues) directly.


--------------------------
### Contact

Please see our [contact page](https://www.nuget.org/packages/ErrorControlSystem/ContactOwners).

Email: [Behzad.Khosravifar@gmail.com](mailto:Behzad.Khosravifar@Gmail.com)

--------------------------
### LICENSE INFORMATION      [![OSI-Approved-License-100x137.png](http://opensource.org/trademarks/opensource/OSI-Approved-License-100x137.png)](http://opensource.org/licenses/GPL-3.0.html)

This software is open source, licensed under the GNU General Public License License, Version 3.0.
See [GPL-3.0](http://opensource.org/licenses/GPL-3.0.html) for details.
This Class Library creates a way of handling structured exception handling,
inheriting from the Exception class gives us access to many method
we wouldn't otherwise have access to
                  
Copyright (C) 2014-2015 [Behzad Khosravifar](mailto:Behzad.Khosravifar@Gmail.com)

This program published by the Free Software Foundation,
either version 1.0.1 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
