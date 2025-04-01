using System;

/*
인스턴스 멤버 : 생성된 객체 ( 인스턴스 )
클래스멤버    : 생성된 객체로 사용할 수 없고, 클래스이름으로 사용하는멤버
                static이 붙은 멤버 ( 객체 없이 사용 가능 ) 
 */
namespace WoosongBit41.Sample
{    
    /// <summary>
    /// 내가 만든 입출력 예제
    /// 인텔리센스 주석?
    /// shift + space 에 추천 코드 뜰 때 아래 설명이 있음.
    /// </summary>
    internal class MyIO
    {        
        public static void PrintSample1()
        {
            Console.WriteLine("Hello, World!");
            Console.WriteLine(10);
            Console.WriteLine(10 + "문자열 연결" + 20);  // 10 문자열연결 20 
            Console.WriteLine(10 + 20 + "문자열 연결");  // 30 문자열연결
        }
        /// <summary>
        /// WirteLine : 인덱스 참조 가능.
        /// </summary>
        public static void PrintSample2()
        {
            Console.WriteLine("{0} , {1} , 인덱스 활용" , 10 , 'A');                         // 10 A 인덱스활용
            Console.WriteLine("{0} + {1} = {2}  인덱스 활용" , 10 , 20 , 10+20);             // 10 + 20 = 30
            Console.WriteLine("{0} / {1} = {2}  인덱스 활용" , 10 , 20 , (float)10/20);      // 10  20 = 0.5           
        }
        // Write : 개행처리 없음 , 사용방법은 동일
        public static void PrintSample3()
        {
            Console.Write("문자열 출력");
            Console.Write("개행처리는 직접 \n");
            Console.Write("10");
        }
        // 타입 입력
        public static void InPutSample1()
        {
            Console.Write("이름: ");
            string name = Console.ReadLine();  // return : string

            Console.Write("나이: ");
            string temp = Console.ReadLine();
            int age     = int.Parse(temp);

            Console.Write("몸무게: ");
            float weight = float.Parse(Console.ReadLine());

            Console.Write("성별(남/여): ");          // 한글 -> 유니코드 가능
            char gender = char.Parse(Console.ReadLine());

            Console.WriteLine("\n\n[ 입력결과 ]" ); 
            Console.WriteLine("이름: " + name);
            Console.WriteLine("나이: " + age);
            Console.WriteLine("몸무게: " + weight);
            Console.WriteLine("성별(남/여): " + gender);

        }
        // Console.ReadKey();
        // 특수키
        public static void InPutSample()
        {           
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);      // true -> _getch
                if (key.Key == ConsoleKey.UpArrow)
                {
                    Console.WriteLine("위");
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    Console.WriteLine("아래");
                }
                else if (key.Key == ConsoleKey.F1)
                {
                    Console.WriteLine("F1");
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("Escape");
                    break;
                }
                else if (key.Key == ConsoleKey.D1)
                {
                    Console.WriteLine("D1");
                }
                else if (key.Key == ConsoleKey.Pa1)
                {
                    Console.WriteLine("Pa1");
                }
                else if (key.Key == ConsoleKey.X)
                {
                    Console.WriteLine("X");
                }
                else if (key.Key == ConsoleKey.NumPad0)
                {
                    Console.WriteLine("NumPad0");
                }
            }
        }
    } 
}
