using System;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Text;
using KrSpoken.KrPacker;
using KrSpoken.PlayListLib;
using KrSpoken.Profile;
using KrSpoken.VideoAudioPlayerLib;
using KrSpoken.KrMp3Recoder;
using System.Resources;
using System.Diagnostics;

namespace KrSpoken
{
    public partial class FrmExam : Form
    {
        public string pTitle = "", pfileName = "", pIndex = "", pMix = "", pType = "";
        public FrmMain frmMain;
        public AppProfileClass AppProfile;
        public NetUserData Nu;
        
        private FileOutBox PublicPack; //公有试题包
        private FileOutBox ExamPack; //当前读入的试题包
        private KrPlayList PlayList; //播放列表
        private Thread myThread;//程序运行的线程 
        private Thread TimeThread;//倒计时专用
        private Thread RecodeTimeThread;//录音专用
        private int ThisItemPlayEnd = 0; //发送执行下一条的命令
        private KrPlayListData PlayData;//当前播放项的数据
        private KrAudioPlayer KrAudioPlayer; //声音播放器
        private VideoPlayer KrVideoPlayer;//视频播放器 
        private int NowPlayType = 0;//1=Wave;Time=2;3=Wmv;
        private bool IsRecoding = false;//录音状态指示器 
        private int NowRecodeTime = 0;//当前的录音时间
        private int RecodeTimeTotal = 0;//总的录音时间
        private int TreeListClickItemID = 0;
        private KrMp3Recoder.KrMp3Recoder KrRecoder; //录音模块
        private string RecodePath ;//录音储存位置 
        private string WmvSavePath;//Wmv保存位置
        private int NowItem=0;//当前项目ID（大题）=>对于树列来说。
        private int NowSubItem = 0;//当前项目ID（小题）=>对于树列来说。
        private int NowPlaytime = 0;//当前播放倒计时
        private int NowPlayID=0; //当前播放的项目ID
        private string __StateLabText = ""; //状态Box的内容
        private string __TimeLabText = "";//时间Box的内容
        public string Kaohao = "1234567890";//考号

        #region 窗体事件
        public FrmExam()
        {
            InitializeComponent();
        }
        private void FrmPractice_Load(object sender, EventArgs e)
        {
            Application.DoEvents();
            if (Environment.OSVersion.Version.Major > 5)
            {
                ScriptBox.Font = new Font("微软雅黑", 14);
                Lab_Kaohao.Font = new Font("微软雅黑", 10);
                LabName.Font = new Font("微软雅黑", 13);
                PlayStateLabel.Font=new Font("微软雅黑", 10);
                PlayTimeLabel .Font=new Font("微软雅黑", 10);
            }
            if (Nu.DS_Name != "")
            {
                ScriptBox.Text = Nu.DS_Name;
            }
            Lab_Kaohao.Text = "考号：\n" + Kaohao;
            StartLoad();
            RecodePath=AppProfile.StudentAnswerDir +  "\\" + pTitle + "\\" + DateTime.Today.ToLongDateString().Replace("年", "-").Replace("月", "-").Replace("日", "-") + " " + DateTime.Now.ToLongTimeString().Replace(":", "-") ;
            if (Directory.Exists(RecodePath) == false) Directory.CreateDirectory(RecodePath);
            WmvSavePath =AppProfile. LocalApplicationData + "\\kdata\\ppp";
            try
            {
                if (Directory.Exists(WmvSavePath))
                {
                    foreach (string f in Directory.GetFiles(WmvSavePath))
                    {
                        File.Delete(f);
                    }
                    Directory.Delete(WmvSavePath);
                }
                Directory.CreateDirectory(WmvSavePath);
            }
            catch { }
            frmMain.Hide();
           GC.Collect();
        }
        private void FrmPractice_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (MessageBox.Show("确实要退出吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (MessageBox.Show("要保存本次的录音记录吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (IsRecoding) { RecodeTimeThread.Abort(); KrRecoder.RecStop(); } 
                        StopPlayAny();
                        myThread.Abort();
                        myThread = null;
                        if (Directory.Exists(WmvSavePath))
                        {
                            foreach (string f in Directory.GetFiles(WmvSavePath))
                            {
                                File.Delete(f);
                            }
                            Directory.Delete(WmvSavePath);
                        }
                        PublicPack = null;
                        ExamPack = null;
                        PlayList = null;
                        KrAudioPlayer = null;
                        KrVideoPlayer = null;
                        KrRecoder = null;
                        AppProfile = null;
                        myThread = null;
                        TimeThread = null;
                        RecodeTimeThread = null;
                        Nu = null;
                        GC.Collect();
                    }
                    else
                    {
                        if (IsRecoding) { RecodeTimeThread.Abort(); KrRecoder.RecStop(); }
                        if (Directory.Exists(RecodePath))
                        {
                            foreach (string f in Directory.GetFiles(RecodePath))
                            {
                                File.Delete(f);
                            }
                            Directory.Delete(RecodePath);
                        }
                        StopPlayAny();
                        myThread.Abort();
                        myThread = null;
                        if (Directory.Exists(WmvSavePath))
                        {
                            foreach (string f in Directory.GetFiles(WmvSavePath))
                            {
                                File.Delete(f);
                            }
                            Directory.Delete(WmvSavePath);
                        }
                        PublicPack = null;
                        ExamPack = null;
                        PlayList = null;
                        KrAudioPlayer = null;
                        KrVideoPlayer = null;
                        KrRecoder = null;
                        AppProfile = null;
                        Nu = null;
                        GC.Collect();
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch(Exception ex)
            {
               MessageBox.Show(ex.Message,"关闭时发生错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
              GC.Collect();
            }
        }
        #endregion

        #region 控制部分
        /// <summary>
        /// 初始化播放
        /// </summary>
        public void StartLoad()
        {
            MemoryStream sIndex, sMixIndex,sExampack=new MemoryStream();
            FileStream fsExampack;
            PublicPack = new FileOutBox();
            PublicPack.Init(AppProfile.PackDir + "\\Public.krpk", AppProfile.PackDir + "\\Public.kridx");
            ExamPack = new FileOutBox();
            FileInOutBoxMessage fm = new FileInOutBoxMessage();
            if (pType == "免费")
            {
                switch (ExamPack.Init(AppProfile.PackDir + "\\" + pfileName, AppProfile.PackDir + "\\" + pIndex))
                {//载入试题包
                    case -1:
                        throw new Exception("解包程序没有正常初始化");
                    case 9:
                        throw new Exception("错误的文件流");
                    case 10:
                        throw new Exception("未找到打包文件");
                    case 11:
                        throw new Exception("错误的索引文件");
                    case 12:
                        throw new Exception("错误的打包文件");
                    case 13:
                        throw new Exception("错误的索引文件");
                    case 14:
                        throw new Exception("打包文件与原版文件的校验失败！");
                    case 15:
                        throw new Exception("索引文件不存在");
                }
            }
            else
            {
                if (pType == "标准")
                {
                    //解包
                    using (StreamReader fs = new StreamReader(AppProfile.PackDir + "\\" + pIndex, Encoding.Default)) { sIndex = (MemoryStream)FileDecode.RunFileDecode(Nu.RegCode, FileDecode.GetHardCode(), Nu.CheckCode, Nu.RSAPrivateKey, fs.ReadToEnd()); }
                    using (StreamReader fsa = new StreamReader(AppProfile.PackDir + "\\" + pMix, Encoding.Default)) { sMixIndex = (MemoryStream)FileDecode.RunFileDecode(Nu.RegCode, FileDecode.GetHardCode(), Nu.CheckCode, Nu.RSAPrivateKey, fsa.ReadToEnd()); }
                    fsExampack = new FileStream(AppProfile.PackDir + "\\" + pfileName, FileMode.Open, FileAccess.Read);
                    //反混淆
                    ReBuiltThePack rbPK = new ReBuiltThePack();
                    switch (rbPK.Init(sMixIndex, fsExampack))
                    {
                        case -1:
                            throw new Exception("反混淆模块没有正常初始化");
                        case 1:
                            throw new Exception("反混淆索引或者混淆包数据文件未找到！");
                        case 7:
                            throw new Exception("反混淆过程中发生错误");
                    }
                    switch (rbPK.OutTheFile(out sExampack))
                    {
                        case -1:
                            throw new Exception("反混淆模块没有正常初始化");
                        case 1:
                            throw new Exception("反混淆索引或者混淆包数据文件未找到！");
                        case 7:
                            throw new Exception("反混淆过程中发生错误");
                    }
                    switch (ExamPack.Init(sExampack, sIndex))
                    { //载入试题包
                        case -1:
                            throw new Exception("解包程序没有正常初始化");
                        case 9:
                            throw new Exception("错误的文件流");
                        case 10:
                            throw new Exception("未找到打包文件");
                        case 11:
                            throw new Exception("错误的索引文件");
                        case 12:
                            throw new Exception("错误的打包文件");
                        case 13:
                            throw new Exception("错误的索引文件");
                        case 14:
                            throw new Exception("打包文件与原版文件的校验失败！");
                        case 15:
                            throw new Exception("索引文件不存在");
                    }
                    fsExampack.Close();
                    fsExampack = null;
                    sIndex.Close();
                    sIndex = null;
                    sMixIndex.Close();
                    sMixIndex = null;
                    rbPK = null;
                    GC.Collect();
                }
                else
                {
                    throw new Exception("试题包载入错误！");
                }
            }
            //载入播放列表
            KrPlayListRunTime.GetPlayList(ExamPack.GetFileStream("KrList.Step"), out PlayList);
            myThread = new Thread(ThreadRuntime);
            myThread.IsBackground = true;
            myThread.Start();
           // frmMain.TipBox.Hide(this.frmMain.PKList);
        }
        //播放线程
        public void ThreadRuntime()
        {
            //开始执行线程
            for (int i =1; i <= PlayList.MyList.Count; i++)
            {
                int ID =i;
                NowPlayID = i;
                PlayData = null;
                PlayData = new KrPlayListData((object[])PlayList.MyList[i]);
                //发送窗体Invoke命令
                InvokePlay();
                while (ThisItemPlayEnd == 0)
                {
                    Thread.Sleep(100);//减轻CPU负担
                }
                switch (ThisItemPlayEnd)
                {
                    case 1: //标准跳出（执行到下一页面的指令）
                        break;
                    case 2: //往上跳跃（一个页面）
                        i = i - 2;
                        break;
                    case 3://往下跳跃一个大题
                       ID= FindIdFromItemID(NowItem + 1);
                       if (ID > 0)
                       {
                           i = ID - 1;
                       }
                        break;
                    case 4://往上跳跃一个大题
                        ID= FindIdFromItemID(NowItem - 1);
                       if (ID > 0)
                       {
                           i = ID - 1;
                       }
                        break;
                    case 5:
                        //重复本小题
                        i = i - 1;
                        break;
                    case 6:
                        //树列表切换 
                        i = TreeListClickItemID - 1;
                        break;
                }
                ThisItemPlayEnd = 0;
            }
        }
        //播放顺序切换过程
        private int FindIdFromItemID(int IID)
        {
            if (IID < 0)
            {
                IID = 0;
            }
            if (IID > 3)
            {
                IID = 3;
            }
            for (int i = 1; i <= PlayList.MyList.Count; i++)
            {
                KrPlayListData s = new KrPlayListData((object[])PlayList.MyList[i]);
                if (s.ItemID == IID)
                {
                    return i;
                }
            }
            return 0;
        }
        //播放顺序切换过程
        private int FindIdFromItemID(int IID,int SubIID)
        {
            if (IID < 0)
            {
                IID = 0;
            }
            if (IID > 3)
            {
                IID = 3;
            }
            for (int i = 1; i <= PlayList.MyList.Count; i++)
            {
                KrPlayListData s = new KrPlayListData((object[])PlayList.MyList[i]);
                if (s.ItemID == IID)
                {
                    for (int l = i; l <= PlayList.MyList.Count; l++)
                    {
                        KrPlayListData sb = new KrPlayListData((object[])PlayList.MyList[l]);
                        if (sb.SubItemID == SubIID)
                        {
                            return l;
                        }
                    }
                }
            }
            return 0;
        }
        //回调响应
        private void InvokePlay()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    FromThreadRequestInvokeDelegate v = new FromThreadRequestInvokeDelegate(InvokePlay);
                    this.Invoke(v);
                }
                else
                {
                    //节点选择
                    if (PlayData.SubItemID == 0)
                    {
                        NowItem = PlayData.ItemID;
                        NowSubItem = PlayData.SubItemID;
                    }
                    else
                    {
                        NowItem = PlayData.ItemID;
                        NowSubItem = PlayData.SubItemID;
                    }
                    string file = "";
                    Stream WaveStream = null;
                    __StateLabText = PlayData.PlayHint;
                    switch (PlayData.FileType)
                    {
                        case 0:
                            //Recode
                            if (NowPlayID != 20) { RecodeTimeTotal = PlayData.PlayTime; PlayTimeBar.Maximum = RecodeTimeTotal; } //判断不是那个特殊的录音部分
                            RecodeTimeThread = null;
                            RecodeTimeThread = new Thread(new ThreadStart(RecoderThread));
                            RecodeTimeThread.IsBackground = true;
                            RecodeTimeThread.Start();
                            break;
                        case 1:
                            NowPlayType = 0;
                            file = PlayData.FilePath;
                            if (ExamPack.FileExist(file))
                            {
                                ScriptBox.Text = ExamPack.GetFileText(file, System.Text.Encoding.Default);
                            }
                            else
                            {
                                ScriptBox.Text = PublicPack.GetFileText(file, System.Text.Encoding.Default);
                            }
                            ShowScriptBox();
                            ThisItemPlayEnd = 1;
                            break;
                        case 3:
                            NowPlayType = 1;
                            file = PlayData.FilePath;
                            WaveStream = null;
                            if (ExamPack.FileExist(file.Replace("mp3", "") + "wav"))
                            {
                                WaveStream = ExamPack.GetFileStream(file.Replace("mp3", "") + "wav");
                            }
                            else
                            {
                                WaveStream = PublicPack.GetFileStream(file + ".wav");
                            }
                            WaveInfo Wi = new WaveInfo(WaveStream);
                            NowPlaytime = (int)Wi.Second + 1;
                            RecodeTimeTotal = NowPlaytime;
                            PlayTimeBar.Maximum = NowPlaytime;
                            KrAudioPlayer = new KrAudioPlayer();
                            KrAudioPlayer.PlayStart += new KrAudioPlayerPlayStart(WavePlayStart);
                            WaveStream.Position = 0;
                            KrAudioPlayer.Play(WaveStream);
                            //PlayWave
                            break;
                        case 4:
                            //PlayWmv
                            ToolTip TipBox;
                            TipBox = new ToolTip();
                            TipBox.IsBalloon = true;
                            TipBox.ShowAlways = true;
                            TipBox.ToolTipIcon = ToolTipIcon.Info;
                            TipBox.UseFading = true;
                            TipBox.ToolTipTitle = "提示";
                            TipBox.Show("视频正在加载，请稍等...", this, 200, 30);
                            Application.DoEvents();
                            file = PlayData.FilePath;
                            NowPlayType = 3;
                            ShowVideoPanel();
                            file = GetWmvSavePath(file);
                            if (file == "Error")
                            {
                                MessageBox.Show("视频释放失败！！请重新启动练习程序", "严重错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            KrVideoPlayer = new VideoPlayer();
                            KrVideoPlayer.PlayCompleted += new KrVideoPlayerPlayCompletedHandle(KrVideoPlayer_PlayCompleted);
                            KrVideoPlayer.OpenClip(file, VideoPanel);
                            RecodeTimeTotal = KrVideoPlayer.getDurationTimeInt();
                            PlayTimeBar.Maximum = KrVideoPlayer.getDurationTimeInt();
                            if (PlayData.PlayFlag == 1)
                            {
                                KrVideoPlayer.ToggleMute(true);
                            }
                            TipBox.Hide(this);
                            TimeThread = null;
                            TimeThread = new Thread(new ThreadStart(VideoTimeCapture));
                            TimeThread.IsBackground = true;
                            TimeThread.Start();
                            break;
                        case 5:
                            //PlayTime
                            NowPlayType = 2;
                            NowPlaytime = PlayData.PlayTime + 1;
                            PlayTimeBar.Maximum = NowPlaytime;
                            TimeThread = null;
                            TimeThread = new Thread(new ThreadStart(TimeoutBox2));
                            TimeThread.IsBackground = true;
                            TimeThread.Start();
                            break;

                    }
                    int a = NowPlayID + 1;
                    //  if(a==20 | a=29| a=36|a=43|a=55|a=62|a=69|a=76|83|96)
                    //判断录音
                    switch (a)
                    {
                        case 20:
                            ThisItemPlayEnd = 1; break;
                    }
                }
            }
            catch
            {
            }
        }
        //视频播放完成回调
        private void KrVideoPlayer_PlayCompleted()
        {
            TimeThread.Abort();
            KrVideoPlayer.CloseClip();
            KrVideoPlayer = null;
            //发送跳转命令
            ThisItemPlayEnd = 1;
        }
        //状态栏更新
        private void InvokeChangeStateLab()
        {
            if (this.InvokeRequired)
            {
                FromThreadRequestInvokeDelegate v = new FromThreadRequestInvokeDelegate(InvokeChangeStateLab);
                this.Invoke(v);
            }
            else
            {
                PlayStateLabel.Text = __StateLabText;
                PlayTimeLabel.Text = __TimeLabText;
                if (NowPlayType != 3)
                {
                    PlayTimeBar.Value = PlayTimeBar.Maximum - NowPlaytime;
                }
                else
                {
                    PlayTimeBar.Value =  KrVideoPlayer.getCurrentPositionTimeInt();
                }
                if (IsRecoding)
                {
                    PlayState.Image = PlayerButton.State_Record;
                }
                else
                {
                    PlayState.Image = PlayerButton.State_Listen;
                }
            }
        }
        //音频开始播放事件
        private void WavePlayStart()
        {
            TimeThread = null;
            TimeThread = new Thread(new ThreadStart(TimeoutBox1));
            TimeThread.IsBackground=true;
            TimeThread.Start();
        }
        //录制视频使用的线程
        private void RecoderThread()
        {
            IsRecoding = true;
            NowRecodeTime = 0;
            try
            {
                KrRecoder = new KrMp3Recoder.KrMp3Recoder();
            }
            catch
            {
                MessageBox.Show("设备初始化失败，可能是系统中没有音频捕捉设备。","错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            KrRecoder.ErrorExcepton += new Mp3RecorderErrorExcepton(KrRecoder_ErrorExcepton);
            KrRecoder.SetFileName(RecodePath+"\\"+PlayData.FilePath.Replace(".wav",".mp3"));
            KrRecoder.RecStart();
            lock (this)
            {
                while (NowRecodeTime < RecodeTimeTotal)
                {
                    Thread.Sleep(1000);
                    NowRecodeTime++;
                    if (NowPlayID == 20)//判断不是那个特殊的录音部分
                    {
                        __StateLabText = "录音中：" + Convert.ToString(NowRecodeTime) + "秒";
                    }
                    else
                    {
                        __TimeLabText = "剩余时间：" + Convert.ToString(RecodeTimeTotal-NowRecodeTime) + "秒";
                    }
                    InvokeChangeStateLab();
                }
            }
            __StateLabText = "";
            InvokeChangeStateLab();
            KrRecoder.RecStop();
            IsRecoding = false;
            if (NowPlayID != 20) { ThisItemPlayEnd = 1; } //判断不是那个特殊的录音部分
        }
        //录音错误事件回调
        private void KrRecoder_ErrorExcepton(string ErrMsg)
        {
            MessageBox.Show(ErrMsg);
            return;
        }
        //倒计时专用==Wave
        private void TimeoutBox1()
        {
            lock (this)
            {
                while (NowPlaytime > 0)
                {
                    NowPlaytime--;
                    __TimeLabText = "剩余时间：" + Convert.ToString(NowPlaytime);
                    InvokeChangeStateLab();
                    Thread.Sleep(1000);
                }
            }
                //发送跳转命令
                ThisItemPlayEnd = 1;
        }
        //视频监视
        private void VideoTimeCapture()
        {
            while (true)
            {
                __TimeLabText = KrVideoPlayer.getCurrentPositionTime() +"/"+KrVideoPlayer.getDurationTime();
                InvokeChangeStateLab();
                Thread.Sleep(500);
            }
        }
        //准备时间倒计时
        private void TimeoutBox2()
        {
            lock (this)
            {
                while (NowPlaytime > 0)
                {
                    NowPlaytime--;
                    __TimeLabText = "剩余时间：" + Convert.ToString(NowPlaytime);
                    InvokeChangeStateLab();
                    Thread.Sleep(1000);
                }
            }
            //发送跳转命令
            ThisItemPlayEnd = 1;
        }
        //显示视频播放框
        private void ShowVideoPanel()
        {

            VideoPanel.Location = new Point(147, 4);
            ScriptBox.Location = new Point(147, 700);
        }
        //显示文本框
        private void ShowScriptBox()
        {

            ScriptBox.Location = new Point(147, 4);
            VideoPanel.Location = new Point(147, 700);
        }
        //停止当前播放 
        private void StopPlayAny()
        {
                if (NowPlayType == 1)
                {
                    TimeThread.Abort();
                    TimeThread.Join();
                    TimeThread = null;
                    KrAudioPlayer.Stop();
                    KrAudioPlayer = null;
                    NowPlaytime = 0;
                }
                if (NowPlayType == 3)
                {
                    TimeThread.Abort();
                    TimeThread.Join();
                    TimeThread = null; 
                    KrVideoPlayer.StopClip();
                    KrVideoPlayer.CloseClip();
                    KrVideoPlayer = null;
                    NowPlaytime = 0;
                }
                if (NowPlayType ==2)
                {
                    TimeThread.Abort();
                    TimeThread.Join();
                    TimeThread = null;
                    NowPlaytime = 0;
                }
                GC.Collect();
        }
        //获取Wmv储存位置
        private string GetWmvSavePath(string _getfileName)
        {
            try
            {
                if (File.Exists(WmvSavePath + "\\" + _getfileName))
                {
                    return WmvSavePath + "\\" + _getfileName;
                }
                FileStream fs = new FileStream(WmvSavePath + "\\" + _getfileName, FileMode.Create, FileAccess.Write);
                Stream s = ExamPack.GetFileStream(_getfileName);
                byte[] buffer = new byte[(int)s.Length];
                s.Read(buffer, 0, buffer.Length);
                fs.Write(buffer, 0, buffer.Length);
                fs.Close();
                fs = null;
                s.Close();
                s = null;
                buffer = null;
                GC.Collect();
                return WmvSavePath + "\\" + _getfileName;
            }
            catch
            {
                return "Error";
            }
        }
        #endregion

        
    }
}
