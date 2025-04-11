using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Odio
{
    internal class Program
    {
        public List<int> random = new List<int>();


        public int X { get; set; } = 0;
        public int Id { get; set; } = 0;

        public const int UNDER_POS = 25;
        public object LOCK = new object();
        public object LOCK_2 = new object();
        public object LOCK_3 = new object();

        static void Main(string[] args)
        {
            new Program().start();
            Console.ReadLine();
            Console.ReadLine();
            Console.ResetColor();
        }

        public void start()
        {
            Console.CursorVisible = false;


            for (int i=0; i<15; i++)
            {
                random.Add(0);

                Thread th = new Thread(WorkThread);
                th.IsBackground = true;
                th.Start();
            }
        }

        public void WorkThread()
        {
            int max;
            int id;
            int x_pos;
            ConsoleColor color;

            lock(LOCK)
            {
                id = Id;
                x_pos = X;
                color = (ConsoleColor)id;
                Id++;
                X += 4;
            }

            Thread th = new Thread(RandomThread);
            th.IsBackground = true;
            th.Start(id);

            while (true)
            {
                lock (LOCK_2)
                {
                    max = random[id];
                    Console.ForegroundColor = color;       // 색깔

                    for(int i=0; i <12; i++)   // 네모박스 출력
                    {
                        Console.SetCursorPosition(x_pos, UNDER_POS - i);

                        if(i < max) { Console.WriteLine("\u25A0"); }
                        else        { Console.WriteLine("  "); }
                    }
                    Console.ResetColor();
                }
                Console.SetCursorPosition(0,0);
                Thread.Sleep(100);
            }
        }

        public void RandomThread(object obj)
        {
            Random rnd = new Random();
            int id = (int)obj;

            while(true)
            {
                random[id] = rnd.Next( 1,  13 );
                Thread.Sleep(10);
            }
        }
    }
}
