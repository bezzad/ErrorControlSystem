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
