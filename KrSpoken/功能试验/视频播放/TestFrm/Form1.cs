using System;
using System.Windows.Forms;
using KrSpoken.VideoAudioPlayerLib;

namespace TestFrm
{
    public partial class Form1 : Form
    {
        public VideoPlayer vp = new VideoPlayer();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            vp.OpenClip(@"D:\1\parta.wmv",panel1);
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //vp.setPosition(10);
            label1.Text = vp.getCurretPosition().ToString();
           this.Text=vp.getCurrentPositionTime()+"/"+ vp.getDurationTime();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            vp.StopClip();
            vp.CloseClip();
        }
    }
}
