using System;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Text;

namespace EnDicToMdb
{
    class Program
    {
        static void Main(string[] args)
        {
            OleDbConnection conn = null;
            try
            {
                conn=new OleDbConnection(@"Provider=Microsoft.Jet.Oledb.4.0;Data Source=d:\EnToCn.mdb;");
                conn.Open();
                OleDbCommand mycomm ;

                StreamReader sr = new StreamReader("d:\\英语单词库.txt", Encoding.Unicode);
                int l=0,c=0;
                string tmp="";
                string[] mydata=sr.ReadToEnd().Split(new string[]{"\r\n"},StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < mydata.Length; i++)
                {
                    tmp=mydata[i];
                    l =tmp.IndexOf('[');
                    c = tmp.IndexOf(']');
                    mycomm = conn.CreateCommand();
                    if (i == 52)
                    {
                        var a = 1;
                    }
                    mycomm.CommandText = "INSERT INTO [Dic] (Word,Pronun,Chinese) VALUES ('" + tmp.Substring(0, l).Replace("'", "’") + "','" + tmp.Substring(l + 1, c - l - 1).Replace("'", "’") + "','" + tmp.Substring(c + 1, tmp.Length - c - 1).Replace("'", "’") + "')";
                    mycomm.ExecuteScalar();
                }
            }
            catch
            {
                //throw new Exception(ex.Message);
            }
        }
    }
}
