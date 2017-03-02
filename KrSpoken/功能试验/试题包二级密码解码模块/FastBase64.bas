Attribute VB_Name = "FastBase64"
Option Explicit
'名称:          Base64编码/解码模块
'Name:          Base64 Encode & Decode Module

'作者:          KiteGirl [中国]
'programmer:    KiteGirl [China]

Private priBitMoveTable() As Byte                          '移位缓冲表
Private priBitMoveTable_CellReady() As Boolean             '移位缓冲表标志表
Private priBitMoveTable_Create As Boolean                  '移位缓冲表创建标志

Private priEncodeTable() As Byte                           '编码表（素码转Base64）
Private priEncodeTable_Create As Boolean

Private priDecodeTable() As Byte                           '解码表（Base64转素码）
Private priDecodeTable_Create As Boolean

Private Declare Sub Base64_CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDestination As Any, ByRef pSource As Any, ByVal pLength As Long)

Public conBase64_CodeTableStrng As String
Private Const conBase64_PatchCode As Byte = 61

Private Type tpBase64_Dollop2438                '24Bit(8Bit*3Byte)数据块
    btBytes(0 To 2) As Byte
End Type

Private Type tpBase64_Dollop2446                '24Bit(6Bit*4Byte)数据块
    btBytes(0 To 3) As Byte
End Type
'=========================================================================
'=========================================================================
'=========================================================================

'对文件进行Base64编码并返回编码后的Base64字符串
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

'对文件进行Base64编码，并将编码后的内容直接写入一个文本文件中
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

'将一个Base64字符串解码，并写入二进制文件
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

'将一个Base64编码文件解码，并写入二进制文件
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

'解码
Public Sub Base64_Decode(ByRef tOutBytes() As Byte, ByRef pBytes() As Byte, Optional ByVal pPatchCode As Byte = conBase64_PatchCode)
'Base64Decode函数
'语法：[tOutBytes()] = Base64Decode(pBytes(), [pPatchCode])
'功能：将Byte数组表示的Base64编码Ascii字节数组解码为Byte字节数组，并返回。
'参数：byte pBytes()                  '必要参数。Byte数组表示的Base64编码数据。
'      byte pPatchCode                '可选参数。冗余字节追加码。默认为61（"="的Ascii码）
'返回：byte tOutBytes()               'Byte数组。
'示例：
'      Dim tSurString As String
'      Dim tSurBytes() As Byte
'      tSurString = "S2l0ZUdpcmzKx7j2usO6otfT"
'      tSurBytes() = StrConv(tSurString, vbFromUnicode)
'      Dim tDesString As String
'      Dim tDesBytes() As Byte
'      tDesBytes() = Base64Decode(tSurBytes())
'      tDesString = StrConv(tDesBytes(), vbUnicode) 'tDesString返回"KiteGirl是个好孩子"

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

'编码
Public Sub Base64_Encode(ByRef tOutBytes() As Byte, ByRef pBytes() As Byte, Optional ByVal pPatchCode As Byte = conBase64_PatchCode)
'Base64Encode函数
'语法：[tOutBytes()] = Base64Encode(pBytes(), [pPatchCode])
'功能：将Byte数组编码为Base64编码的Ascii字节数组，并返回。
'参数：byte pBytes()                  '必要参数。Byte数组表示的数据。
'      byte pPatchCode                '可选参数。冗余字节追加码。默认为61（"="的Ascii码）
'返回：byte tOutBytes()               'Base64编码表示的Ascii代码数组。
'注意：如果你想在VB里以字符串表示该函数的返回值，需要用StrConv转换为Unicode。
'示例：
'      Dim tSurString As String
'      Dim tSurBytes() As Byte
'      tSurString = "KiteGirl是个好孩子"
'      tSurBytes() = StrConv(tSurString, vbFromUnicode)
'      Dim tDesString As String
'      Dim tDesBytes() As Byte
'      tDesBytes() = Base64Encode(tSurBytes())
'      tDesString = StrConv(tDesBytes(), vbUnicode) 'tDesString返回"S2l0ZUdpcmzKx7j2usO6otfT"

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
'功能：将Base64数组解码为素码数组

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
'功能：将素码数组编码为Base64数组

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
'功能：根据字符串提供的代码初始化Base64解码/编码码表。

    Dim tBytes() As Byte
    Dim tBytes_Length As Long

    tBytes() = pString
    tBytes_Length = UBound(tBytes())

    If Not tBytes_Length = 127 Then
'        MsgBox "编码/解码表初始化失败", , "错误"
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
'功能：将素码转换为字节。
    Dim tOutBytes() As Byte

    Dim tDollops2438() As tpBase64_Dollop2438
    Dim tDollops2446() As tpBase64_Dollop2446

    tDollops2446() = BytesPutTo2446(pBytes())
    tDollops2438() = Dollops2438GetBy2446(tDollops2446())
    tOutBytes() = BytesGetBy2438(tDollops2438())

    Bytes2438GetBy2446 = tOutBytes()
End Function

Private Function Bytes2438PutTo2446(ByRef pBytes() As Byte) As Byte()
'功能：将字节转换为素码。
    Dim tOutBytes() As Byte

    Dim tDollops2438() As tpBase64_Dollop2438
    Dim tDollops2446() As tpBase64_Dollop2446

    tDollops2438() = BytesPutTo2438(pBytes())
    tDollops2446() = Dollops2438PutTo2446(tDollops2438())
    tOutBytes() = BytesGetBy2446(tDollops2446())

    Bytes2438PutTo2446 = tOutBytes()
End Function

Private Function BytesGetBy2446(ByRef p2446() As tpBase64_Dollop2446) As Byte()
'功能：2446数组转换为字节数组

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
'功能：字节数组转换为2446数组
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
'功能：2438数组转换为字节数组
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
'功能：字节数组转换为2438数组
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
'功能：2446块数组转换为2438块数组
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
'功能：2438块数组转换为2446块数组

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
'功能：2446块转换为2438块
    Dim tOut2438 As tpBase64_Dollop2438

    With tOut2438
        .btBytes(0) = ByteBitMove(p2446.btBytes(0), 2) + ByteBitMove(p2446.btBytes(1), -4)
        .btBytes(1) = ByteBitMove(p2446.btBytes(1), 4) + ByteBitMove(p2446.btBytes(2), -2)
        .btBytes(2) = ByteBitMove(p2446.btBytes(2), 6) + ByteBitMove(p2446.btBytes(3), 0)
    End With

    Dollop2438GetBy2446 = tOut2438
End Function

Private Function Dollop2438PutTo2446(ByRef p2438 As tpBase64_Dollop2438) As tpBase64_Dollop2446
'功能：2438块转换为2446块
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
'功能：对Byte进行移位（带饱和缓冲功能）。
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
'功能：对Byte进行算术移位。
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


