using System;
using System.Text;
using KrSpoken.KrPacker;

namespace CMDRuntime
{
    class Program
    {
        static void Main(string[] args)
        {
          FileInBox a=new FileInBox();
         // FileOutBox b = new FileOutBox();
          Console.WriteLine(a.Init(@"d:\1", @"d:\2.spi", @"d:\2.spk"));
                a.ReadFileToMemory();
             // Console.WriteLine(a.Length);
            //  Console.WriteLine(b.Init(@"d:\NewFile.krpk", @"d:\NewFile.kridx"));
              //  Console.WriteLine(b.GetFileText("",Encoding.Default));
             //  b.SaveToFile("partb.wmv", "d:\\abc.wmv");
            
             //   Console.WriteLine("OK");
             //   Console.ReadKey();
            //    Console.WriteLine("释放内存");
             //   b = null;
           //     GC.Collect();
          //  Console.ReadKey();

         // //  MixThePackUp mk = new MixThePackUp();
         //  mk.Init("d:\\NewFile.krpk", "D:\\NewFile.KrMxIdx", "d:\\MxPK.krpk");
         //  mk.OutTheFile();

           // ReBuiltThePack rp = new ReBuiltThePack();
          //  rp.Init("d:\\NewFile1.krpk", "D:\\NewFile.KrMxIdx", "d:\\MxPK.krpk");
           // rp.OutTheFile();

         //  Console.Write(Convert.ToString(int.MaxValue));
          //  Console.Read();
        }
    }
}
