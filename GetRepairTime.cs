using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AutoShinKen
{
    class GetRepairTime
    {
        Point[,] judge = new Point[10, 2];
        byte[,] judgeResult = new byte[10, 2];
        public GetRepairTime()
        {
            judge[0, 0] = new Point(0, 6); judgeResult[0, 0] = 1;
            judge[0, 1] = new Point(8, 5); judgeResult[0, 1] = 1;

            judge[1, 0] = new Point(3, 2); judgeResult[1, 0] = 1;
            judge[1, 1] = new Point(4, 2); judgeResult[1, 1] = 1;

            judge[2, 0] = new Point(0, 11); judgeResult[2, 0] = 1;
            judge[2, 1] = new Point(0, 11); judgeResult[2, 1] = 1;

            judge[3, 0] = new Point(0, 6);
            judge[3, 1] = new Point(8, 5);

            judge[4, 0] = new Point(5, 9); judgeResult[4, 0] = 1;
            judge[4, 1] = new Point(8, 11); judgeResult[4, 1] = 1;

            judge[5, 0] = new Point(4, 4); judgeResult[5, 0] = 1;
            judge[5, 1] = new Point(3, 3); judgeResult[5, 1] = 0;

            judge[6, 0] = new Point(0, 6); judgeResult[6, 0] = 1;
            judge[6, 1] = new Point(1, 0); judgeResult[6, 1] = 0;

            judge[7, 0] = new Point(1, 0); judgeResult[7, 0] = 1;
            judge[7, 1] = new Point(1, 0); judgeResult[7, 1] = 1;

            judge[8, 0] = new Point(4, 4); judgeResult[8, 0] = 1;
            judge[8, 1] = new Point(8, 1); judgeResult[8, 1] = 1;

            judge[9, 0] = new Point(8, 5); judgeResult[9, 0] = 1;
            judge[9, 1] = new Point(1, 0); judgeResult[9, 1] = 0;
        }
        public int JudgeNumberByImage(int[,] listarr)
        {
            Point p1,p2;
            for (int i = 0; i <= 9; i++)
            {
                if (i == 3) continue;
                p1=judge[i,0];
                p2=judge[i,1];
                if (listarr[p1.X, p1.Y] == judgeResult[i, 0] && listarr[p2.X, p2.Y] == judgeResult[i, 1])
                {
                    return i;
                }
            }
                return 3;
        }
    }
}
