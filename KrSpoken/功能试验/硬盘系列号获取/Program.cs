using System;
using System.Text;
using System.Runtime.InteropServices;

namespace 硬盘系列号获取
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(GetDiskID());
            Console.Read(); 
        }

        [DllImport("DiskID32.dll")]
        public static extern long DiskID32(ref byte DiskModel, ref byte DiskID);

            public static string GetDiskID()
            {

                byte[] DiskModel = new byte[31];
                byte[] DiskID = new byte[31];
                int i;
                //string Model = "";
                string ID = "";

                if (DiskID32(ref DiskModel[0], ref DiskID[0]) != 1)
                {

                    for (i = 0; i < 31; i++)
                    {

                        if (Convert.ToChar(DiskID[i]) != Convert.ToChar(0))
                        {
                            ID = ID + Convert.ToChar(DiskID[i]);
                        }
                    }
                    ID = ID.Trim();
                }
                else
                {
                    Console.WriteLine("获取硬盘序列号出错");
                }
                return ID;
            }
    }
}
