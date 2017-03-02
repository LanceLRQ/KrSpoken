namespace KrSpoken.WordAndSentences
{
    partial class FrmWord
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmWord));
            this.L_WordLab = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.L_DescLab = new System.Windows.Forms.Label();
            this.L_PronLab = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.N_WordLab = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.N_DescLab = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // L_WordLab
            // 
            this.L_WordLab.AutoSize = true;
            this.L_WordLab.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
            this.L_WordLab.Location = new System.Drawing.Point(24, 17);
            this.L_WordLab.Name = "L_WordLab";
            this.L_WordLab.Size = new System.Drawing.Size(37, 14);
            this.L_WordLab.TabIndex = 0;
            this.L_WordLab.Text = "单词";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(389, 215);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.L_DescLab);
            this.tabPage1.Controls.Add(this.L_PronLab);
            this.tabPage1.Controls.Add(this.L_WordLab);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(381, 189);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "本地数据库";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(24, 160);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(339, 27);
            this.label1.TabIndex = 3;
            this.label1.Text = "Tip:由于库里边没有单词过去式、现在分词、过去分词这类的\r\n    单词，所以找不到的话去百度词典看看吧。";
            // 
            // L_DescLab
            // 
            this.L_DescLab.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.L_DescLab.Font = new System.Drawing.Font("宋体", 10F);
            this.L_DescLab.Location = new System.Drawing.Point(24, 68);
            this.L_DescLab.Margin = new System.Windows.Forms.Padding(3);
            this.L_DescLab.Name = "L_DescLab";
            this.L_DescLab.Size = new System.Drawing.Size(339, 86);
            this.L_DescLab.TabIndex = 2;
            this.L_DescLab.Text = "解释";
            // 
            // L_PronLab
            // 
            this.L_PronLab.AutoSize = true;
            this.L_PronLab.Font = new System.Drawing.Font("宋体", 9F);
            this.L_PronLab.Location = new System.Drawing.Point(24, 41);
            this.L_PronLab.Name = "L_PronLab";
            this.L_PronLab.Size = new System.Drawing.Size(29, 12);
            this.L_PronLab.TabIndex = 1;
            this.L_PronLab.Text = "音标";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.N_DescLab);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.N_WordLab);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(381, 189);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "百度词典";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // N_WordLab
            // 
            this.N_WordLab.AutoSize = true;
            this.N_WordLab.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
            this.N_WordLab.Location = new System.Drawing.Point(24, 17);
            this.N_WordLab.Name = "N_WordLab";
            this.N_WordLab.Size = new System.Drawing.Size(37, 14);
            this.N_WordLab.TabIndex = 3;
            this.N_WordLab.Text = "单词";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(296, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(67, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "取消";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // N_DescLab
            // 
            this.N_DescLab.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.N_DescLab.Location = new System.Drawing.Point(27, 49);
            this.N_DescLab.Multiline = true;
            this.N_DescLab.Name = "N_DescLab";
            this.N_DescLab.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.N_DescLab.Size = new System.Drawing.Size(327, 119);
            this.N_DescLab.TabIndex = 7;
            // 
            // FrmWord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 222);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmWord";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "划词搜索";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmWord_FormClosing);
            this.Load += new System.EventHandler(this.FrmWord_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label L_WordLab;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label L_DescLab;
        private System.Windows.Forms.Label L_PronLab;
        private System.Windows.Forms.Label N_WordLab;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox N_DescLab;
    }
}