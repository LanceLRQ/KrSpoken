using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dsr_mp3_Record;


namespace 直接录音到MP3
{
    public partial class Form1 : Form
    {
        private Mp3Recorder recorder;    // 录音
        private bool canclose = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            recorder = new Mp3Recorder();
            recorder.ErrorExcepton +=new Mp3RecorderErrorExcepton(recorder_ErrorExcepton);
            string wavfile = null;
            wavfile = textBox1.Text;            //这个是储存文件的位置
            recorder.SetFileName(wavfile);
            recorder.RecStart();
            button1.Enabled = false;
            button2.Enabled = true;
            canclose = false;
        }

        private void recorder_ErrorExcepton(string ErrMsg)
        {
            MessageBox.Show(ErrMsg);
            return;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            recorder.RecStop();
            recorder = null;
            button1.Enabled = true;
            button2.Enabled = false;
            canclose = true;
        }

        private void FromClosing(object sender, FormClosingEventArgs e)
        {
            if (canclose == true)
            {
            e.Cancel = false;
                Application.Exit(); 
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
