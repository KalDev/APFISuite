Imports System.IO


Public Class Helper

#Region "Properties"
    Public Property ColumnToHash() As Integer = 0
    Public Property ConfigFile() As String = "default.conf"
    Public Property OutputFile() As String = "output.csv"
    Public Property InputFile() As String = "input.csv"
    Public Property HasHeaders() As Boolean = False
    Public Property SplitFiles As String = ""

    Dim _TestRows As Integer
    Public Property TestRows() As Integer
        Set(NumRows As Integer)
            If NumRows < 0 Then
                _TestRows = 1000
            Else
                _TestRows = NumRows
            End If

        End Set
        Get
            Return _TestRows
        End Get
    End Property

    Public Property RealNHSNumber() As Boolean = False
    Public Property SaltFile As Boolean = False
#End Region

#Region "TestFiles - testfile generation code"

    Public Sub GenerateTestFile()
        Dim OutWrite As New StreamWriter(Me.OutputFile)

        If Me.HasHeaders Then
            OutWrite.Write("NHS#, dob, test1, test 2, test 3" & vbCrLf)
        End If

        Randomize()

        For i As Integer = 1 To Me.TestRows
            If Me.RealNHSNumber Then
                OutWrite.Write(GenRealNHSNumber() & ",")
            Else
                OutWrite.Write(GenFakeNHSNumber() & ",")
            End If

            OutWrite.Write(GenDOB() & ",")
            OutWrite.Write(CInt(Int((10 * Rnd()) + 1)) & ",")
            OutWrite.Write(CInt(Int((250 * Rnd()) + 100)) & ",")
            OutWrite.Write(CInt(Int((9999 * Rnd()) + 1000)) & vbCrLf)
        Next

        OutWrite.Close()

    End Sub

#End Region

#Region "Public Functions / Subs"
    Public Function CheckNHSNumber(ByVal iNHSNumber As Integer) As Integer
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
#End Region

#Region "Private Functions / Subs"
    Private Function GenFakeNHSNumber() As String
        Dim vOutput As String = ""

        vOutput = vOutput & CInt(Int((899 * Rnd()) + 100))
        vOutput = vOutput & CInt(Int((899 * Rnd()) + 100))
        vOutput = vOutput & CInt(Int((8999 * Rnd()) + 1000))

        Return vOutput
    End Function

    Private Function GenRealNHSNumber() As String
        Dim vOutput As Integer = 0
        Dim vCheckDigit As Integer = 0

        Randomize()

        Do
            vOutput = CInt(Math.Floor((499999999 - 400000000 + 1) * Rnd())) + 400000000
            vCheckDigit = CheckNHSNumber(vOutput)
        Loop Until vCheckDigit > -1

        Return CStr(vOutput & vCheckDigit)
    End Function

    Private Function BreakNumber(ByVal iNumber As Integer) As Integer()
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




End Class
