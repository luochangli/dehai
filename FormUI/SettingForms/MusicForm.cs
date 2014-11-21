using System;
using System.Windows.Forms;
using FormUI.OperationLayer;

namespace FormUI.SettingForms
{
    public partial class MusicForm : Form
    {
        public MusicForm()
        {
            InitializeComponent();
        }

        public string Music { get; set; }
        public string Style { get; set; }
        public string Time { get; set; }


        private void btLinkOne_Click(object sender, EventArgs e)
        {
            Music = cbMusicList.Text;
            Style = cbPlayStyle.SelectedIndex.ToString();
            Time = txtTime.Text;
            DialogResult = DialogResult.OK;
        }


        private void MusicForm_Load(object sender, EventArgs e)
        {
            OrderDefinition.SetMusicNo(cbMusicList);
            if (cbMusicList.Text == string.Empty)
            {
                cbMusicList.Text = MusicNo.语音1.ToString("D");
            }
            OrderDefinition.SetPlayStyle(cbPlayStyle);
            if (cbPlayStyle.Text == string.Empty)
            {
                cbPlayStyle.Text = PlayStyle.单曲循环.ToString();
            }
        }
    }
}