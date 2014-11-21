using System;
using System.Windows.Forms;
using Infrastructure;

namespace FormUI.ManagerForms
{
    public partial class WhiteForm : Form
    {
        public WhiteForm(string title)
        {
            InitializeComponent();
            Text = title;
            textBox1.KeyPress += Handler.PhoneNumber;
        }

        public string Phone { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            Phone = textBox1.Text;
            DialogResult = DialogResult.OK;
        }
    }
}