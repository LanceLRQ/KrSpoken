using System;
using System.Windows.Forms;
using System.Threading;
using System.Media;
using KrSpoken.KrMp3Recoder;
using System.IO;

namespace KrSpoken
{
    public delegate void FrmTestThreadBakc();
    public partial class FrmTest : Form
    {
        private Thread T;
        private bool IsCanClose = false;
        private bool IsPlaying = false;
        private bool IsRecoding = false;
        private WavRecoder wr;
        public FrmTest()
        {
            InitializeComponent();
        }

        private void FrmTest_Load(object sender, EventArgs e)
        {
            T = new Thread(Sound1);
            T.IsBackground = true;
            T.Start();
        }
        private void Sound1()  //设备测试Start
        {
            SoundPlayer SP=new SoundPlayer(Application.StartupPath +"\\TestWav\\Start.wav");
            SP.PlaySync();
            Req1();
        }
        private void Sound2()  //设备测试ing
        {
            IsPlaying = true;
            SoundPlayer SP = new SoundPlayer(Application.StartupPath + "\\TestWav\\Test.wav");
            SP.PlaySync();
            IsPlaying = false;
        }
        private void Sound4()  //设备测试ing
        {
            IsPlaying = true;
            SoundPlayer SP = new SoundPlayer(Application.StartupPath + "\\TestWav\\tmp.wav");
            SP.PlaySync();
            IsPlaying = false;
        }
        private void Recode()  //录音
        {
            try
            {
                wr = new WavRecoder();
                wr.SetFileName(Application.StartupPath + "\\TestWav\\tmp.wav");
                wr.writeIntoFile = true;
                wr.RecStart();
            }
            catch
            {
                MessageBox.Show("录音设备出现错误，请修复！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void Sound3()  //设备测试End
        {
            SoundPlayer SP = new SoundPlayer(Application.StartupPath + "\\TestWav\\End.wav");
            SP.PlaySync();
            Req5();
        }
        private void Req1()//设备测试Start==回调
        {
            if (this.InvokeRequired)
            {
                FrmTestThreadBakc d = new FrmTestThreadBakc(Req1);
                this.Invoke(d);
            }
            else
            {
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
            }
        }


        private void FrmTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsCanClose) e.Cancel=true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (IsPlaying)
            {
                MessageBox.Show("仍有音频播放中,请稍等", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (IsRecoding)
            {
                MessageBox.Show("录音进行中，请先停止录音", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            T = new Thread(Sound3);
            T.IsBackground = true;
            T.Start();
        }
        private void Req5()//设备测试End==回调
        {
            if (this.InvokeRequired)
            {
                FrmTestThreadBakc d = new FrmTestThreadBakc(Req5);
                this.Invoke(d);
            }
            else
            {
                try
                {
                    T.Abort();
                    T = null;
                }
                catch { }
                IsCanClose = true;
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (IsPlaying)
            {
                MessageBox.Show("仍有音频播放中,请稍等", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (IsRecoding)
            {
                MessageBox.Show("录音进行中，请先停止录音", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            T = new Thread(Sound2);
            T.IsBackground = true;
            T.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (IsPlaying)
            {
                MessageBox.Show("仍有音频播放中,请稍等", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (button2.Text == "开始录音")
            {
                IsRecoding = true;
                Recode();
                button2.Text = "停止录音";
            }
            else
            {
                try
                {
                    wr.RecStop();
                    wr = null;
                }
                catch { }
                IsRecoding = false;
                button2.Text = "开始录音";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (IsPlaying)
            {
                MessageBox.Show("仍有音频播放中，请稍等", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (IsRecoding)
            {
                MessageBox.Show("录音进行中，请先停止录音", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (File.Exists(Application.StartupPath + "\\TestWav\\tmp.wav") == false)
            {
                MessageBox.Show("你还没有录音。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            T = new Thread(Sound4);
            T.IsBackground = true;
            T.Start();
        }
    }
}
