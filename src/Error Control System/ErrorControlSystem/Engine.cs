//**********************************************************************************//
//                           LICENSE INFORMATION                                    //
//**********************************************************************************//
//   Error Control System                                                           //
//   This Class Library creates a way of handling structured exception handling,    //
//   inheriting from the Exception class gives us access to many method             //
//   we wouldn't otherwise have access to                                           //
//                                                                                  //
//   Copyright (C) 2014-2015                                                        //
//   Behzad Khosravifar                                                             //
//   mailto:Behzad.Khosravifar@Gmail.com                                            //
//                                                                                  //
//   This program published by the Free Software Foundation,                        //
//   either version 1.0.0 of the License, or (at your option) any later version.        //
//                                                                                  //
//   This program is distributed in the hope that it will be useful,                //
//   but WITHOUT ANY WARRANTY; without even the implied warranty of                 //
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.                           //
//                                                                                  //
//**********************************************************************************//


namespace ErrorControlSystem
{
    using System;
    using System.Diagnostics;
    using System.Runtime.ExceptionServices;
    using System.Security.Permissions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Threading;

    using ErrorControlSystem.CacheErrors;
    using ErrorControlSystem.DbConnectionManager;
    using ErrorControlSystem.ServerController;

    public static partial class ExceptionHandler
    {
        /// <summary>
        /// Exceptions Handler Engine Class
        /// for handling any exception from your attachment applications. 
        /// </summary>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        public static class Engine
        {
            #region Properties

            private static ErrorHandlingOptions _option;

            #endregion

            #region Static Constructors

            static Engine()
            {
                if (!AssembelyLoaded) LoadAssemblies();

                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;

                //System.Windows.Forms.Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

                // Catch all handled exceptions in managed code, before the runtime searches the Call Stack 
                AppDomain.CurrentDomain.FirstChanceException += FirstChanceException;

                // Catch all unhandled exceptions in all threads.
                AppDomain.CurrentDomain.UnhandledException += UnhandledException;

                // Catch all unobserved task exceptions.
                TaskScheduler.UnobservedTaskException += UnobservedTaskException;

                // Catch all unhandled exceptions.
                System.Windows.Forms.Application.ThreadException += ThreadException;

                // Catch all WPF unhandled exceptions.
                Dispatcher.CurrentDispatcher.UnhandledException += DispatcherUnhandledException;
            }

            #endregion

            #region Methods

            #region Start Methods

            public static async void Start(ErrorHandlingOptions option = ErrorHandlingOptions.Default)
            {
                _option = option & ~ErrorHandlingOptions.SendCacheToServer;

                await ServerTransmitter.InitialTransmitterAsync();
            }


            public static async void Start(Connection conn, ErrorHandlingOptions option = ErrorHandlingOptions.Default)
            {
                _option = option;

                ConnectionManager.Add(conn, "ErrorControlSystemConnection");
                ConnectionManager.SetToDefaultConnection("ErrorControlSystemConnection");

                await ServerTransmitter.InitialTransmitterAsync();

                var publicSetting = await ServerTransmitter.GetErrorHandlingOptionsAsync();
                if (publicSetting != 0)
                    _option = publicSetting;

                await CacheController.CheckStateAsync();
            }


            public static void Start(string server, string database, string username, string pass,
                ErrorHandlingOptions option = ErrorHandlingOptions.Default)
            {
                var conn = new Connection(server, database, username, pass);

                Start(conn, option);
            }

            public static void Start(string server, string database,
                ErrorHandlingOptions option = ErrorHandlingOptions.Default)
            {
                var conn = new Connection(server, database);

                Start(conn, option);
            }

            #endregion


            #region Bridge Handlers

            /// <summary>
            /// Used for handling WPF exceptions bound to the UI thread.
            /// Handles the <see cref="System.Windows.Threading.DispatcherUnhandledExceptionEventHandler"/> events.
            /// </summary>
            public static DispatcherUnhandledExceptionEventHandler DispatcherUnhandledException
            {
                get
                {
                    if (ErrorHandlingOption.HandleProcessCorruptedStateExceptions)
                    {
                        return CorruptDispatcherUnhandledExceptionHandler;
                    }
                    else
                    {
                        return DispatcherUnhandledExceptionHandler;
                    }
                }
            }

            /// <summary>
            /// Used for handling WinForms exceptions bound to the UI thread.
            /// Handles the <see cref="System.Threading.ThreadExceptionEventHandler"/> events in <see cref="System.Windows.Forms.Application"/> namespace.
            /// </summary>
            public static ThreadExceptionEventHandler ThreadException
            {
                get
                {
                    if (ErrorHandlingOption.HandleProcessCorruptedStateExceptions)
                    {
                        return CorruptThreadExceptionHandler;
                    }
                    else
                    {
                        return ThreadExceptionHandler;
                    }
                }
            }

            /// <summary>
            /// Used for handling general exceptions bound to the main thread.
            /// Handles the <see cref="AppDomain.UnhandledException"/> events in <see cref="System"/> namespace.
            /// </summary>
            public static UnhandledExceptionEventHandler UnhandledException
            {
                get
                {
                    if (ErrorHandlingOption.HandleProcessCorruptedStateExceptions)
                    {
                        return CorruptUnhandledExceptionHandler;
                    }
                    else
                    {
                        return UnhandledExceptionHandler;
                    }
                }
            }

            /// <summary>
            /// Used for handling System.Threading.Tasks bound to a background worker thread.
            /// Handles the <see cref="UnobservedTaskException"/> event in <see cref="System.Threading.Tasks"/> namespace.
            /// </summary>
            public static EventHandler<UnobservedTaskExceptionEventArgs> UnobservedTaskException
            {
                get
                {
                    if (ErrorHandlingOption.HandleProcessCorruptedStateExceptions)
                    {
                        return CorruptUnobservedTaskExceptionHandler;
                    }
                    else
                    {
                        return UnobservedTaskExceptionHandler;
                    }
                }
            }

            /// <summary>
            /// This is new to .Net 4 and is extremely useful for ensuring that you ALWAYS log SOMETHING.
            /// Whenever any kind of exception is fired in your application, a FirstChangeExcetpion is raised,
            /// even if the exception was within a Try/Catch block and safely handled.
            /// This is GREAT for logging every wart and boil, but can often result in too much spam, 
            /// if your application has a lot of expected/handled exceptions.
            /// </summary>
            public static EventHandler<FirstChanceExceptionEventArgs> FirstChanceException
            {
                get
                {
                    if (ErrorHandlingOption.HandleProcessCorruptedStateExceptions)
                    {
                        return CorruptFirstChanceExceptionHandler;
                    }
                    else
                    {
                        return FirstChanceExceptionHandler;
                    }
                }
            }

            #endregion


            #region Normal Handlers

            /// <summary>
            /// This is new to .Net 4 and is extremely useful for ensuring that you ALWAYS log SOMETHING.
            /// Whenever any kind of exception is fired in your application, a FirstChangeExcetpion is raised,
            /// even if the exception was within a Try/Catch block and safely handled.
            /// This is GREAT for logging every wart and boil, but can often result in too much spam, 
            /// if your application has a lot of expected/handled exceptions.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The <see cref="FirstChanceExceptionEventArgs"/> instance containing the event data.</param>
            private static void FirstChanceExceptionHandler(object sender, FirstChanceExceptionEventArgs e)
            {
                e.Exception.RaiseLog(_option | ErrorHandlingOptions.IsHandled);
            }

            /// <summary>
            /// If you are using Tasks, then you may have "unobserved task exceptions". 
            /// This event allows you to trap them. It also has a method called SetObserved,
            /// which allows you to try to recover from the issue.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The <see cref="UnobservedTaskExceptionEventArgs"/> instance containing the event data.</param>
            private static void UnobservedTaskExceptionHandler(object sender, UnobservedTaskExceptionEventArgs e)
            {
                e.Exception.RaiseLog(_option & ~ErrorHandlingOptions.IsHandled, "Unobserved Task Exception");
            }

            /// <summary>
            /// If you are hosting any WinForm components in your WPF application, 
            /// this final event is one to watch. There's no way to influence events thereafter, 
            /// but at least you get to see what the problem was.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The <see cref="ThreadExceptionEventArgs"/> instance containing the event data.</param>
            private static void ThreadExceptionHandler(object sender, ThreadExceptionEventArgs e)
            {
                e.Exception.RaiseLog(_option & ~ErrorHandlingOptions.IsHandled, "Unhandled Thread Exception");
            }

            /// <summary>
            /// Catch all unhandled exceptions in all threads.
            /// Although Application.DispatcherUnhandledException covers most issues in the current AppDomain, 
            /// in extremely rare circumstances, you may be running a thread on a second AppDomain. 
            /// This event conveys the other AppDomain unhandled exception, 
            /// but there are no Handled property, just an IsTerminating flag.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The <see cref="UnhandledExceptionEventArgs"/> instance containing the event data.</param>
            private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
            {
                (e.ExceptionObject as Exception).RaiseLog(_option & ~ErrorHandlingOptions.IsHandled,
                    "Unhandled UI Exception");

                Environment.Exit(0);
            }

            /// <summary>
            /// Used for handling WPF exceptions bound to the UI thread.
            /// Handles the <see cref="System.Windows.Threading.DispatcherUnhandledExceptionEventHandler"/> events.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The <see cref="DispatcherUnhandledExceptionEventArgs"/> instance containing the event data.</param>
            private static void DispatcherUnhandledExceptionHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
            {
                // Prevent default unhandled exception processing
                e.Handled = true;

                e.Exception.RaiseLog(_option & ~ErrorHandlingOptions.IsHandled, "Unhandled Thread Exception");
            }

            #endregion


            #region Corrupted Exception Handlers

            [HandleProcessCorruptedStateExceptions]
            private static void CorruptDispatcherUnhandledExceptionHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
            {
                DispatcherUnhandledExceptionHandler(sender, e);
            }

            [HandleProcessCorruptedStateExceptions]
            private static void CorruptThreadExceptionHandler(object sender, ThreadExceptionEventArgs e)
            {
                ThreadExceptionHandler(sender, e);
            }

            [HandleProcessCorruptedStateExceptions]
            private static void CorruptUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
            {
                UnhandledExceptionHandler(sender, e);
            }

            [HandleProcessCorruptedStateExceptions]
            private static void CorruptUnobservedTaskExceptionHandler(object sender, UnobservedTaskExceptionEventArgs e)
            {
                UnobservedTaskExceptionHandler(sender, e);
            }

            [HandleProcessCorruptedStateExceptions]
            private static void CorruptFirstChanceExceptionHandler(object sender, FirstChanceExceptionEventArgs e)
            {
                FirstChanceExceptionHandler(sender, e);
            }

            #endregion

            #endregion
        }
    }
}