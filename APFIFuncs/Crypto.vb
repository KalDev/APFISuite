Imports System.Security.Cryptography.X509Certificates
Imports System.Security.Cryptography
Imports System.IO
Imports System.Text


Public Class Crypto
    Private certLocationValue As String
    Private certNameValues As List(Of String)
    Private activeCertificateValue As X509Certificate
    Private saltValue As String

#Region "Properties"
    Public Property CertLocation() As String
        Get
            ' Gets the property value. 
            Return certLocationValue
        End Get
        Set(ByVal Value As String)
            ' Sets the property value.
            certLocationValue = Value
        End Set
    End Property

    Public ReadOnly Property CertNames() As List(Of String)
        Get
            ' Gets the property value. 
            Return certNameValues
        End Get
    End Property

    Public Property ActiveCertificate As X509Certificate
        Get
            ' Gets the property value. 
            Return activeCertificateValue
        End Get
        Set(ByVal Value As X509Certificate)
            ' Sets the property value.
            activeCertificateValue = Value
        End Set
    End Property

    Public Property Salt As String
        Get
            ' Gets the property value. 
            Return saltValue
        End Get
        Set(ByVal Value As String)
            ' Sets the property value.
            saltValue = Value
        End Set
    End Property
#End Region


    Public Sub Pseudo(ByRef myHelper As Helper)
        Dim LineCount As Integer = 0
        Dim Lines10 As Integer = 0
        Dim LoopLine As Integer = 0
        Dim StartTime As Date = Now
        Dim EndTime As Date

        LineCount = myHelper.GetLineCount(myHelper.InputFile)

        Dim swWriter As New StreamWriter(myHelper.OutputFile)
        Dim swReader As New StreamReader(myHelper.InputFile)

        Dim lineIn As String
        Dim cols() As String

        Dim worker As System.Security.Cryptography.SHA256 = SHA256Managed.Create
        Dim workerByte As Byte()

        lineIn = swReader.ReadLine

        If myHelper.HasHeaders Then
            swWriter.WriteLine(lineIn)
            LineCount = LineCount - 1
            lineIn = swReader.ReadLine
        End If

        Lines10 = LineCount / 10
        Console.WriteLine("Total Lines : {0}", LineCount)

        'Read each line in turn
        Do
            cols = lineIn.Split(",")

            For i = 0 To cols.Length - 1
                If i = myHelper.ColumnToHash Then
                    workerByte = Encoding.UTF8.GetBytes(cols(i) & Me.Salt)
                    swWriter.Write(PrintByteArray(worker.ComputeHash(workerByte)))
                Else
                    swWriter.Write(cols(i))
                End If

                If i <> cols.Length - 1 Then
                    swWriter.Write(",")
                End If

            Next

            swWriter.Write(vbCrLf)
            If LoopLine = Lines10 Then
                Console.Write("{0}Complete: {1}/{2}", vbCr, Lines10, LineCount)
                Lines10 = Lines10 + (LineCount / 10)
            End If

            LoopLine = LoopLine + 1
            ' read the next line
            lineIn = swReader.ReadLine
        Loop Until lineIn Is Nothing

        EndTime = Now

        Console.Write("{0}Complete: {1}/{2}", vbCr, LineCount, LineCount)
        Console.Write(vbCrLf)
        Console.WriteLine("Total Time Taken : {0} seconds", DateDiff(DateInterval.Second, StartTime, EndTime))

        swWriter.Close()
        swReader.Close()

    End Sub

    Public Function GeMainCryptolt(Optional ByVal pChars As Integer = 20) As String
        Dim saltValue As String = ""
        Dim worker As System.Security.Cryptography.SHA256 = SHA256Managed.Create
        Dim workerByte As Byte()

        workerByte = Encoding.UTF8.GetBytes(Guid.NewGuid.ToString)
        workerByte = worker.ComputeHash(workerByte)

        saltValue = PrintByteArray(workerByte)

        Return saltValue.Substring(0, pChars)
    End Function



    Public Sub GetCertificates()
        ' Read the cert files from the given location

    End Sub

    Public Overloads Function PrintCertificateDetails() As String
        Dim sOutput As String = ""

        Return sOutput
    End Function

    Public Overloads Function PrintCertificateDetails(ByVal sSubject As String) As String
        Dim sOutput As String = ""

        Return sOutput
    End Function


    Public Shared Function PrintByteArray(ByVal array() As Byte) As String
        Dim i As Integer
        Dim outputVal As String = ""

        For i = 0 To array.Length - 1
            outputVal = outputVal & String.Format("{0:X2}", array(i))
        Next i

        Return outputVal

    End Function







End Class
