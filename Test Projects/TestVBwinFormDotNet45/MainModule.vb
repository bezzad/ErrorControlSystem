Imports DbConnectionsManager

Module MainModule
    Sub Main()
        ExceptionManager.ExpHandlerEngine.Start(New Connection(".", "UsersManagements"))

        Application.Run(New Form1())
    End Sub
End Module
