using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace _0401
{
    /// <summary>
    /// 박싱 언박싱
    /// </summary>
    class _6_박싱언박싱
    {
        // pg 23 ~ pg 24
        static void Main(string[] args)
        {
            int num = 10;
            object obj = num;           // 박싱: (obj 참조형 타입 = int 값타임 ) 
            Console.WriteLine(obj);     // 1) 힙메모리 생성 -> num 의 값을 저장          2) 주소 반환


            int num1 = (int)obj;        // 언박싱  : 값타입 = 참조형 타입
            Console.WriteLine(num1);    // 1) obj가  참조한 주소로 이동 하여 값 획득      2) 획득한 값을 반환

            // str static 함수 예시
            string str = string.Format("이름: {0,-5} 나이: {1,10}", "가나다", 10);
            Console.WriteLine(str);
            string str2 = "abc";
            Console.WriteLine(str2.Insert(1,"def"));
        }

    }
}
