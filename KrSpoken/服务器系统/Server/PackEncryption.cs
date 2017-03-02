using System;
using System.Security.Cryptography;
using System.Text;
using Lyoko_KR_PkMod;

namespace KrSpoken.Server
{
    /// <summary>
    /// 用户注册码
    /// </summary>
    public class UserCodeManager
    {
        /// <summary>
        /// 获取一个新的注册码和机器码
        /// </summary>
        /// <param name="UID">Exp:KrSpoken_User_1</param>
        /// <returns></returns>
        public static string[] GetNewInfo(string UID)
        {
            UID = "KrSpoken_User_" + UID;
            string[] u = new string[2];
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            u[0] = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(UID))).Replace("-","");
            PKModClass P = new PKModClass();
            u[1] = P.GetHardCode();
            return u;
        }
        /// <summary>
        /// 获取校验码 
        /// </summary>
        /// <returns></returns>
        public static string GetNewCheckCode(string _RegCode,string Hard_Code)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            return BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes("<KRSpkoen_User_RegCode ?==>" + _RegCode + "</==?><KRSpkoen_User_HardCode ?==>" + Hard_Code + "</==?>"))).Replace("-", "");
        }
    }

  

        /// <summary>
    /// Base64编码类。
    /// 将byte[]类型转换成Base64编码的string类型。
    /// </summary>
        public class Base64Encoder
    {
        byte[] source;
        int length, length2;
        int blockCount;
        int paddingCount;
        public char[] EncodeChr=new char[64];

        public Base64Encoder(string CheckCode)
        {
           string base64chr_1 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
            string CharSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            CheckCode = CheckCode.ToUpper();
            for (int i = 0; i < 9; i++)
            {
                for (int l = 0; l < 32; l++)
                {
                    string a = CheckCode.Substring(l, 1);
                    int b=CharSet.IndexOf(a);
                    string tmp = base64chr_1.Substring(b, 1);
                    base64chr_1 = base64chr_1.Replace(tmp, "") + tmp;
                }
            }
            for (int i = 0; i < 64; i++)
            {
                EncodeChr[i] =Convert.ToChar(base64chr_1.Substring(i, 1));
            }
        }

        private void init(byte[] input)
        {
            source = input;
            length = input.Length;
            if ((length % 3) == 0)
            {
                paddingCount = 0;
                blockCount = length / 3;
            }
            else
            {
                paddingCount = 3 - (length % 3);
                blockCount = (length + paddingCount) / 3;
            }
            length2 = length + paddingCount;
        }

        public string GetEncoded(byte[] input)
        {
            //初始化
            init(input);

            byte[] source2;
            source2 = new byte[length2];

            for (int x = 0; x < length2; x++)
            {
                if (x < length)
                {
                    source2[x] = source[x];
                }
                else
                {
                    source2[x] = 0;
                }
            }

            byte b1, b2, b3;
            byte temp, temp1, temp2, temp3, temp4;
            byte[] buffer = new byte[blockCount * 4];
            char[] result = new char[blockCount * 4];
            for (int x = 0; x < blockCount; x++)
            {
                b1 = source2[x * 3];
                b2 = source2[x * 3 + 1];
                b3 = source2[x * 3 + 2];

                temp1 = (byte)((b1 & 252) >> 2);

                temp = (byte)((b1 & 3) << 4);
                temp2 = (byte)((b2 & 240) >> 4);
                temp2 += temp;

                temp = (byte)((b2 & 15) << 2);
                temp3 = (byte)((b3 & 192) >> 6);
                temp3 += temp;

                temp4 = (byte)(b3 & 63);

                buffer[x * 4] = temp1;
                buffer[x * 4 + 1] = temp2;
                buffer[x * 4 + 2] = temp3;
                buffer[x * 4 + 3] = temp4;

            }

            for (int x = 0; x < blockCount * 4; x++)
            {
                result[x] = sixbit2char(buffer[x]);
            }


            switch (paddingCount)
            {
                case 0: break;
                case 1: result[blockCount * 4 - 1] = '='; break;
                case 2: result[blockCount * 4 - 1] = '=';
                    result[blockCount * 4 - 2] = '=';
                    break;
                default: break;
            }
            return new string(result);
        }
        private char sixbit2char(byte b)
        {
            if ((b >= 0) && (b <= 63))
            {
                return EncodeChr[(int)b];
            }
            else
            {

                return ' ';
            }
        }
    }
        /// <summary>
        /// Base64解码类
        /// 将Base64编码的string类型转换成byte[]类型
        /// </summary>
        public class Base64Decoder
        {
            char[] source;
            int length, length2, length3;
            int blockCount;
            int paddingCount;
            public char[] EncodeChr = new char[64];

            public Base64Decoder(string CheckCode)
            {
                string base64chr_1 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
                string CharSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                CheckCode = CheckCode.ToUpper();
                for (int i = 0; i < 9; i++)
                {
                    for (int l = 0; l < 32; l++)
                    {
                        string a = CheckCode.Substring(l, 1);
                        int b = CharSet.IndexOf(a);
                        string tmp = base64chr_1.Substring(b, 1);
                        base64chr_1 = base64chr_1.Replace(tmp, "") + tmp;
                    }
                }
                for (int i = 0; i < 64; i++)
                {
                    EncodeChr[i] = Convert.ToChar(base64chr_1.Substring(i, 1));
                }
            }

            private void init(char[] input)
            {
                int temp = 0;
                source = input;
                length = input.Length;

                for (int x = 0; x < 2; x++)
                {
                    if (input[length - x - 1] == '=')
                        temp++;
                }
                paddingCount = temp;

                blockCount = length / 4;
                length2 = blockCount * 3;
            }

            public byte[] GetDecoded(string strInput)
            {
                //初始化
                init(strInput.ToCharArray());

                byte[] buffer = new byte[length];
                byte[] buffer2 = new byte[length2];

                for (int x = 0; x < length; x++)
                {
                    buffer[x] = char2sixbit(source[x]);
                }

                byte b, b1, b2, b3;
                byte temp1, temp2, temp3, temp4;

                for (int x = 0; x < blockCount; x++)
                {
                    temp1 = buffer[x * 4];
                    temp2 = buffer[x * 4 + 1];
                    temp3 = buffer[x * 4 + 2];
                    temp4 = buffer[x * 4 + 3];

                    b = (byte)(temp1 << 2);
                    b1 = (byte)((temp2 & 48) >> 4);
                    b1 += b;

                    b = (byte)((temp2 & 15) << 4);
                    b2 = (byte)((temp3 & 60) >> 2);
                    b2 += b;

                    b = (byte)((temp3 & 3) << 6);
                    b3 = temp4;
                    b3 += b;

                    buffer2[x * 3] = b1;
                    buffer2[x * 3 + 1] = b2;
                    buffer2[x * 3 + 2] = b3;
                }

                length3 = length2 - paddingCount;
                byte[] result = new byte[length3];

                for (int x = 0; x < length3; x++)
                {
                    result[x] = buffer2[x];
                }

                return result;
            }

            private byte char2sixbit(char c)
            {
                if (c == '=')
                    return 0;
                else
                {
                    for (int x = 0; x < 64; x++)
                    {
                        if (EncodeChr[x] == c)
                            return (byte)x;
                    }

                    return 0;
                }

            }
        }
        //解码类结束
}
