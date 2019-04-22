'BasicPawn
'Copyright(C) 2018 TheTimocop

'This program Is free software: you can redistribute it And/Or modify
'it under the terms Of the GNU General Public License As published by
'the Free Software Foundation, either version 3 Of the License, Or
'(at your option) any later version.

'This program Is distributed In the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty Of
'MERCHANTABILITY Or FITNESS FOR A PARTICULAR PURPOSE.  See the
'GNU General Public License For more details.

'You should have received a copy Of the GNU General Public License
'along with this program. If Not, see < http: //www.gnu.org/licenses/>.


Imports ICSharpCode.TextEditor
Imports ICSharpCode.TextEditor.Actions
Imports ICSharpCode.TextEditor.Document

Public Class TextEditorControlEx
    Inherits TextEditorControl

    <ComponentModel.Browsable(False)>
    ReadOnly Property m_LineStatesHistory As New Queue(Of ClassTextEditorTools.ClassLineState.LineStateMark)

    Public Event ProcessCmdKeyEvent(ByRef bBlock As Boolean, ByRef iMsg As Message, iKeys As Keys)

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, e As Keys) As Boolean
        Dim bBLock As Boolean = False

        RaiseEvent ProcessCmdKeyEvent(bBLock, msg, e)

        If (bBLock) Then
            Return True
        End If

        If (e = (Keys.LButton Or Keys.Back Or Keys.Shift)) Then
            'Block the default glitchy ShiftTab version and use this instead.
            With New FixedShiftTab
                .Execute(Me.ActiveTextAreaControl.TextArea)
            End With

            Return True
        End If

        Return MyBase.ProcessCmdKey(msg, e)
    End Function

    Public Class FixedShiftTab
        Inherits AbstractEditAction

        Private Sub RemoveTabs(mDocument As IDocument, mSelection As ISelection, mCaret As Caret, iLineStart As Integer, iLineEnd As Integer)
            mDocument.UndoStack.StartUndoGroup()

            For i As Integer = iLineEnd To iLineStart Step -1
                Dim mLine As LineSegment = mDocument.GetLineSegment(i)

                If i = iLineEnd AndAlso mSelection IsNot Nothing AndAlso mLine.Offset = mSelection.EndOffset Then
                    Continue For
                End If

                If (mLine.Length > 0) Then
                    Dim iRemoveCount As Integer = 0

                    If mDocument.GetCharAt(mLine.Offset) = vbTab Then
                        iRemoveCount = 1

                    ElseIf mDocument.GetCharAt(mLine.Offset) = " "c Then
                        Dim iLeadingSpaces As Integer = 1
                        Dim iTabIndent As Integer = mDocument.TextEditorProperties.IndentationSize

                        iLeadingSpaces = 1

                        While iLeadingSpaces < mLine.Length AndAlso mDocument.GetCharAt(mLine.Offset + iLeadingSpaces) = " "c
                            iLeadingSpaces += 1
                        End While

                        If iLeadingSpaces >= iTabIndent Then
                            iRemoveCount = iTabIndent
                        ElseIf mLine.Length > iLeadingSpaces AndAlso mDocument.GetCharAt(mLine.Offset + iLeadingSpaces) = vbTab Then
                            iRemoveCount = iLeadingSpaces + 1
                        Else
                            iRemoveCount = iLeadingSpaces
                        End If
                    End If

                    If iRemoveCount > 0 Then
                        mDocument.Remove(mLine.Offset, iRemoveCount)
                    End If
                End If
            Next

            If (mSelection IsNot Nothing) Then
                'Just select everything, looks prettier
                Dim mLineStart As LineSegment = mDocument.GetLineSegment(mSelection.StartPosition.Line)
                Dim mLineEnd As LineSegment = mDocument.GetLineSegment(mSelection.EndPosition.Line)

                mSelection.StartPosition = New TextLocation(0, mLineStart.LineNumber)

                If (mSelection.EndPosition.Column > 0) Then
                    mSelection.EndPosition = New TextLocation(mLineEnd.Length, mLineEnd.LineNumber)
                End If
            ElseIf (mCaret IsNot Nothing) Then
                Dim mLineStart As LineSegment = mDocument.GetLineSegment(mCaret.Line)

                'Set caret at the beginning
                For i = mLineStart.Offset To mLineStart.Offset + mLineStart.Length - 1
                    If (mDocument.GetCharAt(i) = " "c OrElse mDocument.GetCharAt(i) = vbTab) Then
                        Continue For
                    End If

                    mCaret.Position = mCaret.ValidatePosition(New TextLocation(i - mLineStart.Offset, mLineStart.LineNumber))
                    Exit For
                Next
            End If

            mDocument.UndoStack.EndUndoGroup()
        End Sub

        Public Overrides Sub Execute(mTextArea As TextArea)
            If mTextArea.SelectionManager.HasSomethingSelected Then
                For Each mSelection As ISelection In mTextArea.SelectionManager.SelectionCollection
                    Dim iLineStart As Integer = mSelection.StartPosition.Line
                    Dim iLineEnd As Integer = mSelection.EndPosition.Line

                    mTextArea.BeginUpdate()

                    RemoveTabs(mTextArea.Document, mSelection, Nothing, iLineStart, iLineEnd)

                    mTextArea.Document.UpdateQueue.Clear()
                    mTextArea.Document.RequestUpdate(New TextAreaUpdate(TextAreaUpdateType.LinesBetween, iLineStart, iLineEnd))

                    mTextArea.EndUpdate()
                Next
                mTextArea.AutoClearSelection = False
                mTextArea.Caret.UpdateCaretPosition()
            Else
                Dim mLine As LineSegment = mTextArea.Document.GetLineSegmentForOffset(mTextArea.Caret.Offset)
                mTextArea.BeginUpdate()

                RemoveTabs(mTextArea.Document, Nothing, mTextArea.Caret, mLine.LineNumber, mLine.LineNumber)

                mTextArea.Document.UpdateQueue.Clear()
                mTextArea.Document.RequestUpdate(New TextAreaUpdate(TextAreaUpdateType.SingleLine, mLine.LineNumber))
                mTextArea.Caret.UpdateCaretPosition()

                mTextArea.EndUpdate()
            End If
        End Sub
    End Class
End Class
