using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FormUI.OperationLayer;

namespace FormUI.SettingForms
{
    public partial class MusicSet : Form
    {
        private readonly OrderDefinition _order;
        private readonly IList<ListViewItem> itmes;

        public MusicSet(IList<ListViewItem> itmes)
        {
            InitializeComponent();
            this.itmes = itmes;
            _order = new OrderDefinition();
            lbSelectNum.Text = "选中的终端数有：" + this.itmes.Count + "个";
        }

        public MusicSet()
        {
            InitializeComponent();
        }

        private void MusicSet_Load(object sender, EventArgs e)
        {
            OrderDefinition.SetPlayStyle(cbPlayStlye);
            cbPlayStlye.SelectedIndex = 0;
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in itmes)
            {
                _order.PlayMusic(item.Text, item.ToolTipText, cbPlayStlye.SelectedIndex.ToString(),
                                 numericUpDown1.Value.ToString(),
                                 tbPlayTime.Text);
            }
            Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbPlayStlye_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPlayStlye.SelectedIndex == 0)
            {
                tbPlayTime.Text = string.Empty;
                tbPlayTime.Enabled = false;
            }
            else
            {
                tbPlayTime.Enabled = true;
                tbPlayTime.Text = "03";
            }
        }
    }
}