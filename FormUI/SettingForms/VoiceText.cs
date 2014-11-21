using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using FormUI.OperationLayer;
using Infrastructure;

namespace FormUI.SettingForms
{
    public partial class VoiceText : Form
    {
        private OrderDefinition _order;
        private readonly IList< ListViewItem> Terminals;
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
                    foreach (var item in Terminals)
                    {

                        _order.PlayTextVoice(item.Text, item.ToolTipText, txtPlayTimes.Text.Trim(),
                                             txtVoiceText.Text.Trim());
                    }
                    this.Close();
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
