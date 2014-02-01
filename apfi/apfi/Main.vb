Module Main

    Dim MainHelper As New APFIHelper
    Dim MainCrypto As New APFICrypto


    Sub Main()
        Dim doAction As String = ""
        doAction = ParseCommandLine()

        Select Case doAction
            Case "pseud"
                'Actually do some pseudonimisation
                MainCrypto.Pseudo(MainHelper)
                Console.WriteLine("Success, SALT is " & MainCrypto.Salt)
            Case "gentest"
                MainHelper.GenerateTestFile(MainHelper.TestRows)
            Case "gennum"
                MainHelper.GenNHSNumberList()
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

        If clArgs(1).ToLower = "-help" Then MainHelper.PrintHelp()

        ' The harder part, parse the arguments and do something useful
        Dim argValue As Boolean = False

        For i = 1 To clArgs.Length - 1
            Select Case clArgs(i)
                Case "-c"
                    MainHelper.ConfigFile = clArgs(i + 1)
                    i = i + 1
                Case "-cl"
                    MainHelper.ColumnToHash = clArgs(i + 1)
                    i = i + 1
                Case "-o"
                    MainHelper.OutputFile = clArgs(i + 1)
                    i = i + 1
                Case "-i"
                    MainHelper.InputFile = clArgs(i + 1)
                    i = i + 1
                Case "-s"
                    MainCrypto.Salt = clArgs(i + 1)
                    If MainCrypto.Salt.Length > 64 Then
                        MainCrypto.Salt = MainCrypto.Salt.Substring(0, 63)
                    End If
                    i = i + 1
                Case "-sr"
                    MainCrypto.Salt = MainCrypto.GeMainCryptolt()
                Case "-genNum"
                    postAction = "gennum"
                Case "-rn"
                    MainHelper.RealNHSNumber = True
                Case "-h"
                    MainHelper.HasHeaders = True
                Case "-p"
                    postAction = "pseud"
                Case "-lc"
                    MainCrypto.CertLocation = clArgs(i + 1)
                    MainCrypto.GetCertificates()
                    Console.Write(MainCrypto.PrintCertificateDetails())
                    i = i + 1
                Case "-g"
                    MainHelper.TestRows = clArgs(i + 1)
                    postAction = "gentest"
                    i = i + 1
            End Select
        Next i

        Return postAction
    End Function


End Module
