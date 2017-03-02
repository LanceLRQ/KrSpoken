using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using KrSpoken.KrPacker;
using System.Security.Cryptography;
using System.Diagnostics;
using SevenZip;

namespace KrSpoken.Server
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public string button8_ClickUSerPaaa = "";

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = "d:\\";
            fbd.ShowNewFolderButton = false;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = textBox1.Text + "\r\n" + "正在删除重复数据....";
                Application.DoEvents();
                //重复数据删除
                string[] a= Directory.GetFiles(fbd.SelectedPath);
                foreach(string b in a){
                    string c = Application.StartupPath + @"\the same\" + b.Replace(fbd.SelectedPath + @"\", "");
                    if (File.Exists(c) == true)
                    {
                        File.Delete(b);
                    }
                }
                textBox1.Text = textBox1.Text + "\r\n" + "删除完成。准备打包....";
                Application.DoEvents();
                SaveFileDialog sf = new SaveFileDialog();
                sf.Filter = "(*.krpk）试题包文件|*.krpk";
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    label1.Text = "选择文件夹：" + fbd.SelectedPath;
                    label2.Text = "储存文件：" + sf.FileName;
                    label3.Text = "索引文件：" + sf.FileName.Replace(".krpk",".kridx");
                    textBox1.Text = textBox1.Text + "\r\n" + "开始打包....";
                    Application.DoEvents();
                    FileInBox fb = new FileInBox(65535);
                    fb.Init(fbd.SelectedPath, sf.FileName.Replace(".krpk", ".kridx"), sf.FileName);
                    fb.ReadFileToMemory();
                    textBox1.Text = textBox1.Text + "\r\n" + "打包完毕。";
                    Application.DoEvents();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "(Information.xls）启明口语流程文件|*.xls";
            if (of.ShowDialog() == DialogResult.OK)
            {
                XLSFileReader.XLSFileReader x = new XLSFileReader.XLSFileReader(of.FileName);
                x.changeAList(of.FileName.Replace("Information.xls", "KrList.Step"), textBox1.Text);
                x = null;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
           string[] u= UserCodeManager.GetNewInfo(textBox5.Text);
           textBox6.Text = u[0];
           textBox7.Text = u[1];

        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox8.Text = UserCodeManager.GetNewCheckCode(textBox6.Text, textBox7.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider(1024);
            textBox9.Text =Convert.ToBase64String( provider.ExportCspBlob(true));
            textBox10.Text = Convert.ToBase64String(provider.ExportCspBlob(false));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string file1 = "", file2 = "", file3 = "", file4 = "";
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "*.kridx(数据包索引文件)|*.kridx";
            of.InitialDirectory = "d:\\听力\\标准\\";
            of.Multiselect = false;
            if (of.ShowDialog() == DialogResult.OK)
            {
                file1 = of.FileName.ToLower();
                file2 = of.FileName.ToLower().Replace(".kridx",".krmxidx");
                SaveFileDialog ofa = new SaveFileDialog();
                ofa.Filter = "*.kridx(数据包索引文件)|*.kridx";
                ofa.InitialDirectory = "d:\\听力\\发布\\";
                if (ofa.ShowDialog() == DialogResult.OK)
                {
                    file3 = ofa.FileName.ToLower(); //输出文件
                    file4 = ofa.FileName.ToLower().Replace(".kridx", ".krmxidx"); //输出文件\
                    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                    rsa.ImportCspBlob(Convert.FromBase64String(textBox4.Text));
                    using(FileStream fs=new FileStream(file1,FileMode.Open,FileAccess.Read)){
                        using(FileStream fs1=new FileStream(file3,FileMode.Create,FileAccess.Write)){
                            byte[] buffer=new byte[fs.Length];
                            fs.Read(buffer, 0, buffer.Length);
                            byte[] a;
                            //适应加密模块
                            MemoryStream bm = new MemoryStream(buffer);
                            int leng = 0, ii = 0;
                            leng = buffer.Length;
                            MemoryStream m = new MemoryStream();
                            while (ii < leng)
                            {
                                if (leng - ii >= 110)
                                {
                                    byte[] b = new byte[110];
                                    bm.Read(b, 0, 110);
                                    m.Write(rsa.Encrypt(b, false), 0, 128);
                                    ii += 110;
                                }
                                else
                                {
                                    byte[] b = new byte[leng - ii];
                                    bm.Read(b, 0, leng - ii);
                                    m.Write(rsa.Encrypt(b, false), 0, 128);
                                    ii = leng;
                                }
                               
                            }
                            m.Position = 0;
                            byte[] bb=new byte[(int)m.Length];
                            m.Read(bb, 0,(int)m.Length);
                            Base64Encoder be = new Base64Encoder(textBox3.Text);
                            a = System.Text.Encoding.Default.GetBytes(be.GetEncoded(Encoding.Default.GetBytes(Convert.ToBase64String(bb))));
                            fs1.Write(a, 0, a.Length);
                        }
                    }
                    using (FileStream fs = new FileStream(file2, FileMode.Open, FileAccess.Read))
                    {
                        using (FileStream fs1 = new FileStream(file4, FileMode.Create, FileAccess.Write))
                        {
                            byte[] buffer = new byte[fs.Length];
                            fs.Read(buffer, 0, buffer.Length);
                            byte[] a;
                            //适应加密模块
                            MemoryStream bm = new MemoryStream(buffer);
                            int leng = 0, ii = 0;
                            leng = buffer.Length;
                            MemoryStream m = new MemoryStream();
                            while (ii < leng)
                            {
                                if (leng - ii >= 110)
                                {
                                    byte[] b = new byte[110];
                                    bm.Read(b, 0, 110);
                                    m.Write(rsa.Encrypt(b, false), 0, 128);
                                    ii += 110;
                                }
                                else
                                {
                                    byte[] b = new byte[leng - ii];
                                    bm.Read(b, 0, leng - ii);
                                    byte[] c = rsa.Encrypt(b, false);
                                    m.Write(rsa.Encrypt(b, false), 0, 128);
                                    ii = leng;
                                }

                            }
                            m.Position = 0;
                            byte[] bb = new byte[(int)m.Length];
                            m.Read(bb, 0, (int)m.Length);
                            Base64Encoder be = new Base64Encoder(textBox3.Text);
                            a = System.Text.Encoding.Default.GetBytes(be.GetEncoded(Encoding.Default.GetBytes(Convert.ToBase64String(bb))));
                            fs1.Write(a, 0, a.Length);
                        }
                    }

                    MessageBox.Show("Success");
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string file1 = "", file3 = "", file4 = "";
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "*.krpk(数据包文件)|*.krpk";
            of.InitialDirectory = "d:\\听力\\标准\\";
            of.Multiselect = false;
            if (of.ShowDialog() == DialogResult.OK)
            {
                file1 = of.FileName.ToLower();
                file4 = file1 + ".tmp";
                File.Copy(file1, file4);
                File.Delete(file1);
                file3 = file1.Replace(".krpk", ".krmxidx");
                label14.Text = "开始混淆数据包...";
                Application.DoEvents();
                MixThePackUp mxp = new MixThePackUp();
                mxp.Init(file4, file3, file1);
                mxp.OutTheFile();
                mxp = null;
                File.Delete(file4);
                label14.Text = "完成！";
                Application.DoEvents();
            }
            else
            {
                return;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Text = "标准";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string pname = textBox11.Text;
            string pdi = textBox12.Text;
            string ptyp = comboBox1.Text;
            FolderBrowserDialog fb = new FolderBrowserDialog();
            fb.SelectedPath = "d:\\听力\\标准";
            fb.ShowNewFolderButton = true;
            if (fb.ShowDialog() == DialogResult.OK)
            {
                string pa = fb.SelectedPath;
                button8_ClickUSerPaaa = pa + "\\" + pname + ".dat";
                StreamWriter sr = new StreamWriter(button8_ClickUSerPaaa,false,Encoding.Default);
                sr.WriteLine(pname + ".krpk");
                sr.WriteLine(pname + ".kridx");
                sr.WriteLine(ptyp);
                sr.WriteLine(pdi);
                if (ptyp == "标准")
                {
                    sr.WriteLine(pname + ".krmxidx");
                }
                sr.Close();
                sr = null;

                SevenZipCompressor szc = new SevenZipCompressor();
                szc.ArchiveFormat = OutArchiveFormat.Zip;
                szc.CompressionFinished += new EventHandler<EventArgs>(szc_CompressionFinished);
                szc.Compressing += new EventHandler<ProgressEventArgs>(szc_Compressing);
                if (ptyp == "标准")
                {
                    szc.BeginCompressFiles(pa + "\\" + pname + ".krzip", pa + "\\" + pname + ".krpk", pa + "\\" + pname + ".dat");
                }
                else
                {
                    szc.BeginCompressFiles(pa + "\\" + pname + ".krzip", pa + "\\" + pname + ".krpk", pa + "\\" + pname + ".dat", pa + "\\" + pname + ".kridx");
                }
                Application.DoEvents();
                
            }
        }

        void szc_Compressing(object sender, ProgressEventArgs e)
        {
            progressBar1.Increment(e.PercentDelta);
            progressBar1.Refresh();
        }

        void szc_CompressionFinished(object sender, EventArgs e)
        {
            File.Delete(button8_ClickUSerPaaa);
            MessageBox.Show("Success");
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }
        
    }
}
