Attribute VB_Name = "Base64"
Option Explicit

Public BASE64CHR As String
Private psBase64Chr(0 To 63)    As String

'从一个经过Base64的字符串中解码到源字符串
Public Function DecodeBase64String(str2Decode As String) As String
    DecodeBase64String = StrConv(DecodeBase64Byte(str2Decode), vbUnicode)
End Function
 
'从一个经过Base64的字符串中解码到源字节数组
Public Function DecodeBase64Byte(str2Decode As String) As Byte()
 
    Dim lPtr            As Long
    Dim iValue          As Integer
    Dim iLen            As Integer
    Dim iCtr            As Integer
    Dim bits(1 To 4)    As Byte
    Dim strDecode       As String
    Dim str             As String
    Dim Output()        As Byte
    
    Dim iIndex          As Long

    Dim lFrom As Long
    Dim lTo As Long
    
    InitBase
    
    '//除去回车
    str = Replace(str2Decode, vbCrLf, "")
 
    '//每4个字符一组（4个字符表示3个字）
    For lPtr = 1 To Len(str) Step 4
        iLen = 4
        For iCtr = 0 To 3
            '//查找字符在BASE64字符串中的位置
            iValue = InStr(1, BASE64CHR, Mid$(str, lPtr + iCtr, 1), vbBinaryCompare)
            Select Case iValue  'A~Za~z0~9+/
                Case 1 To 64:
                    bits(iCtr + 1) = iValue - 1
                Case 65         '=
                    iLen = iCtr
                    Exit For
                    '//没有发现
                Case 0: Exit For
            End Select
        Next
 
        '//转换4个6比特数成为3个8比特数
        bits(1) = bits(1) * &H4 + (bits(2) And &H30) \ &H10
        bits(2) = (bits(2) And &HF) * &H10 + (bits(3) And &H3C) \ &H4
        bits(3) = (bits(3) And &H3) * &H40 + bits(4)
 
        '//计算数组的起始位置
        lFrom = lTo
        lTo = lTo + (iLen - 1) - 1
                
        '//重新定义输出数组
        ReDim Preserve Output(0 To lTo)
        
        For iIndex = lFrom To lTo
            Output(iIndex) = bits(iIndex - lFrom + 1)
        Next
 
        lTo = lTo + 1
        
    Next
    DecodeBase64Byte = Output
End Function

'将一个Base64字符串解码，并写入二进制文件
'Public Sub DecodeBase64StringToFile(strBase64 As String, strFilePath As String)
 '   Dim fso As New Scripting.FileSystemObject, _
 '       I As Long

 '   If fso.FileExists(strFilePath) Then
    '    fso.DeleteFile strFilePath, True
 '   End If
'
  '  I = FreeFile
  '  Open strFilePath For Binary Access Write As I
  '  Put I, , DecodeBase64Byte(strBase64)
  '  Close I
 '   Set fso = Nothing
'End Sub

'将一个Base64编码文件解码，并写入二进制文件
'Public Sub DecodeBase64FileToFile(strBase64FilePath As String, strFilePath As String)
 '   Dim fso As New Scripting.FileSystemObject
 '   Dim ts As TextStream

 '   If fso.FileExists(strBase64FilePath) = False Then Exit Sub

'    Set ts = fso.OpenTextFile(strBase64FilePath)
 '   DecodeBase64StringToFile ts.ReadAll, strFilePath
'End Sub
 
 

Private Sub InitBase()
    Dim iPtr    As Integer
    '初始化 BASE64数组
    For iPtr = 0 To 63
        psBase64Chr(iPtr) = Mid$(BASE64CHR, iPtr + 1, 1)
    Next
End Sub

