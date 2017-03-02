Attribute VB_Name = "FastBase64"
Option Explicit
'����:          Base64����/����ģ��
'Name:          Base64 Encode & Decode Module

'����:          KiteGirl [�й�]
'programmer:    KiteGirl [China]

Private priBitMoveTable() As Byte                          '��λ�����
Private priBitMoveTable_CellReady() As Boolean             '��λ������־��
Private priBitMoveTable_Create As Boolean                  '��λ���������־

Private priEncodeTable() As Byte                           '���������תBase64��
Private priEncodeTable_Create As Boolean

Private priDecodeTable() As Byte                           '�����Base64ת���룩
Private priDecodeTable_Create As Boolean

Private Declare Sub Base64_CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDestination As Any, ByRef pSource As Any, ByVal pLength As Long)

Public conBase64_CodeTableStrng As String
Private Const conBase64_PatchCode As Byte = 61

Private Type tpBase64_Dollop2438                '24Bit(8Bit*3Byte)���ݿ�
    btBytes(0 To 2) As Byte
End Type

Private Type tpBase64_Dollop2446                '24Bit(6Bit*4Byte)���ݿ�
    btBytes(0 To 3) As Byte
End Type
'=========================================================================
'=========================================================================
'=========================================================================

'���ļ�����Base64���벢���ر�����Base64�ַ���
Public Function EncodFileToBase64String(strFileSource As String)
    Dim lpdata() As Byte, _
        I As Long, _
        n As Long ', _
        fso As New Scripting.FileSystemObject

    'If Not fso.FileExists(strFileSource) Then Exit Function

    I = FreeFile

    Open strFileSource For Binary Access Read Lock Write As I

    n = LOF(I) - 1

    ReDim lpdata(0 To n)
    Get I, , lpdata
    Close I
    Dim Outbyte() As Byte
    
    Base64_Encode Outbyte, lpdata
    EncodFileToBase64String = StrConv(Outbyte, vbUnicode)
End Function

'���ļ�����Base64���룬��������������ֱ��д��һ���ı��ļ���
Public Sub EncodFileToBase64File(strFileSource As String, strFileBase64Desti As String)
Dim I

    'ts.Write (EncodFileToBase64String(strFileSource))
    'ts.Close
    Dim Data
    Data = EncodFileToBase64String(strFileSource)

    
        Dim fso As New FileSystemObject
        Dim txtfile As File
        Dim ts As TextStream
        If fso.FileExists(strFileBase64Desti) = True Then
        fso.DeleteFile (strFileBase64Desti)
        End If
        fso.CreateTextFile strFileBase64Desti
        Set txtfile = fso.GetFile(strFileBase64Desti)
        Set ts = txtfile.OpenAsTextStream(ForWriting)
        ts.Write Data
        ts.Close
    
End Sub

'��һ��Base64�ַ������룬��д��������ļ�
Public Sub DecodeBase64StringToFile(strBase64 As String, strFilePath As String)
    Dim fso As New Scripting.FileSystemObject, _
        I As Long

    If fso.FileExists(strFilePath) Then
        fso.DeleteFile strFilePath, True
    End If

Dim tSurString As String
Dim tSurBytes() As Byte
Dim tOutBytes() As Byte
tSurString = strBase64
tSurBytes() = StrConv(tSurString, vbFromUnicode)
tSurString = ""
Base64_Decode tOutBytes, tSurBytes
    I = FreeFile
    Open strFilePath For Binary Access Write As I
    Put I, , tOutBytes
    Close I
    Set fso = Nothing
End Sub

'��һ��Base64�����ļ����룬��д��������ļ�
Public Sub DecodeBase64FileToFile(strBase64FilePath As String, strFilePath As String)
    Dim fso As New Scripting.FileSystemObject
    Dim ts As TextStream
    If fso.FileExists(strBase64FilePath) = False Then Exit Sub
    Set ts = fso.OpenTextFile(strBase64FilePath)
    DecodeBase64StringToFile ts.ReadAll, strFilePath
End Sub

'=========================================================================
'=========================================================================
'=========================================================================

'����
Public Sub Base64_Decode(ByRef tOutBytes() As Byte, ByRef pBytes() As Byte, Optional ByVal pPatchCode As Byte = conBase64_PatchCode)
'Base64Decode����
'�﷨��[tOutBytes()] = Base64Decode(pBytes(), [pPatchCode])
'���ܣ���Byte�����ʾ��Base64����Ascii�ֽ��������ΪByte�ֽ����飬�����ء�
'������byte pBytes()                  '��Ҫ������Byte�����ʾ��Base64�������ݡ�
'      byte pPatchCode                '��ѡ�����������ֽ�׷���롣Ĭ��Ϊ61��"="��Ascii�룩
'���أ�byte tOutBytes()               'Byte���顣
'ʾ����
'      Dim tSurString As String
'      Dim tSurBytes() As Byte
'      tSurString = "S2l0ZUdpcmzKx7j2usO6otfT"
'      tSurBytes() = StrConv(tSurString, vbFromUnicode)
'      Dim tDesString As String
'      Dim tDesBytes() As Byte
'      tDesBytes() = Base64Decode(tSurBytes())
'      tDesString = StrConv(tDesBytes(), vbUnicode) 'tDesString����"KiteGirl�Ǹ��ú���"

    Dim tOutBytes_Length As Long

    Dim tBytes_Length As Long

    Dim tBytes2446() As Byte

    Dim tSurBytes_Length As Long
    Dim tDesBytes_Length As Long

    Err.Clear
    On Error Resume Next

    tBytes_Length = UBound(pBytes())


    If CBool(Err.Number) Then Exit Sub

    tBytes2446() = BytesPrimeDecode(pBytes())
    tOutBytes() = Bytes2438GetBy2446(tBytes2446())

    Dim tPatchNumber As Long

    Dim tIndex As Long
    Dim tBytesIndex As Long

    For tIndex = 0 To 1
        tBytesIndex = tBytes_Length - tIndex
        tPatchNumber = tPatchNumber + ((pBytes(tIndex) = pPatchCode) And 1)
    Next

    tSurBytes_Length = tBytes_Length - tPatchNumber
    tDesBytes_Length = (tSurBytes_Length * 3) / 4

    ReDim Preserve tOutBytes(tDesBytes_Length)

End Sub

'����
Public Sub Base64_Encode(ByRef tOutBytes() As Byte, ByRef pBytes() As Byte, Optional ByVal pPatchCode As Byte = conBase64_PatchCode)
'Base64Encode����
'�﷨��[tOutBytes()] = Base64Encode(pBytes(), [pPatchCode])
'���ܣ���Byte�������ΪBase64�����Ascii�ֽ����飬�����ء�
'������byte pBytes()                  '��Ҫ������Byte�����ʾ�����ݡ�
'      byte pPatchCode                '��ѡ�����������ֽ�׷���롣Ĭ��Ϊ61��"="��Ascii�룩
'���أ�byte tOutBytes()               'Base64�����ʾ��Ascii�������顣
'ע�⣺���������VB�����ַ�����ʾ�ú����ķ���ֵ����Ҫ��StrConvת��ΪUnicode��
'ʾ����
'      Dim tSurString As String
'      Dim tSurBytes() As Byte
'      tSurString = "KiteGirl�Ǹ��ú���"
'      tSurBytes() = StrConv(tSurString, vbFromUnicode)
'      Dim tDesString As String
'      Dim tDesBytes() As Byte
'      tDesBytes() = Base64Encode(tSurBytes())
'      tDesString = StrConv(tDesBytes(), vbUnicode) 'tDesString����"S2l0ZUdpcmzKx7j2usO6otfT"

    Dim tOutBytes_Length As Long

    Dim tBytes2446() As Byte

    Dim tSurBytes_Length As Long
    Dim tDesBytes_Length As Long

    Err.Clear
    On Error Resume Next

    tSurBytes_Length = UBound(pBytes())

    If CBool(Err.Number) Then Exit Sub

    tBytes2446() = Bytes2438PutTo2446(pBytes())
    tOutBytes() = BytesPrimeEncode(tBytes2446())

    tOutBytes_Length = UBound(tOutBytes())

    Dim tPatchNumber As Long

    tDesBytes_Length = (tSurBytes_Length * 4 + 3) / 3
    tPatchNumber = tOutBytes_Length - tDesBytes_Length

    Dim tIndex As Long
    Dim tBytesIndex As Long

    For tIndex = 1 To tPatchNumber
        tBytesIndex = tOutBytes_Length - tIndex + 1
        tOutBytes(tBytesIndex) = pPatchCode
    Next
End Sub

Private Function BytesPrimeDecode(ByRef pBytes() As Byte) As Byte()
'���ܣ���Base64�������Ϊ��������

    Dim tOutBytes() As Byte

    Dim tBytes_Length As Long

    Err.Clear
    On Error Resume Next

    tBytes_Length = UBound(pBytes())

    If CBool(Err.Number) Then Exit Function

    ReDim tOutBytes(tBytes_Length)

    If Not priDecodeTable_Create Then Base64CodeTableCreate

    Dim tIndex As Long

    For tIndex = 0 To tBytes_Length
        tOutBytes(tIndex) = priDecodeTable(pBytes(tIndex))
    Next

    BytesPrimeDecode = tOutBytes()
End Function

Private Function BytesPrimeEncode(ByRef pBytes() As Byte) As Byte()
'���ܣ��������������ΪBase64����

    Dim tOutBytes() As Byte

    Dim tBytes_Length As Long

    Err.Clear
    On Error Resume Next

    tBytes_Length = UBound(pBytes())

    If CBool(Err.Number) Then Exit Function

    ReDim tOutBytes(tBytes_Length)

    If Not priEncodeTable_Create Then Base64CodeTableCreate

    Dim tIndex As Long

    For tIndex = 0 To tBytes_Length
        tOutBytes(tIndex) = priEncodeTable(pBytes(tIndex))
    Next

    BytesPrimeEncode = tOutBytes()
End Function

Private Sub Base64CodeTableCreate()
    Dim pString As String
    pString = conBase64_CodeTableStrng
'���ܣ������ַ����ṩ�Ĵ����ʼ��Base64����/�������

    Dim tBytes() As Byte
    Dim tBytes_Length As Long

    tBytes() = pString
    tBytes_Length = UBound(tBytes())

    If Not tBytes_Length = 127 Then
'        MsgBox "����/������ʼ��ʧ��", , "����"
        Exit Sub
    End If

    Dim tIndex As Byte

    ReDim priEncodeTable(0 To 255)
    ReDim priDecodeTable(0 To 255)

    Dim tTableIndex As Byte
    Dim tByteValue As Byte

    For tIndex = 0 To tBytes_Length Step 2
        tTableIndex = tIndex / 2
        tByteValue = tBytes(tIndex)
        priEncodeTable(tTableIndex) = tByteValue
        priDecodeTable(tByteValue) = tTableIndex
    Next

    priEncodeTable_Create = True
    priDecodeTable_Create = True
End Sub

Private Function Bytes2438GetBy2446(ByRef pBytes() As Byte) As Byte()
'���ܣ�������ת��Ϊ�ֽڡ�
    Dim tOutBytes() As Byte

    Dim tDollops2438() As tpBase64_Dollop2438
    Dim tDollops2446() As tpBase64_Dollop2446

    tDollops2446() = BytesPutTo2446(pBytes())
    tDollops2438() = Dollops2438GetBy2446(tDollops2446())
    tOutBytes() = BytesGetBy2438(tDollops2438())

    Bytes2438GetBy2446 = tOutBytes()
End Function

Private Function Bytes2438PutTo2446(ByRef pBytes() As Byte) As Byte()
'���ܣ����ֽ�ת��Ϊ���롣
    Dim tOutBytes() As Byte

    Dim tDollops2438() As tpBase64_Dollop2438
    Dim tDollops2446() As tpBase64_Dollop2446

    tDollops2438() = BytesPutTo2438(pBytes())
    tDollops2446() = Dollops2438PutTo2446(tDollops2438())
    tOutBytes() = BytesGetBy2446(tDollops2446())

    Bytes2438PutTo2446 = tOutBytes()
End Function

Private Function BytesGetBy2446(ByRef p2446() As tpBase64_Dollop2446) As Byte()
'���ܣ�2446����ת��Ϊ�ֽ�����

    Dim tOutBytes() As Byte
    Dim tOutBytes_Length As Long

    Dim t2446Length As Long

    Err.Clear
    On Error Resume Next

    t2446Length = UBound(p2446())

    If CBool(Err.Number) Then Exit Function

    tOutBytes_Length = t2446Length * 4 + 3

    ReDim tOutBytes(0 To tOutBytes_Length)

    Dim tCopyLength As Long

    tCopyLength = tOutBytes_Length + 1

    Base64_CopyMemory tOutBytes(0), p2446(0), tCopyLength

    BytesGetBy2446 = tOutBytes()
End Function

Private Function BytesPutTo2446(ByRef pBytes() As Byte) As tpBase64_Dollop2446()
'���ܣ��ֽ�����ת��Ϊ2446����
    Dim tOut2446() As tpBase64_Dollop2446
    Dim tOut2446_Length As Long

    Dim tBytesLength As Long

    Err.Clear
    On Error Resume Next

    tBytesLength = UBound(pBytes())

    If CBool(Err.Number) Then Exit Function

    tOut2446_Length = tBytesLength / 4

    ReDim tOut2446(0 To tOut2446_Length)

    Dim tCopyLength As Long

    tCopyLength = tBytesLength + 1

    Base64_CopyMemory tOut2446(0), pBytes(0), tCopyLength

    BytesPutTo2446 = tOut2446()
End Function

Private Function BytesGetBy2438(ByRef p2438() As tpBase64_Dollop2438) As Byte()
'���ܣ�2438����ת��Ϊ�ֽ�����
    Dim tOutBytes() As Byte
    Dim tOutBytes_Length As Long

    Dim t2438Length As Long

    Err.Clear
    On Error Resume Next

    t2438Length = UBound(p2438())

    If CBool(Err.Number) Then Exit Function

    tOutBytes_Length = t2438Length * 3 + 2

    ReDim tOutBytes(0 To tOutBytes_Length)

    Dim tCopyLength As Long

    tCopyLength = tOutBytes_Length + 1

    Base64_CopyMemory tOutBytes(0), p2438(0), tCopyLength

    BytesGetBy2438 = tOutBytes()
End Function

Private Function BytesPutTo2438(ByRef pBytes() As Byte) As tpBase64_Dollop2438()
'���ܣ��ֽ�����ת��Ϊ2438����
    Dim tOut2438() As tpBase64_Dollop2438
    Dim tOut2438_Length As Long

    Dim tBytesLength As Long

    Err.Clear
    On Error Resume Next

    tBytesLength = UBound(pBytes())

    If CBool(Err.Number) Then Exit Function

    tOut2438_Length = tBytesLength / 3

    ReDim tOut2438(0 To tOut2438_Length)

    Dim tCopyLength As Long

    tCopyLength = tBytesLength + 1

    Base64_CopyMemory tOut2438(0), pBytes(0), tCopyLength

    BytesPutTo2438 = tOut2438()
End Function

Private Function Dollops2438GetBy2446(ByRef p2446() As tpBase64_Dollop2446) As tpBase64_Dollop2438()
'���ܣ�2446������ת��Ϊ2438������
    Dim tOut2438() As tpBase64_Dollop2438
    Dim tOut2438_Length As Long

    Dim t2446_Length As Long

    Err.Clear
    On Error Resume Next

    If CBool(Err.Number) Then Exit Function

    t2446_Length = UBound(p2446())
    tOut2438_Length = t2446_Length

    ReDim tOut2438(tOut2438_Length)

    Dim tIndex As Long

    For tIndex = 0 To t2446_Length
        tOut2438(tIndex) = Dollop2438GetBy2446(p2446(tIndex))
    Next

    Dollops2438GetBy2446 = tOut2438()
End Function

Private Function Dollops2438PutTo2446(ByRef p2438() As tpBase64_Dollop2438) As tpBase64_Dollop2446()
'���ܣ�2438������ת��Ϊ2446������

    Dim tOut2446() As tpBase64_Dollop2446
    Dim tOut2446_Length As Long

    Dim t2438_Length As Long

    Err.Clear
    On Error Resume Next

    If CBool(Err.Number) Then Exit Function

    t2438_Length = UBound(p2438())
    tOut2446_Length = t2438_Length

    ReDim tOut2446(tOut2446_Length)

    Dim tIndex As Long

    For tIndex = 0 To t2438_Length
        tOut2446(tIndex) = Dollop2438PutTo2446(p2438(tIndex))
    Next

    Dollops2438PutTo2446 = tOut2446()
End Function

Private Function Dollop2438GetBy2446(ByRef p2446 As tpBase64_Dollop2446) As tpBase64_Dollop2438
'���ܣ�2446��ת��Ϊ2438��
    Dim tOut2438 As tpBase64_Dollop2438

    With tOut2438
        .btBytes(0) = ByteBitMove(p2446.btBytes(0), 2) + ByteBitMove(p2446.btBytes(1), -4)
        .btBytes(1) = ByteBitMove(p2446.btBytes(1), 4) + ByteBitMove(p2446.btBytes(2), -2)
        .btBytes(2) = ByteBitMove(p2446.btBytes(2), 6) + ByteBitMove(p2446.btBytes(3), 0)
    End With

    Dollop2438GetBy2446 = tOut2438
End Function

Private Function Dollop2438PutTo2446(ByRef p2438 As tpBase64_Dollop2438) As tpBase64_Dollop2446
'���ܣ�2438��ת��Ϊ2446��
    Dim tOut2446 As tpBase64_Dollop2446

    With tOut2446
        .btBytes(0) = ByteBitMove(p2438.btBytes(0), -2, 63)
        .btBytes(1) = ByteBitMove(p2438.btBytes(0), 4, 63) + ByteBitMove(p2438.btBytes(1), -4, 63)
        .btBytes(2) = ByteBitMove(p2438.btBytes(1), 2, 63) + ByteBitMove(p2438.btBytes(2), -6, 63)
        .btBytes(3) = ByteBitMove(p2438.btBytes(2), 0, 63)
    End With

    Dollop2438PutTo2446 = tOut2446
End Function

Private Function ByteBitMove(ByVal pByte As Byte, ByVal pMove As Integer, Optional ByVal pConCode As Byte = &HFF) As Byte
'���ܣ���Byte������λ�������ͻ��幦�ܣ���
    Dim tOutByte As Byte

    If Not priBitMoveTable_Create Then

        ReDim priBitMoveTable(0 To 255, -8 To 8)
        ReDim priBitMoveTable_CellReady(0 To 255, -8 To 8)

        priBitMoveTable_Create = True

    End If

    If Not priBitMoveTable_CellReady(pByte, pMove) Then

        priBitMoveTable(pByte, pMove) = ByteBitMove_Operation(pByte, pMove)
        priBitMoveTable_CellReady(pByte, pMove) = True

    End If

    tOutByte = priBitMoveTable(pByte, pMove) And pConCode

    ByteBitMove = tOutByte
End Function

Private Function ByteBitMove_Operation(ByVal pByte As Byte, ByVal pMove As Integer) As Byte
'���ܣ���Byte����������λ��
    Dim tOutByte As Byte

    Dim tMoveLeft As Boolean
    Dim tMoveRight As Boolean
    Dim tMoveCount As Integer

    tMoveLeft = pMove > 0
    tMoveRight = pMove < 0

    tMoveCount = Abs(pMove)

    If tMoveLeft Then
        tOutByte = (pByte Mod (2 ^ (8 - tMoveCount))) * (2 ^ tMoveCount)
    ElseIf tMoveRight Then
        tOutByte = pByte / 2 ^ tMoveCount
    Else
        tOutByte = pByte
    End If

    ByteBitMove_Operation = tOutByte
End Function


