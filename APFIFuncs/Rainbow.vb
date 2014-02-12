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
        Dim _lTempList As New List(Of String)

        Dim _Directory As String = Environment.GetCommandLineArgs()(0).Substring(0, Environment.GetCommandLineArgs()(0).LastIndexOf("\") + 1) & "SPLITFILES - " & mHelper.InputFile.Substring(0, mHelper.InputFile.IndexOf("."))


        If My.Computer.FileSystem.DirectoryExists(_Directory) Then
            My.Computer.FileSystem.DeleteDirectory(_Directory, FileIO.DeleteDirectoryOption.DeleteAllContents)
        End If

        Directory.CreateDirectory(_Directory)

        If mHelper.HasHeaders Then
            swReader.ReadLine()
        End If

        'Initialise the LIST
        For x As Integer = 0 To 15
            OutputList.Add(New List(Of String))
        Next

        inLine = swReader.ReadLine()

        While swReader.EndOfStream = False

            StartChar = inLine.Substring(0, 1)

            'SELECT for now, must be a better way!!!
            Select Case StartChar
                Case "0"
                    _lTempList = OutputList(0)
                Case "1"
                    _lTempList = OutputList(1)
                Case "2"
                    _lTempList = OutputList(2)
                Case "3"
                    _lTempList = OutputList(3)
                Case "4"
                    _lTempList = OutputList(4)
                Case "5"
                    _lTempList = OutputList(5)
                Case "6"
                    _lTempList = OutputList(6)
                Case "7"
                    _lTempList = OutputList(7)
                Case "8"
                    _lTempList = OutputList(8)
                Case "9"
                    _lTempList = OutputList(9)
                Case "A"
                    _lTempList = OutputList(10)
                Case "B"
                    _lTempList = OutputList(11)
                Case "C"
                    _lTempList = OutputList(12)
                Case "D"
                    _lTempList = OutputList(13)
                Case "E"
                    _lTempList = OutputList(14)
                Case "F"
                    _lTempList = OutputList(15)
            End Select

            _lTempList.Add(inLine)

            ' Check every 10000 lines if we have to dump the arrays out.
            If LineCounter >= 10000 Then
                WriteToFile(OutputList, _Directory)
                LineCounter = 0
            Else
                LineCounter = LineCounter + 1
            End If

            ' Read the next line
            inLine = swReader.ReadLine()
        End While

        ' This cleans up the arrays and finalises the writes
        WriteToFile(OutputList, _Directory, True)
    End Sub

    Public Sub CompareRainbow(ByVal mHelper As Helper)
        Dim swReader As New StreamReader(mHelper.InputFile)
        Dim swHash As StreamReader = Nothing
        Dim swWriter As New StreamWriter(mHelper.OutputFile)

        Dim inLine As String = ""
        Dim inHashLine As String = ""
        Dim fExit As Boolean = False
        Dim TotalLines As Integer = 0
        Dim StartTime As Date = Now()
        Dim FailCount As Integer = 0


        TotalLines = mHelper.GetLineCount(mHelper.InputFile)

        Console.WriteLine("Total Lines to search - " & TotalLines)
        Console.WriteLine("Start time - " & StartTime.TimeOfDay.ToString)

        Do Until swReader.EndOfStream
            inLine = swReader.ReadLine()
            fExit = False
            swHash = New StreamReader("SPLITFILES - " & mHelper.SplitFiles & "\" & inLine.Substring(0, 1) & "Out.txt")

            Do Until swHash.EndOfStream Or fExit = True
                inHashLine = swHash.ReadLine
                If inHashLine.Contains(inLine.Substring(0, inLine.IndexOf(","))) Then
                    swWriter.WriteLine(inHashLine.Substring(inHashLine.IndexOf(",") + 1, inHashLine.Length - inHashLine.IndexOf(",") - 1) & "," & inLine)
                    fExit = True
                End If
            Loop

            If fExit = False Then
                swWriter.WriteLine("NO MATCH," & inLine)
                FailCount = FailCount + 1
            End If

            swHash.Close()
        Loop

        swWriter.Close()

        Console.WriteLine("Search finished, total time {0} seconds", DateDiff(DateInterval.Second, StartTime, Now()))
        Console.WriteLine("Total Failed Matches - {0} or {1}%", FailCount, Math.Round((FailCount / TotalLines) * 100, 2))

    End Sub


    Private Sub WriteToFile(ByRef mList As List(Of List(Of String)), oDirectory As String, Optional mFinalise As Boolean = False)
        Dim swWriter As StreamWriter
        For Each j As List(Of String) In mList
            If (j.Count > 9999 Or mFinalise = True) And j.Count > 0 Then
                swWriter = New StreamWriter(oDirectory & "\" & j(0).Substring(0, 1) & "Out.txt", True)
                For i As Integer = 0 To j.Count - 1
                    swWriter.WriteLine(j(i))
                Next
                j.Clear()
                swWriter.Close()
            End If
        Next j
    End Sub



End Class
