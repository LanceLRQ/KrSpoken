using System;
using System.Windows.Forms;
using KrSpoken.Net;
using System.IO;

namespace KrSpoken
{
    public partial class FrmDownPkList : Form
    {
        private string PkName = "", HardCode = "", SessionID = "", fileMx = "", fileIdx = "";
        private KrClient kc;
        public FrmDownPkList(string _PkName, string _HardCode, string _KrServer, string _SessionID,string _fileMx,string _fileIdx)
        {
            InitializeComponent();
            kc = new KrClient(_KrServer);
            PkName = _PkName;
            HardCode = _HardCode;
            SessionID = _SessionID;
            fileMx = _fileMx;
            fileIdx = _fileIdx;
        }
        private void FrmDownPkList_Load(object sender, EventArgs e)
        {
            kc.DownKrPkIndex(PkName,HardCode,SessionID);
            kc.DownPkIdxProgress += new KrNetServer_DownloadProgress(kc_DownPkIdxProgress);
            kc.DownPkIdxComplete += new KrNetServer_DownPkIdx_Msg_Result(kc_DownPkIdxComplete);
        }

        void kc_DownPkIdxComplete(string Message, string _Idx, string _MxIdx)
        {
            try
            {
                if (Message == "下载完成")
                {
                    using (FileStream fs = new FileStream(fileIdx, FileMode.Create, FileAccess.Write))
                    {
                        byte[] buffer = Convert.FromBase64String(_Idx);
                        fs.Write(buffer, 0, buffer.Length);
                        fs.Close();
                    }
                    using (FileStream fs = new FileStream(fileMx, FileMode.Create, FileAccess.Write))
                    {
                        byte[] buf = Convert.FromBase64String(_MxIdx);
                        fs.Write(buf, 0, buf.Length);
                        fs.Close();
                    }
                    MessageBox.Show(Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void kc_DownPkIdxProgress(int ProgressPercentage)
        {
            progressBar1.Value = ProgressPercentage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            kc.StopProgressing();
        }

    }
}
