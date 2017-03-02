using System;
using System.IO;
using System.Windows.Forms;
using System.Resources;
using KrSpoken.Profile;
using System.Collections;
using KrSpoken.Net;
using System.Net;

namespace KrSpoken
{
    public partial class FrmMain : Form
    {
        //试题包列表
        public UserPkLib UserPkList;
        private FrmPractice frmPractice;
        private FrmExam frmExam;
        public AppProfileClass AppProfile;
        private FrmLogin frmLogin;
        private FrmUserReg frmUserReg;
        public NetUserData NetUser;//网络用户数据模块
        private FrmDownPkList frmDownPkList;
        private bool IsExamMode = false;
        private FrmAnswer frmAnswer;
        public FrmMyInfo frmMyInfo;
        public WebClient webClient_GG;

        #region 窗体初始化部分
        public FrmMain()
        {
            InitializeComponent();
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            if (Environment.OSVersion.Version.Major > 5)
            {
                GongGaoBox.Font = new System.Drawing.Font("微软雅黑", 12);
            }
            System.Diagnostics.Process.GetCurrentProcess().MaxWorkingSet = (IntPtr)750000;
            AppProfileRuntime.Infile(Application.StartupPath+"\\KrSpoken.profile", out AppProfile);
            NetUserDataRuntime.Infile(Application.StartupPath + "\\NetUser.profile", out NetUser);
            UserPkLibRuntime.Infile(AppProfile.PkLibfile, out UserPkList);
            this.Text = AppProfile.Title;
            if (AppProfile.FrmMain_BackGround == null || AppProfile.FrmMain_BackGround == "")
            {
                this.BackgroundImage = Background.Bg_0;
            }
            else
            {
                try
                {
                    this.BackgroundImage = System.Drawing.Image.FromFile(AppProfile.FrmMain_BackGround);
                }
                catch
                {
                    this.BackgroundImage = Background.Bg_0;
                    AppProfile.FrmMain_BackGround = "";
                }
            }
            //DateTime now = DateTime.UtcNow;
            RefreshNetState();
           // MessageBox.Show(now.Year.ToString() + now.Month.ToString() + now.Day.ToString() + now.Hour.ToString() + now.Minute.ToString() + now.Second.ToString() + now.Millisecond.ToString() + Convert.ToString((new Random().Next(9999))));
           // UserPkList.Add("2011年广东省高考听说练习题1", "2011年广东省高考听说练习题1.krpk", "标准", "入门", "2011年广东省高考听说练习题1.kridx", "2011年广东省高考听说练习题1.krmxidx");
           // UserPkLibRuntime.Outfile(AppProfile.PkLibfile, UserPkList);
            webClient_GG = new WebClient();
            webClient_GG.Proxy=null;
            webClient_GG.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_GG_DownloadStringCompleted);
            webClient_GG.DownloadStringAsync(new Uri(AppProfile.GetKrServer+"/Gonggao.txt"));
        }
        //公告信息返回对象
        private void webClient_GG_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                GongGaoBox.Text = e.Result;
                webClient_GG = null;
            }
            else
            {
                GongGaoBox.Text = "公告获取失败，请检查网络。";
                webClient_GG = null;
            }
        }
        #endregion

        //标题框切换
        private void SelectMainBoxTab(int _index)
        {
            for (int i = 0; i < 6; i++)
            {
                if (i != _index)
                {
                    MainBox.Tabs[i].Visible = false;
                }
            }
            MainBox.Tabs[_index].Visible = true;
            MainBox.SelectedTabIndex = _index;
        }

        #region 按钮部分
        private void pBut1_Click(object sender, EventArgs e)
        {
            Title2.Text = "请选择试题包>>练习模式";
            IsExamMode = false;
            SelectMainBoxTab(1);
            UserPkLibRuntime.Infile(AppProfile.PkLibfile,out UserPkList);
            ChangePKList();
        }
        private void pBut2_Click(object sender, EventArgs e)
        {
            Title2.Text = "请选择试题包>>考试模式";
            IsExamMode = true;
            SelectMainBoxTab(1);
            UserPkLibRuntime.Infile(AppProfile.PkLibfile,out UserPkList);
            ChangePKList();
        }
        private void pBut3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("功能即将开发，敬请期待！");
            //新程序(窗口)
        }
        private void pBut4_Click(object sender, EventArgs e)
        {
            frmAnswer = new FrmAnswer();
            frmAnswer.frmMain = this;
            frmAnswer.UserPkList = UserPkList;
            frmAnswer.AppProfile = AppProfile;
            frmAnswer.ShowDialog();
        }
        private void pBut5_Click(object sender, EventArgs e)
        {
            SelectMainBoxTab(2);
            UserPkLibRuntime.Infile(AppProfile.PkLibfile,out UserPkList);
            ChangePKListA();
        }
        private void pBut6_Click(object sender, EventArgs e)
        {
            SelectMainBoxTab(3);
            Txt_NetServer.Text = AppProfile.GetKrServer.Replace("http://","").Replace("/krspoken/krserver/","");
        }
        private void pBut7_Click(object sender, EventArgs e)
        {
            SelectMainBoxTab(4);
        }
        private void pBut8_Click(object sender, EventArgs e)
        {
            SelectMainBoxTab(5);
        }
        private void pBut9_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        //刷新试题包列表 
        private void ChangePKList()
        {
            PKList.Items.Clear();
            foreach (System.Collections.DictionaryEntry objDE in UserPkList._PackTitle)
            {
                string PKName = objDE.Value.ToString();
                PKList.Items.Add(PKName);
                PKList.Items[PKList.Items.Count - 1].SubItems.Add(UserPkList.get_PackType(PKName));
                PKList.Items[PKList.Items.Count - 1].SubItems.Add(UserPkList.get_PackDiff(PKName));
                PKList.Items[PKList.Items.Count - 1].SubItems.Add(Convert.ToString(UserPkList.get_PackExamTime(PKName)));
                PKList.Items[PKList.Items.Count - 1].SubItems.Add(Convert.ToString(UserPkList.get_PackPracticeTime(PKName)));
            }
        }
        //刷新试题包列表 
        private void ChangePKListA()
        {
            PKListA.Items.Clear();
            foreach (System.Collections.DictionaryEntry objDE in UserPkList._PackTitle)
            {
                string PKName = objDE.Value.ToString();
                PKListA.Items.Add(PKName);
                PKListA.Items[PKListA.Items.Count - 1].SubItems.Add(UserPkList.get_PackType(PKName));
                PKListA.Items[PKListA.Items.Count - 1].SubItems.Add(UserPkList.get_PackDiff(PKName));
                if (UserPkList.get_IsCanUse(PKName)==true){
                    PKListA.Items[PKListA.Items.Count - 1].SubItems.Add("生效");
                }else{
                    PKListA.Items[PKListA.Items.Count - 1].SubItems.Add("未生效");
                }
            }
        }
        //试题包部分 双击
        private void PKList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = PKList.HitTest(e.X, e.Y);
            if (info != null)
            {
                try
                {
                    if (IsExamMode == false)
                    {
                        frmPractice = null;
                        frmPractice = new FrmPractice();
                        string t = info.Item.Text;
                        if (!UserPkList.get_IsCanUse(t))
                        {
                            MessageBox.Show("请先激活试题包！\n\n小提示：在“导入试题”处选择该试题包并点击“下载试题包索引”按钮。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        frmPractice.pTitle = t;
                        frmPractice.AppProfile = AppProfile;
                        frmPractice.Nu = NetUser;
                        frmPractice.pfileName = UserPkList.get_fileName(t);
                        frmPractice.pType = UserPkList.get_PackType(t);
                        frmPractice.pIndex = UserPkList.get_PkIndex(t);
                        frmPractice.pMix = UserPkList.get_PkMix(t);
                        frmPractice.frmMain = this;
                        PKList.Enabled = false;
                        //this.Hide();
                        ToolTip TipBox;
                        TipBox = new ToolTip();
                        TipBox.IsBalloon = true;
                        TipBox.AutomaticDelay = 3000;
                        TipBox.ShowAlways = true;
                        TipBox.ToolTipIcon = ToolTipIcon.Info;
                        TipBox.UseFading = true;
                        TipBox.ToolTipTitle = "提示";
                        TipBox.Show("试题包正在加载，请稍等...", this, 50, -10);
                        Application.DoEvents();
                        frmPractice.ShowDialog();
                        TipBox = null;
                        this.Show();
                        PKList.Enabled = true;
                        frmPractice = null;
                        GC.Collect();
                    }
                    else
                    {
                        //Exam
                        FrmTest ft = new FrmTest();
                        this.Hide();
                        ft.ShowDialog();
                        ft.Close();
                        ft = null;
                        GC.Collect();
                        frmExam = null;
                        string t = info.Item.Text;
                        if (!UserPkList.get_IsCanUse(t))
                        {
                            MessageBox.Show("请先激活试题包！\n\n小提示：在“导入试题”处选择该试题包并点击“下载试题包索引”按钮。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.Show();
                            return;
                        }
                        frmExam = new FrmExam();
                        PKList.Enabled = false;
                        this.Show();
                        frmExam.pTitle = t;
                        frmExam.AppProfile = AppProfile;
                        frmExam.Nu = NetUser;
                        frmExam.pfileName = UserPkList.get_fileName(t);
                        frmExam.pType = UserPkList.get_PackType(t);
                        frmExam.pIndex = UserPkList.get_PkIndex(t);
                        frmExam.pMix = UserPkList.get_PkMix(t);
                        //this.Hide();
                        ToolTip TipBox;
                        TipBox = new ToolTip();
                        TipBox.IsBalloon = true;
                        TipBox.AutomaticDelay = 3000;
                        TipBox.ShowAlways = true;
                        TipBox.ToolTipIcon = ToolTipIcon.Info;
                        TipBox.UseFading = true;
                        TipBox.ToolTipTitle = "提示";
                        TipBox.Show("试题包正在加载，请稍等...", this, 50, -10);
                        Application.DoEvents();
                        frmExam.ShowDialog();
                        TipBox = null;
                        this.Show();
                        PKList.Enabled = true;
                        frmExam = null;
                        GC.Collect();
                    }
                    System.Diagnostics.Process.GetCurrentProcess().MaxWorkingSet = (IntPtr)750000;
                }
                catch
                {
                    MessageBox.Show("抱歉，试题包加载失败，请重试。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    PKList.Enabled = true;
                    frmPractice = null;
                    GC.Collect();
                }
            }
        }
        private void SkinBut_Click(object sender, EventArgs e)
        {
            FrmChangeBg fcb = new FrmChangeBg();
            fcb.Selected += new FrmChangeBg_SelectedDelegate(fcb_Selected);
            fcb.AppProfile = AppProfile;
            fcb.ShowDialog();
            fcb = null;
            GC.Collect();
        }
        private void fcb_Selected(string _Path)
        {
            AppProfile.FrmMain_BackGround = _Path;
            try
            {
                this.BackgroundImage = System.Drawing.Image.FromFile(AppProfile.FrmMain_BackGround);
            }
            catch
            {
                this.BackgroundImage = Background.Bg_0;
                AppProfile.FrmMain_BackGround = "";
            }
            AppProfileRuntime.Outfile(Application.StartupPath + "\\KrSpoken.profile", AppProfile);
        }
        //登陆状态刷新
        private void RefreshNetState()
        {
            //判断登陆完毕
            if (NetUser.Login_Session != null && NetUser.Login_Session != "")
            {
                UserBox_Default.Visible = false;
                UserBox_Logined.Visible = true;
                this.Text = AppProfile.Title + "    [已注册]";
            }
            else
            {
                UserBox_Default.Visible = true;
                UserBox_Logined.Visible = false;
                this.Text = AppProfile.Title + "    [未注册]";
            }
        }

        private void User_But_Login_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLogin = new FrmLogin(AppProfile.GetKrServer);
            frmLogin.fm = this;
            frmLogin.ShowDialog();
            if (frmLogin.isLogined)
            {
                NetUser = frmLogin.nud;
                NetUserDataRuntime.Outfile(Application.StartupPath + "\\NetUser.profile", NetUser);
                RefreshNetState();
            }
            frmLogin.Close();
            frmLogin = null;
            GC.Collect();
        }
        private void User_But_Reg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmUserReg = new FrmUserReg(AppProfile.GetKrServer);
            frmUserReg.ShowDialog();
            frmUserReg = null;
            GC.Collect();
        }

        private void But_Down_Index_Click(object sender, EventArgs e)
        { 
            string t = PKListA.SelectedItems[0].Text;
            frmDownPkList = new FrmDownPkList(t, (new Lyoko_KR_PkMod.PKModClass()).GetHardCode(), AppProfile.GetKrServer, NetUser.Login_Session, AppProfile.PackDir + "\\" + UserPkList.get_PkMix(t), AppProfile.PackDir + "\\" + UserPkList.get_PkIndex(t));
            if (frmDownPkList.ShowDialog() == DialogResult.Cancel)
            {
                frmDownPkList = null;
                GC.Collect();
            }
            else
            {
                UserPkList.Update(t, 3, 0, 0, true);
                UserPkLibRuntime.Outfile(AppProfile.PkLibfile, UserPkList);
                frmDownPkList = null;
                GC.Collect();
                But_Down_Index.Enabled = false;
                ChangePKListA();
            }
        }
        private void PKListA_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                if (!UserPkList.get_IsCanUse(PKListA.SelectedItems[0].Text))
                {
                    But_Down_Index.Enabled = true;
                }
                else
                {
                    But_Down_Index.Enabled = false;
                }
                But_Delete.Enabled = true;
            }
            else
            {
                But_Down_Index.Enabled = false;
                But_Delete.Enabled = false;
            }
        }
        private void But_Delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确定要删除试题包吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                string t = PKListA.SelectedItems[0].Text;
                File.Delete(AppProfile.PackDir + "\\" + UserPkList.get_fileName(t));
                File.Delete(AppProfile.PackDir + "\\" + UserPkList.get_PkIndex(t));
                File.Delete(AppProfile.PackDir + "\\" + UserPkList.get_PkMix(t));
                UserPkList.Delete(t, UserPkList.FindPackID(t));
                UserPkLibRuntime.Outfile(AppProfile.PkLibfile, UserPkList);
                ChangePKListA();
            }
        }

        private void User_But_MyInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmMyInfo = new FrmMyInfo(AppProfile.GetKrServer,(new Lyoko_KR_PkMod.PKModClass()).GetHardCode());
            frmMyInfo.nu = NetUser;
            if (frmMyInfo.ShowDialog() == DialogResult.OK)
            {
               NetUser = frmMyInfo.nu;
               NetUserDataRuntime.Outfile(Application.StartupPath + "\\NetUser.profile",NetUser);
            }
            frmMyInfo = null;
            GC.Collect();
        }

        private void But_InputPack_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "(*.krzip) 试题包安装文件|*.krzip";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = Application.StartupPath + "\\KrPKInstall.exe";
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.Arguments = "\"" + ofd.FileName + "\"";
                p.Start();
                p.WaitForExit();
            }
        }

        private void User_But_Exit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            KrClient kc = new KrClient(AppProfile.GetKrServer);
            if (kc.UserLogout(NetUser.Login_Session, (new Lyoko_KR_PkMod.PKModClass()).GetHardCode())=="Success")
            {
                NetUser = new NetUserData();
                File.Delete(Application.StartupPath + "\\NetUser.profile");
                RefreshNetState();
                MessageBox.Show("退出成功！");
            }
            else
            {
                MessageBox.Show("操作失败，请重试！");
                return;
            }
            kc = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AppProfile.GetKrServer = "http://" + Txt_NetServer.Text + "/krspoken/krserver/";
            AppProfileRuntime.Outfile(Application.StartupPath + "\\KrSpoken.profile", AppProfile);
            MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void User_But_Reg_2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("内测版不提供此功能", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void User_But_FindPass_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("忘记密码？\n\n用注册时填写的邮箱发邮件到lyokolrq@qq.com，标题：“密码找回”，内容:你的班级、真实姓名。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
