using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FormUI.Camera
{
    public class FullScreenHelper
    {
        bool m_bFullScreen = false;
        IntPtr m_OldWndParent = IntPtr.Zero;
        WINDOWPLACEMENT m_OldWndPlacement = new WINDOWPLACEMENT();
        Control m_control = null;
        public FullScreenHelper(Control c)
        {
            m_control = c;
        }
        [DllImport("User32.dll")]
        static extern bool LockWindowUpdate(IntPtr hWndLock);

        struct POINT
        {
            int x;
            int y;
        } ;
        struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        } ;

        [DllImport("User32.dll")]
        static extern bool SetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);
        [DllImport("User32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("User32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("User32.dll")]
        static extern IntPtr GetDesktopWindow();
        [DllImport("User32.dll")]
        static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);
        [DllImport("User32.dll")]
        static extern int GetSystemMetrics(int nIndex);

        public void FullScreen()
        {
            if (m_bFullScreen)
            {
                LockWindowUpdate(m_control.Handle);
                SetParent(m_control.Handle, m_OldWndParent);
                SetWindowPlacement(m_control.Handle, ref m_OldWndPlacement);
                SetForegroundWindow(m_OldWndParent);
                LockWindowUpdate(IntPtr.Zero);
            }
            else
            {
                GetWindowPlacement(m_control.Handle, ref m_OldWndPlacement);
                int nScreenWidth = GetSystemMetrics(0);
                int nScreenHeight = GetSystemMetrics(1);
                m_OldWndParent = m_control.Parent.Handle;
                SetParent(m_control.Handle, GetDesktopWindow());

                WINDOWPLACEMENT wp1 = new WINDOWPLACEMENT();
                wp1.length = (uint)Marshal.SizeOf(wp1);
                wp1.showCmd = 1;
                wp1.rcNormalPosition.left = 0;
                wp1.rcNormalPosition.top = 0;
                wp1.rcNormalPosition.right = nScreenWidth;
                wp1.rcNormalPosition.bottom = nScreenHeight;
                SetWindowPlacement(m_control.Handle, ref wp1);
                SetForegroundWindow(GetDesktopWindow());
                SetForegroundWindow(m_control.Handle);
            }

            m_bFullScreen = !m_bFullScreen;
        }
        struct WINDOWPLACEMENT
        {
            public uint length;
            public uint flags;
            public uint showCmd;
            public POINT ptMinPosition;
            public POINT ptMaxPosition;
            public RECT rcNormalPosition;
        } ;
    }
}
