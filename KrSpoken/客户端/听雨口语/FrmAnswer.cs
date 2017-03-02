using System;
using System.Windows.Forms;
using KrSpoken.Profile;
using System.IO;

namespace KrSpoken
{
    public partial class FrmAnswer : Form
    {
        public FrmMain frmMain;
        public UserPkLib UserPkList;
        public AppProfileClass AppProfile;
        public FrmAnswer()
        {
            InitializeComponent();
        }

        private void FrmAnswer_Load(object sender, EventArgs e)
        {
            PKList.Items.Clear();
            foreach (System.Collections.DictionaryEntry objDE in UserPkList._PackTitle)
            {
                string PKName = objDE.Value.ToString();
                PKList.Items.Add(PKName);
                PKList.Items[PKList.Items.Count - 1].SubItems.Add(UserPkList.get_PackType(PKName));
                PKList.Items[PKList.Items.Count - 1].SubItems.Add(UserPkList.get_PackDiff(PKName));
                PKList.Items[PKList.Items.Count - 1].SubItems.Add(Convert.ToString(UserPkList.get_PackExamTime(PKName)));
                PKList.Items[PKList.Items.Count - 1].SubItems.Add(Convert.ToString(UserPkList.get_PackPracticeTime(PKName)));
            }
        }

        private void PKList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {

                listView1.Items.Clear();
                if (Directory.Exists(AppProfile.ComputerAnswerDir + "\\" + e.Item.Text))
                {
                    listView1.Items.Add("参考答案");
                }
                try
                {
                    string[] dirs = Directory.GetDirectories(AppProfile.StudentAnswerDir + "\\" + e.Item.Text);
                    foreach (string aa in dirs)
                    {
                        listView1.Items.Add(aa.Replace(AppProfile.StudentAnswerDir + "\\" + e.Item.Text + "\\", ""));
                    }
                }
                catch
                {
                    MessageBox.Show("可能您没有使用过这个试题包，或是文件夹不存在。", "放心，这个问题无碍使用。", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                listView1.Items.Clear();
                listView2.Items.Clear();
            }
        }

        private void listView2_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                Player1.URL= AppProfile.StudentAnswerDir + "\\" + PKList.SelectedItems[0].Text + "\\" + listView1.SelectedItems[0].Text + "\\"  +e.Item.Text;
                Player1.Ctlcontrols.play();
            }
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                listView2.Items.Clear();
                if (e.Item.Text=="参考答案")
                {
                    string[] dirs = Directory.GetFiles(AppProfile.ComputerAnswerDir + "\\" + PKList.SelectedItems[0].Text);
                        foreach (string aa in dirs)
                        {
                            listView2.Items.Add(aa.Replace(AppProfile.ComputerAnswerDir + "\\" + PKList.SelectedItems[0].Text + "\\", ""));
                        }
                }
                else
                {
                    try
                    {
                        string[] dirs = Directory.GetFiles(AppProfile.StudentAnswerDir + "\\" + PKList.SelectedItems[0].Text + "\\" + e.Item.Text);
                        foreach (string aa in dirs)
                        {
                            listView2.Items.Add(aa.Replace(AppProfile.StudentAnswerDir + "\\" + PKList.SelectedItems[0].Text + "\\" + e.Item.Text + "\\", ""));
                        }
                    }
                    catch
                    {
                        MessageBox.Show("可能您没有保存过录音文件。", "放心，这个问题无碍使用。", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            else
            {
                listView2.Items.Clear();
            }
        }

        private void FrmAnswer_FormClosing(object sender, FormClosingEventArgs e)
        {
            Player1.Ctlcontrols.stop();
        }
    }
}
