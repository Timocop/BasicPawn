'BasicPawn
'Copyright(C) 2020 TheTimocop

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

    Public Event ProcessCmdKeyEvent(ByRef bBlock As Boolean, ByRef iMsg As Message, iKeys As Keys)

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, e As Keys) As Boolean
        Dim bBLock As Boolean = False

        RaiseEvent ProcessCmdKeyEvent(bBLock, msg, e)

        If (bBLock) Then
            Return True
        End If

        If (e = (Keys.LButton Or Keys.Back Or Keys.Shift)) Then
            'Block the default glitchy ShiftTab version and use this instead. 
            Call (New FixedShiftTab).Execute(Me.ActiveTextAreaControl.TextArea)

            Return True
        End If

        Return MyBase.ProcessCmdKey(msg, e)
    End Function

    Public Sub InvalidateTextArea()
        Me.Document.RequestUpdate(New TextAreaUpdate(TextAreaUpdateType.WholeTextArea))
        Me.Document.CommitUpdate()
    End Sub

    Public Class MoveSelectedLine
        Inherits AbstractEditAction

        Enum ENUM_DIRECTION
            UP = 1
            DOWN = -1
        End Enum

        Property m_Direction As ENUM_DIRECTION = ENUM_DIRECTION.DOWN

        Public Overrides Sub Execute(mTextArea As TextArea)
            If (mTextArea.Document.ReadOnly) Then
                Return
            End If

            Dim iDirection As Integer = If(m_Direction = ENUM_DIRECTION.UP, 1, -1)

            Dim mSelection As ISelection
            Dim iSelectionDelimiterLength As Integer = 0

            If (mTextArea.SelectionManager.HasSomethingSelected) Then
                Dim mStartPosition = mTextArea.SelectionManager.SelectionCollection(0).StartPosition
                Dim mEndPosition = mTextArea.SelectionManager.SelectionCollection(0).EndPosition
                Dim mLineSegment = mTextArea.Document.GetLineSegment(mEndPosition.Line)

                mSelection = New DefaultSelection(mTextArea.Document, New TextLocation(0, mStartPosition.Line), New TextLocation(mLineSegment.Length, mLineSegment.LineNumber))
                iSelectionDelimiterLength = mLineSegment.DelimiterLength
            Else
                Dim iCaretOffset As Integer = mTextArea.Caret.Offset
                Dim mLineSegment = mTextArea.Document.GetLineSegmentForOffset(iCaretOffset)

                mSelection = New DefaultSelection(mTextArea.Document, New TextLocation(0, mLineSegment.LineNumber), New TextLocation(mLineSegment.Length, mLineSegment.LineNumber))
                iSelectionDelimiterLength = mLineSegment.DelimiterLength
            End If

            Dim iTargetLine As Integer = mSelection.StartPosition.Line - iDirection
            If (iTargetLine < 0 OrElse iTargetLine > mTextArea.Document.TotalNumberOfLines - 1) Then
                Return
            End If

            Dim iTargetLineLength = (mSelection.Length + iSelectionDelimiterLength)
            If ((mSelection.Offset + iTargetLineLength) > mTextArea.Document.TextLength) Then
                Return
            End If

            Dim sLine = mTextArea.Document.GetText(mSelection.Offset, iTargetLineLength)
            If (sLine.Length = 0) Then
                Return
            End If

            If (iSelectionDelimiterLength = 0) Then
                sLine &= Environment.NewLine
            End If

            Try
                mTextArea.BeginUpdate()

                mTextArea.Document.UndoStack.StartUndoGroup()
                mTextArea.Document.Remove(mSelection.Offset, iTargetLineLength)

                If (iTargetLine < 0 OrElse iTargetLine > mTextArea.Document.TotalNumberOfLines - 1) Then
                    Dim mInsertLineSegment = mTextArea.Document.GetLineSegment(mTextArea.Document.TotalNumberOfLines - 1)
                    mTextArea.Document.Insert(mInsertLineSegment.Offset, Environment.NewLine & sLine)
                Else
                    Dim mInsertLineSegment = mTextArea.Document.GetLineSegment(iTargetLine)
                    mTextArea.Document.Insert(mInsertLineSegment.Offset, sLine)
                End If

                mTextArea.Document.UndoStack.EndUndoGroup()
            Finally
                mTextArea.EndUpdate()
            End Try

            'TODO: Fix extended selection glitches e.g via ShiftCaretLeft(), ShiftCaretRight()
            mTextArea.SelectionManager.SetSelection(New TextLocation(0, mSelection.StartPosition.Line - iDirection),
                                                        New TextLocation(mTextArea.Document.GetLineSegment(mSelection.EndPosition.Line - iDirection).Length, mSelection.EndPosition.Line - iDirection))
            mTextArea.Caret.Position = mTextArea.Caret.ValidatePosition(mTextArea.SelectionManager.SelectionCollection(0).EndPosition)
            mTextArea.Caret.UpdateCaretPosition()

            mTextArea.AutoClearSelection = False

            mTextArea.Document.RequestUpdate(New TextAreaUpdate(TextAreaUpdateType.WholeTextArea))
            mTextArea.Document.CommitUpdate()
        End Sub
    End Class

    Public Class DuplicateSelectedLine
        Inherits AbstractEditAction

        Enum ENUM_DIRECTION
            UP = 1
            DOWN = -1
        End Enum

        Property m_Direction As ENUM_DIRECTION = ENUM_DIRECTION.DOWN

        Public Overrides Sub Execute(mTextArea As TextArea)
            If (mTextArea.Document.ReadOnly) Then
                Return
            End If

            Try
                mTextArea.BeginUpdate()

                mTextArea.Document.UndoStack.StartUndoGroup()

                If (mTextArea.SelectionManager.HasSomethingSelected) Then
                    Dim sText As String = mTextArea.SelectionManager.SelectedText
                    Dim iCaretOffset As Integer = mTextArea.Caret.Offset

                    mTextArea.Document.Insert(iCaretOffset, sText)
                Else
                    Dim iCaretOffset As Integer = mTextArea.Caret.Offset
                    Dim iLineOffset As Integer = mTextArea.Document.GetLineSegmentForOffset(iCaretOffset).Offset
                    Dim iLineLen As Integer = mTextArea.Document.GetLineSegmentForOffset(iCaretOffset).Length

                    Select Case m_Direction
                        Case ENUM_DIRECTION.UP
                            mTextArea.Document.Insert(iLineOffset, mTextArea.Document.GetText(iLineOffset, iLineLen) & Environment.NewLine)

                            ' Move caret down because the inserted newline
                            Call (New CaretDown).Execute(mTextArea)
                        Case Else
                            mTextArea.Document.Insert(iLineOffset + iLineLen, Environment.NewLine & mTextArea.Document.GetText(iLineOffset, iLineLen))
                    End Select
                End If

                mTextArea.Document.UndoStack.EndUndoGroup()
            Finally
                mTextArea.EndUpdate()
            End Try

            mTextArea.Document.RequestUpdate(New TextAreaUpdate(TextAreaUpdateType.WholeTextArea))
            mTextArea.Document.CommitUpdate()
        End Sub
    End Class

    Public Class InsertBlankSelectedLine
        Inherits AbstractEditAction

        Enum ENUM_DIRECTION
            UP = 1
            DOWN = -1
        End Enum

        Property m_Direction As ENUM_DIRECTION = ENUM_DIRECTION.DOWN

        Public Overrides Sub Execute(mTextArea As TextArea)
            If (mTextArea.Document.ReadOnly) Then
                Return
            End If

            Try
                mTextArea.BeginUpdate()

                mTextArea.Document.UndoStack.StartUndoGroup()

                mTextArea.SelectionManager.ClearSelection()

                Select Case m_Direction
                    Case ENUM_DIRECTION.UP
                        Call (New CaretUp).Execute(mTextArea)
                        Call (New [End]).Execute(mTextArea)
                        Call (New [Return]).Execute(mTextArea)
                    Case Else
                        Call (New [End]).Execute(mTextArea)
                        Call (New [Return]).Execute(mTextArea)
                End Select

                mTextArea.Document.UndoStack.EndUndoGroup()
            Finally
                mTextArea.EndUpdate()
            End Try

            mTextArea.Document.RequestUpdate(New TextAreaUpdate(TextAreaUpdateType.WholeTextArea))
            mTextArea.Document.CommitUpdate()
        End Sub
    End Class

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
