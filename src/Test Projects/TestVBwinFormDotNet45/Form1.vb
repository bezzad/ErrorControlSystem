Public Class Form1

    Private _exps As List(Of Action)

    Protected Overrides Sub OnLoad(ByVal e As EventArgs)
        MyBase.OnLoad(e)

        _exps = New List(Of Action)() From {
                Sub() Throw New Exception("Test"),
                Sub() Throw New InvalidExpressionException("Test"),
                 Sub() Throw New ApplicationException("Test"),
                 Sub() Throw New SystemException("Test"),
                 Sub() Throw New Exception("Test"),
                 Sub() Throw New InvalidExpressionException("Test"),
                 Sub() Throw New ApplicationException("Test"),
                 Sub() Throw New SystemException("Test"),
                 Sub() Throw New Exception("Test"),
                 Sub() Throw New InvalidExpressionException("Test"),
                 Sub() Throw New ApplicationException("Test"),
                 Sub() Throw New SystemException("Test"),
                 Sub() Throw New Exception("Test"),
                 Sub() Throw New InvalidExpressionException("Test"),
                 Sub() Throw New ApplicationException("Test"),
                 Sub() Throw New SystemException("Test"),
                 Sub() Throw New Exception("Test"),
                 Sub() Throw New InvalidExpressionException("Test"),
                 Sub() Throw New ApplicationException("Test"),
                 Sub() Throw New SystemException("Test"),
                 Sub() Throw New Exception("Test"),
                 Sub() Throw New InvalidExpressionException("Test"),
                 Sub() Throw New ApplicationException("Test"),
                 Sub() Throw New SystemException("Test"),
                 Sub() Throw New ArithmeticException("Test"),
                 Sub() Throw New Exception("Test"),
                 Sub() Throw New InvalidExpressionException("Test"),
                 Sub() Throw New ApplicationException("Test"),
                 Sub() Throw New SystemException("Test"),
                 Sub() Throw New Exception("Test"),
                 Sub() Throw New InvalidExpressionException("Test"),
                 Sub() Throw New ApplicationException("Test"),
                 Sub() Throw New SystemException("Test"),
                 Sub() Throw New Exception("Test"),
                 Sub() Throw New InvalidExpressionException("Test"),
                 Sub() Throw New ApplicationException("Test"),
                 Sub() Throw New SystemException("Test"),
                 Sub() Throw New Exception("Test"),
                 Sub() Throw New InvalidExpressionException("Test"),
                 Sub() Throw New ApplicationException("Test"),
                 Sub() Throw New SystemException("Test"),
                 Sub() Throw New Exception("Test"),
                 Sub() Throw New InvalidExpressionException("Test"),
                 Sub() Throw New ApplicationException("Test"),
                 Sub() Throw New SystemException("Test"),
                 Sub() Throw New Exception("Test"),
                 Sub() Throw New InvalidExpressionException("Test"),
                 Sub() Throw New ApplicationException("Test"),
                 Sub() Throw New SystemException("Test")
            }
    End Sub

    Private Async Sub btnThrowHandledExps_Click(sender As Object, e As EventArgs) Handles btnThrowHandledExps.Click
        Await Task.Run(Sub() Parallel.[For](0, 8, Function(i)
                                                      Try
                                                          _exps(New Random().[Next](0, _exps.Count - 1))()
                                                      Catch
                                                      End Try
                                                  End Function))
    End Sub

    Private Sub btnThrowUnHandledExps_Click(sender As Object, e As EventArgs) Handles btnThrowUnHandledExps.Click
        _exps(New Random().[Next](0, _exps.Count - 1))()
    End Sub
End Class
