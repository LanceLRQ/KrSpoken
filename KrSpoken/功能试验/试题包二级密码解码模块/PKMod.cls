VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "PKMod"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Private Declare Function DiskID32 Lib "DiskID32.dll" (ByRef DiskModel As Byte, ByRef DiskID As Byte) As Long

'Private abc(27) As String
Private ACCC(6) As String

'解包专用类
'代码解释：RegCode：输入系统的注册码
'HardCode：输入系统的机器码
'CheckCode：输入系统的校验码
'PKCont：输入系统的内容
'OutID：放置文件夹ID，取模计算
Public Function OutputPK(ByVal RegCode As String, ByVal HardCode As String, ByVal CheckCode As String, ByVal PKCont As String) As String
On Error Resume Next
    base64chr_1 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
    base64chr_1 = base64chr_1 & "abcdefghijklmnopqrstuvwxyz0123456789+/"
'abc(17) = "4"
'abc(18) = "1"
'abc(19) = "R"


'abc(20) = "e"
'abc(21) = "g"
'abc(22) = "i"
'abc(23) = "t"
'abc(24) = "C"
'abc(3) = "o"
'abc(4) = "k"
'abc(5) = "_"
 '   GoTo 111
    If Len(CheckCode) <> 32 Then
        OutputPK = 1 'Err1：校验码格式错误！
        Exit Function
   End If
    'abc(0) = "<"
'abc(1) = "L"
'abc(2) = "y"
'abc(6) = "P"
'abc(7) = "K"
'abc(8) = "a"
'abc(9) = "s"
'abc(10) = "w"
'abc(11) = "r"
'abc(12) = "d"
'abc(13) = "5"
'abc(14) = "9"
'abc(15) = "2"
'abc(16) = "6"
'abc(25) = "?"
'abc(26) = "/"
'abc(27) = ">"
'    If RegCodeChecker(RegCode, HardCode, CheckCode) = False Then
 '       OutputPK = 2 'Err2：校验失败
 '       Exit Function
''    End If
'111:




    base64chr_1 = GetBase64Chr(base64chr_1, CheckCode)
    base64chr_1 = base64chr_1
    
    BASE64CHR = base64chr_1
    
   ' EncodFileToBase64File "d:\split.avi", "d:\a.txt"
    'DecodeBase64FileToFile PKHref, OutHref
    OutputPK = DecodeBase64String(PKCont)
    If Err Then
            OutputPK = 3 'Err3：解包错误！
            Exit Function
    End If
End Function

'Private Function RegCodeChecker(ByVal RegCode As String, ByVal HardCode As String, ByVal CheckCode As String) As Boolean
'    Dim RC, HC, CC, RunTmp, RunTmp1, RunTmp2 As String
  '  RC = RegCode
 '   HC = HardCode
 '   CC = CheckCode
' GoTo 23
'443:

'    RunTmp = MD5(CStr(MyCode), True)   '第二步，组合拼凑
    
 '   RunTmp1 = ""
'    RunTmp2 = ""
 '   GoTo 5766
'23:
 '   RunTmp = ""
 '   RunTmp1 = MD5(CStr(RC), True) '第一步，计算RC的MD5
  '  RunTmp2 = MD5(CStr(HC), True)  '第一步，计算HC的MD5
 '   MyCode = abc(0) & abc(1) & abc(2) & abc(3) & abc(4) & abc(3) & abc(5) & abc(6) & abc(7) & abc(5) & abc(6) & abc(8) & abc(9) & abc(9) & abc(10) & abc(3) & abc(11) & abc(12) & abc(25)
 '   MyCode = MyCode & RC
 '   MyCode = MyCode & abc(25) & abc(13) & abc(14) & abc(15) & abc(16) & abc(17) & abc(18) & abc(25)
 '   MyCode = MyCode & HC
 '   MyCode = MyCode & abc(25) & abc(19) & abc(20) & abc(21) & abc(22) & abc(9) & abc(23) & abc(20) & abc(11) & abc(24) & abc(3) & abc(12) & abc(20) & abc(26) & abc(27)
'   GoTo 443
'5766:
  ' If LCase(CC) = LCase(RunTmp) Then '第三步，校验
  ' RegCodeChecker = True
'   Else
'   RegCodeChecker = False
'   RunTmp = ""
'End Function

Private Function GetBase64Chr(ByVal B64Chr As String, ByVal CheckCode As String) As String
    Const CharSet As String = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"
    a = Len(CheckCode)
    For l = 1 To 9
    For i = 1 To 32
    CT = Mid(B64Chr, InStr(CharSet, Mid(CheckCode, i, 1)), 1)
    B64Chr = Replace(B64Chr, CT, "") & CT
    Next i
    Next l
    GetBase64Chr = B64Chr & "="
    Debug.Print B64Chr
End Function

Public Function GetHardCode() As String

ACCC(0) = "B2412"
ACCC(1) = "FE0A88"
ACCC(2) = "BF617F"
ACCC(3) = "E104"
ACCC(5) = "2EFAAC"
ACCC(6) = "9910D"

Dim HASh As String
HASh = ACCC(0) & ACCC(3) & ACCC(2)
HASh = HASh & ACCC(6) & ACCC(5) & ACCC(1)

If (HashFile(App.Path & "\DiskID32.DLL") <> HASh) Then
GetHardCode = "GetDllErr"
Err.Raise 0, , "DiskID32.dll损坏，请重新安装程序"
Exit Function
End If

  Dim DiskModel(31) As Byte, DiskID(31) As Byte, i As Integer, Model As String, ID As String
  If DiskID32(DiskModel(0), DiskID(0)) <> 1 Then
  GetHardCode = ""
  Exit Function
  End If
  For i = 0 To 31
  If Chr(DiskModel(i)) <> Chr(0) Then
  Model = Model & Chr(DiskModel(i))
  End If
  If Chr(DiskID(i)) <> Chr(0) Then
  ID = ID & Chr(DiskID(i))
  End If
  Next
  GetHardCode = MDFive(Replace(Model & ID, " ", ""), False)
End Function



