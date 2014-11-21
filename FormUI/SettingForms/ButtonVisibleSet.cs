using System;
using System.Windows.Forms;
using FormUI.Properties;

namespace FormUI.SettingForms
{
    public partial class ButtonVisibleSet : Form
    {
        private readonly TerminalMonitor form;

        public ButtonVisibleSet(TerminalMonitor form)
        {
            InitializeComponent();
            this.form = form;
        }


        private void btSave_Click(object sender, EventArgs e)
        {
            Settings.Default.Save();
            form.ButtonVisible();
            Close();
        }
    }
}