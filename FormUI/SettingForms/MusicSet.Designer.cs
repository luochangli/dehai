namespace FormUI.SettingForms
{
    partial class MusicSet
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
            this.btOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPlayTime = new System.Windows.Forms.TextBox();
            this.cbPlayStlye = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.lbSelectNum = new System.Windows.Forms.Label();
            this.btCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(107, 185);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(64, 43);
            this.btOk.TabIndex = 0;
            this.btOk.Text = "确定";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "音乐选择：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "播放模式：";
            // 
            // tbPlayTime
            // 
            this.tbPlayTime.Location = new System.Drawing.Point(107, 133);
            this.tbPlayTime.Name = "tbPlayTime";
            this.tbPlayTime.Size = new System.Drawing.Size(100, 21);
            this.tbPlayTime.TabIndex = 3;
            this.tbPlayTime.Text = "03";
            // 
            // cbPlayStlye
            // 
            this.cbPlayStlye.FormattingEnabled = true;
            this.cbPlayStlye.Location = new System.Drawing.Point(106, 96);
            this.cbPlayStlye.Name = "cbPlayStlye";
            this.cbPlayStlye.Size = new System.Drawing.Size(101, 20);
            this.cbPlayStlye.TabIndex = 5;
            this.cbPlayStlye.SelectedIndexChanged += new System.EventHandler(this.cbPlayStlye_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "播放时间：";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(106, 56);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(101, 21);
            this.numericUpDown1.TabIndex = 7;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lbSelectNum
            // 
            this.lbSelectNum.AutoSize = true;
            this.lbSelectNum.Location = new System.Drawing.Point(37, 23);
            this.lbSelectNum.Name = "lbSelectNum";
            this.lbSelectNum.Size = new System.Drawing.Size(0, 12);
            this.lbSelectNum.TabIndex = 8;
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(197, 185);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(64, 43);
            this.btCancel.TabIndex = 9;
            this.btCancel.Text = "取消";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // MusicSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 266);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.lbSelectNum);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbPlayStlye);
            this.Controls.Add(this.tbPlayTime);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btOk);
            this.MaximizeBox = false;
            this.Name = "MusicSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "自定义音乐播放";
            this.Load += new System.EventHandler(this.MusicSet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbPlayTime;
        private System.Windows.Forms.ComboBox cbPlayStlye;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label lbSelectNum;
        private System.Windows.Forms.Button btCancel;
    }
}