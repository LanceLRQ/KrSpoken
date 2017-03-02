using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace KrSpoken.Profile
{
    #region 用户数据储存类
    /// <summary>
    /// 网络用户数据
    /// </summary>
    [Serializable()]
    public class NetUserData
    {
        private string _ID = "", _Email = "", _NiName = "";
        private string _RegCode = "", _HardCode = "", _CheckCode = "";
        private string _RSAPrivateKey = "", _RSAPublicKey = "", _Phone = "";
        private string _DS_Name = "", _DS_Class = "", _DS_Year = "", _Login_Session = "";

        public string ID { get { return _ID; } set { _ID = value; } }
        public string Email { get { return _Email; } set { _Email = value; } }
        public string NiName { get { return _NiName; } set { _NiName = value; } }
        public string RegCode { get { return _RegCode; } set { _RegCode = value; } }
        public string HardCode { get { return _HardCode; } set { _HardCode = value; } }
        public string CheckCode { get { return _CheckCode; } set { _CheckCode = value; } }
        public string RSAPrivateKey { get { return _RSAPrivateKey; } set { _RSAPrivateKey = value; } }
        public string Phone { get { return _Phone; } set { _Phone = value; } }
        public string DS_Name { get { return _DS_Name; } set { _DS_Name = value; } }
        public string DS_Class { get { return _DS_Class; } set { _DS_Class = value; } }
        public string DS_Year { get { return _DS_Year; } set { _DS_Year = value; } }
        public string Login_Session { get { return _Login_Session; } set { _Login_Session = value; } }
    }

    /// <summary>
    /// 网络用户数据配置模块
    /// </summary>
    public class NetUserDataRuntime
    {
        /// <summary>
        /// 载入配置文件
        /// </summary>
        /// <param name="_getNetUserDatafile">配置文件位置</param>
        /// <param name="getNetUserData">网络用户数据</param>
        public static void Infile(string _getNetUserDatafile, out NetUserData getNetUserData)
        {
            try
            {
                if (File.Exists(_getNetUserDatafile) == false)
                {
                    using (FileStream fs = File.Create(_getNetUserDatafile)) { fs.Close(); }
                    getNetUserData = new NetUserData();
                    return;
                }
                using (FileStream fs = new FileStream(_getNetUserDatafile, FileMode.Open, FileAccess.Read))
                {
                    if (fs.Length > 0)
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        getNetUserData = (NetUserData)bf.Deserialize(fs);
                    }
                    else
                    {
                        getNetUserData = new NetUserData();
                    }
                }
            }
            catch
            {
                throw new Exception("载入网络用户数据错误！");
            }
        }
        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="_getNetUserDatafile">配置文件位置</param>
        /// <param name="getNetUserData">网络用户数据</param>
        public static void Outfile(string _getNetUserDatafile, NetUserData getNetUserData)
        {
            try
            {
                using (FileStream fs = new FileStream(_getNetUserDatafile, FileMode.Create, FileAccess.Write))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, getNetUserData);
                    fs.Close();
                }
            }
            catch
            {
                throw new Exception("输出网络用户数据错误！");
            }
        }
    }

    #endregion
}
