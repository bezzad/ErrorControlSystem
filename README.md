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
using ErrorControlSystem.ErrorHandlerEngine;

namespace TestApplication
{
    static class Program
    {
        [STAThread]
        private static void Main()
        {
               HandleExceptions.Start(".", "UsersManagements", 5, ErrorHandlingOption.All & ~ErrorHandlingOption.ReSizeSnapshots);

               Application.Run(new Form1());
        }
    }
}
```

In the above code snippet you've seen that, for the `HandleExceptions` method should be an option. This option is used to specify the error data, which includes the following values:

-  All
-  AlertUnHandledError
-  Default (AlertUnHandledError + FetchServerDateTime + Snapshot)
-  FetchServerDateTime
-  IsHandled
-  None
-  ReSizeSnapshots
-  Snapshot

For example in above codes, this code means is:
```csharp
HandleExceptions.Start(".", "UsersManagements", 5, ErrorHandlingOption.All & ~ErrorHandlingOption.ReSizeSnapshots);
```
Select all options by excepted `ReSizeSnapshots`

By adding the our module starter code to the beginning of your program code, you can raise all exceptions history, including __Handled__ or __UnHandled__ exceptions on the your database.


--------------------------
### How To Install The Modules ###

1. Install Examples for Developers (unpacking it to your Drupal
/sites/all/modules directory if you're installing by hand, for example).

2. Enable any Example modules in Admin menu > Site building > Modules.

3. Rebuild access permissions if you are prompted to.

4. Profit!  The examples will appear in your Navigation menu (on the left
sidebar by default; you'll need to reenable it if you removed it).

Now you can read the code and its comments and see the result, experiment with
it, and hopefully quickly grasp how things work.

If you find a problem, incorrect comment, obsolete or improper code or such,
please search for an issue about it at http://drupal.org/project/issues/examples
If there isn't already an issue for it, please create a new one.


--------------------------
### LICENSE INFORMATION ###

Error Handler Engine 1.0.0.2
This Class Library creates a way of handling structured exception handling,
inheriting from the Exception class gives us access to many method
we wouldn't otherwise have access to
                  
Copyright (C) 2015  [Shoniz](www.shoniz.com) corporation
[Behzad Khosravifar](mailto: Behzad.Khosravifar@Gmail.com)

This program published by the Free Software Foundation,
either version 2 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.