using System;
using KrSpoken.Downloader115;

namespace _115Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("正在获取列表文件...");
            FileDownload115 fd115 = new FileDownload115("aqpousjt", KR_NetworkType.CnUnicom, "D:\\Down", "D:\\Down\\Cache", "My.rar", 5, 1);
            Console.WriteLine("成功");
            Console.WriteLine("开始下载...");
            fd115.CreateDown();
            Console.Read();
        }

    }
}
