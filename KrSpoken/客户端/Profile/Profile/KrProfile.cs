using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace KrSpoken.Profile
{

    /// <summary>
    /// 程序配置运行模块
    /// </summary>
    public class AppProfileRuntime
    {
        /// <summary>
        /// 载入配置文件
        /// </summary>
        /// <param name="_AppProfile">配置文件位置</param>
        /// <param name="getAppProfile">应用程序配置数据</param>
        public static void Infile(string _AppProfile, out AppProfileClass getAppProfile)
        {
            try
            {
                if (File.Exists(_AppProfile) == false)
                {
                    using (FileStream fs = File.Create(_AppProfile)) { fs.Close(); }
                    getAppProfile = new AppProfileClass();
                    return;
                }
                using (FileStream fs = new FileStream(_AppProfile, FileMode.Open, FileAccess.Read))
                {
                    if (fs.Length > 0)
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        getAppProfile = (AppProfileClass)bf.Deserialize(fs);
                    }
                    else
                    {
                        getAppProfile = new AppProfileClass();
                    }
                }
            }
            catch
            {
                throw new Exception("载入应用程序配置数据错误！");
            }
        }
        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="_AppProfile">输出的应用程序配置数据文件</param>
        /// <param name="getAppProfile">应用程序配置数据</param>
        public static void Outfile(string _AppProfile, AppProfileClass getAppProfile)
        {
            try
            {
                using (FileStream fs = new FileStream(_AppProfile, FileMode.Create, FileAccess.Write))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, getAppProfile);
                    fs.Close();
                }
            }
            catch
            {
                throw new Exception("输出应用程序配置数据错误！");
            }
        }
    }

    /// <summary>
    /// 程序配置模块
    /// </summary>
        [Serializable()]
        public class AppProfileClass
        {
            private  string _KrProfileDir = "D:\\KrSpoken";
            private  string _FrmMain_BackGround="";
            private string _RegCode = "";
            private  string _CheckCode = "";
            private string _RSAPrivateCode = "";
            private  bool _IsRegistered = false;
            private string _KrServer = "http://97.74.58.160/krspoken/krserver/";
            
            /// <summary>
            /// 程序主用户文件夹
            /// </summary>
            public  string KrProfileDir { get { return _KrProfileDir; } set { _KrProfileDir = value; } }
            /// <summary>
            /// 应用程序名称
            /// </summary>
            public  string AppName { get { return "听雨口语"; } }
            /// <summary>
            /// 应用程序标题
            /// </summary>
            public string Title { get { return "听雨口语2012(内测) - 广东版 - 网络版"; } }
            /// <summary>
            /// 试题包文件存放
            /// </summary>
            public  string PackDir
            {
                get
                {
                    if (Directory.Exists(KrProfileDir + "\\KrPack") == false)
                        Directory.CreateDirectory(KrProfileDir + "\\KrPack");
                    return KrProfileDir + "\\KrPack";
                }
            }
            /// <summary>
            /// 试题包列表文件
            /// </summary>
            public  string PkLibfile{
                get
                {
                    if (Directory.Exists(KrProfileDir + "\\Profile") == false)
                        Directory.CreateDirectory(KrProfileDir + "\\Profile");
                    return KrProfileDir + "\\Profile\\PkLibFile.profile";
                }
            }
            /// <summary>
            /// 回答所用的Dir
            /// </summary>
            public  string StudentAnswerDir
            {
                get
                {
                    if (Directory.Exists(KrProfileDir + "\\学生答案") == false)
                        Directory.CreateDirectory(KrProfileDir + "\\学生答案");
                    return KrProfileDir +"\\学生答案";
                }
            }
            /// <summary>
            /// 参考答案所用的Dir
            /// </summary>
            public string ComputerAnswerDir
            {
                get
                {
                    if (Directory.Exists(KrProfileDir + "\\参考答案") == false)
                        Directory.CreateDirectory(KrProfileDir + "\\参考答案");
                    return KrProfileDir + "\\参考答案";
                }
            }
            /// <summary>
            /// 背景图片的文件夹
            /// </summary>
            public  string BackGroundDir
            {
                get
                {
                    if (Directory.Exists(KrProfileDir + "\\BackGround") == false)
                        Directory.CreateDirectory(KrProfileDir + "\\BackGround");
                    return KrProfileDir + "\\BackGround";
                }
            }
            /// <summary>
            /// FrmMain用的背景
            /// </summary>
            public  string FrmMain_BackGround { get { return _FrmMain_BackGround; } set { _FrmMain_BackGround = value; } }
            /// <summary>
            /// 本地单词数据库
            /// </summary>
            public  string WordCaptureDatabasePath
            {
                get
                {
                    return KrProfileDir + "\\WordLib\\EnToCn.krdb";
                }
            }
            /// <summary>
            /// 用户数据文件储存位置
            /// </summary>
            public  string LocalApplicationData { get { return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); } }
            /// <summary>
            /// 用户注册码
            /// </summary>
            public  string RegCode { get { return _RegCode; } set { _RegCode = value; } }
            /// <summary>
            /// 注册码校验
            /// </summary>
            public  string CheckCode { get { return _CheckCode; } set { _CheckCode = value; } }
            /// <summary>
            /// RSA解密私钥
            /// </summary>
            public  string RSAPrivateCode { get { return _RSAPrivateCode; } set { _RSAPrivateCode = value; } }
            /// <summary>
            /// 判断用户是否注册 
            /// </summary>
            public  bool IsRegistered { get { return _IsRegistered; } set { _IsRegistered = value; } }
            /// <summary>
            /// 软件的服务器
            /// </summary>
            public string GetKrServer { get { return _KrServer; } set { _KrServer = value; } }
        }
}