using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FormUI.Properties;

namespace FormUI.SettingForms
{
    public partial class ButtonVisibleSet : Form
    {
        private TerminalMonitor form;
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
