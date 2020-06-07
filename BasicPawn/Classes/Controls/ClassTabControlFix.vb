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


Public Class ClassTabControlFix
    Inherits TabControl

    ''' <summary>
    ''' Updates the line overflow.
    ''' Should be used after removing tab pages.
    ''' </summary>
    Public Sub UpdateLineOverflow()
        If (Me.Multiline) Then
            Return
        End If

        Me.SuspendLayout()
        Me.Multiline = True
        Me.Multiline = False
        Me.ResumeLayout()
    End Sub

    Public Sub SelectTabNoFocus(sTabPageName As String)
        If (sTabPageName Is Nothing) Then
            Throw New ArgumentNullException("TabPage name null")
        End If

        Dim mTabPage As TabPage = Me.TabPages(sTabPageName)
        SelectTabNoFocus(mTabPage)
    End Sub

    Public Sub SelectTabNoFocus(iIndex As Integer)
        If (iIndex < 0 OrElse iIndex > Me.TabCount - 1) Then
            Throw New ArgumentException("index out of range")
        End If

        Dim mTabPage As TabPage = Me.TabPages(iIndex)
        SelectTabNoFocus(mTabPage)
    End Sub

    ''' <summary>
    ''' Selects a tab  without focusing it.
    ''' </summary>
    ''' <param name="mTabPage"></param>
    Public Sub SelectTabNoFocus(mTabPage As TabPage)
        Dim mParentForm = Form.ActiveForm

        If (mParentForm IsNot Nothing) Then
            'See: https://stackoverflow.com/a/439606
            Dim mFocusedControl As Control = mParentForm
            Dim mContainer = TryCast(mFocusedControl, IContainerControl)

            While (mContainer IsNot Nothing)
                mFocusedControl = mContainer.ActiveControl
                mContainer = TryCast(mFocusedControl, IContainerControl)
            End While

            Me.SelectTab(mTabPage)

            If (mFocusedControl IsNot Nothing) Then
                mFocusedControl.Focus()
            End If

            Return
        End If

        Me.SelectTab(mTabPage)
    End Sub
End Class
