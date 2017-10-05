#pragma warning disable 0618

[assembly: System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.RequestMinimum, UnmanagedCode = true)]
namespace Infrastructure
{
    public class MessageBoxManager
    {
        private delegate System.IntPtr HookProc(int nCode, System.IntPtr wParam, System.IntPtr lParam);
        private delegate bool EnumChildProc(System.IntPtr hWnd, System.IntPtr lParam);

        private const int WH_CALLWNDPROCRET = 12;
        private const int WM_DESTROY = 0x0002;
        private const int WM_INITDIALOG = 0x0110;
        private const int WM_TIMER = 0x0113;
        private const int WM_USER = 0x400;
        private const int DM_GETDEFID = WM_USER + 0;

        private const int MBOK = 1;
        private const int MBCancel = 2;
        private const int MBAbort = 3;
        private const int MBRetry = 4;
        private const int MBIgnore = 5;
        private const int MBYes = 6;
        private const int MBNo = 7;


        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern System.IntPtr SendMessage(System.IntPtr hWnd, int Msg, System.IntPtr wParam, System.IntPtr lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern System.IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, System.IntPtr hInstance, int threadId);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int UnhookWindowsHookEx(System.IntPtr idHook);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern System.IntPtr CallNextHookEx(System.IntPtr idHook, int nCode, System.IntPtr wParam, System.IntPtr lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetWindowTextLengthW", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern int GetWindowTextLength(System.IntPtr hWnd);

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetWindowTextW", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern int GetWindowText(System.IntPtr hWnd,  System.Text.StringBuilder text, int maxLength);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int EndDialog(System.IntPtr hDlg, System.IntPtr nResult);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool EnumChildWindows(System.IntPtr hWndParent, EnumChildProc lpEnumFunc, System.IntPtr lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetClassNameW", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern int GetClassName(System.IntPtr hWnd,  System.Text.StringBuilder lpClassName, int nMaxCount);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int GetDlgCtrlID(System.IntPtr hwndCtl);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern System.IntPtr GetDlgItem(System.IntPtr hDlg, int nIDDlgItem);

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetWindowTextW", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern bool SetWindowText(System.IntPtr hWnd, string lpString);


        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct CWPRETSTRUCT
        {
            public System.IntPtr lResult;
            public System.IntPtr lParam;
            public System.IntPtr wParam;
            public uint message;
            public System.IntPtr hwnd;
        };

        private static HookProc hookProc;
        private static EnumChildProc enumProc;
        [System.ThreadStatic]
        private static System.IntPtr hHook;
        [System.ThreadStatic]
        private static int nButton;

        /// <summary>
        /// OK text
        /// </summary>
        public static string OK = "&OK";
        /// <summary>
        /// Cancel text
        /// </summary>
        public static string Cancel = "&Cancel";
        /// <summary>
        /// Abort text
        /// </summary>
        public static string Abort = "&Abort";
        /// <summary>
        /// Retry text
        /// </summary>
        public static string Retry = "&Retry";
        /// <summary>
        /// Ignore text
        /// </summary>
        public static string Ignore = "&Ignore";
        /// <summary>
        /// Yes text
        /// </summary>
        public static string Yes = "&Yes";
        /// <summary>
        /// No text
        /// </summary>
        public static string No = "&No";

        static MessageBoxManager()
        {
            hookProc = new HookProc(MessageBoxHookProc);
            enumProc = new EnumChildProc(MessageBoxEnumProc);
            hHook = System.IntPtr.Zero;
        }

        /// <summary>
        /// Enables MessageBoxManager functionality
        /// </summary>
        /// <remarks>
        /// MessageBoxManager functionality is enabled on current thread only.
        /// Each thread that needs MessageBoxManager functionality has to call this method.
        /// </remarks>
        public static void Register()
        {
            if (hHook != System.IntPtr.Zero)
            {
                throw new System.NotSupportedException("One hook per thread allowed.");
            }

            hHook = SetWindowsHookEx(WH_CALLWNDPROCRET, hookProc, System.IntPtr.Zero, System.AppDomain.GetCurrentThreadId());
        }

        /// <summary>
        /// Disables MessageBoxManager functionality
        /// </summary>
        /// <remarks>
        /// Disables MessageBoxManager functionality on current thread only.
        /// </remarks>
        public static void Unregister()
        {
            if (hHook != System.IntPtr.Zero)
            {
                UnhookWindowsHookEx(hHook);

                hHook = System.IntPtr.Zero;
            }
        }

        private static System.IntPtr MessageBoxHookProc(int nCode, System.IntPtr wParam, System.IntPtr lParam)
        {
            if (nCode < 0)
                return CallNextHookEx(hHook, nCode, wParam, lParam);

            CWPRETSTRUCT msg = (CWPRETSTRUCT)System.Runtime.InteropServices.Marshal.PtrToStructure(lParam, typeof(CWPRETSTRUCT));
            System.IntPtr hook = hHook;

            if (msg.message == WM_INITDIALOG)
            {
                int nLength = GetWindowTextLength(msg.hwnd);
                 System.Text.StringBuilder className = new  System.Text.StringBuilder(10);
                GetClassName(msg.hwnd, className, className.Capacity);
                if (className.ToString() == "#32770")
                {
                    nButton = 0;
                    EnumChildWindows(msg.hwnd, enumProc, System.IntPtr.Zero);
                    if (nButton == 1)
                    {
                        System.IntPtr hButton = GetDlgItem(msg.hwnd, MBCancel);
                        if (hButton != System.IntPtr.Zero)
                        {
                            SetWindowText(hButton, OK);
                        }
                    }
                }
            }

            return CallNextHookEx(hook, nCode, wParam, lParam);
        }

        private static bool MessageBoxEnumProc(System.IntPtr hWnd, System.IntPtr lParam)
        {
             System.Text.StringBuilder className = new  System.Text.StringBuilder(10);
            GetClassName(hWnd, className, className.Capacity);
            if (className.ToString() == "Button")
            {
                int ctlId = GetDlgCtrlID(hWnd);
                switch (ctlId)
                {
                    case MBOK:
                        SetWindowText(hWnd, OK);
                        break;
                    case MBCancel:
                        SetWindowText(hWnd, Cancel);
                        break;
                    case MBAbort:
                        SetWindowText(hWnd, Abort);
                        break;
                    case MBRetry:
                        SetWindowText(hWnd, Retry);
                        break;
                    case MBIgnore:
                        SetWindowText(hWnd, Ignore);
                        break;
                    case MBYes:
                        SetWindowText(hWnd, Yes);
                        break;
                    case MBNo:
                        SetWindowText(hWnd, No);
                        break;

                }
                nButton++;
            }

            return true;
        }


    }
}
