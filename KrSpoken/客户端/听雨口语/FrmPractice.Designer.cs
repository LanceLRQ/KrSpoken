namespace KrSpoken
{
    partial class FrmPractice
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Title");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("第1小题");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("第一大题", new System.Windows.Forms.TreeNode[] {
            treeNode2});
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("第1小题");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("第2小题");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("第3小题");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("第4小题");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("第5小题");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("第6小题");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("第7小题");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("第8小题");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("第二大题", new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10,
            treeNode11});
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("第1小题");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("第三大题", new System.Windows.Forms.TreeNode[] {
            treeNode13});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPractice));
            this.PlayerList = new System.Windows.Forms.TreeView();
            this.VideoPanel = new System.Windows.Forms.Panel();
            this.PlayContral = new System.Windows.Forms.Panel();
            this.PlayTimeLabel = new System.Windows.Forms.Label();
            this.PlayStateLabel = new System.Windows.Forms.Label();
            this.PlayTimeBar = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.PlayState = new System.Windows.Forms.PictureBox();
            this.RestartBut = new System.Windows.Forms.PictureBox();
            this.MyMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Menu_Answer = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_info = new System.Windows.Forms.ToolStripMenuItem();
            this.TipLab = new System.Windows.Forms.Label();
            this.ScriptBox = new System.Windows.Forms.TextBox();
            this.PlayContral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PlayState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RestartBut)).BeginInit();
            this.MyMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // PlayerList
            // 
            this.PlayerList.Font = new System.Drawing.Font("宋体", 9F);
            this.PlayerList.HideSelection = false;
            this.PlayerList.Location = new System.Drawing.Point(3, 4);
            this.PlayerList.Name = "PlayerList";
            treeNode1.Checked = true;
            treeNode1.Name = "pkTitle";
            treeNode1.Text = "Title";
            treeNode2.Name = "第1小题";
            treeNode2.Text = "第1小题";
            treeNode3.Name = "第一大题";
            treeNode3.Text = "第一大题";
            treeNode4.Name = "第1小题";
            treeNode4.Text = "第1小题";
            treeNode5.Name = "第2小题";
            treeNode5.Text = "第2小题";
            treeNode6.Name = "第3小题";
            treeNode6.Text = "第3小题";
            treeNode7.Name = "第4小题";
            treeNode7.Text = "第4小题";
            treeNode8.Name = "第5小题";
            treeNode8.Text = "第5小题";
            treeNode9.Name = "第6小题";
            treeNode9.Text = "第6小题";
            treeNode10.Name = "第7小题";
            treeNode10.Text = "第7小题";
            treeNode11.Name = "第8小题";
            treeNode11.Text = "第8小题";
            treeNode12.Name = "第二大题";
            treeNode12.Text = "第二大题";
            treeNode13.Name = "第1小题";
            treeNode13.Text = "第1小题";
            treeNode14.Name = "第三大题";
            treeNode14.Text = "第三大题";
            this.PlayerList.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode3,
            treeNode12,
            treeNode14});
            this.PlayerList.Size = new System.Drawing.Size(138, 254);
            this.PlayerList.TabIndex = 0;
            this.PlayerList.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.PlayerList_NodeMouseClick);
            // 
            // VideoPanel
            // 
            this.VideoPanel.BackColor = System.Drawing.Color.Transparent;
            this.VideoPanel.Location = new System.Drawing.Point(147, 700);
            this.VideoPanel.Name = "VideoPanel";
            this.VideoPanel.Size = new System.Drawing.Size(640, 480);
            this.VideoPanel.TabIndex = 6;
            // 
            // PlayContral
            // 
            this.PlayContral.BackColor = System.Drawing.Color.Transparent;
            this.PlayContral.BackgroundImage = global::KrSpoken.PlayerButton.Play_Bar;
            this.PlayContral.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PlayContral.Controls.Add(this.PlayTimeLabel);
            this.PlayContral.Controls.Add(this.PlayStateLabel);
            this.PlayContral.Controls.Add(this.PlayTimeBar);
            this.PlayContral.Controls.Add(this.PlayState);
            this.PlayContral.Controls.Add(this.RestartBut);
            this.PlayContral.Location = new System.Drawing.Point(147, 506);
            this.PlayContral.Name = "PlayContral";
            this.PlayContral.Size = new System.Drawing.Size(640, 50);
            this.PlayContral.TabIndex = 7;
            // 
            // PlayTimeLabel
            // 
            this.PlayTimeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PlayTimeLabel.Font = new System.Drawing.Font("宋体", 10F);
            this.PlayTimeLabel.ForeColor = System.Drawing.Color.White;
            this.PlayTimeLabel.Location = new System.Drawing.Point(510, 27);
            this.PlayTimeLabel.Name = "PlayTimeLabel";
            this.PlayTimeLabel.Size = new System.Drawing.Size(125, 20);
            this.PlayTimeLabel.TabIndex = 6;
            this.PlayTimeLabel.Text = "倒计时：";
            this.PlayTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PlayStateLabel
            // 
            this.PlayStateLabel.AutoSize = true;
            this.PlayStateLabel.Font = new System.Drawing.Font("宋体", 10F);
            this.PlayStateLabel.ForeColor = System.Drawing.Color.White;
            this.PlayStateLabel.Location = new System.Drawing.Point(114, 27);
            this.PlayStateLabel.Name = "PlayStateLabel";
            this.PlayStateLabel.Size = new System.Drawing.Size(63, 14);
            this.PlayStateLabel.TabIndex = 5;
            this.PlayStateLabel.Text = "播放状态";
            // 
            // PlayTimeBar
            // 
            this.PlayTimeBar.Location = new System.Drawing.Point(117, 6);
            this.PlayTimeBar.Name = "PlayTimeBar";
            this.PlayTimeBar.Size = new System.Drawing.Size(518, 18);
            this.PlayTimeBar.TabIndex = 4;
            // 
            // PlayState
            // 
            this.PlayState.Image = global::KrSpoken.PlayerButton.State_Listen;
            this.PlayState.Location = new System.Drawing.Point(81, 6);
            this.PlayState.Name = "PlayState";
            this.PlayState.Size = new System.Drawing.Size(30, 18);
            this.PlayState.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PlayState.TabIndex = 3;
            this.PlayState.TabStop = false;
            // 
            // RestartBut
            // 
            this.RestartBut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.RestartBut.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RestartBut.Image = global::KrSpoken.PlayerButton.But_RePlay_1;
            this.RestartBut.Location = new System.Drawing.Point(45, 16);
            this.RestartBut.Name = "RestartBut";
            this.RestartBut.Size = new System.Drawing.Size(30, 18);
            this.RestartBut.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.RestartBut.TabIndex = 2;
            this.RestartBut.TabStop = false;
            this.RestartBut.Tag = "重复本节（录音时不可用）";
            this.RestartBut.Click += new System.EventHandler(this.RestartBut_Click);
            this.RestartBut.MouseLeave += new System.EventHandler(this.RestartBut_MouseLeave);
            this.RestartBut.MouseHover += new System.EventHandler(this.RestartBut_MouseHover);
            // 
            // MyMenu
            // 
            this.MyMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Answer,
            this.Menu_info});
            this.MyMenu.Name = "MyMenu";
            this.MyMenu.Size = new System.Drawing.Size(185, 70);
            // 
            // Menu_Answer
            // 
            this.Menu_Answer.Name = "Menu_Answer";
            this.Menu_Answer.Size = new System.Drawing.Size(184, 22);
            this.Menu_Answer.Text = "学生答案与参考答案";
            // 
            // Menu_info
            // 
            this.Menu_info.Name = "Menu_info";
            this.Menu_info.Size = new System.Drawing.Size(184, 22);
            this.Menu_info.Text = "试题包信息";
            // 
            // TipLab
            // 
            this.TipLab.BackColor = System.Drawing.Color.Transparent;
            this.TipLab.Font = new System.Drawing.Font("宋体", 9F);
            this.TipLab.ForeColor = System.Drawing.Color.White;
            this.TipLab.Location = new System.Drawing.Point(1, 287);
            this.TipLab.Name = "TipLab";
            this.TipLab.Size = new System.Drawing.Size(140, 197);
            this.TipLab.TabIndex = 8;
            this.TipLab.Text = "小提示：\r\n\r\n1.点击上面的大题可以\r\n  切换哦。\r\n2.录音模式是不允许重复\r\n  开始和切换的，请留意\r\n3.请记得插上话筒。\r\n  录音开始时，请不要断" +
                "\r\n  开话筒与电脑的连线，\r\n  否则程序很可能会崩\r\n  溃！";
            // 
            // ScriptBox
            // 
            this.ScriptBox.Location = new System.Drawing.Point(147, 4);
            this.ScriptBox.Multiline = true;
            this.ScriptBox.Name = "ScriptBox";
            this.ScriptBox.Size = new System.Drawing.Size(640, 480);
            this.ScriptBox.TabIndex = 9;
            this.ScriptBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ScriptBox_MouseUp);
            // 
            // FrmPractice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::KrSpoken.Background.Bg_1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(794, 572);
            this.Controls.Add(this.ScriptBox);
            this.Controls.Add(this.PlayContral);
            this.Controls.Add(this.VideoPanel);
            this.Controls.Add(this.PlayerList);
            this.Controls.Add(this.TipLab);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 600);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "FrmPractice";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmPractice_Venus";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmPractice_FormClosing);
            this.Load += new System.EventHandler(this.FrmPractice_Load);
            this.PlayContral.ResumeLayout(false);
            this.PlayContral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PlayState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RestartBut)).EndInit();
            this.MyMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView PlayerList;
        private System.Windows.Forms.Panel VideoPanel;
        private System.Windows.Forms.Panel PlayContral;
        private System.Windows.Forms.Label PlayTimeLabel;
        private System.Windows.Forms.Label PlayStateLabel;
        private DevComponents.DotNetBar.Controls.ProgressBarX PlayTimeBar;
        private System.Windows.Forms.PictureBox PlayState;
        private System.Windows.Forms.PictureBox RestartBut;
        private System.Windows.Forms.ContextMenuStrip MyMenu;
        private System.Windows.Forms.ToolStripMenuItem Menu_Answer;
        private System.Windows.Forms.ToolStripMenuItem Menu_info;
        private System.Windows.Forms.Label TipLab;
        private System.Windows.Forms.TextBox ScriptBox;
    }
}