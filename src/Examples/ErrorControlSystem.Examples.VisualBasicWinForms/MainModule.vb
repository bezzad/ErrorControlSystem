Imports ErrorControlSystem

Module MainModule
    Sub Main()

        ' ------------------ Initial Error Control System --------------------------------
        ExceptionHandler.Engine.Start("localhost", "UsersManagements")

        'Except 'NotImplementedException' from raise log
        ExceptionHandler.Filter.ExemptedExceptionTypes.Add(GetType(NotImplementedException))

        'Filter 'Exception' type from Snapshot capturing 
        ExceptionHandler.Filter.NonSnapshotExceptionTypes.Add(GetType(FormatException))

        'Add extra data for labeling exceptions
        ExceptionHandler.Filter.AttachExtraData.Add("VBWinForms v3.7", "beta version")

        ' ---------------------------------------------------------------------------------

        Application.Run(New Form1())
    End Sub
End Module
