<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnThrowHandledExps = New System.Windows.Forms.Button()
        Me.btnThrowUnHandledExps = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnThrowHandledExps
        '
        Me.btnThrowHandledExps.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!)
        Me.btnThrowHandledExps.Location = New System.Drawing.Point(12, 12)
        Me.btnThrowHandledExps.Name = "btnThrowHandledExps"
        Me.btnThrowHandledExps.Size = New System.Drawing.Size(381, 133)
        Me.btnThrowHandledExps.TabIndex = 0
        Me.btnThrowHandledExps.Text = "Throw Handled Exps"
        Me.btnThrowHandledExps.UseVisualStyleBackColor = True
        '
        'btnThrowUnHandledExps
        '
        Me.btnThrowUnHandledExps.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!)
        Me.btnThrowUnHandledExps.Location = New System.Drawing.Point(12, 177)
        Me.btnThrowUnHandledExps.Name = "btnThrowUnHandledExps"
        Me.btnThrowUnHandledExps.Size = New System.Drawing.Size(381, 133)
        Me.btnThrowUnHandledExps.TabIndex = 0
        Me.btnThrowUnHandledExps.Text = "Throw UnHandled Exps"
        Me.btnThrowUnHandledExps.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(404, 374)
        Me.Controls.Add(Me.btnThrowUnHandledExps)
        Me.Controls.Add(Me.btnThrowHandledExps)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnThrowHandledExps As System.Windows.Forms.Button
    Friend WithEvents btnThrowUnHandledExps As System.Windows.Forms.Button

End Class
