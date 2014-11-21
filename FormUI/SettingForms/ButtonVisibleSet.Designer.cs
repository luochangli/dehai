namespace FormUI.SettingForms
{
    partial class ButtonVisibleSet
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
            this.btSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.cbR5 = new System.Windows.Forms.CheckBox();
            this.cbR4 = new System.Windows.Forms.CheckBox();
            this.cbR3 = new System.Windows.Forms.CheckBox();
            this.cbR2 = new System.Windows.Forms.CheckBox();
            this.cbR1 = new System.Windows.Forms.CheckBox();
            this.cb5 = new System.Windows.Forms.CheckBox();
            this.cb4 = new System.Windows.Forms.CheckBox();
            this.cb3 = new System.Windows.Forms.CheckBox();
            this.cb2 = new System.Windows.Forms.CheckBox();
            this.cb1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(154, 160);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(60, 42);
            this.btSave.TabIndex = 14;
            this.btSave.Text = "保存";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 138);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 15;
            this.label1.Text = "泄洪：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(152, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 17;
            this.label2.Text = "排渣：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 182);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 19;
            this.label3.Text = "发电：";
            // 
            // textBox3
            // 
            this.textBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::FormUI.Properties.Settings.Default, "alarm3", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox3.Location = new System.Drawing.Point(74, 177);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(48, 21);
            this.textBox3.TabIndex = 20;
            this.textBox3.Text = global::FormUI.Properties.Settings.Default.alarm3;
            // 
            // textBox2
            // 
            this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::FormUI.Properties.Settings.Default, "alarm2", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox2.Location = new System.Drawing.Point(200, 133);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(48, 21);
            this.textBox2.TabIndex = 18;
            this.textBox2.Text = global::FormUI.Properties.Settings.Default.alarm2;
            // 
            // textBox1
            // 
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::FormUI.Properties.Settings.Default, "alarm1", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox1.Location = new System.Drawing.Point(74, 133);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(48, 21);
            this.textBox1.TabIndex = 16;
            this.textBox1.Text = global::FormUI.Properties.Settings.Default.alarm1;
            // 
            // cbR5
            // 
            this.cbR5.AutoSize = true;
            this.cbR5.Checked = global::FormUI.Properties.Settings.Default.cbR5;
            this.cbR5.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FormUI.Properties.Settings.Default, "cbR5", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbR5.Location = new System.Drawing.Point(150, 101);
            this.cbR5.Name = "cbR5";
            this.cbR5.Size = new System.Drawing.Size(48, 16);
            this.cbR5.TabIndex = 13;
            this.cbR5.Text = "警报";
            this.cbR5.UseVisualStyleBackColor = true;
            // 
            // cbR4
            // 
            this.cbR4.AutoSize = true;
            this.cbR4.Checked = global::FormUI.Properties.Settings.Default.cbR4;
            this.cbR4.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FormUI.Properties.Settings.Default, "cbR4", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbR4.Location = new System.Drawing.Point(150, 79);
            this.cbR4.Name = "cbR4";
            this.cbR4.Size = new System.Drawing.Size(48, 16);
            this.cbR4.TabIndex = 12;
            this.cbR4.Text = "检测";
            this.cbR4.UseVisualStyleBackColor = true;
            // 
            // cbR3
            // 
            this.cbR3.AutoSize = true;
            this.cbR3.Checked = global::FormUI.Properties.Settings.Default.cbR3;
            this.cbR3.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FormUI.Properties.Settings.Default, "cbR3", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbR3.Location = new System.Drawing.Point(150, 57);
            this.cbR3.Name = "cbR3";
            this.cbR3.Size = new System.Drawing.Size(48, 16);
            this.cbR3.TabIndex = 11;
            this.cbR3.Text = "音乐";
            this.cbR3.UseVisualStyleBackColor = true;
            // 
            // cbR2
            // 
            this.cbR2.AutoSize = true;
            this.cbR2.Checked = global::FormUI.Properties.Settings.Default.cbR2;
            this.cbR2.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FormUI.Properties.Settings.Default, "cbR2", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbR2.Location = new System.Drawing.Point(150, 35);
            this.cbR2.Name = "cbR2";
            this.cbR2.Size = new System.Drawing.Size(48, 16);
            this.cbR2.TabIndex = 10;
            this.cbR2.Text = "停播";
            this.cbR2.UseVisualStyleBackColor = true;
            // 
            // cbR1
            // 
            this.cbR1.AutoSize = true;
            this.cbR1.Checked = global::FormUI.Properties.Settings.Default.cbR1;
            this.cbR1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FormUI.Properties.Settings.Default, "cbR1", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbR1.Location = new System.Drawing.Point(150, 13);
            this.cbR1.Name = "cbR1";
            this.cbR1.Size = new System.Drawing.Size(48, 16);
            this.cbR1.TabIndex = 9;
            this.cbR1.Text = "查询";
            this.cbR1.UseVisualStyleBackColor = true;
            // 
            // cb5
            // 
            this.cb5.AutoSize = true;
            this.cb5.Checked = global::FormUI.Properties.Settings.Default.cbL5;
            this.cb5.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FormUI.Properties.Settings.Default, "cbL5", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb5.Location = new System.Drawing.Point(42, 101);
            this.cb5.Name = "cb5";
            this.cb5.Size = new System.Drawing.Size(72, 16);
            this.cb5.TabIndex = 8;
            this.cb5.Text = "记忆组播";
            this.cb5.UseVisualStyleBackColor = true;
            // 
            // cb4
            // 
            this.cb4.AutoSize = true;
            this.cb4.Checked = global::FormUI.Properties.Settings.Default.cbL4;
            this.cb4.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FormUI.Properties.Settings.Default, "cbL4", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb4.Location = new System.Drawing.Point(42, 79);
            this.cb4.Name = "cb4";
            this.cb4.Size = new System.Drawing.Size(72, 16);
            this.cb4.TabIndex = 6;
            this.cb4.Text = "文本广播";
            this.cb4.UseVisualStyleBackColor = true;
            // 
            // cb3
            // 
            this.cb3.AutoSize = true;
            this.cb3.Checked = global::FormUI.Properties.Settings.Default.cbL3;
            this.cb3.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FormUI.Properties.Settings.Default, "cbL3", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb3.Location = new System.Drawing.Point(42, 57);
            this.cb3.Name = "cb3";
            this.cb3.Size = new System.Drawing.Size(72, 16);
            this.cb3.TabIndex = 4;
            this.cb3.Text = "发电告警";
            this.cb3.UseVisualStyleBackColor = true;
            // 
            // cb2
            // 
            this.cb2.AutoSize = true;
            this.cb2.Checked = global::FormUI.Properties.Settings.Default.cbL2;
            this.cb2.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FormUI.Properties.Settings.Default, "cbL2", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb2.Location = new System.Drawing.Point(42, 35);
            this.cb2.Name = "cb2";
            this.cb2.Size = new System.Drawing.Size(72, 16);
            this.cb2.TabIndex = 2;
            this.cb2.Text = "排渣告警";
            this.cb2.UseVisualStyleBackColor = true;
            // 
            // cb1
            // 
            this.cb1.AutoSize = true;
            this.cb1.Checked = global::FormUI.Properties.Settings.Default.cbL1;
            this.cb1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::FormUI.Properties.Settings.Default, "cbL1", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb1.Location = new System.Drawing.Point(42, 13);
            this.cb1.Name = "cb1";
            this.cb1.Size = new System.Drawing.Size(72, 16);
            this.cb1.TabIndex = 0;
            this.cb1.Text = "泄洪告警";
            this.cb1.UseVisualStyleBackColor = true;
            // 
            // ButtonVisibleSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 221);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.cbR5);
            this.Controls.Add(this.cbR4);
            this.Controls.Add(this.cbR3);
            this.Controls.Add(this.cbR2);
            this.Controls.Add(this.cbR1);
            this.Controls.Add(this.cb5);
            this.Controls.Add(this.cb4);
            this.Controls.Add(this.cb3);
            this.Controls.Add(this.cb2);
            this.Controls.Add(this.cb1);
            this.MaximizeBox = false;
            this.Name = "ButtonVisibleSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "主界面功能按钮显示隐藏";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cb1;
        private System.Windows.Forms.CheckBox cb2;
        private System.Windows.Forms.CheckBox cb3;
        private System.Windows.Forms.CheckBox cb5;
        private System.Windows.Forms.CheckBox cb4;
        private System.Windows.Forms.CheckBox cbR5;
        private System.Windows.Forms.CheckBox cbR4;
        private System.Windows.Forms.CheckBox cbR3;
        private System.Windows.Forms.CheckBox cbR2;
        private System.Windows.Forms.CheckBox cbR1;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label3;
    }
}