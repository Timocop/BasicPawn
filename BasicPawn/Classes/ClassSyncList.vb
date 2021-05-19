'BasicPawn
'Copyright(C) 2021 Externet

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


''' <summary>
''' A thread-safe list.
''' </summary>
''' <typeparam name="T"></typeparam>
Public Class ClassSyncList(Of T)
    Implements IList(Of T)

    Private _lock As New Object()
    Private _storage As New List(Of T)()
#Region "Stack Like"
    Public Function PopFirst() As T
        SyncLock _lock
            Try
                Return _storage(0)
            Finally
                _storage.RemoveAt(0)
            End Try
        End SyncLock
    End Function

    Public Function PopLast() As T
        SyncLock _lock
            Try
                Return _storage(_storage.Count - 1)
            Finally
                _storage.RemoveAt(_storage.Count - 1)
            End Try
        End SyncLock
    End Function
#End Region

#Region "List"
    Public Property Capacity As Integer
        Get
            SyncLock _lock
                Return _storage.Capacity
            End SyncLock
        End Get
        Set(value As Integer)
            SyncLock _lock
                _storage.Capacity = value
            End SyncLock
        End Set
    End Property
    Public ReadOnly Property Count As Integer
        Get
            SyncLock _lock
                Return _storage.Count
            End SyncLock
        End Get
    End Property
    Default Public Property Item(index As Integer) As T
        Get
            SyncLock _lock
                Return _storage(index)
            End SyncLock
        End Get
        Set
            SyncLock _lock
                _storage(index) = Value
            End SyncLock
        End Set
    End Property

    Public Sub Add(item As T)
        SyncLock _lock
            _storage.Add(item)
        End SyncLock
    End Sub
    Public Sub AddRange(collection As IEnumerable(Of T))
        SyncLock _lock
            _storage.AddRange(collection)
        End SyncLock
    End Sub
    Public Sub Clear()
        SyncLock _lock
            _storage.Clear()
        End SyncLock
    End Sub
    Public Sub CopyTo(array() As T)
        SyncLock _lock
            _storage.CopyTo(array)
        End SyncLock
    End Sub
    Public Sub CopyTo(array() As T, arrayIndex As Integer)
        SyncLock _lock
            _storage.CopyTo(array, arrayIndex)
        End SyncLock
    End Sub
    Public Sub CopyTo(index As Integer, array() As T, arrayIndex As Integer, count As Integer)
        SyncLock _lock
            _storage.CopyTo(index, array, arrayIndex, count)
        End SyncLock
    End Sub
    Public Sub ForEach(action As Action(Of T))
        SyncLock _lock
            _storage.ForEach(action)
        End SyncLock
    End Sub
    Public Sub Insert(index As Integer, item As T)
        SyncLock _lock
            _storage.Insert(index, item)
        End SyncLock
    End Sub
    Public Sub InsertRange(index As Integer, collection As IEnumerable(Of T))
        SyncLock _lock
            _storage.InsertRange(index, collection)
        End SyncLock
    End Sub
    Public Sub RemoveAt(index As Integer)
        SyncLock _lock
            _storage.RemoveAt(index)
        End SyncLock
    End Sub
    Public Sub RemoveRange(index As Integer, count As Integer)
        SyncLock _lock
            _storage.RemoveRange(index, count)
        End SyncLock
    End Sub
    Public Sub Reverse()
        SyncLock _lock
            _storage.Reverse()
        End SyncLock
    End Sub
    Public Sub Reverse(index As Integer, count As Integer)
        SyncLock _lock
            _storage.Reverse(index, count)
        End SyncLock
    End Sub
    Public Sub Sort()
        SyncLock _lock
            _storage.Sort()
        End SyncLock
    End Sub
    Public Sub Sort(comparer As IComparer(Of T))
        SyncLock _lock
            _storage.Sort(comparer)
        End SyncLock
    End Sub
    Public Sub Sort(comparison As Comparison(Of T))
        SyncLock _lock
            _storage.Sort(comparison)
        End SyncLock
    End Sub
    Public Sub Sort(index As Integer, count As Integer, comparer As IComparer(Of T))
        SyncLock _lock
            _storage.Sort(index, count, comparer)
        End SyncLock
    End Sub
    Public Sub TrimExcess()
        SyncLock _lock
            _storage.TrimExcess()
        End SyncLock
    End Sub

    Public Function AsReadOnly() As ObjectModel.ReadOnlyCollection(Of T)
        SyncLock _lock
            Return _storage.AsReadOnly
        End SyncLock
    End Function
    Public Function BinarySearch(item As T) As Integer
        SyncLock _lock
            Return _storage.BinarySearch(item)
        End SyncLock
    End Function
    Public Function BinarySearch(item As T, comparer As IComparer(Of T)) As Integer
        SyncLock _lock
            Return _storage.BinarySearch(item, comparer)
        End SyncLock
    End Function
    Public Function BinarySearch(index As Integer, count As Integer, item As T, comparer As IComparer(Of T)) As Integer
        SyncLock _lock
            Return _storage.BinarySearch(index, count, item, comparer)
        End SyncLock
    End Function
    Public Function Contains(item As T) As Boolean
        SyncLock _lock
            Return _storage.Contains(item)
        End SyncLock
    End Function
    Public Function ConvertAll(Of TOutput)(converter As Converter(Of T, TOutput)) As List(Of TOutput)
        SyncLock _lock
            Return _storage.ConvertAll(converter)
        End SyncLock
    End Function
    Public Function Exists(match As Predicate(Of T)) As Boolean
        SyncLock _lock
            Return _storage.Exists(match)
        End SyncLock
    End Function
    Public Function Find(match As Predicate(Of T)) As T
        SyncLock _lock
            Return _storage.Find(match)
        End SyncLock
    End Function
    Public Function FindAll(match As Predicate(Of T)) As List(Of T)
        SyncLock _lock
            Return _storage.FindAll(match)
        End SyncLock
    End Function
    Public Function FindIndex(match As Predicate(Of T)) As Integer
        SyncLock _lock
            Return _storage.FindIndex(match)
        End SyncLock
    End Function
    Public Function FindIndex(startIndex As Integer, match As Predicate(Of T)) As Integer
        SyncLock _lock
            Return _storage.FindIndex(startIndex, match)
        End SyncLock
    End Function
    Public Function FindIndex(startIndex As Integer, count As Integer, match As Predicate(Of T)) As Integer
        SyncLock _lock
            Return _storage.FindIndex(startIndex, count, match)
        End SyncLock
    End Function
    Public Function FindLast(match As Predicate(Of T)) As T
        SyncLock _lock
            Return _storage.FindLast(match)
        End SyncLock
    End Function
    Public Function FindLastIndex(match As Predicate(Of T)) As Integer
        SyncLock _lock
            Return _storage.FindLastIndex(match)
        End SyncLock
    End Function
    Public Function FindLastIndex(startIndex As Integer, match As Predicate(Of T)) As Integer
        SyncLock _lock
            Return _storage.FindLastIndex(startIndex, match)
        End SyncLock
    End Function
    Public Function FindLastIndex(startIndex As Integer, count As Integer, match As Predicate(Of T)) As Integer
        SyncLock _lock
            Return _storage.FindLastIndex(startIndex, count, match)
        End SyncLock
    End Function
    Public Function GetEnumerator() As IEnumerator(Of T)
        SyncLock _lock
            Return DirectCast(_storage.GetEnumerator(), IEnumerator(Of T))
        End SyncLock
    End Function
    Public Function GetRange(index As Integer, count As Integer) As List(Of T)
        SyncLock _lock
            Return _storage.GetRange(index, count)
        End SyncLock
    End Function
    Public Function IndexOf(item As T) As Integer
        SyncLock _lock
            Return _storage.IndexOf(item)
        End SyncLock
    End Function
    Public Function IndexOf(item As T, index As Integer) As Integer
        SyncLock _lock
            Return _storage.IndexOf(item, index)
        End SyncLock
    End Function
    Public Function IndexOf(item As T, index As Integer, count As Integer) As Integer
        SyncLock _lock
            Return _storage.IndexOf(item, index, count)
        End SyncLock
    End Function
    Public Function LastIndexOf(item As T) As Integer
        SyncLock _lock
            Return _storage.LastIndexOf(item)
        End SyncLock
    End Function
    Public Function LastIndexOf(item As T, index As Integer) As Integer
        SyncLock _lock
            Return _storage.LastIndexOf(item, index)
        End SyncLock
    End Function
    Public Function LastIndexOf(item As T, index As Integer, count As Integer) As Integer
        SyncLock _lock
            Return _storage.LastIndexOf(item, index, count)
        End SyncLock
    End Function
    Public Function Remove(item As T) As Boolean
        SyncLock _lock
            Return _storage.Remove(item)
        End SyncLock
    End Function
    Public Function RemoveAll(match As Predicate(Of T)) As Integer
        SyncLock _lock
            Return _storage.RemoveAll(match)
        End SyncLock
    End Function
    Public Function ToArray() As T()
        SyncLock _lock
            Return _storage.ToArray
        End SyncLock
    End Function
    Public Function TrueForAll(match As Predicate(Of T)) As Boolean
        SyncLock _lock
            Return _storage.TrueForAll(match)
        End SyncLock
    End Function

    Public Delegate Function Func(Of ConcurrentList, TResult)(i As ConcurrentList) As TResult

    Public Sub DoSync(action As Action(Of ClassSyncList(Of T)))
        GetSync(Function(lList)
                    action(lList)
                    Return 0
                End Function)
    End Sub

    Public Function GetSync(Of TResult)(func As Func(Of ClassSyncList(Of T), TResult)) As TResult
        SyncLock _lock
            Return func(Me)
        End SyncLock
    End Function
#End Region

#Region "ICollection"
    Private ReadOnly Property ICollection_IsReadOnly() As Boolean Implements ICollection(Of T).IsReadOnly
        Get
            Return DirectCast(_storage, IList(Of T)).IsReadOnly
        End Get
    End Property

    Private ReadOnly Property ICollection_Count As Integer Implements ICollection(Of T).Count
        Get
            Return DirectCast(_storage, ICollection(Of T)).Count
        End Get
    End Property

    Private Sub ICollection_Add(item As T) Implements ICollection(Of T).Add
        SyncLock _lock
            DirectCast(_storage, ICollection(Of T)).Add(item)
        End SyncLock
    End Sub

    Private Sub ICollection_Clear() Implements ICollection(Of T).Clear
        SyncLock _lock
            DirectCast(_storage, ICollection(Of T)).Clear()
        End SyncLock
    End Sub

    Private Function ICollection_Contains(item As T) As Boolean Implements ICollection(Of T).Contains
        SyncLock _lock
            Return DirectCast(_storage, ICollection(Of T)).Contains(item)
        End SyncLock
    End Function

    Private Sub ICollection_CopyTo(array() As T, arrayIndex As Integer) Implements ICollection(Of T).CopyTo
        SyncLock _lock
            DirectCast(_storage, ICollection(Of T)).CopyTo(array, arrayIndex)
        End SyncLock
    End Sub

    Private Function ICollection_Remove(item As T) As Boolean Implements ICollection(Of T).Remove
        SyncLock _lock
            Return DirectCast(_storage, ICollection(Of T)).Remove(item)
        End SyncLock
    End Function
#End Region

#Region "IEnumerable"
    Private Function IEnumerable_GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
        SyncLock _lock
            Return DirectCast(_storage.ToArray().GetEnumerator(), IEnumerator(Of T))
        End SyncLock
    End Function

    Private Function IEnumerable_GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
        SyncLock _lock
            Return DirectCast(_storage.ToArray().GetEnumerator(), IEnumerator)
        End SyncLock
    End Function
#End Region

#Region "IList"
    Private Property IList_Item(index As Integer) As T Implements IList(Of T).Item
        Get
            SyncLock _lock
                Return DirectCast(_storage, IList(Of T)).Item(index)
            End SyncLock
        End Get
        Set(value As T)
            SyncLock _lock
                DirectCast(_storage, IList(Of T)).Item(index) = value
            End SyncLock
        End Set
    End Property

    Private Function IList_IndexOf(item As T) As Integer Implements IList(Of T).IndexOf
        SyncLock _lock
            Return DirectCast(_storage, IList(Of T)).IndexOf(item)
        End SyncLock
    End Function

    Private Sub IList_Insert(index As Integer, item As T) Implements IList(Of T).Insert
        SyncLock _lock
            DirectCast(_storage, IList(Of T)).Insert(index, item)
        End SyncLock
    End Sub

    Private Sub IList_RemoveAt(index As Integer) Implements IList(Of T).RemoveAt
        SyncLock _lock
            DirectCast(_storage, IList(Of T)).RemoveAt(index)
        End SyncLock
    End Sub
#End Region
End Class