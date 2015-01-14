using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using FormUI.OperationLayer;
using Infrastructure;

namespace FormUI.ManagerForms
{
    public partial class Alarm : Form
    {
        private readonly IList<ListViewItem> Items;
        private readonly OrderDefinition _order;

        public Alarm(IList<ListViewItem> items)
        {
            InitializeComponent();
            Items = items;

            _order = new OrderDefinition();
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format(@"确定发送至选中的{0}个终端？", Items.Count),
                                "提示",
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    string music = cmbMusic.Text;
                    string minute = txtＭinute.Text.Trim();
                    var t = new ThreadStart(() =>
                        {
                            foreach (ListViewItem item in Items)
                            {
                                _order.Alarm(item.Text, item.ToolTipText, minute);
                            }
                        });
                    new Thread(t).Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Close();
            }
        }

        private void Alarm_Load(object sender, EventArgs e)
        {
            OrderDefinition.SetMusicNo(cmbMusic);
            cmbMusic.SelectedIndex = 0;
            txtＭinute.KeyPress += Handler.Nuber09;
        }
    }
}