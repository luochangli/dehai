using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using BLL;
using FormUI.Attributes;
using FormUI.Filters;
using Infrastructure;
using TomorrowSoft.BLL;
using TomorrowSoft.Model;

namespace FormUI.OperationLayer
{
    [Aop]
    public class Port : AopObject
    {
        private static volatile Port instance;

        private static readonly object syncRoot = new Object();
        public SerialPort SerialPort { get; private set; }
        private Port()
        {
            ReceiveEventEnabled = true;
            SerialPort = new SerialPort();
        }

        public TerminalMonitor Owner { get; set; }

        /// <summary>
        ///     十六进制模式
        /// </summary>
        public bool HexMode { get; set; }

        /// <summary>
        ///     接收事件是否有效
        ///     true表示有效
        /// </summary>
        public bool ReceiveEventEnabled { get; set; }

        /// <summary>
        ///     接收到回应
        /// </summary>
        public bool IsReceived { get; set; }

        /// <summary>
        ///     多线程单例模式，确保SerialPort只实例化一次
        /// </summary>
        /// <returns></returns>
        public static Port Instance
        {
            get
            {
                if (instance != null) return instance;
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new Port();
                }
                return instance;
            }
        }


       

        public bool IsOpen
        {
            get { return SerialPort.IsOpen; }
        }

        public bool Received { get; set; }

        private string Name
        {
            get { return "收信"; }
        }

        [Operation]
        public Port Send(string at)
        {
            SerialPort.WriteLine(at);
            Suspend();
            return this;
        }

        /// <summary>
        ///     挂起线程，等待串口回应
        /// </summary>
        private void Suspend()
        {
            int i = 0;
            Received = true;
            while (!IsReceived)
            {
                Thread.Sleep(20);
                i++;
                if (i > 360)
                {
                    Received = false;
                    throw new Exception("发送超时，请检查串口设备或波特率参数是否正确！");
                }
            }
            IsReceived = false;
        }


        [Operation]
        public Port SendNoWrap(string at)
        {
            //Thread.Sleep(150);
            SerialPort.Write(at);
            return this;
        }

        [Operation]
        public Port SendNoResponse(string at)
        {
            SerialPort.WriteLine(at);
            Thread.Sleep(3000);
            return this;
        }

        [Operation]
        public Port Send(byte[] at)
        {
//            Thread.Sleep(150);
            SerialPort.Write(at, 0, at.Length);
            Suspend();
            return this;
        }

        public bool Open()
        {
            if (SerialPort.IsOpen)
                SerialPort.Close();
            try
            {
                SerialPort.Open();
                SerialPort.NewLine = "\r\n";
                SerialPort.DataReceived += port_DataReceived;
                SerialPort.ErrorReceived += port_ErrorReceived;
                //Thread.Sleep(150);
            }
            catch
            {
            }
            return IsOpen;
        }

        public void Close()
        {
            SerialPort.Close();
        }


        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
           
            lock (this)
            {
                var port = (SerialPort)sender;
                string strCollect = string.Empty;
                try
                {
                    port.ReceivedBytesThreshold = port.ReadBufferSize;
                    while (true)
                    {
                        string message = port.ReadExisting();
                        if (string.Equals(message, string.Empty))
                        {
                            break;
                        }
                        strCollect += message;
                        Thread.Sleep(30);
                    }
                    //                var message = port.ReadExisting();
                    //                var content = message.Replace("\r", string.Empty)
                    //                                     .Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);
                    string[] content = strCollect.Replace("\r", string.Empty)
                                                 .Split(new[] {"\n", "ERROR"}, StringSplitOptions.RemoveEmptyEntries);

                    ReadCardMes(content);

                    foreach (string t1 in content)
                    {
                        if (t1.Contains("+CMT:") || t1.Contains("NO CARRIER") || t1.Contains("RING"))
                        {
                            var tHigh = new Thread(() =>
                                {
                                    Filter filter = new FilterProcessor(content).Run();
                                    if (filter == null) return;
                                    Owner.Invoke(new Action<Filter>(Owner.Popup), filter);
                                }) {Priority = ThreadPriority.AboveNormal};
                            tHigh.Start();
                            //Thread.Sleep(260);
                        }
                    }
                    if (!ReceiveEventEnabled)
                    {
                        IsReceived = true;
                        //return;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    port.ReceivedBytesThreshold = 1;
                }
            }
        }

        private void ReadCardMes(string[] content)
        {
            for (int index = 0; index < content.Length; index ++)
            {
                if (!content[index].Contains("+CMGL:")) continue;
                MessageSave(content[index + 1]);
                string[] mesNo = content[index].Split(new[] {"+CMGL:", ","}, StringSplitOptions.RemoveEmptyEntries);
                new MessageIndexService().Add(mesNo[0]);
            }
        }

        private void port_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            if (!Instance.ReceiveEventEnabled) return;
            var port = (SerialPort) sender;
            string output = port.ReadExisting();
            MessageBox.Show(output, "AT指令错误");
        }

        #region Setter

        public Port SetPortName(string portName)
        {
            SerialPort.PortName = portName;
            return this;
        }

        public Port SetBaudRate(string baudRate)
        {
            SerialPort.BaudRate = Convert.ToInt32(baudRate);
            return this;
        }

        public Port SetDataBit(string dataBit)
        {
            SerialPort.DataBits = Convert.ToInt32(dataBit);
            return this;
        }

        public Port SetStopBit(string stopBit)
        {
            SerialPort.StopBits = Enum<StopBits>.Parse(stopBit);
            return this;
        }

        public Port SetParity(string parity)
        {
            SerialPort.Parity = Enum<Parity>.Parse(parity);
            return this;
        }

        #endregion

        #region 启动前读取短信并保存

        protected readonly TerminalService Terminal = new TerminalService();
        protected readonly WhiteListService White = new WhiteListService();

        private void MessageSave(string Content)
        {
            int current;
            int total;
            string identifier;
            bool isLongMessage;
            string phone;
            DateTime time;
            string content;
            AT.GetSmsContent(Content, out isLongMessage, out phone, out time, out content, out current, out total,
                             out identifier);
            if (phone.StartsWith("86"))
                phone = phone.Remove(0, 2);
            if (White.PhoneExists(phone) || Terminal.PhoneExists(phone))
            {
                if (isLongMessage)
                {
                    var service = new LongSmsService();
                    service.Add(new LongSms
                        {
                            Content = content,
                            Current = current,
                            Identifier = identifier,
                            Phone = phone,
                            Time = time,
                            Total = total
                        });

                    IList<LongSms> longSmses = service.GetBy(phone, identifier, time);
                    if (longSmses.Count < total)
                        return;
                    content = string.Empty;
                    foreach (LongSms sms in longSmses)
                    {
                        content += sms.Content;
                    }
                }
                string name = new TerminalService().GetList("PhoneNo=" + phone).Tables[0].Rows[0]["Name"].ToString();

                new RecMesSave().SaveMes(content, phone, name);
            }
        }

        #endregion
    }
}