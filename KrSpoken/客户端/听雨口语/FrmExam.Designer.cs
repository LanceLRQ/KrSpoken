namespace KrSpoken
{
    partial class FrmExam
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmExam));
            this.VideoPanel = new System.Windows.Forms.Panel();
            this.PlayContral = new System.Windows.Forms.Panel();
            this.PlayTimeLabel = new System.Windows.Forms.Label();
            this.PlayStateLabel = new System.Windows.Forms.Label();
            this.PlayTimeBar = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.PlayState = new System.Windows.Forms.PictureBox();
            this.MyMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Menu_Answer = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_info = new System.Windows.Forms.ToolStripMenuItem();
            this.ScriptBox = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Lab_Kaohao = new System.Windows.Forms.Label();
            this.LabName = new System.Windows.Forms.Label();
            this.PlayContral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PlayState)).BeginInit();
            this.MyMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
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
            this.PlayStateLabel.Location = new System.Drawing.Point(33, 27);
            this.PlayStateLabel.Name = "PlayStateLabel";
            this.PlayStateLabel.Size = new System.Drawing.Size(63, 14);
            this.PlayStateLabel.TabIndex = 5;
            this.PlayStateLabel.Text = "播放状态";
            // 
            // PlayTimeBar
            // 
            this.PlayTimeBar.Location = new System.Drawing.Point(36, 6);
            this.PlayTimeBar.Name = "PlayTimeBar";
            this.PlayTimeBar.Size = new System.Drawing.Size(599, 18);
            this.PlayTimeBar.TabIndex = 4;
            // 
            // PlayState
            // 
            this.PlayState.Image = global::KrSpoken.PlayerButton.State_Listen;
            this.PlayState.Location = new System.Drawing.Point(0, 6);
            this.PlayState.Name = "PlayState";
            this.PlayState.Size = new System.Drawing.Size(30, 18);
            this.PlayState.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PlayState.TabIndex = 3;
            this.PlayState.TabStop = false;
            // 
            // MyMenu
            // 
            this.MyMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Answer,
            this.Menu_info});
            this.MyMenu.Name = "MyMenu";
            this.MyMenu.Size = new System.Drawing.Size(185, 48);
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
            // ScriptBox
            // 
            this.ScriptBox.Font = new System.Drawing.Font("宋体", 17F);
            this.ScriptBox.Location = new System.Drawing.Point(147, 4);
            this.ScriptBox.Multiline = true;
            this.ScriptBox.Name = "ScriptBox";
            this.ScriptBox.Size = new System.Drawing.Size(640, 480);
            this.ScriptBox.TabIndex = 9;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Image = global::KrSpoken.MainButton.NoBody;
            this.pictureBox1.Location = new System.Drawing.Point(22, 34);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // Lab_Kaohao
            // 
            this.Lab_Kaohao.AutoSize = true;
            this.Lab_Kaohao.BackColor = System.Drawing.Color.Transparent;
            this.Lab_Kaohao.Font = new System.Drawing.Font("宋体", 10F);
            this.Lab_Kaohao.ForeColor = System.Drawing.Color.White;
            this.Lab_Kaohao.Location = new System.Drawing.Point(12, 173);
            this.Lab_Kaohao.Name = "Lab_Kaohao";
            this.Lab_Kaohao.Size = new System.Drawing.Size(49, 14);
            this.Lab_Kaohao.TabIndex = 11;
            this.Lab_Kaohao.Text = "考号：";
            // 
            // LabName
            // 
            this.LabName.AutoSize = true;
            this.LabName.BackColor = System.Drawing.Color.Transparent;
            this.LabName.Font = new System.Drawing.Font("宋体", 12F);
            this.LabName.ForeColor = System.Drawing.Color.White;
            this.LabName.Location = new System.Drawing.Point(51, 139);
            this.LabName.Name = "LabName";
            this.LabName.Size = new System.Drawing.Size(40, 16);
            this.LabName.TabIndex = 12;
            this.LabName.Text = "张三";
            // 
            // FrmExam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::KrSpoken.Background.Bg_1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(794, 572);
            this.Controls.Add(this.ScriptBox);
            this.Controls.Add(this.LabName);
            this.Controls.Add(this.Lab_Kaohao);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.PlayContral);
            this.Controls.Add(this.VideoPanel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 600);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "FrmExam";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmPractice_Venus";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmPractice_FormClosing);
            this.Load += new System.EventHandler(this.FrmPractice_Load);
            this.PlayContral.ResumeLayout(false);
            this.PlayContral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PlayState)).EndInit();
            this.MyMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel VideoPanel;
        private System.Windows.Forms.Panel PlayContral;
        private System.Windows.Forms.Label PlayTimeLabel;
        private System.Windows.Forms.Label PlayStateLabel;
        private DevComponents.DotNetBar.Controls.ProgressBarX PlayTimeBar;
        private System.Windows.Forms.PictureBox PlayState;
        private System.Windows.Forms.ContextMenuStrip MyMenu;
        private System.Windows.Forms.ToolStripMenuItem Menu_Answer;
        private System.Windows.Forms.ToolStripMenuItem Menu_info;
        private System.Windows.Forms.TextBox ScriptBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label Lab_Kaohao;
        private System.Windows.Forms.Label LabName;
    }
}