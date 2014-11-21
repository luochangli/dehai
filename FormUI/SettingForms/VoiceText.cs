using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FormUI.OperationLayer;
using Infrastructure;

namespace FormUI.SettingForms
{
    public partial class VoiceText : Form
    {
        private readonly IList<ListViewItem> Terminals;
        private readonly OrderDefinition _order;

        public VoiceText(IList<ListViewItem> terminals)
        {
            InitializeComponent();
            txtPlayTimes.KeyPress += Handler.Nuber09;
            _order = new OrderDefinition();
            Terminals = terminals;
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            if (txtVoiceText.MaxLength <= 139 && Terminals.Count > 0)
            {
                foreach (ListViewItem item in Terminals)
                {
                    _order.PlayTextVoice(item.Text, item.ToolTipText, txtPlayTimes.Text.Trim(),
                                         txtVoiceText.Text.Trim());
                }
                Close();
            }
            else
            {
                MessageBox.Show("语音文本最多字符数不得超过70个!");
            }
        }

        private void txtVoiceText_TextChanged(object sender, EventArgs e)
        {
            label2.Text = txtVoiceText.TextLength.ToString();
        }
    }
}