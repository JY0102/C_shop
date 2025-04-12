using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Data
{
    internal class User
    {
        public string Id {  get; private set; }
        public string Pw {  get; private set; }

        public User(string id , string pw)
        {
            Id = id;
            Pw = pw;
        }
    }
}
