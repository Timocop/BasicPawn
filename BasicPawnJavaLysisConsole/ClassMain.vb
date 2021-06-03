Module ClassMain

    Sub Main()
        Try
            Dim sCmdArgs As String() = Environment.GetCommandLineArgs

            Dim sArgs As New List(Of String)
            For i = 1 To sCmdArgs.Length - 1
                sArgs.Add(sCmdArgs(i))
            Next

            lysis.Lysis.main(sArgs.ToArray)

#If DEBUG Then
            Console.ReadLine()
#End If
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Environment.Exit(-1)
        End Try
    End Sub

End Module
