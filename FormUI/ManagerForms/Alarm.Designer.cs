﻿namespace FormUI.ManagerForms
{
    partial class Alarm
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
            this.txtＭinute = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbMusic = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(113, 164);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(66, 31);
            this.btOk.TabIndex = 0;
            this.btOk.Text = "确认";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "警报时间：";
            // 
            // txtＭinute
            // 
            this.txtＭinute.Location = new System.Drawing.Point(106, 117);
            this.txtＭinute.Name = "txtＭinute";
            this.txtＭinute.Size = new System.Drawing.Size(73, 21);
            this.txtＭinute.TabIndex = 2;
            this.txtＭinute.Text = "03";
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(36, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 41);
            this.label2.TabIndex = 3;
            this.label2.Text = "说明：参数××：2位数字，取值01~99，表示警报时间，以分钟为单位。";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(24, 82);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 15;
            this.label6.Text = "语音段选择：";
            this.label6.Visible = false;
            // 
            // cmbMusic
            // 
            this.cmbMusic.DataBindings.Add(new System.Windows.Forms.Binding("Name", global::FormUI.Properties.Settings.Default, "cmbMusicR", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbMusic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMusic.FormattingEnabled = true;
            this.cmbMusic.Location = new System.Drawing.Point(106, 78);
            this.cmbMusic.Name = global::FormUI.Properties.Settings.Default.cmbMusicR;
            this.cmbMusic.Size = new System.Drawing.Size(73, 20);
            this.cmbMusic.TabIndex = 23;
            this.cmbMusic.Visible = false;
            // 
            // Alarm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(229, 207);
            this.Controls.Add(this.cmbMusic);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtＭinute);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btOk);
            this.Name = "Alarm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "警报";
            this.Load += new System.EventHandler(this.Alarm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtＭinute;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbMusic;
    }
}