Attribute VB_Name = "FileHash"
Option Explicit
Public Declare Function CryptAcquireContext Lib "advapi32.dll" Alias "CryptAcquireContextA" (ByRef phProv As Long, ByVal pszContainer As String, ByVal pszProvider As String, ByVal dwProvType As Long, ByVal dwFlags As Long) As Long
Public Declare Function CryptReleaseContext Lib "advapi32.dll" (ByVal hProv As Long, ByVal dwFlags As Long) As Long
Public Declare Function CryptCreateHash Lib "advapi32.dll" (ByVal hProv As Long, ByVal Algid As Long, ByVal hKey As Long, ByVal dwFlags As Long, ByRef phHash As Long) As Long
Public Declare Function CryptDestroyHash Lib "advapi32.dll" (ByVal hHash As Long) As Long
Public Declare Function CryptHashData Lib "advapi32.dll" (ByVal hHash As Long, pbData As Any, ByVal dwDataLen As Long, ByVal dwFlags As Long) As Long
Public Declare Function CryptGetHashParam Lib "advapi32.dll" (ByVal hHash As Long, ByVal dwParam As Long, pbData As Any, pdwDataLen As Long, ByVal dwFlags As Long) As Long
Public Const PROV_RSA_FULL = 1
Public Const CRYPT_NEWKEYSET = &H8
Public Const ALG_CLASS_HASH = 32768
Public Const ALG_TYPE_ANY = 0
Public Const ALG_SID_MD2 = 1
Public Const ALG_SID_MD4 = 2
Public Const ALG_SID_MD5 = 3
Public Const ALG_SID_SHA1 = 4
Enum HashAlgorithm
   MD2 = ALG_CLASS_HASH Or ALG_TYPE_ANY Or ALG_SID_MD2
   MD4 = ALG_CLASS_HASH Or ALG_TYPE_ANY Or ALG_SID_MD4
   MD51 = ALG_CLASS_HASH Or ALG_TYPE_ANY Or ALG_SID_MD5
   SHA1 = ALG_CLASS_HASH Or ALG_TYPE_ANY Or ALG_SID_SHA1
End Enum
Public Const HP_HASHVAL = 2
Public Const HP_HASHSIZE = 4
Public Function HashFile(ByVal FileName As String, Optional ByVal Algorithm As HashAlgorithm = MD51) As String
    Dim hCtx As Long
    Dim hHash As Long
    Dim lFile As Long
    Dim lRes As Long
    Dim lLen As Long
    Dim lIdx As Long
    Dim abHash() As Byte
    If Len(Dir$(FileName)) = 0 Then Err.Raise 53
    lRes = CryptAcquireContext(hCtx, vbNullString, vbNullString, PROV_RSA_FULL, 0)
    If lRes = 0 And Err.LastDllError = &H80090016 Then
      lRes = CryptAcquireContext(hCtx, vbNullString, vbNullString, PROV_RSA_FULL, CRYPT_NEWKEYSET)
    End If
    If lRes <> 0 Then
       lRes = CryptCreateHash(hCtx, Algorithm, 0, 0, hHash)
       If lRes <> 0 Then
          lFile = FreeFile
          Open FileName For Binary As lFile
          If Err.Number = 0 Then
             Const BLOCK_SIZE As Long = 32 * 1024& ' 32K
             ReDim abBlock(1 To BLOCK_SIZE) As Byte
             Dim lCount As Long
             Dim lBlocks As Long
             Dim lLastBlock As Long
             lBlocks = LOF(lFile) \ BLOCK_SIZE
             lLastBlock = LOF(lFile) - lBlocks * BLOCK_SIZE
             For lCount = 1 To lBlocks
                Get lFile, , abBlock
                lRes = CryptHashData(hHash, abBlock(1), BLOCK_SIZE, 0)
                If lRes = 0 Then Exit For
             Next
             If lLastBlock > 0 And lRes <> 0 Then
                ReDim abBlock(1 To lLastBlock) As Byte
                Get lFile, , abBlock
                lRes = CryptHashData(hHash, abBlock(1), lLastBlock, 0)
             End If
             Close lFile
          End If
          If lRes <> 0 Then
             lRes = CryptGetHashParam(hHash, HP_HASHSIZE, lLen, 4, 0)
             If lRes <> 0 Then
                 ReDim abHash(0 To lLen - 1)
                 lRes = CryptGetHashParam(hHash, HP_HASHVAL, abHash(0), lLen, 0)
                 If lRes <> 0 Then
                     For lIdx = 0 To UBound(abHash)
                         HashFile = HashFile & Right$("0" & Hex$(abHash(lIdx)), 2)
                         DoEvents
                     Next
                 End If
             End If
          End If
          CryptDestroyHash hHash
       End If
    End If
    CryptReleaseContext hCtx, 0
    If lRes = 0 Then Err.Raise Err.LastDllError
End Function

