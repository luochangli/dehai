using System;
using System.Drawing;
using System.Windows.Forms;
using FormUI.ManagerForms;
using FormUI.OperationLayer;
using FormUI.Properties;
using FormUI.SettingForms;

namespace FormUI
{
    public partial class MainPage : Form
    {
        private readonly Printer _printer;
        private readonly AT comAT;
        private readonly Port port = Port.Instance;


        public MainPage()
        {
            comAT = new AT();
            InitializeComponent();
            _printer = new Printer();
        }

        private void MainPage_Load(object sender, EventArgs e)
        {
            if (Settings.Default.picPath != string.Empty)
            {
                BackgroundImage = Image.FromFile(Settings.Default.picPath);
                BackgroundImageLayout = ImageLayout.Stretch;
            }
            try
            {
                port.SetPortName(Settings.Default.PortName)
                    .SetBaudRate(Settings.Default.BaudRate)
                    .SetDataBit(Settings.Default.DataBits)
                    .SetParity(Settings.Default.Parity)
                    .SetStopBit(Settings.Default.StopBit);
                if (port.Open())
                {
                    ChangeState(true);
                    comAT.MessageHint();
                }
                else
                {
                    var portForm = new PortForm();
                    if (portForm.ShowDialog() == DialogResult.OK && port.Open())
                        portForm.Close();
                }
            }
            catch (Exception ex)
            {
            }

            if (LoginForm.Level == "管理员")
            {
                口令ToolStripMenuItem.Enabled = true;
            }
            new ControlHelpers().FormChange(this);


            WindowState = FormWindowState.Maximized;
        }

        private void 端口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var portForm = new PortForm();
            portForm.ShowDialog();
            ChangeState(port.IsOpen);
        }

        private void btDialing_Click(object sender, EventArgs e)
        {
            new CallForm().ShowDialog();
        }

        private void btHandup_Click(object sender, EventArgs e)
        {
            try
            {
                comAT.HangUp();
            }
            catch
            {
            }
        }

        private void btAnswer_Click(object sender, EventArgs e)
        {
            comAT.CallAnswer();
        }


        private void 终端ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new TerminalForm().ShowDialog();
        }


        private void btBroadcast_Click(object sender, EventArgs e)
        {
            new SpeakForm().ShowDialog();
        }

        private void 口令ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new UserFrm().ShowDialog();
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LoginForm.Level == "管理员")
            {
                口令ToolStripMenuItem.Enabled = true;
            }
        }

        private void 打印ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Printer.PrintSetup();
        }

        private void ChangeState(bool isOpen)
        {
            if (isOpen)
            {
                lbPortState.Text = port.SerialPort.PortName + "已打开";
                lbPortState.ForeColor = Color.Green;
            }
            else
            {
                lbPortState.Text = "串口未打开";
                lbPortState.ForeColor = Color.DarkRed;
            }
        }


        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox1().ShowDialog();
        }


        private void 主界面皮肤设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog
                {
                    Filter = Resources.Image
                };
            if (openDialog.ShowDialog() != DialogResult.OK) return;
            BackgroundImage.Dispose();
            FileOperation.SaveFile(openDialog.FileName);
            string desPath = FileOperation.GetFile();
            Settings.Default.picPath = desPath;
            Settings.Default.Save();
            BackgroundImage = Image.FromFile(desPath);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new TerminalMonitor().ShowDialog();
        }

        private void 白名单ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var whiteListForm = new WhiteListForm();
            whiteListForm.ShowDialog();
        }

        private void 历史信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new HistoryForm().ShowDialog();
        }

        private void 终端状态ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ConditionForm().ShowDialog();
        }

        private void 选播ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new OnDemandForm().ShowDialog();
        }

        private void 喊话ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SpeakForm().ShowDialog();
        }
    }
}