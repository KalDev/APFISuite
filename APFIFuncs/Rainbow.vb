Imports APFI.Helper
Imports System.IO

Public Class Rainbow

    Public Sub GenNHSNumberList(ByVal mHelper As Helper)
        'Open the file for writing
        Dim swWriter As New StreamWriter(mHelper.OutputFile)
        Dim iOutput As Integer = 0
        Dim StartTime As Date = Now
        Dim EndTime As Date

        'BIG LOOP
        For i As Integer = 400000000 To 499999999
            iOutput = mHelper.CheckNHSNumber(i)
            If iOutput > -1 Then
                swWriter.WriteLine(i & iOutput)
            End If
        Next

        EndTime = Now()
        Console.WriteLine("First Batch Written in {0} minutes", DateDiff(DateInterval.Minute, StartTime, EndTime))

        'SECOND BIG LOOP
        For i As Integer = 600000000 To 708800001
            iOutput = mHelper.CheckNHSNumber(i)
            If iOutput > -1 Then
                swWriter.WriteLine(i & iOutput)
            End If
        Next

        EndTime = Now()
        Console.WriteLine("Finished in {0} minutes", DateDiff(DateInterval.Minute, StartTime, EndTime))

        swWriter.Close()
    End Sub



End Class
