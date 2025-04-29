using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Chess
{
    internal class Piece
    {
        private static bool[,] Enables = new bool[8,8];
        public static List<int[]> MovePiece(Button btn, int x, int y, bool[,] e)
        {
            Enables = e;

            switch(btn.Content.ToString())
            {
                case "폰":   return MovePawn(btn.Foreground, x, y);
                case "룩":   return MoveRook(x, y);
                case "말":   return MoveKnight(x, y);
                case "비숍": return MoveBishop(x, y);
                case "퀸":   return MoveQueen(x, y);
                case "킹":   return MoveKing(x, y);
            }
            return null;
        }

        public static List<int[]> MovePawn(Brush color , int x, int y)
        {
            List<int[]> list = new List<int[]>();

            if(color == Brushes.Black)
            {
                if (y == 1)
                {
                    if(!Enables[x, y + 1])
                        list.Add(new int[] { x , y + 1 });
                    if(!Enables[x, y + 2])
                        list.Add(new int[] { x , y + 2 });
                }
                else if( y == 7)
                {
                    // 할꺼
                    // 폰 기물변환
                }
                else
                {
                    if(!Enables[x,y +1])
                        list.Add(new int[] { x, y + 1 });

                    if (Enables[Math.Max( 0 , x - 1 ) , y+1])
                    {
                        list.Add(new int[] { x -1 , y+1 });
                    }
                    if (Enables[x + 1, y + 1])
                    {
                        list.Add(new int[] {x +1 , y + 1 });
                    }

                }
            }
            else
            {
                if (y == 6)
                {
                    if (!Enables[Math.Max(0, x - 1), y - 1])
                        list.Add(new int[] { x, y - 1 });
                    if (!Enables[x, y - 2])
                        list.Add(new int[] { x, y - 2 });
                }
                else if( y == 0)
                {
                    // 할꺼
                    // 폰 기물변환
                }
                else
                {
                    if (!Enables[x, y - 1])
                        list.Add(new int[] { x, y - 1 });

                    if (Enables[Math.Max(0, x - 1), y - 1])
                    {
                        list.Add(new int[] { x - 1, y - 1 });
                    }
                    if (Enables[x + 1, y - 1])
                    {
                        list.Add(new int[] { x + 1, y - 1 });
                    }
                }
            }

            return list;
        }
        public static List<int[]> MoveRook(int x, int y)
        {
            List<int[]> list = new List<int[]>();

            // 상
            for(int i= y + 1; i<8; i++)
            {
                if (Enables[x, i])
                    break;

                list.Add(new int[] {x,i});
            }
            // 우
            for(int i= x + 1; i<8; i++)
            {
                if (Enables[i, y])
                    break;

                list.Add(new int[] { i , y });
            }
            // 하
            for(int i = y - 1; i >= 0; i--)
            {
                if (Enables[x, i])
                    break;

                list.Add(new int[] { x , i });
            }
            // 좌
            for (int i = x - 1; i >= 0; i--)
            {
                if (Enables[i, y])
                    break;

                list.Add(new int[] { i , y });
            }

            return list;
        }
        public static List<int[]> MoveKnight(int x, int y)
        {
            List<int[]> list = new List<int[]>();

            int[] dx = { -2, -2, 2, 2, -1, 1, -1, 1 };
            int[] dy = { 1, -1, 1, -1, -2, -2, 2, 2 };

            // 반복문으로 각 방향을 순회
            for (int i = 0; i < dx.Length; i++)
            {
                int newX = x + dx[i];
                int newY = y + dy[i];

                if( newX  < 0 || newX > 7 )
                     continue;
                if (newY < 0 || newY > 7)
                    continue;

                list.Add(new int[] { newX, newY });
            }

            return list;
        }
        public static List<int[]> MoveBishop(int x, int y)
        {
            List<int[]> list = new List<int[]>();

            // 우 하
            for(int i = 1; i < 8 - x; i++)
            {
                if (y + i < 8)
                {
                    if (Enables[x + i,  y + i])
                        break;
                    list.Add(new int[] { x + i , y + i});
                }
            }
            // 우 상
            for(int i = 1; i < 8 - x; i++)
            {
                if (y - i >= 0)
                {
                    if (Enables[x + i, y - i])
                        break;
                    list.Add(new int[] { x + i, y - i });
                }
            }
            // 좌 하
            for (int i = 1; i <= x; i++)
            {
                if (y + i < 8)
                {
                    if (Enables[x - i , y + i])
                        break;
                    list.Add(new int[] { x - i, y + i });
                }
            }
            // 좌 상
            for (int i = 1; i <= x; i++)
            {
                if (y - i >= 0 )
                {
                    if (Enables[x - i, y - i])
                        break;
                    list.Add(new int[] { x - i, y - i });
                }
            }



            return list;
        }
        public static List<int[]> MoveQueen(int x, int y)
        {
            List<int[]> list = new List<int[]>();

            var temp1 = MoveRook(x,y);
            var temp2 = MoveBishop(x,y);

            list.AddRange(temp1);
            list.AddRange(temp2);

            return list;
        }
        public static List<int[]> MoveKing(int x, int y)
        {
            List<int[]> list = new List<int[]>();

            int[] move_x = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] move_y = { -1, 0, 1, -1, 1, 1, 0, -1 };

            for(int i=0; i<8; i++)
            {
                if (x + move_x[i] < 0 || x + move_x[i] > 7)
                    continue;
                if (y + move_y[i] < 0 || y + move_y[i] > 7)
                    continue;
                if (Enables[x + move_x[i], y + move_y[i]])
                    continue;

                list.Add(new int[] { x + move_x[i], y + move_y[i] });
            }
            return list;
        }
    }
}
