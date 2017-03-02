using System;
using System.IO;
using System.Text;
using Lyoko_KR_PkMod;
using System.Security.Cryptography;

namespace KrSpoken
{
    /// <summary>
    /// 实现标准试题包的解密
    /// </summary>
    public class FileDecode
    {
        /// <summary>
        /// 进行解密
        /// </summary>
        /// <param name="RegCode">注册码</param>
        /// <param name="HardCode">硬件码</param>
        /// <param name="CheckCode">验证码</param>
        /// <param name="RSAPrivateCode">RSA私钥</param>
        /// <param name="BeDecodeText">被解码的内容</param>
        /// <returns></returns>
        public static Stream RunFileDecode(string RegCode, string HardCode, string CheckCode, string RSAPrivateCode, string BeDecodeText)
        {
            try
            {
                PKModClass PKMod = new PKModClass();
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.ImportCspBlob(Convert.FromBase64String(RSAPrivateCode));
                byte[] buffer = Convert.FromBase64String(PKMod.OutputPK(RegCode, HardCode, CheckCode, BeDecodeText));
                //适应加密模块
                MemoryStream bm = new MemoryStream(buffer);
                int leng = 0, ii = 0;
                leng = buffer.Length;
                MemoryStream m = new MemoryStream();
                while (ii < leng)
                {
                    if (leng - ii >= 128)
                    {
                        byte[] b = new byte[128];
                        bm.Read(b, 0, 128);
                        byte[] tmp = rsa.Decrypt(b, false);
                        m.Write(tmp, 0, tmp.Length);
                        ii += 128;
                    }
                }
                buffer = null;
                m.Position = 0;
                bm.Close();
                bm = null;
                return m;
            }
            catch
            {
                throw new Exception("试题包解密失败！！请重试。");
            }
        }
       /// <summary>
       /// 获取机器码
       /// </summary>
       /// <returns></returns>
        public static string GetHardCode()
        {
            PKModClass PKModel = new PKModClass();
            return PKModel.GetHardCode(); 
        }
    }
}

