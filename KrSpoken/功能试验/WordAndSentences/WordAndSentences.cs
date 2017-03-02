using System;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Net;
using System.Text.RegularExpressions;

namespace KrSpoken.WordAndSentences
{

    public delegate void WordCapture_BaiduDownloadComplete(string Results);
    /// <summary>
    /// 划词搜索实现模块
    /// </summary>
    public class WordCapture
    {
        private OleDbConnection Conn;
        private WebClient wc;
        public event WordCapture_BaiduDownloadComplete FindFromBaiduComplete;
        private bool IsInit=false;
        /// <summary>
        /// 初始化划词搜索实现模块
        /// </summary>
        /// <param name="DBFilePath">绑定数据库</param>
        public WordCapture(string DBFilePath)
        {
            Conn = new OleDbConnection();
            Conn.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0;Data Source=" + DBFilePath;
            Conn.Open();
            if (!(Conn.State == System.Data.ConnectionState.Open))
            {
                throw new Exception("划词搜索模块初始化错误！程序将禁用划词搜索！");
            }
            IsInit=true;
        }
        /// <summary>
        /// 新建划词搜索
        /// </summary>
        /// <param name="Word">单词</param>
        public string[] CreateNewFind(string Word)
        {
            OleDbDataAdapter Oc = new OleDbDataAdapter("Select * from [Dic] where Word='" + Word + "'", Conn);
            DataSet ODataSet= new DataSet();
            Oc.Fill(ODataSet, "Dic");
            string[] s = new string[3];
            if (ODataSet.Tables["Dic"].Rows.Count < 1) { s[0] = "没找到，试试百度搜索吧。"; }
            else
            {
                s[0] = ODataSet.Tables["Dic"].Rows[0]["Word"].ToString();
                s[1] = ODataSet.Tables["Dic"].Rows[0]["Pronun"].ToString();
                s[2] = ODataSet.Tables["Dic"].Rows[0]["Chinese"].ToString();
            }
            return s;
        }
        /// <summary>
        /// 新建异步划词搜索(百度)
        /// </summary>
        /// <param name="Word">单词</param>
        public void CreateNewFindFromBaiduDic(string Word)
        {
            wc = new WebClient();
            wc.Proxy = null;
            wc.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.8 (KHTML, like Gecko) Chrome/17.0.942.0 Safari/535.8";
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
            wc.DownloadStringAsync(new Uri("http://dict.baidu.com/s?wd=" + Word + "&tn=dict&dt=explain"));
        }
        public void StopProgressing()
        {
            wc.CancelAsync();
        }
        private void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                FindFromBaiduComplete("查询任务取消。");
            }
            else
            {
                try
                {
                    if (e.Result != "")
                    {
                        string tmpData = e.Result.Replace("\r", "").Replace("\n","") ;
                        Regex re = new Regex(@"<div id\=\""en\""\>(.*)\<div id\=\""qbox\"">");
                        if (!re.IsMatch(tmpData)) throw new Exception();
                        tmpData = re.Match(tmpData).Value;
                        
                       Regex  re1 = new Regex(@"<script(.[^\<]*)</script>", RegexOptions.IgnoreCase);
                        foreach (Match m in re1.Matches(tmpData)) tmpData = tmpData.Replace(m.Value, "");
                        re1 = new Regex(@"<h2(.[^\<]*)</h2>", RegexOptions.IgnoreCase);
                        foreach (Match m in re1.Matches(tmpData)) tmpData = tmpData.Replace(m.Value, "");
                        re1 = new Regex(@"<span(.[^<]*)>", RegexOptions.IgnoreCase);
                        foreach (Match m in re1.Matches(tmpData)) tmpData = tmpData.Replace(m.Value, "");
                        re1 = new Regex(@"<div(.[^<]*)>", RegexOptions.IgnoreCase);
                        foreach (Match m in re1.Matches(tmpData)) tmpData = tmpData.Replace(m.Value, "");
                        re1 = new Regex(@"<img(.[^<]*)>", RegexOptions.IgnoreCase);
                        foreach (Match m in re1.Matches(tmpData)) tmpData = tmpData.Replace(m.Value, "");
                        tmpData = tmpData.Replace("</span>", "");
                        tmpData = tmpData.Replace("<span>", "");
                        tmpData = tmpData.Replace("</div>", "");
                        tmpData = tmpData.Replace("<div>", "\r\n");
                        tmpData = tmpData.Replace("\t", "");
                        tmpData = tmpData.Replace("</b>", "");
                        re1 = new Regex(@"<a(.[^\<]*)</a>", RegexOptions.IgnoreCase);
                        foreach (Match m in re1.Matches(tmpData)) tmpData = tmpData.Replace(m.Value, "");
                        re1 = new Regex(@"<h3(.[^<]*)>", RegexOptions.IgnoreCase);
                        foreach (Match m in re1.Matches(tmpData)) tmpData = tmpData.Replace(m.Value, "");  
                        tmpData = tmpData.Replace("</a>", "");
                        tmpData = tmpData.Replace("</h3>", "");
                        tmpData = tmpData.Replace("以下结果由译典通提供", "");
                        tmpData = tmpData.Replace("显示更多网络释义结果", "");
                        tmpData = tmpData.Replace("查看百科释义", "");
                        
                        re1 = new Regex(@"<br(.[^<]*)/>", RegexOptions.IgnoreCase);
                        foreach (Match m in re1.Matches(tmpData)) tmpData = tmpData.Replace(m.Value, "\r\n");
                        re1 = new Regex(@"<b(.[^<]*)>", RegexOptions.IgnoreCase);
                        foreach (Match m in re1.Matches(tmpData)) tmpData = tmpData.Replace(m.Value, "");
                        FindFromBaiduComplete(tmpData);
                    }
                    else
                    {
                        FindFromBaiduComplete("查询失败。");
                    }
                }
                catch
                {
                    FindFromBaiduComplete("查询失败。");
                }
            }
        }
    }

    public class WordCaptureRuntime
    {
        public event OnFrmWordClosingDelegate FrmClose;
        private FrmWord fWord;
        private string DbFile;
        public WordCaptureRuntime(string _dbfile)
        {
            DbFile = _dbfile;
        }
        public void OpenWordCapture(string Word)
        {
            fWord = new FrmWord();
            fWord.FrmCloseing += new OnFrmWordClosingDelegate(fWord_FrmCloseing);
            fWord.pWord = Word;
            fWord.pDbPath = DbFile;
            fWord.Show();
        }

       private void fWord_FrmCloseing()
        {
            FrmClose();
        }
       public void CloseFrm()
       {
           fWord.Close();
           fWord = null;
       }
    }
}
