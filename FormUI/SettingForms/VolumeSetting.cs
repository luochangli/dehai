using System;
using System.Windows.Forms;
using FormUI.OperationLayer;

namespace FormUI.SettingForms
{
    public partial class VolumeSetting : Form
    {
        public VolumeSetting()
        {
            InitializeComponent();
            OrderDefinition.SetVoice(cbVol);
        }

        public string Volume { get; set; }

        private void btVolumeSettingOK_Click(object sender, EventArgs e)
        {
            Volume = cbVol.Text;
            DialogResult = DialogResult.OK;
        }
    }
}