using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using KrSpoken.Profile;
using System.Threading;

namespace KrSpoken
{
    public delegate void FrmChangeBg_SelectedDelegate(string _Path);
    public delegate void FrmChangeBg_InvokeBack();
    public partial class FrmChangeBg : Form
    {
        private Hashtable PicList=new Hashtable();
        public event FrmChangeBg_SelectedDelegate Selected;
        public AppProfileClass AppProfile;
        private double Opt = 0.0;
        public FrmChangeBg()
        {
            InitializeComponent();
        }

        private void FrmChangeBg_Load(object sender, EventArgs e)
        {
            ImageList imageList1=new ImageList();
            imageList1.ColorDepth = ColorDepth.Depth24Bit;
            imageList1.ImageSize = new Size(150, 111);
            int i=0;
            foreach (string f in Directory.GetFiles(AppProfile.BackGroundDir))
            {
                try
                {
                    imageList1.Images.Add(Image.FromFile(f));
                    listView1.LargeImageList = imageList1;
                    listView1.Items.Add("背景" + i.ToString());
                    listView1.Items[i].ImageIndex = i;
                    PicList.Add(i, f);
                    i++;
                }
                catch
                {
                    MessageBox.Show("背景图片加载失败！请重试", "错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            Thread t = new Thread(Into);
            t.IsBackground = true;
            t.Start();
        }


        private void Into()
        {
            while (Opt < 1)
            {
                Opt += 0.1;
                BackInvoke();
                Thread.Sleep(50);
            }
        }


        private void BackInvoke()
        {
            if (this.InvokeRequired)
            {
                FrmChangeBg_InvokeBack d = new FrmChangeBg_InvokeBack(BackInvoke);
                this.Invoke(d);
            }
            else
            {
                this.Opacity = Opt;
            }
        }

        private void ClostBut_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = listView1.HitTest(e.X, e.Y);
            if (info != null)
            {
                string tmp="";
                try
                {
                     tmp = (string)PicList[Convert.ToInt32(info.Item.Text.Replace("背景", ""))];
                }
                catch { MessageBox.Show("程序发生错误，请重试。", "错误！", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
              Selected(tmp);
              this.Close();
            }
        }


    }
}
