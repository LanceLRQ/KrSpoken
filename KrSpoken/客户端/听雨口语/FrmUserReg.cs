using System;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using KrSpoken.Net;

namespace KrSpoken
{
    public partial class FrmUserReg : Form
    {
        public FrmUserReg(string _KrServer)
        {
            InitializeComponent();
            KrServer = _KrServer;
        }
        private KrClient kc;
        private string KrServer="";

        private bool iEmail = false;
        private bool iPassword = false;
        private bool iRePassword = false;
        private bool iNiName = true;
        private bool iDS_Name = false;
        private bool iPhone = true;
        private void FrmUserReg_Load(object sender, EventArgs e)
        {
            for (int i = 1; i < 100; i++)
            {
                Ds_Class.Items.Add(Convert.ToString(i));
            }
            Ds_Class.Text = "1";
            Ds_Year.Text = "2011";
        }
        private void RePassword_Leave(object sender, EventArgs e)
        {
            if (RePassword.Text.Length<1||RePassword.Text != Password.Text)
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
        private void Email_Leave(object sender, EventArgs e)
        {
            Regex re = new Regex(@"(?:[a-z\d]+[_\-\+\.]?)*[a-z\d]+@(?:([a-z\d]+\-?)*[a-z\d]+\.)+([a-z]{2,})+");
            if (!re.IsMatch(Email.Text))
            {
                iEmail = false;
                Email.BackColor = Color.Red;
            }
            else
            {
                iEmail = true;
                Email.BackColor = Color.GreenYellow;
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
            if (NiName.Text.Length >12)
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
        private void Ds_Name_Leave(object sender, EventArgs e)
        {
            Regex re = new Regex(@"^[\u4e00-\u9fa5]+$");
            if (!re.IsMatch(Ds_Name.Text))
            {
                iDS_Name = false;
                Ds_Name.BackColor = Color.Red;
                return;
            }
            if (Ds_Name.Text.Length < 1 || Ds_Name.Text.Length > 5)
            {
                iDS_Name = false;
                Ds_Name.BackColor = Color.Red;
            }
            else
            {
                iDS_Name = true;
                Ds_Name.BackColor = Color.GreenYellow;
            }
        }
        private void Phone_TextChanged(object sender, EventArgs e)
        {
            if (Phone.Text == "") { Phone.BackColor = Color.White; iPhone = true; return; }
            Regex re = new Regex(@"^\d*$");
            if (!re.IsMatch(Phone.Text))
            {
                iPhone = false;
                Phone.BackColor = Color.Red;
                return;
            }
            if (Phone.Text.Length < 1 || Phone.Text.Length > 11)
            {
                iPhone = false;
                Phone.BackColor = Color.Red;
            }
            else
            {
                iPhone = true;
                Phone.BackColor = Color.GreenYellow;
            }
        }
        private void Password_Leave(object sender, EventArgs e)
        {
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
        private void ButOk_Click(object sender, EventArgs e)
        {
            if (ButOk.Text == "注册")
            {
                Email.Enabled = false; Password.Enabled = false; NiName.Enabled = false; Ds_Name.Enabled = false; Ds_Class.Enabled = false; Ds_Year.Enabled = false;RePassword.Enabled = false; Phone.Enabled = false;
                if (!iEmail || !iPassword || !iRePassword || !iNiName || !iPhone || !iDS_Name)
                {
                    MessageBox.Show("您的信息填写有误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                ButOk.Text = "取消";
                kc = new KrClient(KrServer);
                kc.UserRegComplete += new KrNetServer_UserReg_Msg_Result(kc_UserRegComplete);
                kc.UserReg(Email.Text, Password.Text, NiName.Text, Ds_Name.Text, Ds_Class.Text, Ds_Year.Text, Phone.Text);
                return;
            }
            if (ButOk.Text == "取消")
            {
                if(MessageBox.Show("如果不是长时间没有响应，请不要取消此过程。\n\n您确定要取消吗", "提示", MessageBoxButtons.OKCancel,MessageBoxIcon.Exclamation)==DialogResult.OK){
                kc.StopProgressing();
                }
            }
        }

        void kc_UserRegComplete(string Message)
        {
            if (Message == "注册成功！")
            {
                MessageBox.Show(Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ButOk.Text = "注册成功";
                ButOk.Enabled = false;
            }
            else
            {
                MessageBox.Show(Message,"提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
                ButOk.Text = "注册";
                Email.Enabled = true; Password.Enabled = true; NiName.Enabled = true; Ds_Name.Enabled = true; Ds_Class.Enabled = true; Ds_Year.Enabled = true; RePassword.Enabled = true; Phone.Enabled = true;
            }
            
        }
    }
}
