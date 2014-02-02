Imports System.IO


Public Class Helper
    Public Property ColumnToHash() As Integer
    Public Property ConfigFile() As String
    Public Property OutputFile() As String = ""
    Public Property InputFile() As String
    Public Property HasHeaders() As Boolean
    Public Property TestRows() As Integer
    Public Property RealNHSNumber() As Boolean = False

#Region "TestFiles - testfile generation code"

    Public Sub GenerateTestFile(Optional ByVal NumRows As Integer = 1000)
        'This will generate a sample file for us.
        Dim _OutputFile As String = Me.OutputFile

        If _OutputFile = "" Then
            _OutputFile = "testfile.csv"
        End If

        Dim OutWrite As New StreamWriter(_OutputFile)


        OutWrite.Write("NHS#, dob, test1, test 2, test 3" & vbCrLf)

        For i As Integer = 1 To NumRows
            OutWrite.Write(GenFakeNHSNumber() & ",")
            OutWrite.Write(GenDOB() & ",")
            OutWrite.Write(CInt(Int((10 * Rnd()) + 1)) & ",")
            OutWrite.Write(CInt(Int((250 * Rnd()) + 100)) & ",")
            OutWrite.Write(CInt(Int((9999 * Rnd()) + 1000)) & vbCrLf)
        Next

        OutWrite.Close()

    End Sub

    Private Function GenFakeNHSNumber() As String
        Dim vOutput As String = ""

        vOutput = vOutput & CInt(Int((999 * Rnd()) + 100)) & " "
        vOutput = vOutput & CInt(Int((999 * Rnd()) + 100)) & " "
        vOutput = vOutput & CInt(Int((9999 * Rnd()) + 1000))

        Return vOutput
    End Function


    Public Sub GenNHSNumberList()
        'Open the file for writing
        Dim swWriter As New StreamWriter(Me.OutputFile)
        Dim iOutput As Integer = 0
        Dim StartTime As Date = Now
        Dim EndTime As Date

        'BIG LOOP
        For i As Integer = 400000000 To 499999999
            iOutput = CheckNHSNumber(i)
            If iOutput > -1 Then
                swWriter.WriteLine(i & iOutput)
            End If
        Next

        EndTime = Now()
        Console.WriteLine("First Batch Written in {0} minutes", DateDiff(DateInterval.Minute, StartTime, EndTime))

        'SECOND BIG LOOP
        For i As Integer = 600000000 To 708800001
            iOutput = CheckNHSNumber(i)
            If iOutput > -1 Then
                swWriter.WriteLine(i & iOutput)
            End If
        Next

        EndTime = Now()
        Console.WriteLine("Finished in {0} minutes", DateDiff(DateInterval.Minute, StartTime, EndTime))

        swWriter.Close()

    End Sub

    Private Function CheckNHSNumber(ByVal iNHSNumber As Integer) As Integer
        Dim iArray() As Integer
        Dim numSum As Integer = 0
        Dim DivRem As Integer = 0

        iArray = BreakNumber(iNHSNumber)
        For j As Integer = 0 To iArray.Length - 1
            numSum = numSum + (iArray(j) * (11 - (j + 1)))
        Next

        DivRem = numSum Mod 11

        DivRem = 11 - DivRem

        If DivRem = 11 Then
            DivRem = 0
        ElseIf DivRem = 10 Then
            Return -1
        End If
        Return DivRem
    End Function

    Public Function BreakNumber(ByVal iNumber As Integer) As Integer()
        Dim sString As String = iNumber.ToString
        Dim Output(sString.Length - 1) As Integer
        For i As Integer = 0 To sString.Length - 1
            Output(i) = sString.Substring(i, 1)
        Next
        Return Output
    End Function

    Private Function GenDOB() As String
        Dim vOutput As String = ""

        vOutput = DateAdd("d", Int((36500 * Rnd()) + 1), "01/01/1910")
        vOutput = FormatDateTime(vOutput, DateFormat.ShortDate)
        Return vOutput
    End Function

#End Region


    ' Code taken from http://bytes.com/topic/net/answers/439009-what-fastest-way-count-lines-text-file
    Public Function GetLineCount(ByVal FileName As String) As Integer
        Dim total As Integer = 0

        If File.Exists(FileName) Then
            Dim buffer(32 * 1024) As Char
            Dim i As Integer
            Dim read As Integer

            Dim reader As TextReader = File.OpenText(FileName)
            read = reader.Read(buffer, 0, buffer.Length)

            While (read > 0)
                i = 0
                While i < read

                    If buffer(i) = Chr(10) Then
                        total += 1
                    End If

                    i += 1
                End While

                read = reader.Read(buffer, 0, buffer.Length)
            End While

            reader.Close()
            reader = Nothing

            If Not buffer(i - 1) = Chr(10) Then
                total += 1
            End If

        End If

        Return total
    End Function

    Public Sub PrintHelp()
        Console.Write(vbCrLf & _
                "***** APFI Help v0.0.1 *****" & vbCrLf & _
                " " & vbCrLf & _
                "APFI is a command line Pseuodonimisation tool free for use." & vbCrLf & _
                "Released under GPL License 3.0, James Wood - apfi@twistedknowledge.co.uk" & vbCrLf & _
                " " & vbCrLf & _
                "NOTE Error Checking is NOT implemented so use correctly!" & vbCrLf & _
                " " & vbCrLf & _
                "**Any variables set by switch will override the default AND config file values**" & vbCrLf & _
                " " & vbCrLf & _
                "General Parameters" & vbCrLf & _
                "-help        - This help file" & vbCrLf & _
                "-c  <file>   - Override the default.conf config file and use" & vbCrLf & _
                "-cl <x>      - Zero based index of the column to hash" & vbCrLf & _
                "-o  <file>   - Override the default output location of output.csv" & vbCrLf & _
                "-i  <file>   - Override the default input location of input.csv" & vbCrLf & _
                "-s  <salt>   - Use <salt> as string for salt, 6-64 chars only" & vbCrLf & _
                "-sr <6-64>   - Use a random salt <6-64> chars in length" & vbCrLf & _
                "-h           - Input and Output Files have column headers" & vbCrLf & _
                " " & vbCrLf & _
                "Exclusive Parameters" & vbCrLf & _
                "-p           - Perform Pseudo Operation" & vbCrLf & _
                "-g  <x>      - Generate a test file (testfile.csv) of <x> rows" & vbCrLf & _
                " " & vbCrLf & _
                "Testing and Debugging" & vbCrLf & _
                "-lc <loc>    - Load Certificates from specified location and display results" & vbCrLf & _
                "-rn          - Generate Real NHS Number instead of default fake" & vbCrLf & _
                "-genNum      - Generate valid NHS Number List - WARNING LONG TIME TO RUN!" & vbCrLf & _
                "....." & vbCrLf)
    End Sub


End Class
