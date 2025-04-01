using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace _0401
{
    class _7_인자전달
    {
        static void foo(int n1, ref int n2, out int n3)
        {
            n1 = 11;    // 값을 못 바꿈
            n2 = 22;    // ref -> 값을 바꾸든 안바꾸든 선택해서 바꿀 수 있음
            n3 = 33;    // out -> 값을 무조건 바꿔야함 // 안바꾸면 ERROR
        }
        // TryParse 로 예외 처리
        static void Main(string[] args)
        {
            Console.Write("정수 입력 : ");
            int num;
            if (int.TryParse(Console.ReadLine(), out num) == true)
            {
                Console.WriteLine(num);
            }
            else
            {
                Console.WriteLine("잘못된 입력");
            }
        }

        // try , catch 를 Exception 으로 예외 처리
        private static void sample2()
        {
            try
            {
                Console.Write("정수 입력 : ");
                int num = int.Parse(Console.ReadLine());
                Console.WriteLine(num);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // "입력 문자열의 형식이 잘못되었습니다." 출력
            }
        }

        // 값전달 , ref , out 차이
        private static void sample1()
        {
            int num1 = 1, num2 = 2, num3 = 3;

            // 값전달 , ref(레퍼런스) , out 전달
            foo(num1, ref num2, out num3);
            Console.WriteLine(num1 + "\t" + num2 + "\t" + num3);
        }
    }
}
