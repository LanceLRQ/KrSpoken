using System;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using KrSpoken.Net;
using KrSpoken.Profile;

namespace KrSpoken
{
    public partial class FrmMyInfo : Form
    {
        public NetUserData nu;
        private bool isPasswordChange = false;
        private bool iPassword = true;
        private bool iRePassword = true;
        private bool iNiName = true;
        private bool iPhone = true;
        private KrClient kc;
        private string KrServer = "", HardCode = "";
        private bool IsProgressOK = false;


        private void OldPassword_Leave(object sender, EventArgs e)
        {
            if (OldPassword.Text.Length < 1)
            {
                isPasswordChange = false;
                OldPassword.BackColor = Color.White;
                return;
            }
            if (OldPassword.Text.IndexOf("'") > -1)
            {

                isPasswordChange = false;
                OldPassword.BackColor = Color.Red;
                return;
            }

            if (OldPassword.Text.Length > 30)
            {
                isPasswordChange = false;
                OldPassword.BackColor = Color.Red;
            }
            else
            {
                isPasswordChange = true;
                OldPassword.BackColor = Color.GreenYellow;
            }
        }
        private void Password_Change(object sender, EventArgs e)
        {
            if (OldPassword.Text == "")
            {
                return;
            }
            if (Password.Text.Length < 1)
            {
                iPassword = false;
                HowStrong.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Normal;
                HowStrong.Text = "请先输入密码";
                HowStrong.Value = 0;
                Password.BackColor = Color.Red;
                return;
            }
            if (Password.Text.IndexOf("'") > -1)
            {

                iPassword = false;
                HowStrong.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Normal;
                HowStrong.Text = "密码中存在非法字符 ";
                HowStrong.Value = 3;
                Password.BackColor = Color.Red;
                return;
            }
            if (Password.Text.Length < 8)
            {
                HowStrong.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Error;
                HowStrong.Text = "密码强度：弱";
                HowStrong.Value = 1;
            }
            if (Password.Text.Length >= 10 && Password.Text.Length <= 12)
            {
                HowStrong.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Paused;
                HowStrong.Text = "密码强度：中";
                HowStrong.Value = 2;
            }
            if (Password.Text.Length > 12)
            {
                HowStrong.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Normal;
                HowStrong.Text = "密码强度：强";
                HowStrong.Value = 3;
            }
            if (Password.Text.Length > 30)
            {
                iPassword = false;
                Password.BackColor = Color.Red;
            }
            else
            {
                iPassword = true;
                Password.BackColor = Color.GreenYellow;
            }
        }
        private void Password_Leave(object sender, EventArgs e)
        {
            if (OldPassword.Text == "")
            {
                return;
            }
            if (Password.Text.Length < 1)
            {
                iPassword = false;
                HowStrong.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Normal;
                HowStrong.Text = "请先输入密码";
                HowStrong.Value = 0;
                Password.BackColor = Color.Red;
                return;
            }
            if (Password.Text.IndexOf("'") > -1)
            {

                iPassword = false;
                HowStrong.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Normal;
                HowStrong.Text = "密码中存在非法字符 ";
                HowStrong.Value = 3;
                Password.BackColor = Color.Red;
                return;
            }
            if (Password.Text.Length < 8)
            {
                HowStrong.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Error;
                HowStrong.Text = "密码强度：弱";
                HowStrong.Value = 1;
            }
            if (Password.Text.Length >= 10 && Password.Text.Length <= 12)
            {
                HowStrong.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Paused;
                HowStrong.Text = "密码强度：中";
                HowStrong.Value = 2;
            }
            if (Password.Text.Length > 12)
            {
                HowStrong.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Normal;
                HowStrong.Text = "密码强度：强";
                HowStrong.Value = 3;
            }
            if (Password.Text.Length > 30)
            {
                iPassword = false;
                Password.BackColor = Color.Red;
            }
            else
            {
                iPassword = true;
                Password.BackColor = Color.GreenYellow;
            }
        }
        private void NiName_Leave(object sender, EventArgs e)
        {
            if (NiName.Text == "") { NiName.BackColor = Color.White; iNiName = true; return; }
            if (NiName.Text.IndexOf("'") > -1)
            {
                iNiName = false;
                NiName.BackColor = Color.Red;
                return;
            }
            if (NiName.Text.Length > 12)
            {
                iNiName = false;
                NiName.BackColor = Color.Red;
            }
            else
            {
                iNiName = true;
                NiName.BackColor = Color.GreenYellow;
            }
        }
        private void Phone_TextChanged(object sender, EventArgs e)
        {

        }
        private void RePassword_Leave(object sender, EventArgs e)
        {
            if (OldPassword.Text == "")
            {
                return;
            }
            if (RePassword.Text.Length < 1 || RePassword.Text != Password.Text)
            {
                iRePassword = false;
                RePassword.BackColor = Color.Red;
            }
            else
            {
                iRePassword = true;
                RePassword.BackColor = Color.GreenYellow;
            }
        }
        private void RePassword_Changed(object sender, EventArgs e)
        {
            if (OldPassword.Text == "")
            {
                return;
            }
            if (RePassword.Text.Length < 1 || RePassword.Text != Password.Text)
            {
                iRePassword = false;
                RePassword.BackColor = Color.Red;
            }
            else
            {
                iRePassword = true;
                RePassword.BackColor = Color.GreenYellow;
            }
        }

        public FrmMyInfo(string _KrServer,string _HardCode)
        {
            InitializeComponent();
            KrServer = _KrServer;
            HardCode=_HardCode;
        }
        private void FrmMyInfo_Load(object sender, EventArgs e)
        {
            if (nu.Email=="")
            {
                this.Close();
                return;
            }
            for (int i = 1; i < 100; i++)
            {
                Ds_Class.Items.Add(Convert.ToString(i));
            }
            Email.Text = "Email：" + nu.Email;
            NiName.Text = nu.NiName;
            DS_Name.Text = "真实姓名：" + nu.DS_Name;
            Phone.Text =  nu.Phone;
            Ds_Class.Text = nu.DS_Class;
            Ds_Year.Text = nu.DS_Year;
        }

        private void ButOK_Click(object sender, EventArgs e)
        {
            if (ButOk.Text == "保存")
            {
                OldPassword.Enabled = false; RePassword.Enabled = false; Password.Enabled = false; NiName.Enabled = false; Ds_Class.Enabled = false; Ds_Year.Enabled = false; Phone.Enabled = false;
                if ( !iNiName || !iPhone)
                {
                    MessageBox.Show("您的信息填写有误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (isPasswordChange)
                {
                    if (!iPassword || !iRePassword)
                    {
                        MessageBox.Show("密码信息填写错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                ButOk.Text = "取消";
                kc = new KrClient(KrServer);
                kc.ChangeInfoComplete+=new KrNetServer_ChangeInfo_Msg_Result(kc_ChangeInfoComplete);
                kc.UserInfoUpdate(OldPassword.Text,Password.Text, NiName.Text,  Ds_Class.Text, Ds_Year.Text, Phone.Text,nu.Login_Session,nu,HardCode);
                return;
            }
            if (ButOk.Text == "取消")
            {
                if (MessageBox.Show("如果不是长时间没有响应，请不要取消此过程。\n\n您确定要取消吗", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
                {
                    kc.StopProgressing();
                }
            }
        }

        void kc_ChangeInfoComplete(string Message, NetUserData _NetUser)
        {
            if (Message == "修改成功！")
            {
                MessageBox.Show(Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                nu = _NetUser;
                IsProgressOK = true;
                this.Close();
                ButOk.Text = "修改成功";
                ButOk.Enabled = false;
            }
            else
            {
                MessageBox.Show(Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ButOk.Text = "保存";
                OldPassword.Enabled = true; Password.Enabled = true; NiName.Enabled = true; Ds_Class.Enabled = true; Ds_Year.Enabled = true; Phone.Enabled = true; RePassword.Enabled = true;
            }
        }

        private void FrmMyInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsProgressOK)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }

    }
}
