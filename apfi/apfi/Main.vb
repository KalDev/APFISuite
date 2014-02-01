Module Main

    Dim HAL2000 As New APFIHelper
    Dim NSA As New APFIcrypto


    Sub Main()
        Dim doAction As String = ""
        doAction = ParseCommandLine()

        Select Case doAction
            Case "pseud"
                'Actually do some pseudonimisation
                NSA.Pseudo(HAL2000)
                Console.WriteLine("Success, SALT is " & NSA.Salt)
            Case "gentest"
                HAL2000.GenerateTestFile(HAL2000.TestRows)
            Case "gennum"
                HAL2000.GenNHSNumberList()
            Case Else

        End Select
    End Sub

    Public Function ParseCommandLine() As String
        ' Get the values of the command line in an array
        ' Index  Discription
        ' 0      Full path of executing program with program name
        ' 1      First switch in command in your example -t
        ' 2      First value in command in your example text1
        ' 3      Second switch in command in your example -s
        ' 4      Second value in command in your example text2

        Dim clArgs() As String = Environment.GetCommandLineArgs()
        ' Hold the command line values

        'This will hold any actions to perform once the command line params have been parsed.
        Dim postAction As String = ""

        If clArgs.Length = 1 Then
            Console.WriteLine("Type -help for help")
            End
        End If

        If clArgs(1).ToLower = "-help" Then HAL2000.PrintHelp()

        ' The harder part, parse the arguments and do something useful
        Dim argValue As Boolean = False

        For i = 1 To clArgs.Length - 1
            Select Case clArgs(i)
                Case "-c"
                    HAL2000.ConfigFile = clArgs(i + 1)
                    i = i + 1
                Case "-cl"
                    HAL2000.ColumnToHash = clArgs(i + 1)
                    i = i + 1
                Case "-o"
                    HAL2000.OutputFile = clArgs(i + 1)
                    i = i + 1
                Case "-i"
                    HAL2000.InputFile = clArgs(i + 1)
                    i = i + 1
                Case "-s"
                    NSA.Salt = clArgs(i + 1)
                    If NSA.Salt.Length > 64 Then
                        NSA.Salt = NSA.Salt.Substring(0, 63)
                    End If
                    i = i + 1
                Case "-sr"
                    NSA.Salt = NSA.GenSalt()
                Case "-genNum"
                    postAction = "gennum"
                Case "-h"
                    HAL2000.HasHeaders = True
                Case "-p"
                    postAction = "pseud"
                Case "-lc"
                    NSA.CertLocation = clArgs(i + 1)
                    NSA.GetCertificates()
                    Console.Write(NSA.PrintCertificateDetails())
                    i = i + 1
                Case "-g"
                    HAL2000.TestRows = clArgs(i + 1)
                    postAction = "gentest"
                    i = i + 1
            End Select
        Next i

        Return postAction
    End Function


End Module
