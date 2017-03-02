using System;
using System.Windows.Forms;
using KrSpoken.Profile;
using SevenZip;
using System.IO;
using Microsoft.Win32;


namespace KrSpoken.KrPKInstall
{
    public partial class FrmM : Form
    {
        private string[] file = new string[3];
        private AppProfileClass app;
        private UserPkLib upk;
        public FrmM()
        {
            InitializeComponent();
        }
        private void FrmM_Load(object sender, EventArgs e)
        {
            string cmd = Environment.CommandLine;
            if (cmd.IndexOf("-install") > 0)
            {
                try
                {
                    //进行安装文件关联 
                    string MyFileName = Application.ExecutablePath;
                    string MyIconFileName = Application.StartupPath +"\\KrPack.ico";
                    string MyExtName = ".krzip";
                    string MyExtName2 = "krzipfile";
                    string MyType = "听雨口语试题包安装文件";
                    RegistryKey MyReg = Registry.ClassesRoot.CreateSubKey(MyExtName);
                    MyReg.SetValue("", MyExtName2);
                    RegistryKey MyReg2 = Registry.ClassesRoot.CreateSubKey(MyExtName2);
                    MyReg2.SetValue("", MyType);
                    MyReg2 = MyReg2.CreateSubKey("DefaultIcon");
                    MyReg2.SetValue("", MyIconFileName);
                    MyReg2.Close();
                    MyReg2 = null; 
                    MyReg2 = Registry.ClassesRoot.OpenSubKey(MyExtName2,true);
                    MyReg2 = MyReg2.CreateSubKey("shell");
                    MyReg2.SetValue("", "open");
                    MyReg2 = MyReg2.CreateSubKey("open");
                    MyReg2.SetValue("", "打开(&O)");
                    MyReg2 = MyReg2.CreateSubKey("command");
                    MyReg2.SetValue("", "\"" + MyFileName + "\" %1");
                    MyReg2.Close();
                    MyReg.Close();
                    //==========
                    MyExtName = ".krpk";
                    MyExtName2 = "krpkfile";
                    MyType = "听雨口语试题包文件";
                    MyReg = Registry.ClassesRoot.CreateSubKey(MyExtName);
                    MyReg.SetValue("", MyExtName2);
                    MyReg2 = Registry.ClassesRoot.CreateSubKey(MyExtName2);
                    MyReg2.SetValue("", MyType);
                    MyReg2.Close();
                    MyReg.Close();
                    Application.Exit();
                    return;
                }
                catch
                {
                    MessageBox.Show("T.T 文件关联失败！如果您的系统是Win7，需要权限。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Application.Exit();
                    return;
                }
            }
            try
            {
                string fileHref = cmd.Substring(cmd.IndexOf(" ")).Replace("\"", "").Trim();
                if (fileHref == "")
                {
                    Application.Exit();
                    return;
                }
                if (MessageBox.Show("【即将安装试题包】\n文件位置：" + fileHref + "\n安装提示：如果“听雨口语”软件正在运行，请不要操作它，等待本程序安装完成后再操作。（防止因冲突导致的列表数据损坏）\n\n按【确定】继续，按【取消】退出", "请注意", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                {
                    Application.Exit();
                    return;
                }
                AppProfileRuntime.Infile(Application.StartupPath + "\\Profile.config", out app);
                SevenZipExtractor sze = new SevenZipExtractor(fileHref);
                sze.Extracting += new EventHandler<ProgressEventArgs>(sze_Extracting);
                sze.ExtractionFinished += new EventHandler<EventArgs>(sze_ExtractionFinished);
                foreach (ArchiveFileInfo acc in sze.ArchiveFileData)
                {
                    if (acc.FileName.ToLower().IndexOf(".dat") > -1)
                    {
                        file[1] = app.PackDir + "\\" + sze.ArchiveFileData[acc.Index].FileName;
                    }
                    if (acc.FileName.ToLower().IndexOf(".krpk") > -1)
                    {
                        file[0] = app.PackDir + "\\" + sze.ArchiveFileData[acc.Index].FileName;
                    }
                    if (acc.FileName.ToLower().IndexOf(".kridx") > -1)
                    {
                        file[2] = app.PackDir + "\\" + sze.ArchiveFileData[acc.Index].FileName;
                    }
                }
                sze.BeginExtractArchive(app.PackDir);
            }
            catch
            {
                MessageBox.Show("T.T 安装失败！再来一次试试看吧。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }
        }

        void sze_Extracting(object sender, ProgressEventArgs e)
        {
            progressBar1.Increment(e.PercentDelta);
            progressBar1.Refresh();
        }
        void sze_ExtractionFinished(object sender, EventArgs e)
        {
            try
            {
                UserPkLibRuntime.Infile(app.PkLibfile, out upk);
                using (StreamReader sr = new StreamReader(file[1], System.Text.Encoding.Default))
                {
                    string[] abc = new string[5];
                    abc[0] = sr.ReadLine();
                    if (upk._PackTitle.ContainsValue(abc[0].Replace(".krpk", "")))
                    {
                        MessageBox.Show("你已经安装过这个包了。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        sr.Close();
                        File.Delete(file[1]);
                        Application.Exit();
                        return;
                    }
                    abc[1] = sr.ReadLine();
                    abc[2] = sr.ReadLine();
                    abc[3] = sr.ReadLine();
                    if (abc[2] == "标准")
                    {
                        abc[4] = sr.ReadLine();
                    }
                    upk.Add(abc[0].Replace(".krpk", ""), abc[0], abc[2], abc[3], abc[1], abc[4]);

                    sr.Close();
                }
                File.Delete(file[1]);
            }
            catch 
            {
                MessageBox.Show("T.T 安装失败！再来一次试试看吧。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }
            finally
            {
                UserPkLibRuntime.Outfile(app.PkLibfile, upk);
                MessageBox.Show("安装成功！！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
        }
     
    }
}
