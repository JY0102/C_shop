using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0401
{
    // C++ : 멤버필드 -> 생성자 & 소멸자              -> get , set 메서드    ->기능메서드
    // C#  : 멤버필드 -> **속성(get , set ,프로퍼티)  -> 생성자              -> 기능메서드
    internal class Member
    {
        // region , end region
        #region 1. 멤버필드 및 속성
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private int age;
        public int Age
        {
            get { return age; }
            set
            {
                if (value < 0)
                    return;
                age = value;
            }
        }

        private char gender;
        public char Gender
        {
            get { return gender; }
            set
            {
                if (value == '남' || value == '여')
                    gender = value;
            }
        }
        #endregion

        #region 2. 생성자  ( 멤버 메서드에서 멤버 속성(프로퍼티)를 사용 권장 ) 
        public Member(string _name, int _age, char _gender)
        {   // 프로퍼티 안에서 예외 처리 가능
            Name = _name;
            Age = _age;
            Gender = _gender;
        }
        #endregion

        #region 3. 기능 메서드
        public void Print()
        {
            Console.WriteLine(Name + " " + Age + " " + Gender);
        }
        #endregion
    }

    class _8_캡슐화
    {
        static void Main(string[] args)
        {
            Member m = new Member("가나다", 20, '남');

            m.Age = -10;    // 값 예외처리 
            m.Print();
                
        }
    }
}
