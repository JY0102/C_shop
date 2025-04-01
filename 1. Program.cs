using System;
using WoosongBit41.Sample;


// 헤더파일 x
// 네임스페이스만 동일 -> 같은 공간으로 인식
namespace _0401
{
    class Program
    {
        static void Main(string[] args)
        {                     

        }
        private static void NewMethod_member()
        {
            Member member1 = new Member("가나다", "1234");
            member1.Print();
            Console.WriteLine(member1.ToString());
            Console.WriteLine(member1);

            Member member2 = new Member("가나다", "1234");

            if (member1 == member2)      // 저장된 주소 비교            
                Console.WriteLine("동일");


            if (member1.Equals(member2) == true)
                Console.WriteLine("동일");
        }
        private static void NewMethod()
        {
            WoosongBit41.Sample.MyIO.PrintSample1();
            MyIO.PrintSample1();
        }
    }
}
