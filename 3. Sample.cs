using System;


namespace _0401
{
    /// <summary>
    /// string 예제
    /// </summary>
    internal class Sample
    {
        /// <summary>
        /// 문자열
        /// </summary>
        public static 
        void Example1()
        {
            string s = string.Empty;
            Console.WriteLine(s);
                        
            s = "Hello";
            Console.WriteLine(s);

            string s1 = s;
            Console.WriteLine(s1);

            // "Hello 문자열 수정" 이 임시로 저장되고 ( 새로운 주소 생성 ) 
            // 저장된 주소가 s 에 대입된다.   s = 새로운 주소 , s1 = 기존 주소            
            s = "Hello 문자열 수정";
            Console.WriteLine(s);       // Hello 문자열 수정
            Console.WriteLine(s1);      // Hello       ** 수정 안됨 ** 
        }
        //string -> 값 형식 처럼사용 ( 일반적)
        public static void Example2()
        {
            char[] temp = { 'a', 'b', 'c', '\0' };
            string str1 = new string(temp);
            Console.WriteLine(str1);            // 100 번지 저장

            string str2 = str1;                 // 100번지 저장
            string str3 = new string(temp);     // 200번지 저장

            Console.WriteLine(str1);
            Console.WriteLine(str2);
            Console.WriteLine(str3);

        }
        public static void Example()
        {
            string str1 = "abc";    // 어딘가 'abc' 저장 + 주소 반환            
            str1 = "abc" + "ABC";   // 값형식 연산  // 'abcABC' 저장 + 주소 반환
            str1 += "DEF";          // 'abcABCDEF'
            Console.WriteLine(str1);

            string str2 = "abc";        // A번지
            string str3 = "abc";        // B번지
               
            // 값형식 동일
            // 참조형식은 동일하지 않음
            if (str2 == str3)                   // strcmp 안써도 됨. // 값형식 이면 값형식으로 비교  ( 값 ) , 참조형식이면 참조형식으로 비교 ( 주소 ) 
            {
                Console.WriteLine("동일");
            }
            if(str2.Equals(str3) == true)       // 주소를 따라가서 있는 값을 비교
            {
                Console.WriteLine("동일");
            }
        }
    }
}
