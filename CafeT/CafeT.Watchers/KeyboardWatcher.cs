using CafeT.Html;
using CafeT.Objects;
using CafeT.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CafeT.Watchers
{
    public class GlobalKeyboardHook
    {
        //http://www.c-sharpcorner.com/article/key-logger-application-in-C-Sharp/
        #region Constant, Structure and Delegate Definitions
        /// <summary>
        /// defines the callback type for the hook
        /// </summary>
        public delegate int keyboardHookProc(int code, int wParam, ref KeyboardHookStruct lParam);

        public struct KeyboardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        public bool _hookAll = false;
        public bool HookAllKeys
        {
            get
            {
                return _hookAll;
            }
            set
            {
                _hookAll = value;
            }
        }
        const int WH_KEYBOARD_LL = 13;
        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        const int WM_KEYPRESS = 0x102;
        const int WM_SYSKEYDOWN = 0x104;
        const int WM_SYSKEYUP = 0x105;
        const int WM_SYSKEYPRESS = 0x0106;

        /// <summary>
        /// windows virtual key codes
        /// </summary>
        private const byte VK_RETURN = 0X0D; //Enter
        private const byte VK_SPACE = 0X20; //Space
        private const byte VK_SHIFT = 0x10;
        private const byte VK_CAPITAL = 0x14;

        #endregion

        #region Instance Variables
        /// <summary>
        /// The collections of keys to watch for
        /// </summary>
        public List<Keys> HookedKeys = new List<Keys>();
        /// <summary>
        /// Handle to the hook, need this to unhook and call the next hook
        /// </summary>
        IntPtr hhook = IntPtr.Zero;
        keyboardHookProc khp;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when one of the hooked keys is pressed
        /// </summary>
        public event KeyEventHandler KeyDown;
        /// <summary>
        /// Occurs when one of the hooked keys is released
        /// </summary>
        public event KeyEventHandler KeyUp;
        /// <summary>
        /// Occurs when one of the hooked keys is released
        /// </summary>
        public event KeyPressEventHandler KeyPress;
        #endregion

        #region Constructors and Destructors
        /// <summary>
        /// Initializes a new instance of the <see cref="globalKeyboardHook"/> class and installs the keyboard hook.
        /// </summary>
        public GlobalKeyboardHook()
        {
            khp = new keyboardHookProc(hookProc);
            hook();
        }
        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="globalKeyboardHook"/> is reclaimed by garbage collection and uninstalls the keyboard hook.
        /// </summary>
        ~GlobalKeyboardHook()
        {
            Unhook();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Installs the global hook
        /// </summary>
        public void hook()
        {
            IntPtr hInstance = LoadLibrary("User32");
            hhook = SetWindowsHookEx(WH_KEYBOARD_LL, khp, hInstance, 0);
        }

        /// <summary>
        /// Uninstalls the global hook
        /// </summary>
        public void Unhook()
        {
            UnhookWindowsHookEx(hhook);
        }

        /// <summary>
        /// The callback for the keyboard hook
        /// </summary>
        /// <param name="code">The hook code, if it isn't >= 0, the function shouldn't do anyting</param>
        /// <param name="wParam">The event type</param>
        /// <param name="lParam">The keyhook event information</param>
        /// <returns></returns>
        public int hookProc(int code, int wParam, ref KeyboardHookStruct lParam)
        {
            if (code >= 0)
            {
                if (wParam == WM_KEYDOWN)
                {
                    byte[] keyState = new byte[256];
                    GetKeyboardState(keyState);
                    byte[] inBuffer = new byte[2];
                    if (ToAscii(lParam.vkCode, lParam.scanCode, keyState, inBuffer, lParam.flags) == 1)
                    {
                        char key = (char)inBuffer[0];
                        bool isDownShift = ((GetKeyState(VK_SHIFT) & 0x80) == 0x80 ? true : false);
                        bool isDownCapslock = (GetKeyState(VK_CAPITAL) != 0 ? true : false);
                        if ((isDownCapslock ^ isDownShift) && Char.IsLetter(key))
                        {
                            key = Char.ToUpper(key);
                        }

                        if (KeyPress != null)
                        {
                            KeyPressEventArgs e = new KeyPressEventArgs(key);
                            KeyPress(this, e);
                            if (e.Handled)
                                return 1;
                        }
                    }
                }
            }
            return CallNextHookEx(hhook, code, wParam, ref lParam);
        }

        #endregion

        #region DLL imports
        /// <summary>
        /// Sets the windows hook, do the desired event, one of hInstance or threadId must be non-null
        /// </summary>
        /// <param name="idHook">The id of the event you want to hook</param>
        /// <param name="callback">The callback.</param>
        /// <param name="hInstance">The handle you want to attach the event to, can be null</param>
        /// <param name="threadId">The thread you want to attach the event to, can be null</param>
        /// <returns>a handle to the desired hook</returns>
        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, keyboardHookProc callback, IntPtr hInstance, uint threadId);

        /// <summary>
        /// Unhooks the windows hook.
        /// </summary>
        /// <param name="hInstance">The hook handle that was returned from SetWindowsHookEx</param>
        /// <returns>True if successful, false otherwise</returns>
        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        /// <summary>
        /// Calls the next hook.
        /// </summary>
        /// <param name="idHook">The hook id</param>
        /// <param name="nCode">The hook code</param>
        /// <param name="wParam">The wparam.</param>
        /// <param name="lParam">The lparam.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref KeyboardHookStruct lParam);

        /// <summary>
        /// Loads the library.
        /// </summary>
        /// <param name="lpFileName">Name of the library</param>
        /// <returns>A handle to the library</returns>
        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="pbKeyState"></param>
        /// <returns></returns>
        [DllImport("user32")]
        private static extern int GetKeyboardState(byte[] pbKeyState);

        /// <summary>
        ///
        /// </summary>
        /// <param name="uVirtKey"></param>
        /// <param name="uScanCode"></param>
        /// <param name="lpbKeyState"></param>
        /// <param name="lpwTransKey"></param>
        /// <param name="fuState"></param>
        /// <returns></returns>
        [DllImport("user32")]
        private static extern int ToAscii(
            int uVirtKey,
            int uScanCode,
            byte[] lpbKeyState,
            byte[] lpwTransKey,
            int fuState);

        /// <summary>
        ///
        /// </summary>
        /// <param name="vKey"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern short GetKeyState(int vKey);

        #endregion
    }

    public class KeyboardWatcher
    {
        //public string Url { set; get; }
        //public Timer AutoTime { set; get; }
        //public double Interval { set; get; }
        //public string LastHtmlContent { set; get; }
        //public string CurrentHtmlContent { set; get; }
        //public string HtmlNews { set; get; }
        //public DateTime LastRead { set; get; }
        //public WebPage Page { set; get; }
        //public int CountOfRead { set; get; }

        //public KeyboardWatcher(string url, double interval)
        //{
        //}

        //private void AutoTime_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    Read();
        //    if (HasNews())
        //    {
        //        GetNews();
        //        this.SaveToText(@"C:\Users\taipm\Downloads\UrlWatcher-"+ LastRead.ToShortTimeString().Replace(":","") + ".txt");
        //    }
        //}
        //public void SaveToText(string pathFile)
        //{
        //    StreamWriter _w = new StreamWriter(pathFile, true, UTF8Encoding.UTF8);
        //    string _content = this.ToString();
        //    _w.Write(_content);
        //    _w.Close();
        //}
        //public bool CanRead()
        //{
        //    try
        //    {
        //        using (var client = new WebClient())
        //        {
        //            using (var stream = client.OpenRead(Url))
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public void Read()
        //{
        //    if(CanRead())
        //    {
        //        Page = new WebPage(Url);
        //        if(CountOfRead == 0)
        //        {
        //            LastRead = DateTime.Now;
        //            LastHtmlContent = Page.Smart.HtmlContent;
        //            CurrentHtmlContent = LastHtmlContent;
        //        }
        //        else
        //        {
        //            LastRead = DateTime.Now;
        //            LastHtmlContent = CurrentHtmlContent;
        //            CurrentHtmlContent = Page.Smart.HtmlContent;
        //        }
        //        CountOfRead = CountOfRead + 1;
        //    }
        //}

        //public bool HasNews()
        //{
        //    return LastHtmlContent.IsDifference(CurrentHtmlContent);
        //}

        //public string GetNews()
        //{
        //    HtmlNews =  "+ HasNews - Not implementation";
        //    return HtmlNews;
        //}

        //public override string ToString()
        //{
        //    return this.DisplayObjectInfo("Url, LastRead, HtmlNews");
        //}
    }
}
