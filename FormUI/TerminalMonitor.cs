using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using BLL;
using FormUI.Filters;
using FormUI.ManagerForms;
using FormUI.OperationLayer;
using FormUI.Properties;
using FormUI.SettingForms;
using TomorrowSoft.BLL;
using TomorrowSoft.Model;

namespace FormUI
{
    public partial class TerminalMonitor : Form
    {
        public delegate void Listener(string phone, string context);

        public delegate void MyEvent(object sender, FilterEventArgs e);

        public static bool CallLock = true;

        private readonly OrderDefinition _order;
        private readonly TerminalService _service;
        private readonly AT cmd;
        private readonly Port port = Port.Instance;
        private Printer _printer = new Printer();
        private AlarmClock alarmClock;


        public TerminalMonitor()
        {
            InitializeComponent();
            cmd = new AT();
            alarmClock = new AlarmClock();
            _order = new OrderDefinition();
            _service = new TerminalService();
            ButtonVisible();
        }

        public event MyEvent NewPhone;
        public static event Listener ListBox1Listener;

        public void OnListBox1Listener(string phone, string context)
        {
            if (phone != string.Empty)
            {
                ListBox1Listener(phone, context);
            }
        }


        private void SendMesShow(string phone, string context)
        {
            ControlListboxAmount();
            listBox1.Items.Add(new Item("发送至：" + phone, context));
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Text == phone)
                {
                    listView1.Items[i].ForeColor = Color.Red;
                    break;
                }
            }
        }


        public void ButtonVisible()
        {
            btFloodWarn.Enabled  = !Settings.Default.cbL1;
            btSlaggWarn.Enabled = !Settings.Default.cbL2;
            btPowerAlert.Enabled = !Settings.Default.cbL3;
            btTextPlay.Enabled = !Settings.Default.cbL4;
            btWaterLevel.Enabled = !Settings.Default.cbL5;

            btTest.Enabled = !Settings.Default.cbR1;
            btStopMusic.Enabled = !Settings.Default.cbR2;
            btTestMusic.Enabled = !Settings.Default.cbR3;
            btOpenCTerminal.Enabled = !Settings.Default.cbR4;
            btAlarm.Enabled = !Settings.Default.cbR5;
            btFloodWarn.Text = Settings.Default.alarm1;
            btSlaggWarn.Text = Settings.Default.alarm2;
            btPowerAlert.Text = Settings.Default.alarm3;
        }
        /// <summary>
        ///     ListBox中显示的记录的条数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControlListboxAmount()
        {
            if (listBox1.Items.Count > 6)
            {
                listBox1.Items.RemoveAt(0);
            }
        }

        /// <summary>
        ///     来信提示
        /// </summary>
        /// <param name="filter"></param>
        public void Popup(Filter filter)
        {
            if (filter.Phone != string.Empty)
            {
                NewPhone(this, new FilterEventArgs(filter));
            }
        }

        /// <summary>
        ///     刷新ListView中的终端
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshListBox(object sender, FilterEventArgs e)
        {
            lock (this)
            {
                string str = e.Filter.Phone;
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (listView1.Items[i].ToolTipText == e.Filter.Phone)
                    {
                        if (e.Filter.Context.Contains("本终端已启动"))
                        {
                            _order.TimeSet(e.Filter.Phone, e.Filter.Phone,
                                           DateTime.Now.ToString("yyyyMMddHHmmss").Substring(2));
                            listView1.Items[i].ImageKey = TerminalState.RunningChecked.ToString();

                        }
                        if (e.Filter.Context.Contains("本地喊话") || e.Filter.Context.Contains("播放"))
                        {
                            listView1.Items[i].ImageKey = TerminalState.GreenChecked.ToString();

                        }

                        if (e.Filter.Context.Contains("已停播") || e.Filter.Context.Contains("OK") || e.Filter.Context.Contains("充电"))
                        {
                            listView1.Items[i].ImageKey = TerminalState.RunningChecked.ToString();
                        }
                        if (e.Filter.Context.Contains("error"))
                        {
                            listView1.Items[i].ImageKey = TerminalState.StopedChecked.ToString();
                        }
                        if (e.Filter.Context.Contains("告警") || e.Filter.IsQsDown)
                        {
                            listView1.Items[i].ImageKey = TerminalState.RedChecked.ToString();
                            new MessageBoxTimeOut().Show(3000, string.Format("{0}", e.Filter.Context), "告警",
                                                         MessageBoxButtons.OK);
                        }
                        /* if (e.Filter.IsQsDown)
                        {
                            listView1.Items[i].ImageKey = TerminalState.Red.ToString();
                        }*/
                        if (e.Filter.Name.Contains("来电"))
                        {
                            cmd.HangUp();
                        }
                        if (e.Filter.Name.Contains("挂机"))
                        {
                            CallLock = true;
                        }
                        str = listView1.Items[i].Text;
                        listView1.Items[i].Tag = new object();
                        listView1.Items[i].ForeColor = Color.Green;
                        break;
                    }
                }
                cmd.SmsAnswer();
                ControlListboxAmount();
                listBox1.Items.Add(new Item(e.Filter.Name + "于：" + str, e.Filter.Context, e.Filter.Time));
                if (e.Filter.Content1 == null) return;
                new RecMesSave().SaveMes(e.Filter.Content1, e.Filter.Phone, str);
            }
            
        }

        private void TerminalMonitor_Load(object sender, EventArgs e)
        {
//            Opacity = 0;
            AT.MyListView = this;
            port.Owner = this;
            timer1.Start();
            try
            {
                port.SetPortName(Settings.Default.PortName)
                    .SetBaudRate(Settings.Default.BaudRate)
                    .SetDataBit(Settings.Default.DataBits)
                    .SetParity(Settings.Default.Parity)
                    .SetStopBit(Settings.Default.StopBit);

                if (port.Open())
                {
                    cmd.MessageHint();
                    GetAllMessage();
                }
                else
                {
                    var portForm = new PortForm();
                    if (portForm.ShowDialog() == DialogResult.OK && port.Open())
                    {
                        portForm.Close();
                    }
                }
                ChangeState();
                DeleteReadMsg();
            }
            catch (Exception ex)
            {
            }

            if (LoginForm.Level == "管理员")
            {
                口令管理ToolStripMenuItem.Enabled = true;
                功能按钮显示隐藏ToolStripMenuItem.Enabled = true;
            }
            new ControlHelpers().FormChange(this);

            LoadTerminals();
            NewPhone += RefreshListBox;
            ListBox1Listener += SendMesShow;
            WindowState = FormWindowState.Maximized;
            AutoSend();

        }

        private void DeleteReadMsg()
        {
            DataTable mesIndex = new MessageIndexService().GetAll();
            int receiveCount = mesIndex.Rows.Count;
            if (receiveCount <= 0) return;
            for (int i = 0; i < mesIndex.Rows.Count; i++)
            {
                cmd.DeleteMes(mesIndex.Rows[i]["MessageIndex"].ToString());
            }
            new MessageIndexService().Delete();
//            new MessageBoxTimeOut().Show(3000, string.Format("后台接受{0}条短信！", receiveCount), "提示",
//                                         MessageBoxButtons.OK);
        }

        /// <summary>
        ///     加载软件是获取未开机的短信
        /// </summary>
        private void GetAllMessage()
        {
            cmd.GetAllMessage();
        }


        private void ChangeState()
        {
            try
            {
                cmd.SmsAnswer();
            }
            catch
            {
                CallLock = false;
            }
            if (port.IsOpen && port.Received)
            {
                timer1.Interval = 4000;
                timer1.Enabled = true;
                lbPortState.Text = port.SerialPort.PortName;
                lbPortState.ForeColor = Color.Green;
            }
            else
            {
                lbPortState.Text = "COM";
                lbPortState.ForeColor = Color.Red;
            }
        }


        private void LoadTerminals()
        {
            listView1.LargeImageList = imageList;
            listView1.Clear();
            List<Terminal> terminals = _service.GetModelList(string.Empty);
            foreach (Terminal terminal in terminals)
            {
                var group = new ListViewGroup(terminal.GroupPhone, terminal.Grouping);
                bool contants = false;
                foreach (ListViewGroup g in listView1.Groups)
                {
                    if (g.Name == terminal.GroupPhone)
                    {
                        group = g;
                        contants = true;
                        break;
                    }
                }
                if (!contants)
                    listView1.Groups.Add(group);
                var item = new ListViewItem(terminal.Name, TerminalState.Stoped.ToString(), group)
                    {
                        ToolTipText = terminal.PhoneNo
                    };

                listView1.Items.Add(item);
            }
        }

        private IList<ListViewItem> GetSelectedPhone()
        {
            var items = new List<ListViewItem>();
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Tag != null)
                {
                    items.Add(item);
                }
            }
            if (items.Count < 1)
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    items.Add(item);
                }
            }
            return items;
        }


        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox1().ShowDialog();
        }

        private void 串口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new PortForm().ShowDialog();
            ChangeState();
        }

        private void 打印ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Printer.PrintSetup();
        }

        private void 白名单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new WhiteListForm().Show();
        }


        private void 告警ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new MusicTimeSetting().Show();
        }

        private void 终端ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new TerminalForm().ShowDialog();
            listView1.Clear();
            LoadTerminals();
        }

        private void 口令管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new UserFrm().ShowDialog();
        }

        private void 历史查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new HistoryForm().ShowDialog();
        }

        private void 状态查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ConditionForm().ShowDialog();
        }

        private void terminalCall_Click(object sender, EventArgs e)
        {
            IList<ListViewItem> items = GetSelectedPhone();
            if (items.Count != 1)
            {
                MessageBox.Show("暂不支持会议通话");
                return;
            }
            if (items[0].ToolTipText == string.Empty)
            {
                MessageBox.Show("该终端号码为空");
            }
            try
            {
                cmd.CallUp(items[0].ToolTipText, items[0].Text);
                CallLock = false;
                listBox1.Items.Add(new Item("拨号至：" + items[0].Text, null));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void terminialHungUp_Click(object sender, EventArgs e)
        {
            try
            {
                cmd.HangUp();
                CallLock = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btTest_Click(object sender, EventArgs e)
        {
            IList<ListViewItem> items = GetSelectedPhone();
            try
            {
                if (MessageBox.Show(string.Format(@"确定发送至选中的{0}个终端？", items.Count), "提示",
                                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    var t1 = new ThreadStart(() =>
                        {
                            foreach (ListViewItem item in items)
                            {
                                _order.ConditionQuery(item.Text, item.ToolTipText);
                            }
                        });
                    new Thread(t1).Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }
        public void AutoSend()
        {
            IList<ListViewItem> items = GetSelectedPhone();
            var t1 = new ThreadStart(() =>
            {
                foreach (ListViewItem item in items)
                {
                   _order.InitTerminal(item.Text, item.ToolTipText);
                }
            });
            new Thread(t1).Start();
        }
        private void btTestMusic_Click(object sender, EventArgs e)
        {
            IList<ListViewItem> items = GetSelectedPhone();
            new MusicSet(items).ShowDialog();
            /* if (MessageBox.Show(string.Format(@"确定发送至选中的{0}个终端？", items.Count),
                                "提示",
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    var t = new ThreadStart(() =>
                        {
                            foreach (ListViewItem item in items)
                            {
                                string content = _order.PlayMusic(item.Text, item.ToolTipText, "1",
                                                                  "4",
                                                                  (Settings.Default.Ceshi == string.Empty
                                                                       ? "3"
                                                                       : Settings.Default.Ceshi).PadLeft(2,
                                                                                                         Convert.ToChar(
                                                                                                             "0")));
                            }
                        });
                    new Thread(t).Start();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }*/
        }


        private void 状态ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem item = listView1.FocusedItem;
            try
            {
                
                _order.Detection(item.Text, item.ToolTipText);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 音量ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var volumeSettingForm = new VolumeSetting();
            if (volumeSettingForm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _order.VolumeSetting(listView1.FocusedItem.Text, listView1.FocusedItem.ToolTipText,
                                         volumeSettingForm.Volume);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void 电压ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var batterySetting = new BattryParatemerSetting();
            if (batterySetting.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _order.ParameterSetting(listView1.FocusedItem.Text, listView1.FocusedItem.ToolTipText,
                                            batterySetting.InMax,
                                            batterySetting.InMin, batterySetting.OutMax,
                                            batterySetting.OutMin);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btFloodWarn_Click(object sender, EventArgs e)
        {
            IList<ListViewItem> items = GetSelectedPhone();
            if (MessageBox.Show(string.Format(@"确定发送至选中的{0}个终端？", items.Count),
                                "提示",
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    var t = new ThreadStart(() =>
                        {
                            foreach (ListViewItem item in items)
                            {
                                string content = _order.PlayMusic(item.Text, item.ToolTipText, "1",
                                                                  "1",
                                                                  (Settings.Default.Ceshi == string.Empty
                                                                       ? "3"
                                                                       : Settings.Default.Xiehong).PadLeft(2,
                                                                                                           Convert
                                                                                                               .ToChar(
                                                                                                                   "0")));
                            }
                        });
                    new Thread(t).Start();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btSlaggWarn_Click(object sender, EventArgs e)
        {
            IList<ListViewItem> items = GetSelectedPhone();
            if (MessageBox.Show(string.Format(@"确定发送至选中的{0}个终端？", items.Count),
                                "提示",
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    var t = new ThreadStart(() =>
                        {
                            foreach (ListViewItem item in items)
                            {
                                string content = _order.PlayMusic(item.Text, item.ToolTipText, "1",
                                                                  "2",
                                                                  (Settings.Default.Ceshi == string.Empty
                                                                       ? "3"
                                                                       : Settings.Default.Paizha).PadLeft(2,
                                                                                                          Convert.ToChar
                                                                                                              ("0")));
                            }
                        });
                    new Thread(t).Start();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btPowerAlert_Click(object sender, EventArgs e)
        {
            IList<ListViewItem> items = GetSelectedPhone();
            if (MessageBox.Show(string.Format(@"确定发送至选中的{0}个终端？", items.Count),
                                "提示",
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    var t = new ThreadStart(() =>
                        {
                            foreach (ListViewItem item in items)
                            {
                                _order.PlayMusic(item.Text, item.ToolTipText, "1",
                                                 "3",
                                                 (Settings.Default.Ceshi == string.Empty
                                                      ? "3"
                                                      : Settings.Default.Fadian).PadLeft(2, Convert.ToChar("0")));
                            }
                        });
                    new Thread(t).Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        private void 添加管理号码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IList<ListViewItem> items = GetSelectedPhone();
            var white = new WhiteForm("添加管理号码");
            white.ShowDialog();
            if (white.DialogResult == DialogResult.OK && white.Phone != string.Empty)
            {
                try
                {
                    foreach (var item in items)
                    {
                        _order.AddManagerPhone(item.Text,item.ToolTipText, white.Phone);  
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void 添加授权号码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IList<ListViewItem> items = GetSelectedPhone();
            var white = new WhiteForm("添加授权号码");
            white.ShowDialog();
            if (white.DialogResult == DialogResult.OK && white.Phone != string.Empty)
            {
                try
                {
                    foreach (var item in items)
                    {
                        _order.Authorization(item.Text, item.ToolTipText, white.Phone);
                    }
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void 删除号码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var white = new WhiteForm("删除号码");
            white.ShowDialog();
            if (white.DialogResult == DialogResult.OK && white.Phone != string.Empty)
            {
                try
                {
                    _order.DeletePhone(listView1.FocusedItem.Text, listView1.FocusedItem.ToolTipText, white.Phone);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void 清空白名单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format(@"清空白名单？"),
                                "提示",
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    string content = _order.ClearPhone(listView1.FocusedItem.Text, listView1.FocusedItem.ToolTipText);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void 查看ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string content = _order.PhoneCheck(listView1.FocusedItem.Text, listView1.FocusedItem.ToolTipText);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = listBox1.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                new PopupLayer((Item) listBox1.Items[index]).Show();
            }
        }


        private void btStopMusic_Click(object sender, EventArgs e)
        {
            IList<ListViewItem> items = GetSelectedPhone();
            if (MessageBox.Show(string.Format(@"确定选中的{0}个终端停播？", items.Count),
                                "提示",
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Question) == DialogResult.OK)
            {
                var t = new ThreadStart(() =>
                    {
                        foreach (ListViewItem item in items)
                        {
                            _order.MusicStop(item.Text, item.ToolTipText);
                        }
                    });
                new Thread(t).Start();

            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
//            Opacity += dou;
//            if (Opacity == 1)
//            {
//                timer1.Stop();
//                dou = -0.05;
//            }
            if (CallLock)
                ChangeState();
        }

        private void btRainFull_Click(object sender, EventArgs e)
        {
            IList<ListViewItem> items = GetSelectedPhone();
            if (MessageBox.Show(string.Format(@"确定发送至选中的{0}个终端？", items.Count),
                                "提示",
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
//                    IList<string> phone = items.Select(item => item.ToolTipText).ToList();
                    new VoiceText(items).ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btWaterLevel_Click(object sender, EventArgs e)
        {
            new AutoPlayer().ShowDialog();
        }


        private void 定时开关机ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Shutdown();
        }

        /// <summary>
        ///     启动定时开关机
        /// </summary>
        private void Shutdown()
        {
            Process.Start(@"AutoShutdownHelper\AutoShutdownHelper.exe");
        }

        private void 定时播放ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IList<ListViewItem> items = GetSelectedPhone();
            if (items.Count  == 0)
            {
                MessageBox.Show("终端号码为空！");
            }
            else
            {
                try
                {
                    new AutoStartUpOrEsc(items).ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void 设定延时广播ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AutoPlayer(listView1.FocusedItem.Text, listView1.FocusedItem.ToolTipText).ShowDialog();
        }

        private void btOpenCTerminal_Click(object sender, EventArgs e)
        {
            IList<ListViewItem> items = GetSelectedPhone();
            if (MessageBox.Show(string.Format(@"确定发送至选中的{0}个终端？", items.Count), "提示",
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    var t = new ThreadStart(() =>
                        {
                            foreach (ListViewItem item in items)
                            {
                                _order.Detection(item.Text, item.ToolTipText);
                            }
                        });
                    new Thread(t).Start();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void 终端防盗加锁ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _order.Lock(listView1.FocusedItem.Text, listView1.FocusedItem.ToolTipText);
//            MessageBox.Show(listView1.FocusedItem.Text + "：加锁");
        }

        private void 终端防盗解锁ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _order.UnLock(listView1.FocusedItem.Text, listView1.FocusedItem.ToolTipText);
//            MessageBox.Show(listView1.FocusedItem.Text + "：解锁");
        }

        private void btAlarm_Click(object sender, EventArgs e)
        {
            IList<ListViewItem> items = GetSelectedPhone();
            new Alarm(items).Show();
        }

        /// <summary>
        ///     listbox1中字体可控变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            string s = listBox1.Items[e.Index].ToString();
            if (s.Contains("收信"))
            {
                e.Graphics.DrawString(s, Font, Brushes.Green, e.Bounds);
            }
            else if (s.Contains("发送"))
            {
                e.Graphics.DrawString(s, Font, Brushes.Red, e.Bounds);
            }
            else
                e.Graphics.DrawString(s, Font, new SolidBrush(ForeColor), e.Bounds);
        }

        private void 定时查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new PhotovoltaicAlarm().Show();
        }

        private void c型终端ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem item = listView1.FocusedItem;
            new OpenCTerminal(item).Show();
        }

        public class FilterEventArgs : EventArgs
        {
            public FilterEventArgs(Filter filter)
            {
                Filter = filter;
            }

            public Filter Filter { get; private set; }
        }

        /// <summary>
        ///     listbox中记录数据
        /// </summary>
        public class Item : EventArgs
        {
            public Item(string title, string content)
            {
                Title = title;
                Content = content;
                Time = DateTime.Now;
            }

            public Item(string title, string content, DateTime time)
            {
                Title = title;
                Content = content;
                Time = time;
            }

            public Item(Item item)
            {
                Title = item.Title;
                Content = item.Content;
                Time = DateTime.Now;
            }

            public string Title { get; set; }

            public DateTime Time { get; set; }

            public string Content { get; set; }

            public override string ToString()
            {
                return Title;
            }
        }

        private enum TerminalState
        {
            Send,
            Receive,
            Test,
            Running,
            Stoped,
            RunningChecked,
            StopedChecked,
            Red,
            RedChecked,
            Green,
            GreenChecked
        }

        #region 鼠标左右按键处理

        private void terminalList_MouseUp(object sender, MouseEventArgs e)
        {
            ListViewItem item = listView1.GetItemAt(e.X, e.Y);
            if (item == null)
            {
                listView1.ContextMenuStrip = null;
                return;
            }
            switch (e.Button)
            {
                case MouseButtons.Left:

                    if (item.Tag == null)
                    {
                        item.Tag = new object();
                        item.ImageKey += @"Checked";
                    }
                    else
                    {
                        item.Tag = null;
                        item.ImageKey = item.ImageKey.Replace("Checked", string.Empty);
                    }
                    break;
                case MouseButtons.Right:
                    listView1.ContextMenuStrip = contextMenu;
                    break;
                default:
                    break;
            }
        }

        #endregion

      

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Maximized;
            notifyIcon1.Visible = false; 
        }

        private void TerminalMonitor_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.notifyIcon1.Visible = true;
            } 
        }

        private void 功能按钮显示隐藏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ButtonVisibleSet(this).ShowDialog();
        }
    }
}