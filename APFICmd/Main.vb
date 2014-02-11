Imports APFI

Module Main

    Dim MainHelper As New Helper
    Dim MainCrypto As New Crypto
    Dim MainRainbow As New Rainbow


    Sub Main()
        Dim doAction As String = ""
        doAction = ParseCommandLine()

        Select Case doAction
            Case "pseud"
                'Actually do some pseudonimisation
                MainCrypto.Pseudo(MainHelper)
                Console.WriteLine("Success, SALT is " & MainCrypto.Salt)
            Case "gentest"
                MainHelper.GenerateTestFile()
            Case "gennum"
                MainRainbow.GenNHSNumberList(MainHelper)
            Case "splithash"
                MainRainbow.SplitHashFile(MainHelper)
            Case "raincom"

            Case Else

        End Select
    End Sub

    Public Function ParseCommandLine() As String
        ' Get the values of the command line in an array

        Dim clArgs As String() = Environment.GetCommandLineArgs()
        ' Hold the command line values

        'This will hold any actions to perform once the command line params have been parsed.
        Dim postAction As String = ""

        If clArgs.Length = 1 Then
            Console.WriteLine("Type -help for help")
            End
        End If

        If clArgs(1).ToLower = "-help" Then PrintHelp()

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
                Case "-sf"
                    MainHelper.SaltFile = True
                Case "-rain-com"
                    postAction = "raincom"
                Case "-sr"
                    MainCrypto.Salt = MainCrypto.GenSalt()
                Case "-sh"
                    postAction = "splithash"
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

    Public Sub PrintHelp()
        Console.Write(vbCrLf & _
                "***** APFI Help v0.0.1 *****" & vbCrLf & _
                " " & vbCrLf & _
                "APFI is a command line Pseudonimisation tool free for use." & vbCrLf & _
                "Released under GPL License 3.0, James Wood - apfi@twistedknowledge.co.uk" & vbCrLf & _
                " " & vbCrLf & _
                "NOTE Error Checking is NOT implemented so use correctly!" & vbCrLf & _
                "ALL file parameters expect a file extension of some form i.e. test.txt" & vbCrLf & _
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
                "-g  <x>      - Generate a test file of <x> rows" & vbCrLf & _
                "-sh          - Use to split a large hash file to components" & vbCrLf & _
                " " & vbCrLf & _
                "Rainbow Tables" & vbCrLf & _
                "-sf          - Use when hashing a list to a Rainbow table" & vbCrLf & _
                "-rain-com    - Compare input file to hash files and write to output" & vbCrLf & _
                " " & vbCrLf & _
                "Testing and Debugging" & vbCrLf & _
                "-lc <loc>    - Load Certificates from specified location and display results" & vbCrLf & _
                "-rn          - Generate Real NHS Number instead of default fake" & vbCrLf & _
                "-genNum      - Generate valid NHS Number List - WARNING LONG TIME TO RUN!" & vbCrLf & _
                "....." & vbCrLf)
    End Sub

End Module
