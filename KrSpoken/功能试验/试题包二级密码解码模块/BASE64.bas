Attribute VB_Name = "Base64"
Option Explicit

Public BASE64CHR As String
Private psBase64Chr(0 To 63)    As String

'��һ������Base64���ַ����н��뵽Դ�ַ���
Public Function DecodeBase64String(str2Decode As String) As String
    DecodeBase64String = StrConv(DecodeBase64Byte(str2Decode), vbUnicode)
End Function
 
'��һ������Base64���ַ����н��뵽Դ�ֽ�����
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
    
    '//��ȥ�س�
    str = Replace(str2Decode, vbCrLf, "")
 
    '//ÿ4���ַ�һ�飨4���ַ���ʾ3���֣�
    For lPtr = 1 To Len(str) Step 4
        iLen = 4
        For iCtr = 0 To 3
            '//�����ַ���BASE64�ַ����е�λ��
            iValue = InStr(1, BASE64CHR, Mid$(str, lPtr + iCtr, 1), vbBinaryCompare)
            Select Case iValue  'A~Za~z0~9+/
                Case 1 To 64:
                    bits(iCtr + 1) = iValue - 1
                Case 65         '=
                    iLen = iCtr
                    Exit For
                    '//û�з���
                Case 0: Exit For
            End Select
        Next
 
        '//ת��4��6��������Ϊ3��8������
        bits(1) = bits(1) * &H4 + (bits(2) And &H30) \ &H10
        bits(2) = (bits(2) And &HF) * &H10 + (bits(3) And &H3C) \ &H4
        bits(3) = (bits(3) And &H3) * &H40 + bits(4)
 
        '//�����������ʼλ��
        lFrom = lTo
        lTo = lTo + (iLen - 1) - 1
                
        '//���¶����������
        ReDim Preserve Output(0 To lTo)
        
        For iIndex = lFrom To lTo
            Output(iIndex) = bits(iIndex - lFrom + 1)
        Next
 
        lTo = lTo + 1
        
    Next
    DecodeBase64Byte = Output
End Function

'��һ��Base64�ַ������룬��д��������ļ�
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

'��һ��Base64�����ļ����룬��д��������ļ�
'Public Sub DecodeBase64FileToFile(strBase64FilePath As String, strFilePath As String)
 '   Dim fso As New Scripting.FileSystemObject
 '   Dim ts As TextStream

 '   If fso.FileExists(strBase64FilePath) = False Then Exit Sub

'    Set ts = fso.OpenTextFile(strBase64FilePath)
 '   DecodeBase64StringToFile ts.ReadAll, strFilePath
'End Sub
 
 

Private Sub InitBase()
    Dim iPtr    As Integer
    '��ʼ�� BASE64����
    For iPtr = 0 To 63
        psBase64Chr(iPtr) = Mid$(BASE64CHR, iPtr + 1, 1)
    Next
End Sub

