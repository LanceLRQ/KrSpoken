using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using KrSpoken.Profile;
using KrSpoken.Net;
using Lyoko_KR_PkMod;
using System.Resources;
using System.Text;
using System.Collections;
using System.Security.Cryptography;

namespace KrSpoken
{   

    public partial class FrmLogin : Form
    {

        public FrmMain fm;
        private HttpWebClient wc;
        private KrClient kc;
        public NetUserData nud = new NetUserData();
        public bool isLogined = false;
        private string KrServer = "";
        public FrmLogin(string _KrServer)
        {
            InitializeComponent();
            KrServer=_KrServer;
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            kc = new KrClient(KrServer);
            wc = new HttpWebClient(new CookieContainer());
            wc.Proxy = null;
            
            wc.DownloadDataCompleted += new DownloadDataCompletedEventHandler(wc_DownloadDataCompleted);
            wc.DownloadDataAsync(new Uri(KrServer +"/CheckCode.aspx?src=" +Convert.ToString((new Random()).NextDouble())));
        }

        void wc_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            try
            {
                MemoryStream ms = new MemoryStream(e.Result);
                Bitmap bmp = new Bitmap(ms);
                pictureBox1.Image = bmp;
                linkLabel1.Enabled = true;
                pictureBox1.Cursor = System.Windows.Forms.Cursors.Arrow;
            }
            catch
            {
                MessageBox.Show("验证码加载失败，请重试！");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (wc.IsBusy)
            {
                pictureBox1.Image = MainButton.icon_2 ;
                pictureBox1.Cursor=System.Windows.Forms.Cursors.WaitCursor;
                wc.CancelAsync();
                wc.DownloadDataAsync(new Uri(KrServer + "/CheckCode.aspx?src=" + Convert.ToString((new Random()).NextDouble())));
            }
            else
            {
                pictureBox1.Image = MainButton.icon_2;
                pictureBox1.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                wc.DownloadDataAsync(new Uri(KrServer + "/CheckCode.aspx?src=" + Convert.ToString((new Random()).NextDouble())));
            }
        }

        private void ButLogin_Click(object sender, EventArgs e)
        {
            if (wc.IsBusy)
            {
                MessageBox.Show("请等待验证码的下载完成。");
            }
            else
            {
                Email.Enabled = false; Password.Enabled = false; ButLogin.Enabled = false; linkLabel1.Enabled = false;
                wc.Headers[HttpRequestHeader.UserAgent] = "KrSpoken.Net/1.0 (" + Environment.OSVersion.VersionString + ")[HardCode=" + (new Lyoko_KR_PkMod.PKModClass()).GetHardCode() + "]";
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                wc.UploadStringCompleted+=new UploadStringCompletedEventHandler(wc_UploadStringCompleted);
                wc.UploadStringAsync(new Uri(KrServer + "/C_Login.aspx?ImgCode=" + ImgCheckCode.Text), "post","Data=" +System.Web.HttpUtility.HtmlEncode(kc.UserLogin(Email.Text, Password.Text)));
            }
        }

        void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            string message="";
            try
            {
                if (e.Cancelled)
                {
                    message="用户取消";
                    return;
                }
                switch (e.Result)
                {
                    case "Error+0x00000000":
                        message = "验证码错误！";
                        break;
                    case "Error-0x00000000":
                        message = "登陆时发生错误！";
                        break;
                    case "Error-0x00000001":
                        message = "登陆时发生错误！";
                        break;
                    case "Error-0x00000002":
                        message = "登陆时发生错误！";
                        break;
                    case "Error-0x00000003":
                        message = "请填写登陆信息！";
                        break;
                    case "Error-0x00000004":
                        message = "登陆信息有误！";
                        break;
                    case "Error-0x00000005":
                        message = "登陆时服务器发生错误！请重试";
                        break;
                    default:
                        if (e.Result.IndexOf("Success") < 0)
                        {
                            message = e.Result;
                            return;
                        }
                        else
                        {
                            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                            rsa.ImportCspBlob(Convert.FromBase64String("BwIAAACkAABSU0EyAAQAAAEAAQBBb0ZBs/cNwqZieI+LSgSBUAEzlxr35G5Lo4/sevDdqN2lOIYfWwmZ/t2U1v+EUjnVSFN90dgac9sYgZhSC3IxMauOW0BVxNwPIAu5mnWK6Q1BlFtCGhBnYvYb2aS6UnguCLb82EskKha73OV7sT/XzqOeXnbvK7XQc7P+ztu2mE+eeHODzpEayL2vwTjKw9HV0USQ3Xf/4sxKKGIAjgj3df91+t8uSWHUfUvJIBlg1zMQDnCK0IeE1lUcrT1/oMxvxalkT059lIa1PfNXWtfZPU07tZg8hWYEkIcevF24wG8S3gMlhzWIoTCSnCpCazsNUUPP3p/DDGbJmOMZ5w2/JUmjROwxYsAuptAt7istfiw5QL0XwSdYcBr+fNeDZYXz7V33pEE+U5wvL8TxKxLpmf5cbsnfQwsozU5FkPGGwSEb1IwbNabao68jup3HNzmukiOT75VJ0HFxhZeCknUv2TVaj48pA3bf9tbQq4uABievY07UwU/oDnwxPobUJQvutCttVTnsU2Px6gHHfFYI05LzZNkKAc6IdxMJEblQPv8UBf4wwBbfTfkSOlSSVOgSPP0WqYk8tpUtaq6B+c5LhcORInwlj2/yV3A1crpISae8QXJ9a4UWBC4AZhhxDviDJfWZiXHy0AMaIA0DpW+VsvrQFDwecOI1XNhQfDErhT6f3gm58qxGP6qS1ooaiE3f7agh83tl1kVotHQ6G0vplhajHcxq71dC6f3UQhlAHAxo9SApq+fqwTa144rpnkE="));
                            byte[] buffer = Convert.FromBase64String(e.Result.Replace("Success", "").Replace("\n", ""));
                            string MyData = "";
                            using (MemoryStream ms2 = new MemoryStream(buffer))
                            {
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    int leng = 0, ii = 0;
                                    leng = buffer.Length;
                                    while (ii < leng)
                                    {
                                        if (leng - ii >= 128)
                                        {
                                            byte[] b = new byte[128];
                                            ms2.Read(b, 0, 128);
                                            byte[] tmp = rsa.Decrypt(b, false);
                                            ms.Write(tmp, 0, tmp.Length);
                                            ii += 128;
                                        }
                                    }
                                    buffer = null;
                                    ms.Position = 0;
                                    byte[] Mydata;
                                    Mydata = new byte[(int)ms.Length];
                                    ms.Read(Mydata, 0, (int)ms.Length);
                                    MyData = Encoding.Unicode.GetString(Mydata);
                                    ms.Close();
                                }
                                ms2.Close();
                            }
                            rsa.Clear();
                            rsa = null;
                            //进行解释
                            string[] a = MyData.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                            Hashtable cmd = new Hashtable();
                            foreach (string aa in a)
                            {
                                string[] uss = aa.Split(new string[] { "=" }, StringSplitOptions.None);
                                cmd.Add(uss[0], uss[1].Replace("&Equal;", "="));
                            }
                            nud.CheckCode = cmd["CheckCode"].ToString();
                            nud.DS_Class = cmd["DS_Class"].ToString();
                            nud.DS_Name = cmd["DS_Name"].ToString();
                            nud.DS_Year = cmd["DS_Year"].ToString();
                            nud.Email = cmd["Email"].ToString();
                            nud.HardCode = cmd["HardCode"].ToString();
                            nud.ID = cmd["ID"].ToString();
                            nud.NiName = cmd["NiName"].ToString();
                            nud.Phone = cmd["Phone"].ToString();
                            nud.RegCode = cmd["RegCode"].ToString();
                            nud.RSAPrivateKey = cmd["RSAPrivateKey"].ToString();
                            nud.Login_Session = cmd["Login_Session"].ToString();
                        }
                        isLogined = true;
                        message = "登陆成功！";
                        break;
                }
                if (message == "登陆成功！")
                {
                    MessageBox.Show(message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show(message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Email.Enabled = true; Password.Enabled = true; linkLabel1.Enabled = true; ButLogin.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Email.Enabled = true; Password.Enabled = true; linkLabel1.Enabled = true; ButLogin.Enabled = true;
                MessageBox.Show("异常：" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
           
        }
    }
}
