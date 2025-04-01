using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0401
{
    /// <summary>
    /// override 재정의
    /// </summary>
    class Member
    {
        private string name;
        private string phone;

        public Member(string name, string phone)
        {
            this.name = name;
            this.phone = phone;
        }

        public void Print()
        {
            Console.WriteLine($"name : {name}\tphone:  {phone}");
        }

        public override string ToString()
        {
            return name + "\t" + phone;
        }
        /// <summary>
        /// 주소 비교 -> 값비교
        /// </summary>        
        public override bool Equals(object obj) 
        {
            Member member = (Member)obj;
            return (this.name == member.name && this.phone == member.phone);
        }
    }
}
