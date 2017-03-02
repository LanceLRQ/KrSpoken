VERSION 5.00
Begin VB.Form Form1 
   Caption         =   "Form1"
   ClientHeight    =   3195
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   4680
   LinkTopic       =   "Form1"
   ScaleHeight     =   3195
   ScaleWidth      =   4680
   StartUpPosition =   3  '´°¿ÚÈ±Ê¡
   Begin VB.TextBox Text1 
      Height          =   2535
      Left            =   360
      TabIndex        =   0
      Text            =   "Text1"
      Top             =   480
      Width           =   3855
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub Form_Load()
Dim a As New PKMod
'
  Text1.Text = a.OutputPK("A", "A", "1F28317A6367EFDCAD84EF3AFFC356CF", "wbsW/QRQSlGdIEE51vEuNFOD1uYGyy87INtxEqUrFMjFf/V9RxSs4/lY5mUHFrMRnOQFHMHcxS+hQS+6yk+600jinBnmXm+JxXX1cvlYf/7DBMrKyO6H5Yqs5mUmQyYm5FPWJmlRvriQ5mUmQy0XxYOp0Ujiyl7lym+n1/p9x+wxwv0rnOQFHU3y5SPIxKpevi73EuYmOfQH5YqsSSgCJ0nufv3B5FoQNYEWJm7T5SY+OmgCf/451SgEESB3EukIyY89OMiQvl7Z5Fq4EriQ5mUmQXji5klVFMrSySG4EriQvmghOKY3XfDr5FP51vYmOfHhQS+6yk+60rqsI/yxEqMp5YHUJU35y/OzxXX1cvlYf/7DBMrKf/7p5YHUnOQnEuYGvrpb5gE1fyYmJ0N4yYrSnyglfK7UfmMp0/X15F00umDr5FPnErPrvriQ5mUmQXji5klDyK7U5F00umDr5F8vrYqovMhmvYiQvSghOKXWJrDe5SYprYYNyk+60uHXyuP0um7UOkG3yyNfOSv1fMxnEqt=")
    
  '  MsgBox a.GetHardCode
    
End Sub
