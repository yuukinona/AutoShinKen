using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.IO;
using System.Windows.Forms;

namespace AutoShinKen
{
    public class PageJudgement
    {
        #region PageNo
        public enum PageNo : uint
        {
            Main_Page = 0,
            Repair_Page = 1,
            Select_BigMap_Page = 2,
            Battle_Page = 3,
            Win_Page = 4,
            Loading_Page = 5,
            Repairing_Page = 6,
            Select_Map_Page = 7,
            Choose_Go_Page = 8,
            Scroll_Page = 9,
            Unknow_Page = 255
        }
        #endregion
        //public Bitmap MainPage, BigMap, BattlePage, WinPage, LoadingPage, RepairPage, Repairing; //ChooseMap, GoPage, ScrollPage1, ScrollPage2, ScrollPage3, ScrollPage4,
        public Color[,] PageColor = new Color[12, 4];
        public int[, ,] JudgeSquare = new int[12, 4, 2];
        public Point[] Player = new Point[7];
        private Color JudgePlayerStateColor = Color.FromArgb(255, 1, 1, 1);
        private int[,] RepairJudge = new int [7, 2];

        public PageJudgement()
        {
            Player[1] = new Point(108, 650);
            Player[2] = new Point(188, 650);
            Player[3] = new Point(268, 650);
            Player[4] = new Point(348, 650);
            Player[5] = new Point(428, 650);
            Player[6] = new Point(508, 650);
            RepairJudge[1, 0] = 93 - 8; RepairJudge[1, 1] = 163 - 29;
            RepairJudge[2, 0] = 93 - 8; RepairJudge[2, 1] = 332 - 29;
            RepairJudge[3, 0] = 93 - 8; RepairJudge[3, 1] = 504 - 29;
            RepairJudge[4, 0] = 433 - 8; RepairJudge[4, 1] = 163 - 29;
            RepairJudge[5, 0] = 433 - 8; RepairJudge[5, 1] = 332 - 29;
            RepairJudge[6, 0] = 433 - 8; RepairJudge[6, 1] = 504 - 29;

            #region Set Judge Point
            //0 Main Page
            JudgeSquare[0, 0, 0] = 801; JudgeSquare[0, 0, 1] = 149-29;
            JudgeSquare[0, 1, 0] = 858; JudgeSquare[0, 1, 1] = 145-29;
            PageColor[0, 0] = Color.FromName("ffc4c2c7");
            PageColor[0, 1] = Color.FromName("ff4b406f");
            //1 Repair Page
            JudgeSquare[1, 0, 0] = 278-8; JudgeSquare[1, 0, 1] = 287-29;
            JudgeSquare[1, 1, 0] = 22-8; JudgeSquare[1, 1, 1] = 500 - 29;
            JudgeSquare[1, 2, 0] = 746-8; JudgeSquare[1, 2, 1] = 313 - 29;
            PageColor[1, 0] = Color.FromName("ff3a382f");
            PageColor[1, 1] = Color.FromName("ff5b4a45");
            PageColor[1, 2] = Color.FromName("ff40332d");
            //JudgeSquare[1, 2, 0] = 591; JudgeSquare[1, 2, 1] = 287;
            //2 Big Map
            JudgeSquare[2, 0, 0] = 120; JudgeSquare[2, 0, 1] = 641 - 29;
            JudgeSquare[2, 1, 0] = 223; JudgeSquare[2, 1, 1] = 292 - 29;
            PageColor[2, 0] = Color.FromName("ff3b0f3d");
            PageColor[2, 1] = Color.FromName("ffa7b065");
            //3 BattlePage
            JudgeSquare[3, 0, 0] = 913; JudgeSquare[3, 0, 1] = 149 - 29;
            JudgeSquare[3, 1, 0] = 391; JudgeSquare[3, 1, 1] = 66 - 29;
            JudgeSquare[3, 2, 0] = 845; JudgeSquare[3, 2, 1] = 641 - 29;
            PageColor[3, 0] = Color.FromName("ffefcbce");
            PageColor[3, 1] = Color.FromName("ffcebda2");
            PageColor[3, 2] = Color.FromName("ff181821");
            //4 WinPage
            JudgeSquare[4, 0, 0] = 185; JudgeSquare[4, 0, 1] = 57 - 29;
            JudgeSquare[4, 1, 0] = 272; JudgeSquare[4, 1, 1] = 636 - 29;
            JudgeSquare[4, 2, 0] = 229; JudgeSquare[4, 2, 1] = 145 - 29;
            PageColor[4, 0] = Color.FromName("ffe03705");
            PageColor[4, 1] = Color.FromName("fffa2800");
            PageColor[4, 2] = Color.FromName("ff0b172c");
            //5 LoadingPage
            JudgeSquare[5, 0, 0] = 67; JudgeSquare[5, 0, 1] = 379 - 29;
            JudgeSquare[5, 1, 0] = 186; JudgeSquare[5, 1, 1] = 187 - 29;
            JudgeSquare[5, 2, 0] = 831; JudgeSquare[5, 2, 1] = 511 - 29;
            PageColor[5, 0] = Color.FromName("ff272517");
            PageColor[5, 1] = Color.FromName("ffd9a18e");
            PageColor[5, 2] = Color.FromName("ff0054ff");
            #endregion
            //6 Repairing Page
            JudgeSquare[6, 0, 0] = 853; JudgeSquare[6, 0, 1] = 399 - 29;
            JudgeSquare[6, 1, 0] = 725-8; JudgeSquare[6, 1, 1] = 286 - 29;
            PageColor[6, 0] = Color.FromName("ff2a2a28");
            PageColor[6, 1] = Color.FromName("ff9e989a");


        }

        public bool JudgePlayerState(Bitmap btm, int player)
        {
            if (GetPoint(btm, Player[player].X, Player[player].Y).Name == JudgePlayerStateColor.Name) return true;
            return false;
        }

        public Color GetPoint(Bitmap btm, int x, int y)
        {
            return btm.GetPixel(x, y);
        }

        public PageNo Judge(Bitmap pic)
        {
            if (pic == null) return PageNo.Unknow_Page;

            //0 Main Page
            if (Compare(pic, 0, 0, 1)) return PageNo.Main_Page;

            //1 Repair Page
            if (Compare(pic, 1, 1, 2)) return PageNo.Repair_Page;

            //2 Big Map
            if (Compare(pic, 2, 2, 1)) return PageNo.Select_BigMap_Page;

            //3 BattlePage
            if (Compare(pic, 3, 3, 2)) return PageNo.Battle_Page;

            //4 WinPage
            if (Compare(pic, 4, 4, 2)) return PageNo.Win_Page;

            //5 LoadingPage
            if (Compare(pic, 5, 5, 2)) return PageNo.Loading_Page;

            //Repairing Page
            if (Compare(pic, 6, 6, 1))
            {
                if (!Compare(pic, 1, 1, 2)) return PageNo.Repairing_Page;

            }

            return PageNo.Unknow_Page;
        }

        private bool Compare(Bitmap a, int x, int No, int t)
        {
            for (int i = 0; i <= t; i++)
            {
                //if (GetPoint(a, LocationX + x[i], LocationY + y[i])!=GetPoint(b, x[i], y[i])) return false;
                //if (!Compare_Color(GetPoint(a, JudgeSquare[No, i, 0]-LocationX, JudgeSquare[No, i, 1]-LocationY), GetPoint(b, JudgeSquare[No, i, 0]-LocationX, JudgeSquare[No, i, 1]-LocationY))) return false;
                if (!Compare_Color(GetPoint(a, JudgeSquare[No, i, 0], JudgeSquare[No, i, 1]), PageColor[x, i])) return false;
            }
            return true;
        }

        private bool Compare_Color(Color a, Color b)
        {
            if (a.Name == b.Name) return true;
            //if ((Abs((a.R - b.R)) <= 0.1) && (Abs((a.G - b.G)) <= 0.1) && (Abs((a.B - b.B)) <= 0.1)) return true;
            else return false;
        }

        public float Abs(float i)
        {
            if (i < 0L)
                return i * (-1.0f);
            return i;
        }

        public bool InitRepair(Bitmap bmp,int num)
        {
            Color c = GetPoint(bmp, RepairJudge[num, 0], RepairJudge[num, 1]);
            if (c.R > 150 && (c.R-c.G>60))
            {
                return true;
            }
            return false;
        }

        public Color InitRepairTestor(Bitmap bmp, int num)
        {
            return GetPoint(bmp, RepairJudge[num, 0], RepairJudge[num, 1]);
        }
        public void SetColor(Bitmap bmp,int pageNO)
        {
            int i;
            if (pageNO == 0)
            {
                for (i = 0; i <= 1; i++)
                {
                    PageColor[0, i] = GetPoint(bmp,JudgeSquare[pageNO, i, 0], JudgeSquare[pageNO, i, 1]);
                }
            }
            if (pageNO == 1)
            {
                for (i = 0; i <= 2; i++)
                {
                    PageColor[1, i] = GetPoint(bmp, JudgeSquare[pageNO, i, 0], JudgeSquare[pageNO, i, 1]);
                }
            }
            if (pageNO == 2)
            {
                for (i = 0; i <= 1; i++)
                {
                    PageColor[2, i] = GetPoint(bmp, JudgeSquare[pageNO, i, 0], JudgeSquare[pageNO, i, 1]);
                }
            }
            if (pageNO == 3)
            {
                for (i = 0; i <= 2; i++)
                {
                    PageColor[3, i] = GetPoint(bmp, JudgeSquare[pageNO, i, 0], JudgeSquare[pageNO, i, 1]);
                }
            }
            if (pageNO == 4)
            {
                for (i = 0; i <= 2; i++)
                {
                    PageColor[4, i] = GetPoint(bmp, JudgeSquare[pageNO, i, 0], JudgeSquare[pageNO, i, 1]);
                }
            }
            if (pageNO == 5)
            {
                for (i = 0; i <= 2; i++)
                {
                    PageColor[5, i] = GetPoint(bmp, JudgeSquare[pageNO, i, 0], JudgeSquare[pageNO, i, 1]);
                }
            }
            if (pageNO == 6)
            {
                for (i = 0; i <= 1; i++)
                {
                    PageColor[6, i] = GetPoint(bmp, JudgeSquare[pageNO, i, 0], JudgeSquare[pageNO, i, 1]);
                }
            }

            return;
        }
    }
}
