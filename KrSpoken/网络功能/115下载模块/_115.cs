using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Threading;
using System.Collections;
using System.Diagnostics;

namespace KrSpoken.Downloader115
{
    public enum KR_NetworkType : int
    {
        /// <summary>
        /// 中国电信
        /// </summary>
        CnTelecom=0,
        /// <summary>
        /// 中国联通
        /// </summary>
        CnUnicom = 1,
        /// <summary>
        /// Godaddy服务器
        /// </summary>
        UaGodaddy=2
    }

    /// <summary>
    /// 所有下载完成的回调托管
    /// </summary>
    public delegate void FD115_Down_Finish();
    /// <summary>
    /// 文件拼接完成的回调托管
    /// </summary>
    public delegate void FD115_Down_MakeFile();

    /// <summary>
    /// 对115网盘的文件进行下载
    /// </summary>
    public class FileDownload115
    {
        private static ReaderWriterLock m_readerWriterLock = new ReaderWriterLock();
        private string GetDownUrl="";
        private string TargetDir = "", CacheDir = "", SaveName = "";
        private int p = 1;
        private bool CanDown = true;
        private Thread[] MyThread ;
        private Thread FinishThread;
        /// <summary>
        /// 下载过程发生错误的事件
        /// </summary>
        public event FD115_Down_MakeFile MakeFile;
        /// <summary>
        /// 所有下载完成的事件
        /// </summary>
        public event FD115_Down_Finish Finish;

        //线程数量
        private int tCount = 2;
        //线程下载完成指示
        private bool[] tDownFinish;
        /// <summary>
        /// 下载区块大小
        /// </summary>
        public int PartLength { get; set; }
        //各个线程当前的下载值
        private int[] tNowLength;
        //各个线程当前总的下载值
        private int[] tNowLengthTotal;
        /// <summary>
        /// 下载总长度
        /// </summary>
        public int DownLengthTotal { get; set; }
        /// <summary>
        /// 下载百分比
        /// </summary>
        public int DownLengthPercent() { return ((DownLengthNow() / DownLengthTotal) * 100); }
        /// <summary>
        /// 当前下载总量
        /// </summary>
        public int DownLengthNow(){ 
            int Out=0;
            for (int ii = 1; ii <= tCount; ii++)
            {
                Out += tNowLengthTotal[ii];
            }
            return Out;
        }
        /// <summary>
        /// 分区表
        /// </summary>
        public DownloaderPartData PartData=new DownloaderPartData();
        
        /// <summary>
        /// 对115网盘的文件进行下载
        /// </summary>
        /// <param name="fileCode">文件提取码</param>
        /// <param name="NetWorkType">网络提供商选择</param>
        /// <param name="TargetDir">目标路径</param>
        /// <param name="CacheDir">缓存路径</param>
        /// <param name="SaveName">文件名称(需带扩展名)</param>
        /// <param name="_tCount">线程数量</param>
        /// <param name="CacheLen">缓存大小[单位：MB]</param>
        public FileDownload115(string fileCode, KR_NetworkType NetWorkType, string _TargetDir, string _CacheDir, string _SaveName, int _tCount,int CacheLen)
        {
            try
            {
                System.Net.ServicePointManager.DefaultConnectionLimit = 128;
                PartLength = CacheLen * 1024 * 1024;// 缓存大小

                string getPageData = "", getDownList = "";
                HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create("http://115.com/file/" + fileCode);
                hwr.AllowAutoRedirect = true;
                hwr.Timeout = 5000;
                hwr.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.8 (KHTML, like Gecko) Chrome/17.0.942.0 Safari/535.8";
                HttpWebResponse hwq = (HttpWebResponse)hwr.GetResponse();
                using (StreamReader reader = new StreamReader(hwq.GetResponseStream(), Encoding.UTF8))
                {
                    getPageData = reader.ReadToEnd();
                }
                if (getPageData.IndexOf("提取码不存在") > 0)
                {
                    throw new Exception("文件不存在！");
                }
                getPageData = getPageData.Replace(" ", "").Replace("   ", "");
                string ReadJSDownUrl = "";
                Regex reg = new Regex(@"url\:(.[^\\]*)\/\?ct\=(.[^\,]*)\,");
                ReadJSDownUrl = reg.Match(getPageData).Value;
                ReadJSDownUrl = ReadJSDownUrl.Replace("url", "").Replace("'", "").Replace("\"","").Replace(",","").Replace(":","").Replace(" ","");
                hwr = null;
                hwr = (HttpWebRequest)WebRequest.Create("http://115.com" + ReadJSDownUrl);
                hwr.AllowAutoRedirect = true;
                hwr.Timeout = 5000;
                hwr.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.8 (KHTML, like Gecko) Chrome/17.0.942.0 Safari/535.8";
                hwq = null;
                hwq = (HttpWebResponse)hwr.GetResponse();
                using (StreamReader reader = new StreamReader(hwq.GetResponseStream(), Encoding.UTF8))
                {
                    getDownList = reader.ReadToEnd();
                }
                switch (NetWorkType)
                {
                    case KR_NetworkType.CnTelecom:
                        reg = null;
                        reg = new Regex("\\{\\\"client\\\"\\:(.*)\\\"desc\\\"\\:\\\"\\\\u7535\\\\u4fe1\\\"(.[^\\}]*)\\}");
                        break;
                    case KR_NetworkType.CnUnicom:
                        reg = null;
                        reg = new Regex("\\{\\\"client\\\"\\:(.*)\\\"desc\\\"\\:\\\"\\\\u8054\\\\u901a\\\"(.[^\\}]*)\\}");
                        break;
                }
                GetDownUrl = reg.Match(getDownList).Value.Replace("\"", "");
                reg = null;
                reg = new Regex(@"url\:(.[^\,]*)\,");
                GetDownUrl =reg.Match(GetDownUrl).Value.Replace("url:", "").Replace(",", "").Replace("\\", "");

                TargetDir = _TargetDir;
                SaveName = _SaveName;
                CacheDir = _CacheDir;
                if (_tCount < 1 || _tCount > 5)
                {
                    tCount = 2;
                }
                tCount = _tCount;
                tNowLength = new int[tCount];
                tNowLengthTotal = new int[tCount];
                tDownFinish = new bool[tCount];
            }
            catch (Exception ex)
            {
                throw new Exception("发生错误："+ex.Message);
            }

        }

        //生成区块文件
        private void MadePart()
        {
            try
            {
                HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(GetDownUrl);
                hwr.Method = "HEAD";
                hwr.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.8 (KHTML, like Gecko) Chrome/17.0.942.0 Safari/535.8";
                hwr.Timeout = 5000;
                DownLengthTotal = (int)hwr.GetResponse().ContentLength;
                hwr.Abort();
                int Position = 0;
                while (Position < DownLengthTotal)
                {
                    if (DownLengthTotal - Position >= PartLength)
                    {
                        PartData.AddPart(PartLength, Position, Position + PartLength, CacheDir + "\\" + SaveName + "_" + Convert.ToString(PartData.Index) + ".part");
                        Position += PartLength + 1;
                    }
                    else
                    {
                        PartData.AddPart((DownLengthTotal - Position), Position, DownLengthTotal, CacheDir + "\\" + SaveName + "_" + Convert.ToString(PartData.Index) + ".part");
                        Position = DownLengthTotal;
                    }
                }
            }
            catch {
                throw new Exception("文件分块发生错误！");
            }
        }

        //为线程提供下载类  data=线程编号
        private void DownloadfileThread(object data)
        {
            
            Random r = new Random();
            int t = (int)data;
            while(true){
                m_readerWriterLock.AcquireReaderLock(1000);
                int pp = p;
                m_readerWriterLock.ReleaseReaderLock();
                    int PriNowDown = 0;
                    Thread.Sleep(50);
                    try
                    {
                        int b = 0;
                        //判断是否都下载完成
                        for (short i = 1; i < PartData.Index; i++)
                        {
                            if (PartData.State(i) == 2) { b++; }
                        }
                        if (b == (PartData.Index - 1))
                        {
                            tDownFinish[t] = true;
                            return;
                        }
                        if (PartData.State(pp) == 1) { goto _121; }
                        if (PartData.State(pp) == 2)
                        {
                            goto _121;
                        }
                        tNowLength[t]=0;
                        PartData.State(pp, 1);
                        Trace.Write("\nStartPart=" + Convert.ToString(pp) + ";");
                        HttpWebRequest hwr=(HttpWebRequest)WebRequest.Create(new Uri(GetDownUrl));
                        hwr.AddRange(PartData.Start(pp), PartData.End(pp));
                        hwr.AllowAutoRedirect = true;
                        hwr.KeepAlive = false;
                        hwr.ServicePoint.Expect100Continue = false;
                        hwr.Timeout = 5000;
                        hwr.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.8 (KHTML, like Gecko) Chrome/17.0.942.0 Safari/535.8";
                        FileStream fs = new FileStream(PartData.FileHref(pp), FileMode.Create, FileAccess.Write);
                        byte[] buffer = new byte[4096];
                        Stream a = hwr.GetResponse().GetResponseStream();

                        a.ReadTimeout = 10000;//十秒钟超时
                        int l = 0;
                        l = a.Read(buffer, 0, buffer.Length);
                        while (l > 0)
                        {
                            fs.Write(buffer, 0, l);
                            buffer = new byte[4096];
                            tNowLength[t] += l;
                            tNowLengthTotal[t] += l;
                            PriNowDown += l;
                            l = a.Read(buffer, 0, buffer.Length);
                        }
                        hwr.Abort();
                        a.Close();
                        fs.Close();
                        PartData.State(pp, 2);
                    _121:
                        p++;
                        if (p >= PartData.Index)
                        {
                            p = 1;
                        }
                    }
                    catch
                    {
                        PartData.State(pp, 0);
                        tNowLength[t] -= PriNowDown;
                        tNowLengthTotal[t] -= PriNowDown;
                        p++;
                        if (p >= PartData.Index)
                        {
                            p = 1;
                        }
                 }

             //   }
            }
        }

        /// <summary>
        /// 开始下载
        /// </summary>
        public void CreateDown()
        {
            if (CanDown == true)
            {
                MadePart();
                MyThread = new Thread[tCount];
                FinishThread = new Thread(FinishCapture);
                FinishThread.IsBackground = true;
                for (int t = 0; t < tCount; t++)
                {
                    MyThread[t] = new Thread(new ParameterizedThreadStart(DownloadfileThread));
                    MyThread[t].IsBackground = true;
                    tNowLength[t] = 0;
                    tNowLengthTotal[t] = 0;
                    tDownFinish[t] = false;
                    MyThread[t].Start(t);
                }
                CanDown = false;
                FinishThread.Start();
            }
            else
            {
                throw new Exception("当前对象已经完成一个下载，如要重新下载请创建新的对象。");
            }
        }

        //检测下载完成的回调事件
        private void FinishCapture()
        {
           // Trace.Write("\nIMStart");
            try
            {
                bool IsFinish = false;
                while (!IsFinish)
                {
                    int v = 0;
                    Thread.Sleep(1000);
                    for (int i = 0; i < tCount; i++)
                    {
                        if (tDownFinish[i] == true)
                        {
                            v++;
                        }
                    }
                    if (v == tCount)
                    { //线程完成所有工作  
                      // Trace.Write("\nFinish");
                        IsFinish = true;
                        Finish();
                        MakefileFull();
                        MakeFile();
                    }
                }
            }
            catch
            {
              //  Trace.Write("\nErr");
            }
        }
        //文件合并
        private void MakefileFull()
        {
            FileStream fs = new FileStream(TargetDir + "\\" + SaveName, FileMode.Create, FileAccess.Write);
            for (int i = 1; i < PartData.Index; i++)
            {
                using (FileStream fs1 = new FileStream(PartData.FileHref(i), FileMode.Open, FileAccess.Read))
                {
                     byte[]  buffer =new byte[(int)fs1.Length];
                    fs1.Read(buffer,0,buffer.Length);
                    fs.Write(buffer, 0, buffer.Length);
                    fs1.Close();
                }
                File.Delete(PartData.FileHref(i));
            }
            fs.Close();
        }
    }

    /// <summary>
    /// 区块数据类
    /// </summary>
    public class DownloaderPartData
    {
        private int _Index = 1;
        /// <summary>
        /// 当前位置
        /// </summary>
        public int Index { get { return _Index; } }
        private Hashtable PartLength=new Hashtable();
        private Hashtable PartFileHref = new Hashtable();
        private Hashtable PartState = new Hashtable();
        private Hashtable PartStart = new Hashtable();
        private Hashtable PartEnd = new Hashtable();

        /// <summary>
        /// 添加区块
        /// </summary>
        /// <param name="_Start">起始长度</param>
        /// <param name="_Length">区块长度</param>
        /// <param name="_fileHref">区块文件位置</param>
        public void AddPart(int _Length, int _Start,int _End, string _fileHref)
        {
            PartLength.Add(_Index, _Length);
            PartFileHref.Add(_Index, _fileHref);
            PartStart.Add(_Index, _Start);
            PartState.Add(_Index, 0); //等待下载
            PartEnd.Add(_Index, _End);
            _Index++;
        }

        //获取器部分
        /// <summary>
        /// 区块长度
        /// </summary>
        /// <param name="partIndex">区块编号</param>
        public int Length(int partIndex) 
        {
            return (int)PartLength[partIndex];
        }
        /// <summary>
        /// 区块文件位置
        /// </summary>
        /// <param name="partIndex">区块编号</param>
        public string FileHref(int partIndex)
        {
            return (string)PartFileHref[partIndex];
        }
        /// <summary>
        /// 下载状态
        /// </summary>
        /// <param name="partIndex">区块编号</param>
        public int State(int partIndex) 
        {
            return (int)PartState[partIndex];
        }
        /// <summary>
        /// 起始长度
        /// </summary>
        /// <param name="partIndex">区块编号</param>
        public int Start(int partIndex)
        {
            return (int)PartStart[partIndex];
        }
        /// <summary>
        /// 结束长度
        /// </summary>
        /// <param name="partIndex">区块编号</param>
        public int End(int partIndex)
        {
            return (int)PartEnd[partIndex];
        }
        /// <summary>
        /// 下载状态设置
        /// </summary>
        /// <param name="partIndex">区块编号</param>
        /// <param name="value">状态编号</param>
        public void State(int partIndex, int value)
        {
            if ((int)PartState[partIndex]==2){
                return;
            }
            PartState[partIndex] = value;
        }
    }
}
