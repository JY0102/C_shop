using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0401
{
    class _6_직렬화
    {
        static void Main(string[] args)
        {
            // 직렬화
            string name = "가나다";
            int age = 12;
            string phone = "010-1234-1234";
            char gender = '남';


            string str = name + "@" + age + "@" + phone + "@" + gender;
            Console.WriteLine(str);


            // 역직렬화
            string[] str1 = str.Split('@');
            string name1 = str1[0];
            int age1 = int.Parse(str1[1]);
            string phone1 = str1[2];
            char gender1 = char.Parse(str1[3]);

            Console.WriteLine(name1 + "," + age1 + "," + phone1 + "," + gender1);
        }
    }
}
