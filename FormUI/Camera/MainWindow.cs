﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using NVRCsharpDemo;

namespace FormUI.Camera
{
    
    public partial class MainWindow : Form
    {
        private bool m_bInitSDK = false;
        private bool m_bRecord = false;
        private uint iLastErr = 0;
        private Int32 m_lUserID = -1;
        private Int32 m_lRealHandle = -1;
        private string str1;
        private string str2;
        private Int32 i = 0;
        private Int32 m_lTree=0;
        private string str;
        private long iSelIndex = 0;
        private uint dwAChanTotalNum = 0;
        private Int32 m_lPort = -1;
        private IntPtr m_ptrRealHandle;
        private int[] iIPDevID = new int[96];
        private int[] iChannelNum = new int[96];

        private CHCNetSDK.REALDATACALLBACK RealData = null;
        public CHCNetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo;
        public CHCNetSDK.NET_DVR_IPPARACFG_V40 m_struIpParaCfgV40;
        public CHCNetSDK.NET_DVR_STREAM_MODE m_struStreamMode;
        public CHCNetSDK.NET_DVR_IPCHANINFO m_struChanInfo;
        public CHCNetSDK.NET_DVR_IPCHANINFO_V40 m_struChanInfoV40;
        private PlayCtrl.DECCBFUN m_fDisplayFun = null;
        public delegate void MyDebugInfo(string str);

        private FullScreenHelper fullScreenHelper;
        public MainWindow()
        {
            InitializeComponent();
            m_bInitSDK = CHCNetSDK.NET_DVR_Init();
            if (m_bInitSDK == false)
            {
                MessageBox.Show("NET_DVR_Init error!");
                return;
            }
            else
            {
                //保存SDK日志 To save the SDK log
                CHCNetSDK.NET_DVR_SetLogToFile(3,"C:\\SdkLog\\", true);

                comboBoxView.SelectedIndex = 0;

                for (int i = 0; i < 64; i++)
                {
                    iIPDevID[i] = -1;
                    iChannelNum[i] = -1;
                }
            }
        }

        public void DebugInfo(string str)
        {
            if (str.Length > 0)
            {
                str += "\n";
                TextBoxInfo.AppendText(str);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (m_lUserID < 0)
            {
                string DVRIPAddress = textBoxIP.Text; //设备IP地址或者域名 Device IP
                Int16 DVRPortNumber = Int16.Parse(textBoxPort.Text); //设备服务端口号 Device Port
                string DVRUserName = textBoxUserName.Text; //设备登录用户名 User name to login
                string DVRPassword = textBoxPassword.Text; //设备登录密码 Password to login

                if (checkBoxHiDDNS.Checked)
                {
                    byte[] HiDDNSName = System.Text.Encoding.Default.GetBytes(textBoxIP.Text);
                    byte[] GetIPAddress = new byte[16];
                    uint dwPort = 0;
                    if (
                        !CHCNetSDK.NET_DVR_GetDVRIPByResolveSvr_EX("www.hik-online.com", (ushort)80, HiDDNSName,
                                                                   (ushort)HiDDNSName.Length, null, 0, GetIPAddress, ref dwPort))
                    {
                        iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                        str = "NET_DVR_GetDVRIPByResolveSvr_EX failed, error code= " + iLastErr;
                        //域名解析失败，输出错误号 Failed to login and output the error code
                        DebugInfo(str);
                        return;
                    }
                    else
                    {
                        DVRIPAddress = System.Text.Encoding.UTF8.GetString(GetIPAddress).TrimEnd('\0');
                        DVRPortNumber = (Int16)dwPort;
                    }
                }

                //登录设备 Login the device
                m_lUserID = CHCNetSDK.NET_DVR_Login_V30(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, ref DeviceInfo);
                if (m_lUserID < 0)
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_Login_V30 failed, error code= " + iLastErr;
                    //登录失败，输出错误号 Failed to login and output the error code
                    DebugInfo(str);
                    return;
                }
                else
                {
                    //登录成功
                    DebugInfo("NET_DVR_Login_V30 succ!");
                    btnLogin.Text = "Logout";

                    dwAChanTotalNum = (uint)DeviceInfo.byChanNum;

                    if (DeviceInfo.byIPChanNum > 0)
                    {
                        InfoIPChannel();
                    }
                    else
                    {
                        for (i = 0; i < dwAChanTotalNum; i++)
                        {
                            ListAnalogChannel(i + 1, 1);
                            iChannelNum[i] = i + (int)DeviceInfo.byStartChan;
                        }

                        comboBoxView.SelectedItem = 1;
                        // MessageBox.Show("This device has no IP channel!");
                    }
                }
            }
            else
            {
                //注销登录 Logout the device
                if (m_lRealHandle >= 0)
                {
                    DebugInfo("Please stop live view firstly"); //登出前先停止预览 Stop live view before logout
                    return;
                }

                if (!CHCNetSDK.NET_DVR_Logout(m_lUserID))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_Logout failed, error code= " + iLastErr;
                    DebugInfo(str);
                    return;
                }
                DebugInfo("NET_DVR_Logout succ!");
                listViewIPChannel.Items.Clear(); //清空通道列表 Clean up the channel list
                m_lUserID = -1;
                btnLogin.Text = "Login";
            }
            return;
        }
        public bool IsExistsABC(string str)
        {
            return Regex.Matches(str, "[a-zA-Z]").Count > 0;
        }
        private void HKLoginIn(string DvrAddress,string DvrPortNum)
        {
            if (m_lUserID < 0)
            {
                string DVRIPAddress = DvrAddress; //设备IP地址或者域名 Device IP
                Int16 DVRPortNumber = Int16.Parse(DvrPortNum); //设备服务端口号 Device Port
                string DVRUserName = "admin"; //设备登录用户名 User name to login
                string DVRPassword = "admin"; //设备登录密码 Password to login

                if (IsExistsABC( DvrAddress))
                {
                    byte[] HiDDNSName = System.Text.Encoding.Default.GetBytes(DvrAddress);
                    byte[] GetIPAddress = new byte[16];
                    uint dwPort = 0;
                    if (
                        !CHCNetSDK.NET_DVR_GetDVRIPByResolveSvr_EX("www.hik-online.com", (ushort) 80, HiDDNSName,
                                                                   (ushort) HiDDNSName.Length, null, 0, GetIPAddress, ref dwPort))
                    {
                        iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                        str = "NET_DVR_GetDVRIPByResolveSvr_EX failed, error code= " + iLastErr;
                            //域名解析失败，输出错误号 Failed to login and output the error code
                        DebugInfo(str);
                        return;
                    }
                    else
                    {
                        DVRIPAddress = System.Text.Encoding.UTF8.GetString(GetIPAddress).TrimEnd('\0');
                        DVRPortNumber = (Int16) dwPort;
                    }
                }

                //登录设备 Login the device
                m_lUserID = CHCNetSDK.NET_DVR_Login_V30(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, ref DeviceInfo);
                if (m_lUserID < 0)
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_Login_V30 failed, error code= " + iLastErr;
                        //登录失败，输出错误号 Failed to login and output the error code
                    DebugInfo(str);
                    return;
                }
                else
                {
                    //登录成功
                    DebugInfo("NET_DVR_Login_V30 succ!");
                    NameloginOn.Text = "注销";

                    dwAChanTotalNum = (uint) DeviceInfo.byChanNum;

                    if (DeviceInfo.byIPChanNum > 0)
                    {
                        InfoIPChannel();
                    }
                    else
                    {
                        for (i = 0; i < dwAChanTotalNum; i++)
                        {
                            ListAnalogChannel(i + 1, 1);
                            iChannelNum[i] = i + (int) DeviceInfo.byStartChan;
                        }

                        comboBoxView.SelectedItem = 1;
                        // MessageBox.Show("This device has no IP channel!");
                    }
                }
            }
            else
            {
                //注销登录 Logout the device
                if (m_lRealHandle >= 0)
                {
                    DebugInfo("Please stop live view firstly"); //登出前先停止预览 Stop live view before logout
                    return;
                }

                if (!CHCNetSDK.NET_DVR_Logout(m_lUserID))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_Logout failed, error code= " + iLastErr;
                    DebugInfo(str);
                    return;
                }
                DebugInfo("NET_DVR_Logout succ!");
                listViewIPChannel.Items.Clear(); //清空通道列表 Clean up the channel list
                m_lUserID = -1;
                NameloginOn.Text = "登陆";
            }
            return;
        }

        public void InfoIPChannel()
        {
            uint dwSize = (uint)Marshal.SizeOf(m_struIpParaCfgV40);

            IntPtr ptrIpParaCfgV40 = Marshal.AllocHGlobal((Int32)dwSize);
            Marshal.StructureToPtr(m_struIpParaCfgV40, ptrIpParaCfgV40, false);

            uint dwReturn = 0;
            int iGroupNo = 0;
            if (!CHCNetSDK.NET_DVR_GetDVRConfig(m_lUserID, CHCNetSDK.NET_DVR_GET_IPPARACFG_V40, iGroupNo, ptrIpParaCfgV40, dwSize, ref dwReturn))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                str = "NET_DVR_GET_IPPARACFG_V40 failed, error code= " + iLastErr; 
                //获取IP资源配置信息失败，输出错误号 Failed to get configuration of IP channels and output the error code
                DebugInfo(str);
            }
            else
            {
                DebugInfo("NET_DVR_GET_IPPARACFG_V40 succ!");

                m_struIpParaCfgV40 = (CHCNetSDK.NET_DVR_IPPARACFG_V40)Marshal.PtrToStructure(ptrIpParaCfgV40, typeof(CHCNetSDK.NET_DVR_IPPARACFG_V40));
               
                for (i = 0; i < dwAChanTotalNum; i++)
                {
                    ListAnalogChannel(i+1, m_struIpParaCfgV40.byAnalogChanEnable[i]);
                    iChannelNum[i] = i + (int)DeviceInfo.byStartChan;                     
                }
                
                byte byStreamType;
                for (i = 0; i < m_struIpParaCfgV40.dwDChanNum; i++)
                {
                    iChannelNum[i + dwAChanTotalNum] = i + (int)m_struIpParaCfgV40.dwStartDChan;
                    byStreamType = m_struIpParaCfgV40.struStreamMode[i].byGetStreamType;

                    dwSize = (uint)Marshal.SizeOf(m_struIpParaCfgV40.struStreamMode[i].uGetStream);
                    switch (byStreamType)
                    {
                        //目前NVR仅支持直接从设备取流 NVR supports only the mode: get stream from device directly
                        case 0:
                            IntPtr ptrChanInfo = Marshal.AllocHGlobal((Int32)dwSize);
                            Marshal.StructureToPtr(m_struIpParaCfgV40.struStreamMode[i].uGetStream, ptrChanInfo, false);
                            m_struChanInfo = (CHCNetSDK.NET_DVR_IPCHANINFO)Marshal.PtrToStructure(ptrChanInfo, typeof(CHCNetSDK.NET_DVR_IPCHANINFO));

                            //列出IP通道 List the IP channel
                            ListIPChannel(i + 1, m_struChanInfo.byEnable, m_struChanInfo.byIPID);
                            iIPDevID[i] = m_struChanInfo.byIPID + m_struChanInfo.byIPIDHigh * 256 - iGroupNo * 64 - 1;

                            Marshal.FreeHGlobal(ptrChanInfo);
                            break;
                        case 6:                            
                            IntPtr ptrChanInfoV40 = Marshal.AllocHGlobal((Int32)dwSize);
                            Marshal.StructureToPtr(m_struIpParaCfgV40.struStreamMode[i].uGetStream, ptrChanInfoV40, false);
                            m_struChanInfoV40 = (CHCNetSDK.NET_DVR_IPCHANINFO_V40)Marshal.PtrToStructure(ptrChanInfoV40, typeof(CHCNetSDK.NET_DVR_IPCHANINFO_V40));
                            
                            //列出IP通道 List the IP channel
                            ListIPChannel(i + 1, m_struChanInfoV40.byEnable, m_struChanInfoV40.wIPID);
                            iIPDevID[i] = m_struChanInfoV40.wIPID - iGroupNo * 64 - 1;

                            Marshal.FreeHGlobal(ptrChanInfoV40);
                            break;
                        default:
                            break;
                    }
                }
            }
            Marshal.FreeHGlobal(ptrIpParaCfgV40);
        
        }
        public void ListIPChannel(Int32 iChanNo, byte byOnline, int byIPID)
        {
            str1 = String.Format("IPCamera {0}", iChanNo);
            m_lTree++;

            if (byIPID == 0)
            {
                str2 = "X"; //通道空闲，没有添加前端设备 the channel is idle                  
            }
            else
            { 
                if(byOnline==0)
                {
                    str2 = "offline"; //通道不在线 the channel is off-line
                }
                else
                    str2 = "online"; //通道在线 The channel is on-line
            }

            listViewIPChannel.Items.Add(new ListViewItem(new string[] { str1, str2 }));//将通道添加到列表中 add the channel to the list
        }
        public void ListAnalogChannel(Int32 iChanNo, byte byEnable)
        {
            str1 = String.Format("Camera {0}", iChanNo);
            m_lTree++;

            if (byEnable == 0)
            {
                str2 = "Disabled"; //通道已被禁用 This channel has been disabled               
            }
            else
            {
                str2 = "Enabled"; //通道处于启用状态 This channel has been enabled
            }

            listViewIPChannel.Items.Add(new ListViewItem(new string[] {str1, str2 }));//将通道添加到列表中 add the channel to the list
        }

        private void listViewIPChannel_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listViewIPChannel.SelectedItems.Count > 0) 
            {
                iSelIndex = listViewIPChannel.SelectedItems[0].Index;  //当前选中的行
            }
        }

        //解码回调函数
        private void DecCallbackFUN(int nPort, IntPtr pBuf, int nSize, ref PlayCtrl.FRAME_INFO pFrameInfo, int nReserved1, int nReserved2)
        {
            // 将pBuf解码后视频输入写入文件中（解码后YUV数据量极大，尤其是高清码流，不建议在回调函数中处理）
            if (pFrameInfo.nType == 3) //#define T_YV12	3
            {
            //    FileStream fs = null;
            //    BinaryWriter bw = null;
            //    try
            //    {
            //        fs = new FileStream("DecodedVideo.yuv", FileMode.Append);
            //        bw = new BinaryWriter(fs);
            //        byte[] byteBuf = new byte[nSize];
            //        Marshal.Copy(pBuf, byteBuf, 0, nSize);
            //        bw.Write(byteBuf);
            //        bw.Flush();
            //    }
            //    catch (System.Exception ex)
            //    {
            //        MessageBox.Show(ex.ToString());
            //    }
            //    finally
            //    {
            //        bw.Close();
            //        fs.Close();
            //    }
            }
        }

        public void RealDataCallBack(Int32 lRealHandle, UInt32 dwDataType, IntPtr pBuffer, UInt32 dwBufSize, IntPtr pUser)
        {
            MyDebugInfo AlarmInfo = new MyDebugInfo(DebugInfo);
            switch (dwDataType)
            {
                case CHCNetSDK.NET_DVR_SYSHEAD:     // sys head
                    if (dwBufSize > 0)
                    {
                        //获取播放句柄 Get the port to play
                        if (!PlayCtrl.PlayM4_GetPort(ref m_lPort))
                        {
                            iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                            str = "PlayM4_GetPort failed, error code= " + iLastErr;
                            this.BeginInvoke(AlarmInfo, str);
                            break;
                        }

                        //设置流播放模式 Set the stream mode: real-time stream mode
                        if (!PlayCtrl.PlayM4_SetStreamOpenMode(m_lPort, PlayCtrl.STREAME_REALTIME))
                        {
                            iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                            str = "Set STREAME_REALTIME mode failed, error code= " + iLastErr;
                            this.BeginInvoke(AlarmInfo, str);
                        }

                        //打开码流，送入头数据 Open stream
                        if (!PlayCtrl.PlayM4_OpenStream(m_lPort, pBuffer, dwBufSize, 2*1024*1024))
                        {
                            iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                            str = "PlayM4_OpenStream failed, error code= " + iLastErr;
                            this.BeginInvoke(AlarmInfo, str); 
                            break;
                        }


                        //设置显示缓冲区个数 Set the display buffer number
                        if (!PlayCtrl.PlayM4_SetDisplayBuf(m_lPort, 15))
                        {
                            iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                            str = "PlayM4_SetDisplayBuf failed, error code= " + iLastErr;
                            this.BeginInvoke(AlarmInfo, str);
                        }

                        //设置显示模式 Set the display mode
                        if (!PlayCtrl.PlayM4_SetOverlayMode(m_lPort, 0, 0/* COLORREF(0)*/)) //play off screen 
                        {
                            iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                            str = "PlayM4_SetOverlayMode failed, error code= " + iLastErr;
                            this.BeginInvoke(AlarmInfo, str);
                        }

                        //设置解码回调函数，获取解码后音视频原始数据 Set callback function of decoded data
                        m_fDisplayFun = new PlayCtrl.DECCBFUN(DecCallbackFUN);
                        if (!PlayCtrl.PlayM4_SetDecCallBackEx(m_lPort, m_fDisplayFun,IntPtr.Zero,0))
                        {
                            this.BeginInvoke(AlarmInfo, "PlayM4_SetDisplayCallBack fail");
                        }

                        //开始解码 Start to play                       
                        if (!PlayCtrl.PlayM4_Play(m_lPort, m_ptrRealHandle))
                        {
                            iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                            str = "PlayM4_Play failed, error code= " + iLastErr;
                            this.BeginInvoke(AlarmInfo, str); 
                            break;
                        }
                    }
                    break;
                case CHCNetSDK.NET_DVR_STREAMDATA:     // video stream data
                    if (dwBufSize > 0 && m_lPort != -1)
                    {
                        for (int i = 0; i < 999; i++)
                        {
                            //送入码流数据进行解码 Input the stream data to decode
                            if (!PlayCtrl.PlayM4_InputData(m_lPort, pBuffer, dwBufSize))
                            {
                                iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                                str = "PlayM4_InputData failed, error code= " + iLastErr;
                                Thread.Sleep(2);
                            }
                            else
                            {                                
                                break;
                            }
                        }                      
                    }
                    break;
                default:
                    if (dwBufSize > 0 && m_lPort != -1)
                    {
                        //送入其他数据 Input the other data
                        for (int i = 0; i < 999; i++)
                        {
                            if (!PlayCtrl.PlayM4_InputData(m_lPort, pBuffer, dwBufSize))
                            {
                                iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                                str = "PlayM4_InputData failed, error code= " + iLastErr;
                                Thread.Sleep(2);
                            }
                            else
                            {                                
                                break;
                            }
                        } 
                    }
                    break;
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (m_lUserID < 0)
            {
                MessageBox.Show("Please login the device firstly!");
                return;
            }

            if (m_bRecord)
            {
                MessageBox.Show("Please stop recording firstly!");
                return;           
            }

            if (m_lRealHandle < 0)
            {
                CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();
                lpPreviewInfo.hPlayWnd = RealPlayWnd.Handle;//预览窗口 live view window
                lpPreviewInfo.lChannel = iChannelNum[(int)iSelIndex];//预览的设备通道 the device channel number
                lpPreviewInfo.dwStreamType = 0;//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
                lpPreviewInfo.dwLinkMode = 0;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
                lpPreviewInfo.bBlocked = true; //0- 非阻塞取流，1- 阻塞取流

                IntPtr pUser = IntPtr.Zero;//用户数据 user data 

                if (comboBoxView.SelectedIndex == 0)
                {
                    //打开预览 Start live view 
                    m_lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, null/*RealData*/, pUser);
                }
                else
                {
                    lpPreviewInfo.hPlayWnd = IntPtr.Zero;//预览窗口 live view window
                    m_ptrRealHandle = RealPlayWnd.Handle;
                    RealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);//预览实时流回调函数 real-time stream callback function 
                    m_lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, RealData, pUser);                
                }
                
                if (m_lRealHandle < 0)
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_RealPlay_V40 failed, error code= " + iLastErr; //预览失败，输出错误号 failed to start live view, and output the error code.
                    DebugInfo(str);
                    return;
                }
                else
                {
                    //预览成功
                    DebugInfo("NET_DVR_RealPlay_V40 succ!");
                    btnPreview.Text = "Stop View";
                }
            }
            else
            {
                //停止预览 Stop live view 
                if (!CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_StopRealPlay failed, error code= " + iLastErr;
                    DebugInfo(str);
                    return;
                }

                if ((comboBoxView.SelectedIndex == 1) && (m_lPort>=0))
                {
                    if (!PlayCtrl.PlayM4_Stop(m_lPort))
                    {
                        iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                        str = "PlayM4_Stop failed, error code= " + iLastErr;
                        DebugInfo(str);
                    }
                    if (!PlayCtrl.PlayM4_CloseStream(m_lPort))
                    {
                        iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                        str = "PlayM4_CloseStream failed, error code= " + iLastErr;
                        DebugInfo(str);
                    }
                    if (!PlayCtrl.PlayM4_FreePort(m_lPort))
                    {
                        iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                        str = "PlayM4_FreePort failed, error code= " + iLastErr;
                        DebugInfo(str);
                    }
                    m_lPort = -1;
                }

                DebugInfo("NET_DVR_StopRealPlay succ!");
                m_lRealHandle = -1;
                btnPreview.Text = "Live View";
                RealPlayWnd.Invalidate();//刷新窗口 refresh the window
            }
            return;
        }

        private void btnBMP_Click(object sender, EventArgs e)
        {
            if (m_lRealHandle < 0)
            {
                DebugInfo("Please start live view firstly!"); //BMP抓图需要先打开预览
                return;
            }

            string sBmpPicFileName;
            //图片保存路径和文件名 the path and file name to save
            sBmpPicFileName = "test.bmp";

            //BMP抓图 Capture a BMP picture
            if (!CHCNetSDK.NET_DVR_CapturePicture(m_lRealHandle, sBmpPicFileName))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                str = "NET_DVR_CapturePicture failed, error code= " + iLastErr;
                DebugInfo(str);
                return;
            }
            else
            {
                str = "NET_DVR_CapturePicture succ and the saved file is " + sBmpPicFileName;
                DebugInfo(str);
            }
            return;
        }

        private void btnJPEG_Click(object sender, EventArgs e)
        {
            int lChannel = iChannelNum[(int)iSelIndex]; //通道号 Channel number

            CHCNetSDK.NET_DVR_JPEGPARA lpJpegPara = new CHCNetSDK.NET_DVR_JPEGPARA();
            lpJpegPara.wPicQuality = 0; //图像质量 Image quality
            lpJpegPara.wPicSize = 0xff; //抓图分辨率 Picture size: 0xff-Auto(使用当前码流分辨率) 
            //抓图分辨率需要设备支持，更多取值请参考SDK文档

            //JPEG抓图保存成文件 Capture a JPEG picture
            string sJpegPicFileName;
            sJpegPicFileName = "filetest.jpg";//图片保存路径和文件名 the path and file name to save

            if (!CHCNetSDK.NET_DVR_CaptureJPEGPicture(m_lUserID, lChannel, ref lpJpegPara, sJpegPicFileName))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                str = "NET_DVR_CaptureJPEGPicture failed, error code= " + iLastErr;
                DebugInfo(str);
                return;
            }
            else
            {
                str = "NET_DVR_CaptureJPEGPicture succ and the saved file is " + sJpegPicFileName;
                DebugInfo(str);
            }

            //JEPG抓图，数据保存在缓冲区中 Capture a JPEG picture and save in the buffer
            uint iBuffSize = 400000; //缓冲区大小需要不小于一张图片数据的大小 The buffer size should not be less than the picture size
            byte[] byJpegPicBuffer = new byte[iBuffSize];
            uint dwSizeReturned = 0;

            if (!CHCNetSDK.NET_DVR_CaptureJPEGPicture_NEW(m_lUserID, lChannel, ref lpJpegPara, byJpegPicBuffer, iBuffSize, ref dwSizeReturned))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                str = "NET_DVR_CaptureJPEGPicture_NEW failed, error code= " + iLastErr;
                DebugInfo(str);
                return;
            }
            else
            {
                //将缓冲区里的JPEG图片数据写入文件 save the data into a file
                string str = "buffertest.jpg";
                FileStream fs = new FileStream(str, FileMode.Create);
                int iLen = (int)dwSizeReturned;
                fs.Write(byJpegPicBuffer, 0, iLen);
                fs.Close();

                str = "NET_DVR_CaptureJPEGPicture_NEW succ and save the data in buffer to 'buffertest.jpg'.";
                DebugInfo(str);
            }

            return;
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            //录像保存路径和文件名 the path and file name to save
            string sVideoFileName;
            sVideoFileName = "test.mp4";

            if (m_bRecord == false)
            {
                //强制I帧 Make one key frame
                int lChannel = iChannelNum[(int)iSelIndex]; //通道号 Channel number
                CHCNetSDK.NET_DVR_MakeKeyFrame(m_lUserID, lChannel);

                //开始录像 Start recording
                if (!CHCNetSDK.NET_DVR_SaveRealData(m_lRealHandle, sVideoFileName))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_SaveRealData failed, error code= " + iLastErr;
                    DebugInfo(str);
                    return;
                }
                else
                {
                    DebugInfo("NET_DVR_SaveRealData succ!");
                    btnRecord.Text = "Stop";
                    m_bRecord = true;
                }
            }
            else
            {
                //停止录像 Stop recording
                if (!CHCNetSDK.NET_DVR_StopSaveRealData(m_lRealHandle))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_StopSaveRealData failed, error code= " + iLastErr;
                    DebugInfo(str);
                    return;
                }
                else
                {
                    str = "NET_DVR_StopSaveRealData succ and the saved file is " + sVideoFileName;
                    DebugInfo(str);
                    btnRecord.Text = "Record";
                    m_bRecord = false;
                }
            }
            return;
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            //停止预览
            if (m_lRealHandle >= 0)
            {
                CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle);
                m_lRealHandle = -1;
            }

            //注销登录
            if (m_lUserID >= 0)
            {
                CHCNetSDK.NET_DVR_Logout(m_lUserID);
                m_lUserID = -1;
            }

            CHCNetSDK.NET_DVR_Cleanup();

            Application.Exit();
        }

        private void checkBoxHiDDNS_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxHiDDNS.Checked)
            {
                //HiDDNS域名方式访问设备
                label5.Text = "HiDDNS域名";
                label1.Text = "HiDDNS Domain";
                textBoxIP.Text = "a1234test";
                textBoxPort.Enabled = false;
            }
            else
            {
                //IP地址或者普通域名方式访问设备
                label5.Text = "设备IP/域名";
                label1.Text = "Device IP/Domain";
                textBoxIP.Text = "10.16.3.245";
                textBoxPort.Enabled = true;
            }
        }

        private void listViewIPChannel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int iCurChan = iChannelNum[(int)iSelIndex];
                if (iCurChan >= m_struIpParaCfgV40.dwStartDChan)
                {
                    if (DialogResult.OK == MessageBox.Show("是否配置该IP通道！", "配置提示", MessageBoxButtons.OKCancel))
                    {
                        IPChannelConfig dlg = new IPChannelConfig();
                        dlg.m_struIPParaCfgV40 = m_struIpParaCfgV40;
                        dlg.m_lUserID = m_lUserID;
                        int iCurChanIndex = iCurChan - (int)m_struIpParaCfgV40.dwStartDChan; //通道索引
                        int iCurIPDevIndex = iIPDevID[iCurChanIndex]; //设备ID索引
                        dlg.iIPDevIndex = iCurIPDevIndex;
                        dlg.iChanIndex = iCurChanIndex;                   
                        dlg.ShowDialog();               
                    }
                }
                else
                {

                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //刷新通道列表
            listViewIPChannel.Items.Clear();
            for (i = 0; i < dwAChanTotalNum; i++)
            {
                ListAnalogChannel(i + 1, 1);
                iChannelNum[i] = i + (int)DeviceInfo.byStartChan;
            }
            InfoIPChannel(); 
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            fullScreenHelper = new FullScreenHelper(RealPlayWnd);
            try
            {
                string sqlstr = "select * from EncodeDevice";
                var devices = DbHelperSQLite.ExecDataSet(sqlstr);
                if (devices.Tables[0].Rows.Count > 0)
                {
                    cmbDomain.DataSource = devices.Tables[0];
                    cmbDomain.ValueMember = "Name";
                    cmbDomain.DisplayMember = "Name";
                    cmbDomain.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
          
        }

   

     
        private void NameLogin()
        {
            try
            {
                if (cmbDomain.Text != string.Empty)
                {
                    string sqlstr =
                        string.Format(
                            "select * from EncodeDevice where Name ='{0}'",
                            cmbDomain.Text);
                    var devices = DbHelperSQLite.ExecDataSet(sqlstr).Tables[0];
                    if (devices.Rows[0]["LoginAddress"].ToString() == string.Empty)
                        throw new Exception("设备IP为空，请添加设备。");
                    HKLoginIn(devices.Rows[0]["LoginAddress"].ToString(), devices.Rows[0]["LoginPort"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void NameloginOn_Click(object sender, EventArgs e)
        {
            NameLogin();
        }

        private void DoubleClickFullScreen(object sender, EventArgs e)
        {
            fullScreenHelper.FullScreen();
        }

        private void KeyDownEsc(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                fullScreenHelper.FullScreen();
            }
        }
    }
}
