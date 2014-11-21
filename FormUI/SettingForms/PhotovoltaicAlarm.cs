using System;
using System.Windows.Forms;
using FormUI.Properties;
using TomorrowSoft.BLL;

namespace FormUI.SettingForms
{
    public partial class PhotovoltaicAlarm : Form
    {
        private readonly QsService _service;

        public PhotovoltaicAlarm()
        {
            InitializeComponent();
            _service = new QsService();
        }

        private void PhotovoltaicAlarm_Load(object sender, EventArgs e)
        {
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            string sendTime = txtHh.Text + ":" + txtMm.Text + ":00";
            Settings.Default.Save();
            MessageBox.Show(_service.Add(sendTime.Trim(), txtQS.Text.Trim()) ? "设置成功！" : "设置失败!");
        }
    }
}