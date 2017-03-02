using System;
using System.IO;
using System.Text;
using System.Net;
using System.Security.Cryptography;
using KrSpoken.Profile;


namespace KrSpoken.Net
{
    //重写WebClient
    public class HttpWebClient : WebClient
    {
        private CookieContainer cookie;
        protected override WebRequest GetWebRequest(Uri address)
        {
            //throw new Exception(); 
            WebRequest request;
            request = base.GetWebRequest(address);
            //判断是不是HttpWebRequest.只有HttpWebRequest才有此属性 
            if (request is HttpWebRequest)
            {
                HttpWebRequest httpRequest = request as HttpWebRequest;
                httpRequest.CookieContainer = cookie;
            }
            return request;
        }
        public HttpWebClient(CookieContainer cookie)
        {
            this.cookie = cookie;
        }
    } 

    public delegate void KrNetServer_UserReg_Msg_Result(string Message);
    public delegate void KrNetServer_DownloadProgress(int ProgressPercentage);
    public delegate void KrNetServer_DownPkIdx_Msg_Result(string Message,string _Idx,string _MxIdx);
    public delegate void KrNetServer_ChangeInfo_Msg_Result(string Message,NetUserData _NetUser);
    /// <summary>
    /// 提供口语软件的网络服务
    /// </summary>
    public class KrClient
    {
        private string KrServerHost = "";
        private RSACryptoServiceProvider RSA;
        private WebClient wc;
        private NetUserData NetUser_Tmp;
        /// <summary>
        /// 用户注册回调事件
        /// </summary>
        public event KrNetServer_UserReg_Msg_Result UserRegComplete;
        public event KrNetServer_DownloadProgress DownPkIdxProgress;
        public event KrNetServer_DownPkIdx_Msg_Result DownPkIdxComplete;
        public event KrNetServer_ChangeInfo_Msg_Result ChangeInfoComplete;
        /// <summary>
        /// 初始化网络模块
        /// </summary>
        /// <param name="_KrServerHost">服务器位置</param>
        public KrClient(string _KrServerHost)
        {
            KrServerHost = _KrServerHost;
            RSA = new RSACryptoServiceProvider();
            RSA.ImportCspBlob(Convert.FromBase64String("BgIAAACkAABSU0ExAAQAAAEAAQB5DOdPByTLlX3eEoUz1ua0Yg0yNi+kqOedspqRzr8FbhugcSsCol9En5yAyG0revDl/u3p1hb3NC7fqTyKV+1ZDzNfGWMc0JUjRzDaEiNBbyyr2rSjNXi9VwJc4k+VPLq64dlTimWukiS8Jn9SpOkkg1b4GVmwFNpNL9monZAFlA=="));
        }

        #region 用户注册部分
        /// <summary>
        /// 用户注册 
        /// </summary>
        public void UserReg(string _Email, string _Pass, string _NiName, string _dName, string _dClass, string _dYear, string _Phone)
        {
            StringBuilder sb = new StringBuilder();
            if (_NiName == "") { _NiName = "&null;"; }
            if (_Phone == "") { _Phone = "&null;"; }
            sb.Append("Email=" + _Email.Replace("=","&Equal;") +"\n");
            sb.Append("Password=" + _Pass.Replace("=", "&Equal;") + "\n");
            sb.Append("NiName=" + _NiName.Replace("=", "&Equal;") + "\n");
            sb.Append("DS_Name=" + _dName.Replace("=", "&Equal;") + "\n");
            sb.Append("DS_Class=" + _dClass.Replace("=", "&Equal;") + "\n");
            sb.Append("DS_Year=" + _dYear.Replace("=", "&Equal;") + "\n");
            sb.Append("Phone=" + _Phone.Replace("=", "&Equal;"));
            byte[] buffer = Encoding.Unicode.GetBytes(sb.ToString());
            sb = null;
            //适应加密模块
            MemoryStream bm = new MemoryStream(buffer);
            int leng = 0, ii = 0;
            leng = buffer.Length;
            MemoryStream m = new MemoryStream();
            while (ii < leng)
            {
                if (leng - ii >= 110)
                {
                    byte[] b = new byte[110];
                    bm.Read(b, 0, 110);
                    m.Write( RSA.Encrypt(b, false), 0, 128);
                    ii += 110;
                }
                else
                {
                    byte[] b = new byte[leng - ii];
                    bm.Read(b, 0, leng - ii);
                    m.Write(RSA.Encrypt(b, false), 0, 128);
                    ii = leng;
                }

            }
            m.Position = 0;
            byte[] bb = new byte[(int)m.Length];
            m.Read(bb, 0, (int)m.Length);
            wc = null;
            wc= new WebClient();
            wc.Proxy = null;
            wc.Headers[HttpRequestHeader.UserAgent] = "KrSpoken.Net/1.0 ("+Environment.OSVersion.VersionString+")["+DateTime.Now.ToString()+"]";
            wc.UploadStringCompleted += new UploadStringCompletedEventHandler(wc_UploadStringCompleted);
            wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            wc.UploadStringAsync(new Uri(KrServerHost + "/C_UserReg.aspx?sec="+Convert.ToString((new Random()).NextDouble())),"post", "Data=" + System.Web.HttpUtility.HtmlEncode(Convert.ToBase64String(bb)));
        }
        
        public void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    UserRegComplete("用户取消");
                    return;
                }
                switch (e.Result)
                {
                    case "Error-0x00000000":
                        UserRegComplete("注册时发生错误！");
                        break;
                    case "Error-0x00000001":
                        UserRegComplete("注册时发生错误！");
                        break;
                    case "Error-0x00000002":
                        UserRegComplete("注册时发生错误！");
                        break;
                    case "Error-0x00000003":
                        UserRegComplete("注册信息填写错误！");
                        break;
                    case "Error-0x00000004":
                        UserRegComplete("注册时新的密钥分配错误！");
                        break;
                    case "Error-0x00000005":
                        UserRegComplete("注册时服务器的数据库发生错误！");
                        break;
                    case "Error-0x00000006":
                        UserRegComplete("当前邮箱已经被注册！");
                        break;
                    case "Success":
                        UserRegComplete("注册成功！");
                        break;
                   default:
                        UserRegComplete(e.Result);
                        break;
                }
            }
            catch(Exception ex)
            {
                UserRegComplete("异常：" + ex.Message);
                return;
            }
        }
        #endregion

        /// <summary>
        /// 停止操作
        /// </summary>
        public void StopProgressing()
        {
            wc.CancelAsync();
        }

        #region 用户登陆部分 
        /// <summary>
        /// 用户登录实现基类（返回编写好的数据）
        /// </summary>
        /// <param name="_Email">邮箱</param>
        /// <param name="_Password">密码</param>
        /// <param name="_imgCode">验证码</param>
        /// <param name="Always">是否保持</param>
        public string UserLogin(string _Email,string _Password)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Email=" + _Email.Replace("=", "&Equal;") + "\n");
            sb.Append("Password=" + _Password.Replace("=", "&Equal;") + "\n");
            byte[] buffer = Encoding.Unicode.GetBytes(sb.ToString());
            sb = null;
            //适应加密模块
            MemoryStream bm = new MemoryStream(buffer);
            int leng = 0, ii = 0;
            leng = buffer.Length;
            MemoryStream m = new MemoryStream();
            while (ii < leng)
            {
                if (leng - ii >= 110)
                {
                    byte[] b = new byte[110];
                    bm.Read(b, 0, 110);
                    m.Write(RSA.Encrypt(b, false), 0, 128);
                    ii += 110;
                }
                else
                {
                    byte[] b = new byte[leng - ii];
                    bm.Read(b, 0, leng - ii);
                    m.Write(RSA.Encrypt(b, false), 0, 128);
                    ii = leng;
                }
            }
            m.Position = 0;
            byte[] bb = new byte[(int)m.Length];
            m.Read(bb, 0, (int)m.Length);
            return Convert.ToBase64String(bb);
        }
        #endregion

        #region 试题包索引导入部分
        public void DownKrPkIndex(string PkName, string HardCode,string SessionID)
        {
            wc = new WebClient();
            wc.Proxy = null;
            wc.Headers[HttpRequestHeader.UserAgent] = "KrSpoken.Net/1.0 (" + Environment.OSVersion.VersionString + ")[HardCode=" + HardCode + "]";
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
            wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_DownloadProgressChanged);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string a=KrServerHost + "C_PID.aspx?SessionID=" + SessionID + "&PackName=" + BitConverter.ToString(md5.ComputeHash(Encoding.Unicode.GetBytes(PkName))) +"&Sec="+Convert.ToString((new Random()).NextDouble());
            wc.DownloadStringAsync(new Uri(a));
        }
        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownPkIdxProgress(e.ProgressPercentage);
        }
        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    DownPkIdxComplete("用户取消","","");
                    return;
                }
                switch (e.Result)
                {
                    case "Error-0x00000000":
                        DownPkIdxComplete("会话ID错误！", "", "");
                        break;
                    case "Error-0x00000001":
                        DownPkIdxComplete("要求试题包名称！", "", "");
                        break;
                    case "Error-0x00000002":
                        DownPkIdxComplete("不存在此试题包！", "", "");
                        break;
                    case "Error-0x00000003":
                        DownPkIdxComplete("数据库访问错误！", "", "");
                        break;
                    case "Error-0x00000004":
                        DownPkIdxComplete("登陆信息有误！", "", "");
                        break;
                    case "Error-0x00000005":
                        DownPkIdxComplete("您没有注册！请先注册软件。", "", "");
                        break;
                    case "Error-0x00000006":
                        DownPkIdxComplete("数据输出错误！", "", "");
                        break;
                    default:
                        if (e.Result.IndexOf("Success") >= 0)
                        {
                            string[] tmp = e.Result.Split(new string[]{"\n"}, StringSplitOptions.RemoveEmptyEntries);
                            DownPkIdxComplete("下载完成", tmp[1], tmp[2]);
                        }
                        else
                        {
                            DownPkIdxComplete(e.Result, "","");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                DownPkIdxComplete("异常：" + ex.Message, "", "");
                return;
            }
        }
        #endregion

        #region 用户信息修改部分
        /// <summary>
        /// 用户信息修改
        /// </summary>
        public void UserInfoUpdate(string _oldPass,string _Pass, string _NiName,string _dClass, string _dYear, string _Phone,string SessionID,NetUserData Nu,string HardCode)
        {
            StringBuilder sb = new StringBuilder();
            if (_NiName == "") { _NiName = "&null;"; }
            if (_Phone == "") { _Phone = "&null;"; }
            if (_oldPass == "") { _oldPass = "&null;"; }
            if (_Pass == "") { _Pass = "&null;"; }
            sb.Append("OldPassword=" + _oldPass.Replace("=", "&Equal;") + "\n");
            sb.Append("Password=" + _Pass.Replace("=", "&Equal;") + "\n");
            sb.Append("NiName=" + _NiName.Replace("=", "&Equal;") + "\n");
            sb.Append("DS_Class=" + _dClass.Replace("=", "&Equal;") + "\n");
            sb.Append("DS_Year=" + _dYear.Replace("=", "&Equal;") + "\n");
            sb.Append("Phone=" + _Phone.Replace("=", "&Equal;"));
            Nu.NiName = _NiName;
            Nu.DS_Class = _dClass;
            Nu.DS_Year = _dYear;
            Nu.Phone = _Phone;
            NetUser_Tmp = Nu;
            byte[] buffer = Encoding.Unicode.GetBytes(sb.ToString());
            sb = null;
            //适应加密模块
            MemoryStream bm = new MemoryStream(buffer);
            int leng = 0, ii = 0;
            leng = buffer.Length;
            MemoryStream m = new MemoryStream();
            while (ii < leng)
            {
                if (leng - ii >= 110)
                {
                    byte[] b = new byte[110];
                    bm.Read(b, 0, 110);
                    m.Write(RSA.Encrypt(b, false), 0, 128);
                    ii += 110;
                }
                else
                {
                    byte[] b = new byte[leng - ii];
                    bm.Read(b, 0, leng - ii);
                    m.Write(RSA.Encrypt(b, false), 0, 128);
                    ii = leng;
                }

            }
            m.Position = 0;
            byte[] bb = new byte[(int)m.Length];
            m.Read(bb, 0, (int)m.Length);
            wc = null;
            wc = new WebClient();
            wc.Proxy = null;
            wc.Headers[HttpRequestHeader.UserAgent] = "KrSpoken.Net/1.0 (" + Environment.OSVersion.VersionString + ")[HardCode=" + HardCode + "]";
            wc.UploadStringCompleted += new UploadStringCompletedEventHandler(wc_UploadStringCompleted_UserInfo);
            wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            wc.UploadStringAsync(new Uri(KrServerHost + "/C_ChangeInfo.aspx?SessionID=" + SessionID + "&sec=" + Convert.ToString((new Random()).NextDouble())), "post", "Data=" + System.Web.HttpUtility.HtmlEncode(Convert.ToBase64String(bb)));
        }

        public void wc_UploadStringCompleted_UserInfo(object sender, UploadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    ChangeInfoComplete("用户取消",null);
                    return;
                }
                switch (e.Result)
                {
                    case "Error+0x00000000":
                        ChangeInfoComplete("会话ID错误！", null);
                        break;
                    case "Error-0x00000000":
                        ChangeInfoComplete("服务器发生错误！", null);
                        break;
                    case "Error-0x00000001":
                        ChangeInfoComplete("服务器发生错误！", null);
                        break;
                    case "Error-0x00000002":
                        ChangeInfoComplete("服务器发生错误！", null);
                        break;
                    case "Error-0x00000003":
                        ChangeInfoComplete("如果您要修改密码，请输入新的密码！", null);
                        break;
                    case "Error-0x00000004":
                        ChangeInfoComplete("登陆信息错误！", null);
                        break;
                    case "Error-0x00000005":
                        ChangeInfoComplete("服务器的数据库发生错误！", null);
                        break;
                    case "Error-0x00000006":
                        ChangeInfoComplete("旧密码校验错误！", null);
                        break;
                    case "Success":
                        ChangeInfoComplete("修改成功！", NetUser_Tmp);
                        break;
                    default:
                        ChangeInfoComplete(e.Result, null);
                        break;
                }
            }
            catch (Exception ex)
            {
                ChangeInfoComplete("异常：" + ex.Message, null);
                return;
            }
        }
        #endregion

        #region 用户登出
        public string UserLogout(string Session_ID, string HardCode)
        {
            wc = null;
            wc = new WebClient();
            wc.Proxy = null;
            wc.Headers[HttpRequestHeader.UserAgent] = "KrSpoken.Net/1.0 (" + Environment.OSVersion.VersionString + ")[HardCode=" + HardCode + "]";
           return wc.UploadString(new Uri(KrServerHost + "/C_Logout.aspx?SessionID=" + Session_ID + "&sec=" + Convert.ToString((new Random()).NextDouble())),"");
        }
        #endregion
    }

   
}
