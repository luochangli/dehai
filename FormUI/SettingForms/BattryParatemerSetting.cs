using System;
using System.Windows.Forms;

namespace FormUI.SettingForms
{
    public partial class BattryParatemerSetting : Form
    {
        public BattryParatemerSetting()
        {
            InitializeComponent();
        }

        public string InMax { get; set; }
        public string InMin { get; set; }
        public string OutMax { get; set; }
        public string OutMin { get; set; }

        private void btTabParamOK_Click(object sender, EventArgs e)
        {
            InMax = ChargeMax.Text;
            InMin = ChargeMin.Text;
            OutMax = DisChargeMax.Text;
            OutMin = DisChargeMin.Text;
            if (InMax == string.Empty || InMin == string.Empty || OutMax == string.Empty ||
                OutMin == string.Empty)
            {
                MessageBox.Show("请填写完整");
                return;
            }
            DialogResult = DialogResult.OK;
        }
    }
}