using System;
using System.Threading;
using System.Text;
using System.Collections;

namespace 线程同步锁定测试
{
    class Program
    {
         Hashtable aaa = new Hashtable();
         private int l = 0;
         Random r = new Random();
        
        static void Main(string[] args)
        {
            Program b = new Program();
            b.aa();
            Console.ReadKey();
        }
         public void aa()
         {
             for (int i = 0; i < 100; i++) aaa[i] = i;
             Thread t1 = new Thread(a);
             Thread t2 = new Thread(a);
             t1.IsBackground = true;
             t1.Name = "One";
             t1.Start();         
             t2.IsBackground = true;
             t2.Name = "Two";
             t2.Start();
           //t1.Join();
            // t2.Join();
         }
        public  void a()
        {
            while (l < 99)
            {

                Thread.Sleep(r.Next(10, 50));
                lock (this)
                {
                    Console.WriteLine("Thread:{0};Value:{1}", Thread.CurrentThread.Name, aaa[l]);
                    l++;
                }
            }
        }
    }
}
