//**********************************************************************************//
//                           LICENSE INFORMATION                                    //
//**********************************************************************************//
//   Error Handler Engine 1.0.0.2                                                   //
//   This Class Library creates a way of handling structured exception handling,     //
//   inheriting from the Exception class gives us access to many method             //
//   we wouldn't otherwise have access to                                           //
//                                                                                  //
//   Copyright (C) 2015                                                             //
//   Behzad Khosravifar                                                             //
//   Email: Behzad.Khosravifar@Gmail.com                                            //
//                                                                                  //
//   This program published by the Free Software Foundation,                        //
//   either version 2 of the License, or (at your option) any later version.        //
//                                                                                  //
//   This program is distributed in the hope that it will be useful,                //
//   but WITHOUT ANY WARRANTY; without even the implied warranty of                 //
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.                           //
//                                                                                  //
//**********************************************************************************//

using System;
using System.Windows.Forms;
using ErrorHandlerEngine.CacheHandledErrors;
using ErrorHandlerEngine.ModelObjecting;

namespace ErrorHandlerEngine.ExceptionManager
{
    /// <summary>
    /// Additional Data attached to exception object.
    /// </summary>
    public static class ExceptionHandler
    {
        #region Properties

        public static volatile bool IsSelfException = false;

        #endregion


        #region Methods

        /// <summary>
        /// Raise log of handled error's.
        /// </summary>
        /// <param name="exp">The Error object.</param>
        /// <param name="option">The option to select what jobs must be doing and stored in Error object's.</param>
        /// <param name="errorTitle">Determine the mode of occurrence of an error in the program.</param>
        /// <returns></returns>
        public static Error RaiseLog(this Exception exp, ExceptionHandlerOption option = ExceptionHandlerOption.Default, String errorTitle = "UnHandled Exception")
        {
            if (IsSelfException && option.HasFlag(ExceptionHandlerOption.IsHandled)) // Self exceptions just run in Handled Mode!
            {
                IsSelfException = false;
                return null;
            }

            // initial the error object by additional data 
            var error = new Error(exp, option);

            if (option.HasFlag(ExceptionHandlerOption.AlertUnHandledError) && !option.HasFlag(ExceptionHandlerOption.IsHandled)) // Alert Unhandled Error 
            {
                MessageBox.Show(exp.Message,
                    errorTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }

            CacheController.CacheTheError(error);

            return error;
        }

        #endregion
    }
}
