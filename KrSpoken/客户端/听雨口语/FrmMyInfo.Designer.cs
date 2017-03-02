namespace KrSpoken
{
    partial class FrmMyInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMyInfo));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.Phone = new System.Windows.Forms.TextBox();
            this.NiName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.Ds_Year = new System.Windows.Forms.ComboBox();
            this.Ds_Class = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.DS_Name = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.OldPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.HowStrong = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.RePassword = new System.Windows.Forms.TextBox();
            this.Password = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Email = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ButOk = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.Phone);
            this.groupBox2.Controls.Add(this.NiName);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.Ds_Year);
            this.groupBox2.Controls.Add(this.Ds_Class);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.DS_Name);
            this.groupBox2.Location = new System.Drawing.Point(297, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(285, 166);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "个人信息（我们一定保护您的隐私）";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Location = new System.Drawing.Point(226, 28);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 19;
            this.label10.Text = "(可不填)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(202, 139);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 16;
            this.label9.Text = "(可不填)";
            // 
            // Phone
            // 
            this.Phone.Location = new System.Drawing.Point(77, 136);
            this.Phone.MaxLength = 11;
            this.Phone.Name = "Phone";
            this.Phone.Size = new System.Drawing.Size(119, 21);
            this.Phone.TabIndex = 6;
            this.Phone.TextChanged += new System.EventHandler(this.Phone_TextChanged);
            // 
            // NiName
            // 
            this.NiName.Location = new System.Drawing.Point(77, 25);
            this.NiName.MaxLength = 12;
            this.NiName.Name = "NiName";
            this.NiName.Size = new System.Drawing.Size(147, 21);
            this.NiName.TabIndex = 3;
            this.NiName.Leave += new System.EventHandler(this.NiName_Leave);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 141);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 14;
            this.label8.Text = "联系手机：";
            // 
            // Ds_Year
            // 
            this.Ds_Year.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Ds_Year.FormattingEnabled = true;
            this.Ds_Year.Items.AddRange(new object[] {
            "2007",
            "2008",
            "2009",
            "2010",
            "2011",
            "2012",
            "2013",
            "2014",
            "2015"});
            this.Ds_Year.Location = new System.Drawing.Point(77, 82);
            this.Ds_Year.Name = "Ds_Year";
            this.Ds_Year.Size = new System.Drawing.Size(77, 20);
            this.Ds_Year.TabIndex = 4;
            // 
            // Ds_Class
            // 
            this.Ds_Class.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Ds_Class.FormattingEnabled = true;
            this.Ds_Class.Location = new System.Drawing.Point(77, 109);
            this.Ds_Class.Name = "Ds_Class";
            this.Ds_Class.Size = new System.Drawing.Size(77, 20);
            this.Ds_Class.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "昵称：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 113);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 10;
            this.label7.Text = "所在班级：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 85);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "入学年份：";
            // 
            // DS_Name
            // 
            this.DS_Name.AutoSize = true;
            this.DS_Name.Location = new System.Drawing.Point(6, 57);
            this.DS_Name.Name = "DS_Name";
            this.DS_Name.Size = new System.Drawing.Size(65, 12);
            this.DS_Name.TabIndex = 8;
            this.DS_Name.Text = "真实姓名：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.OldPassword);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.HowStrong);
            this.groupBox1.Controls.Add(this.RePassword);
            this.groupBox1.Controls.Add(this.Password);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.Email);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(268, 166);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "用户信息";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(77, 37);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(149, 12);
            this.label11.TabIndex = 21;
            this.label11.Text = "（如密码无须修改请留空）";
            // 
            // OldPassword
            // 
            this.OldPassword.Location = new System.Drawing.Point(79, 52);
            this.OldPassword.MaxLength = 30;
            this.OldPassword.Name = "OldPassword";
            this.OldPassword.PasswordChar = '*';
            this.OldPassword.Size = new System.Drawing.Size(147, 21);
            this.OldPassword.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 19;
            this.label1.Text = "旧密码：";
            // 
            // HowStrong
            // 
            this.HowStrong.Location = new System.Drawing.Point(78, 106);
            this.HowStrong.Maximum = 3;
            this.HowStrong.Name = "HowStrong";
            this.HowStrong.Size = new System.Drawing.Size(148, 18);
            this.HowStrong.TabIndex = 18;
            this.HowStrong.Text = "请先输入密码";
            this.HowStrong.TextVisible = true;
            // 
            // RePassword
            // 
            this.RePassword.Location = new System.Drawing.Point(79, 131);
            this.RePassword.MaxLength = 30;
            this.RePassword.Name = "RePassword";
            this.RePassword.PasswordChar = '*';
            this.RePassword.Size = new System.Drawing.Size(147, 21);
            this.RePassword.TabIndex = 2;
            this.RePassword.TextChanged += new System.EventHandler(this.RePassword_Changed);
            this.RePassword.Leave += new System.EventHandler(this.RePassword_Leave);
            // 
            // Password
            // 
            this.Password.Location = new System.Drawing.Point(79, 79);
            this.Password.MaxLength = 30;
            this.Password.Name = "Password";
            this.Password.PasswordChar = '*';
            this.Password.Size = new System.Drawing.Size(147, 21);
            this.Password.TabIndex = 1;
            this.Password.TextChanged += new System.EventHandler(this.Password_Change);
            this.Password.Leave += new System.EventHandler(this.Password_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "重复密码：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "密码：";
            // 
            // Email
            // 
            this.Email.AutoSize = true;
            this.Email.Location = new System.Drawing.Point(26, 17);
            this.Email.Name = "Email";
            this.Email.Size = new System.Drawing.Size(47, 12);
            this.Email.TabIndex = 4;
            this.Email.Text = "Email：";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.ButOk);
            this.panel1.Location = new System.Drawing.Point(-12, 239);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(618, 108);
            this.panel1.TabIndex = 13;
            // 
            // ButOk
            // 
            this.ButOk.Location = new System.Drawing.Point(250, 10);
            this.ButOk.Name = "ButOk";
            this.ButOk.Size = new System.Drawing.Size(110, 37);
            this.ButOk.TabIndex = 7;
            this.ButOk.Text = "保存";
            this.ButOk.UseVisualStyleBackColor = true;
            this.ButOk.Click += new System.EventHandler(this.ButOK_Click);
            // 
            // FrmMyInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 298);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 326);
            this.MinimumSize = new System.Drawing.Size(600, 326);
            this.Name = "FrmMyInfo";
            this.Opacity = 0.9D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "用户信息";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMyInfo_FormClosing);
            this.Load += new System.EventHandler(this.FrmMyInfo_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox Phone;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox Ds_Year;
        private System.Windows.Forms.ComboBox Ds_Class;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label DS_Name;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label10;
        private DevComponents.DotNetBar.Controls.ProgressBarX HowStrong;
        private System.Windows.Forms.TextBox NiName;
        private System.Windows.Forms.TextBox RePassword;
        private System.Windows.Forms.TextBox Password;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Email;
        private System.Windows.Forms.TextBox OldPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ButOk;
    }
}