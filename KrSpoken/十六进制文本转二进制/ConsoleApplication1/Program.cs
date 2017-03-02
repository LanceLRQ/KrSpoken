using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            System.IO.StreamReader sr = new System.IO.StreamReader("d:\\aa.txt", System.Text.Encoding.Default);
            string str = sr.ReadToEnd();
            byte[] by = new byte[str.Length / 2];
            for (int i = 0; i < by.Length; i++)
            {
                string s = str.Substring(i * 2, 2);
                by[i] = (byte)Convert.ToInt32(s, 16);
            }
            System.IO.FileStream fs = new System.IO.FileStream("d:\\aaa.jpg", System.IO.FileMode.Create);
            fs.Write(by, 0, by.Length);
            fs.Close();
        }
    }
}
