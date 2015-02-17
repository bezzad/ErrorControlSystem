# [Error Control System](https://BehzadKhosravifar@bitbucket.org/BehzadKhosravifar/error-control-system.git)  README #
---------------------
### What Is This? ###

This is a C#.Net project's for manage .Net applications __Exceptions__ by handling and 
logging that. This is a dll modules to provide error handling worker on your app background's.
The modules strive to be simple, well documented and
modification friendly, in order to help developers quickly learn their inner
workings.

NOTE: 
> This program is only for exception handling of Windows-based applications 
> that are written by .NET.

--------------------------------
### How To Use The Examples ###

There are three main ways to interact with the examples in this project:

1. Enable the modules and use them within Drupal. Not all modules will have
obvious things to see within Drupal. For instance, while the Page and Form API
examples will show you forms, the Database API example will not show you much
within Drupal itself.

2. Read the code. Much effort has gone into making the example code readable,
not only in terms of the code itself, but also the extensive inline comments
and documentation blocks.

3. Browse the code and documentation on the web. There are two main places to
do this:

This is some very random C#:
 
    :::C#
        function foo() 
        {
            int poo = 1;
                return;
        }

* https://api.drupal.org/api/examples is the main API site for all of Drupal.
It has all manner of cross-linked references between the example code and the
APIs being demonstrated.

* http://drupalcode.org/project/examples.git allows you to browse the git
repository for the Examples project.

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
                  
Copyright (C) 2015  
Behzad Khosravifar
Email: Behzad.Khosravifar@Gmail.com

This program published by the Free Software Foundation,
either version 2 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.