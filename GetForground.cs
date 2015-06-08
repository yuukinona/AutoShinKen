using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;   //调用WINDOWS API函数时要用到
using Microsoft.Win32;  //写入注册表时要用到
using System.IO;


namespace AutoShinKen
{
    class GetForground
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        private Int16 width, height;

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern int GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", EntryPoint = "GetWindowText")]
        public static extern int GetWindowText(
            IntPtr hWnd,
            StringBuilder lpString,
            int nMaxCount
        );


        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, Int32 bInheritHandle, UInt32 dwProcessId);

        [DllImport("psapi.dll")]
        static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, [In] [MarshalAs(UnmanagedType.U4)] int nSize);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindow(IntPtr hWnd);

        private System.IO.DirectoryInfo dir;
        private int fileNum;
        private string path;
        private string MyPicPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\";
        public static Point GetRect()
        {
            IntPtr scr = GetForegroundWindow();
            RECT rc1;
            RECT rc2;    
            GetWindowRect(scr, out rc1);  //Get window location
             
            GetClientRect(scr, out rc2);  //Get the active square of window
            Point p = new Point();
            p.X = Control.MousePosition.X - rc1.left;
            p.Y = Control.MousePosition.Y - rc1.top;
            return p;
        }
    }
}
