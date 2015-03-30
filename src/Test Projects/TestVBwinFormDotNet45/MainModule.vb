Imports ErrorHandlerEngine.ExceptionManager

Module MainModule
    Sub Main()

        ' ------------------ Initial Error Handler Engine --------------------------------
        ExpHandlerEngine.Start("localhost", "UsersManagements")

        'Except 'NotImplementedException' from raise log
        ExceptionHandler.Filter.ExemptedExceptionTypes.Add(GetType(NotImplementedException))

        'Filter 'Exception' type from Snapshot capturing 
        ExceptionHandler.Filter.NonSnapshotExceptionTypes.Add(GetType(FormatException))

        'Add extra data for labeling exceptions
        ExceptionHandler.Filter.AttachExtraData.Add("TestVBwinFormDotNet45 v2.1.1.0", "beta version")

        ' ---------------------------------------------------------------------------------

        Application.Run(New Form1())
    End Sub
End Module
