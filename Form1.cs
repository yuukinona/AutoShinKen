using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections;


namespace AutoShinKen
{
    public partial class Form1 : Form
    {

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount); 

        private void _Click(IntPtr xx,Point pos)
        {
            ushort x = (ushort )pos.X; // X coordinate of the click 
            ushort y = (ushort )pos.Y; // Y coordinate of the click 
            IntPtr handle = xx;
            StringBuilder className = new StringBuilder(100);
            int c = 0;
            while (className.ToString() != "MacromediaFlashPlayerActiveX") // The class control for the browser 
            {
                c++;
                handle = GetWindow(handle, 5); // Get a handle to the child window 
                GetClassName(handle, className, className.Capacity);
                if (c == 1000) break;
            }

            IntPtr lParam = (IntPtr)(((uint) y << 16 )| x); // The coordinates 
            IntPtr wParam = IntPtr.Zero; // Additional parameters for the click (e.g. Ctrl) 
            const uint downCode = 0x201; // Left click down code 
            const uint upCode = 0x202; // Left click up code 
            SendMessage(handle, downCode, wParam, lParam); // Mouse button down 
            SendMessage(handle, upCode, wParam, lParam); // Mouse button up 
        } 

        public static void MouseClick(int x, int y, IntPtr handle, string Class)
        {
            StringBuilder className = new StringBuilder(100);
            while (className.ToString() != Class)
            {
                handle = GetWindow(handle, 5);
                GetClassName(handle, className, className.Capacity);
            }

            IntPtr lParam = (IntPtr)((y << 16) | x);
            IntPtr wParam = IntPtr.Zero;
            const uint downCode = 0x201;
            const uint upCode = 0x202;
            SendMessage(handle, downCode, wParam, lParam);
            SendMessage(handle, upCode, wParam, lParam);
        }

        GetWindowScreenShot sc;
        PageJudgement PageJudge;
        int WidthOfBar, HeightOfTitle;
        Bitmap bmp;
        byte[] RepairTank=new byte[7]{0,0,0,0,0,0,0};
        int SysTickCount=0;
        Timer time1;
        Timer SystemTimer;
        IntPtr a=IntPtr.Zero;

        [DllImport("user32.dll", EntryPoint = "FindWindowEx", CharSet = CharSet.Auto)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        IntPtr Tweb;

        #region Clear_Memory
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        /// <summary>
        /// 释放内存
        /// </summary>
        public static void ClearMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }
        #endregion

        static List<IntPtr> GetAllChildrenWindowHandles(IntPtr hParent,  int maxCount)
        {
            List<IntPtr> result = new List<IntPtr>();
            int ct = 0;
            IntPtr prevChild = IntPtr.Zero;
            IntPtr currChild = IntPtr.Zero;
            while (true && ct < maxCount)
            {
                currChild = FindWindowEx(hParent, prevChild, null, null);
                if (currChild == IntPtr.Zero) break;
                result.Add(currChild);
                prevChild = currChild;
                ++ct;
            }
            return result;
        }

        private void InitialData()
        {
            sc = new GetWindowScreenShot(WidthOfBar, HeightOfTitle);
            List<IntPtr> children = GetAllChildrenWindowHandles(a, 100);
            for (int i = 0; i < children.Count; ++i)
                this.richTextBox1.Text += (children[i].ToString() + ",");
            Tweb = children[0];
        }

        public Form1()
        {
            InitializeComponent();

            WidthOfBar = Convert.ToInt32((this.Size.Width - this.ClientRectangle.Width) / 2);
            HeightOfTitle = this.Height - this.ClientRectangle.Height - WidthOfBar;

            a= GetWindowS.FindWindowsWithText("しんけん！");;
            if (a == IntPtr.Zero)
            {
                MessageBox.Show("No ShinKenViewer is running!");
                System.Environment.Exit(0);
                this.Close();
                //MessageBox.Show("1");
            }
            else
            {
                InitialData();
                
            }
            //tHwnd = FindWindowEx(a, IntPtr.Zero, null, "asdfghj");
            //if (tHwnd != IntPtr.Zero) MessageBox.Show("1");
            PageJudge = new PageJudgement();
            this.SystemTimer = new Timer();
            this.SystemTimer.Interval = 100;
            this.SystemTimer.Tick += new System.EventHandler(this.SysTick);
            this.SystemTimer.Start();
        }

        private void snapshot()
        {                        
            bmp=sc.GetWindowCapture(a);
            return;
        }

        #region SetInitialColor
        private void button1_Click(object sender, EventArgs e)
        {
            snapshot();
            PageJudge.SetColor(bmp, 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            snapshot();
            PageJudge.SetColor(bmp, 1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            snapshot();
            PageJudge.SetColor(bmp, 2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            snapshot();
            PageJudge.SetColor(bmp, 3);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            snapshot();
            PageJudge.SetColor(bmp, 4);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            snapshot();
            PageJudge.SetColor(bmp, 5);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            snapshot();
            PageJudge.SetColor(bmp, 6);
        }
        #endregion

        private void Ticking(object sender, EventArgs e)
        {
            this.richTextBox1.Text = GetForground.GetRect().ToString();
            //GetPage();
            return;
        }
        
        private void SysTick(object sender, EventArgs e)
        {
            SysTickCount++;
            if (SysTickCount == 500)
            {
                this.SystemTimer.Dispose();
                this.SystemTimer = new Timer();
                this.SystemTimer.Interval = 100;
                this.SystemTimer.Tick += new System.EventHandler(this.SysTick);
                this.SystemTimer.Start();
            }
            snapshot();
            this.pictureBox1.Image = new Bitmap(bmp, 330, 220);
            ClearMemory();
            //this.richTextBox8.Text = "                "+DateTime.Now.ToString();
            return;
        }

        /// <summary>
        /// [START]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            
            if (1== 0)
            {
                MessageBox.Show("Please go to Reapir Page to Initialize the Repair tank!");
                return;
            }/*
            for (int i = 100; i <= 500; i++)
                for (int j = 100; j <= 500; j++)
                    Mouse_Click(new Point(i, j));*/
            //Mouse_Click(children[0], new Point(950, 31));
            _Click(Tweb, new Point(57, 635));
            
            this.time1 = new Timer();
            this.time1.Interval = 1000;
            this.time1.Tick += new System.EventHandler(this.Ticking);
            time1.Start();
            
        }
        private void GetPage()
        {
            snapshot();
            PageJudgement.PageNo p_no = PageJudge.Judge(bmp);
            bool flag = true;
            if (p_no == PageJudgement.PageNo.Repair_Page)
            {
                CheckRepair();
                for (int i = 1; i <= 6; i++)
                    if (RepairTank[i] == 2)
                    {
                        this.richTextBox1.Text = PageJudgement.PageNo.Repairing_Page.ToString() + "\n";
                        flag = false;
                        break;
                    }
                if (flag) this.richTextBox1.Text = PageJudgement.PageNo.Repair_Page.ToString() + "\n";
            }
            else this.richTextBox1.Text = PageJudge.Judge(bmp).ToString() + "\n";
            /*
            this.richTextBox1.Text += (PageJudge.GetPoint(bmp, PageJudge.JudgeSquare[4, 0, 0], PageJudge.JudgeSquare[4, 0, 1]).Name + "," + PageJudge.PageColor[4, 0].Name);
            this.richTextBox1.Text += "\n";
            this.richTextBox1.Text += (PageJudge.GetPoint(bmp, PageJudge.JudgeSquare[4, 1, 0], PageJudge.JudgeSquare[4, 1, 1]).Name + "," + PageJudge.PageColor[4, 1].Name);
            this.richTextBox1.Text += "\n";
            this.richTextBox1.Text += (PageJudge.GetPoint(bmp, PageJudge.JudgeSquare[4, 2, 0], PageJudge.JudgeSquare[4, 2, 1]).Name + "," + PageJudge.PageColor[4, 2].Name);*/
        }

        /// <summary>
        /// [STOP]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            time1.Stop();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            CheckRepair();
        }

        private void CheckRepair()
        {
            snapshot();
            if (PageJudge.Judge(bmp) != PageJudgement.PageNo.Repair_Page) return;            
            if (RepairTank[1] == 0)
            {
                if (PageJudge.InitRepair(bmp, 1))
                {
                    richTextBox2.Text = "Repair tank 1";
                    RepairTank[1] = 1;
                }
                if (PageJudge.InitRepair(bmp, 2))
                {
                    richTextBox4.Text = "Repair tank 2";
                    RepairTank[2] = 1;
                }
                if (PageJudge.InitRepair(bmp, 3))
                {
                    richTextBox6.Text = "Repair tank 3";
                    RepairTank[3] = 1;
                }
                if (PageJudge.InitRepair(bmp, 4))
                {
                    richTextBox3.Text = "Repair tank 4";
                    RepairTank[4] = 1;
                }
                if (PageJudge.InitRepair(bmp, 5))
                {
                    richTextBox5.Text = "Repair tank 5";
                    RepairTank[5] = 1;
                }
                if (PageJudge.InitRepair(bmp, 6))
                {
                    richTextBox7.Text = "Repair tank 6";
                    RepairTank[6] = 1;
                }
            }
            else
            {
                //1
                if (RepairTank[1] == 1 && !PageJudge.InitRepair(bmp, 1))
                {
                    richTextBox2.Text = "Repair tank 1\n" + "Repairing";
                    RepairTank[1] = 2;
                }
                if (RepairTank[1] == 2 && PageJudge.InitRepair(bmp, 1))
                {
                    richTextBox2.Text = "Repair tank 1";
                    RepairTank[1] = 1;
                }
                //2
                if (RepairTank[2] == 1 && !PageJudge.InitRepair(bmp, 2))
                {
                    richTextBox4.Text = "Repair tank 2\n" + "Repairing";
                    RepairTank[2] = 2;
                }
                if (RepairTank[2] == 2 && PageJudge.InitRepair(bmp, 2))
                {
                    richTextBox4.Text = "Repair tank 2";
                    RepairTank[2] = 1;
                }
                //3
                if (RepairTank[3] == 1 && !PageJudge.InitRepair(bmp, 3))
                {
                    richTextBox6.Text = "Repair tank 3\n" + "Repairing";
                    RepairTank[3] = 2;
                }
                if (RepairTank[3] == 2 && PageJudge.InitRepair(bmp, 3))
                {
                    richTextBox6.Text = "Repair tank 3";
                    RepairTank[3] = 1;
                }
                //4
                if (RepairTank[4] == 1 && !PageJudge.InitRepair(bmp, 4))
                {
                    richTextBox3.Text = "Repair tank 4\n" + "Repairing";
                    RepairTank[4] = 2;
                }
                if (RepairTank[4] == 2 && PageJudge.InitRepair(bmp, 4))
                {
                    richTextBox3.Text = "Repair tank 4";
                    RepairTank[4] = 1;
                }
                //5
                if (RepairTank[5] == 1 && !PageJudge.InitRepair(bmp, 5))
                {
                    richTextBox5.Text = "Repair tank 5\n" + "Repairing";
                    RepairTank[5] = 2;
                }
                if (RepairTank[5] == 2 && PageJudge.InitRepair(bmp, 5))
                {
                    richTextBox5.Text = "Repair tank 5";
                    RepairTank[5] = 1;
                }
                //6
                if (RepairTank[6] == 1 && !PageJudge.InitRepair(bmp, 6))
                {
                    richTextBox7.Text = "Repair tank 6\n" + "Repairing";
                    RepairTank[6] = 2;
                }
                if (RepairTank[6] == 2 && PageJudge.InitRepair(bmp, 6))
                {
                    richTextBox7.Text = "Repair tank 6";
                    RepairTank[6] = 1;
                }
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int Cx = (int)((e.Location.X * 960) / 330);
            int Cy = (int)((e.Location.Y * 640) / 220);
            _Click(Tweb, new Point(Cx, Cy));
        }

        

    }
}
