namespace KrSpoken
{
    partial class FrmUserReg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUserReg));
            this.panel1 = new System.Windows.Forms.Panel();
            this.ButOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.HowStrong = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.NiName = new System.Windows.Forms.TextBox();
            this.RePassword = new System.Windows.Forms.TextBox();
            this.Password = new System.Windows.Forms.TextBox();
            this.Email = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.Phone = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.Ds_Name = new System.Windows.Forms.TextBox();
            this.Ds_Year = new System.Windows.Forms.ComboBox();
            this.Ds_Class = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.ButOk);
            this.panel1.Location = new System.Drawing.Point(-18, 333);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(340, 68);
            this.panel1.TabIndex = 3;
            // 
            // ButOk
            // 
            this.ButOk.Location = new System.Drawing.Point(122, 7);
            this.ButOk.Name = "ButOk";
            this.ButOk.Size = new System.Drawing.Size(77, 28);
            this.ButOk.TabIndex = 8;
            this.ButOk.Text = "注册";
            this.ButOk.UseVisualStyleBackColor = true;
            this.ButOk.Click += new System.EventHandler(this.ButOk_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "Email：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "密码：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "重复密码：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "昵称：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "真实姓名：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.HowStrong);
            this.groupBox1.Controls.Add(this.NiName);
            this.groupBox1.Controls.Add(this.RePassword);
            this.groupBox1.Controls.Add(this.Password);
            this.groupBox1.Controls.Add(this.Email);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(7, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(279, 166);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "用户信息（本软件只授权东中的学生）";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Location = new System.Drawing.Point(77, 150);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 19;
            this.label10.Text = "(可不填)";
            // 
            // HowStrong
            // 
            this.HowStrong.Location = new System.Drawing.Point(78, 75);
            this.HowStrong.Maximum = 3;
            this.HowStrong.Name = "HowStrong";
            this.HowStrong.Size = new System.Drawing.Size(148, 18);
            this.HowStrong.TabIndex = 18;
            this.HowStrong.Text = "请先输入密码";
            this.HowStrong.TextVisible = true;
            // 
            // NiName
            // 
            this.NiName.Location = new System.Drawing.Point(79, 126);
            this.NiName.MaxLength = 12;
            this.NiName.Name = "NiName";
            this.NiName.Size = new System.Drawing.Size(147, 21);
            this.NiName.TabIndex = 3;
            this.NiName.Leave += new System.EventHandler(this.NiName_Leave);
            // 
            // RePassword
            // 
            this.RePassword.Location = new System.Drawing.Point(79, 100);
            this.RePassword.MaxLength = 30;
            this.RePassword.Name = "RePassword";
            this.RePassword.PasswordChar = '*';
            this.RePassword.Size = new System.Drawing.Size(147, 21);
            this.RePassword.TabIndex = 2;
            this.RePassword.Leave += new System.EventHandler(this.RePassword_Leave);
            // 
            // Password
            // 
            this.Password.Location = new System.Drawing.Point(79, 48);
            this.Password.MaxLength = 30;
            this.Password.Name = "Password";
            this.Password.PasswordChar = '*';
            this.Password.Size = new System.Drawing.Size(147, 21);
            this.Password.TabIndex = 1;
            this.Password.Leave += new System.EventHandler(this.Password_Leave);
            // 
            // Email
            // 
            this.Email.Location = new System.Drawing.Point(79, 23);
            this.Email.MaxLength = 30;
            this.Email.Name = "Email";
            this.Email.Size = new System.Drawing.Size(147, 21);
            this.Email.TabIndex = 0;
            this.Email.Leave += new System.EventHandler(this.Email_Leave);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.Phone);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.Ds_Name);
            this.groupBox2.Controls.Add(this.Ds_Year);
            this.groupBox2.Controls.Add(this.Ds_Class);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(7, 186);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(277, 141);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "个人信息（必填，我们一定保护您的隐私）";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(203, 115);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 16;
            this.label9.Text = "(可不填)";
            // 
            // Phone
            // 
            this.Phone.Location = new System.Drawing.Point(78, 110);
            this.Phone.MaxLength = 11;
            this.Phone.Name = "Phone";
            this.Phone.Size = new System.Drawing.Size(119, 21);
            this.Phone.TabIndex = 7;
            this.Phone.TextChanged += new System.EventHandler(this.Phone_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 115);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 14;
            this.label8.Text = "联系手机：";
            // 
            // Ds_Name
            // 
            this.Ds_Name.Location = new System.Drawing.Point(78, 28);
            this.Ds_Name.MaxLength = 5;
            this.Ds_Name.Name = "Ds_Name";
            this.Ds_Name.Size = new System.Drawing.Size(77, 21);
            this.Ds_Name.TabIndex = 4;
            this.Ds_Name.Leave += new System.EventHandler(this.Ds_Name_Leave);
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
            this.Ds_Year.Location = new System.Drawing.Point(78, 56);
            this.Ds_Year.Name = "Ds_Year";
            this.Ds_Year.Size = new System.Drawing.Size(77, 20);
            this.Ds_Year.TabIndex = 5;
            // 
            // Ds_Class
            // 
            this.Ds_Class.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Ds_Class.FormattingEnabled = true;
            this.Ds_Class.Location = new System.Drawing.Point(78, 83);
            this.Ds_Class.Name = "Ds_Class";
            this.Ds_Class.Size = new System.Drawing.Size(77, 20);
            this.Ds_Class.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 87);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 10;
            this.label7.Text = "所在班级：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 59);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "入学年份：";
            // 
            // FrmUserReg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 372);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 400);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 400);
            this.Name = "FrmUserReg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "用户注册";
            this.Load += new System.EventHandler(this.FrmUserReg_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ButOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox Ds_Year;
        private System.Windows.Forms.ComboBox Ds_Class;
        private System.Windows.Forms.TextBox Phone;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox Ds_Name;
        private System.Windows.Forms.TextBox Email;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox RePassword;
        private System.Windows.Forms.TextBox Password;
        private System.Windows.Forms.TextBox NiName;
        private DevComponents.DotNetBar.Controls.ProgressBarX HowStrong;
        private System.Windows.Forms.Label label10;
    }
}