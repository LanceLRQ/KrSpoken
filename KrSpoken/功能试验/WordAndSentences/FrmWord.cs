using System;
using System.Windows.Forms;
using System.Drawing;

namespace KrSpoken.WordAndSentences
{
    public delegate void OnFrmWordClosingDelegate();
    public partial class FrmWord : Form
    {
        public string pWord = "";
        public string pDbPath = "";
        public string pBaiduData = "";
        private WordCapture wc ;
        public event OnFrmWordClosingDelegate FrmCloseing;
        public FrmWord()
        {
            InitializeComponent();
            
        }

        private void FrmWord_Load(object sender, EventArgs e)
        {
            if (Environment.OSVersion.Version.Major >5)
            {
                L_DescLab.Font= new Font("微软雅黑",10);
                L_PronLab.Font = new Font("微软雅黑", 9);
                L_WordLab.Font = new Font("微软雅黑", 10, FontStyle.Bold);
                N_DescLab.Font = new Font("微软雅黑", 9);
                N_WordLab.Font = new Font("微软雅黑", 10, FontStyle.Bold);
            }
            try
            {
                wc = new WordCapture(pDbPath);
                string[] s=wc.CreateNewFind(pWord);
                L_DescLab.Text = s[2];
                L_PronLab.Text ="音标:"+ s[1];
                L_WordLab.Text = s[0];
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void FrmWord_FormClosing(object sender, FormClosingEventArgs e)
        {
            wc.StopProgressing();
            FrmCloseing();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Text == "百度词典")
            {
                if (pBaiduData == "")
                {
                    wc.FindFromBaiduComplete += new WordCapture_BaiduDownloadComplete(wc_FindFromBaiduComplete);
                    wc.CreateNewFindFromBaiduDic(pWord);
                }
            }
        }

        void wc_FindFromBaiduComplete(string Results)
        {
            N_WordLab.Text = pWord;
            N_DescLab.Text = Results;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            wc.StopProgressing();
        }
    }
}
