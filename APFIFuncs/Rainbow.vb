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

    Public Sub SplitHashFile(ByVal mHelper As Helper)

        Dim swReader As New StreamReader(mHelper.InputFile)
        Dim OutputList As New List(Of List(Of String))
        Dim StartChar As Char = ""
        Dim inLine As String = ""
        Dim LineCounter As Integer = 0
        Dim lTempList As List(Of String)

        If mHelper.HasHeaders Then
            swReader.ReadLine()
        End If

        inLine = swReader.ReadLine()

        While swReader.EndOfStream = False

            StartChar = inLine.Substring(0, 1)

            'SELECT for now, must be a better way!!!
            Select StartChar
                Case "0"
                    lTempList = OutputList(0)
                    lTempList(lTempList.Count + 1) = inLine
                Case "1"
                    lTempList = OutputList(1)
                    lTempList(lTempList.Count + 1) = inLine
                Case "2"
                    lTempList = OutputList(2)
                    lTempList(lTempList.Count + 1) = inLine
                Case "3"
                    lTempList = OutputList(3)
                    lTempList(lTempList.Count + 1) = inLine
                Case "4"
                    lTempList = OutputList(4)
                    lTempList(lTempList.Count + 1) = inLine
                Case "5"
                    lTempList = OutputList(5)
                    lTempList(lTempList.Count + 1) = inLine
                Case "6"
                    lTempList = OutputList(6)
                    lTempList(lTempList.Count + 1) = inLine
                Case "7"
                    lTempList = OutputList(7)
                    lTempList(lTempList.Count + 1) = inLine
                Case "8"
                    lTempList = OutputList(8)
                    lTempList(lTempList.Count + 1) = inLine
                Case "9"
                    lTempList = OutputList(9)
                    lTempList(lTempList.Count + 1) = inLine
                Case "A"
                    lTempList = OutputList(10)
                    lTempList(lTempList.Count + 1) = inLine
                Case "B"
                    lTempList = OutputList(11)
                    lTempList(lTempList.Count + 1) = inLine
                Case "C"
                    lTempList = OutputList(12)
                    lTempList(lTempList.Count + 1) = inLine
                Case "D"
                    lTempList = OutputList(13)
                    lTempList(lTempList.Count + 1) = inLine
                Case "E"
                    lTempList = OutputList(14)
                    lTempList(lTempList.Count + 1) = inLine
                Case "F"
                    lTempList = OutputList(15)
                    lTempList(lTempList.Count + 1) = inLine
            End Select

            ' Check every 10000 lines if we have to dump the arrays out.
            If LineCounter >= 10000 Then
                WriteToFile(OutputList)
                LineCounter = 0
            Else
                LineCounter = LineCounter + 1
            End If

            ' Read the next line
            inLine = swReader.ReadLine()
        End While

        ' This cleans up the arrays and finalises the writes
        WriteToFile(OutputList, True)
    End Sub

    Private Sub WriteToFile(ByRef mList As List(Of List(Of String)), Optional mFinalise As Boolean = False)
        Dim swWriter As StreamWriter
        For Each j As List(Of String) In mList
            If j.Count > 9999 Or mFinalise = True Then
                swWriter = New StreamWriter(j(0).Substring(0, 1) & "Out.txt", True)
                For i As Integer = 0 To j.Count - 1
                    swWriter.WriteLine(j(i))
                Next
                j.Clear()
                swWriter.Close()
            End If
        Next


    End Sub



End Class
