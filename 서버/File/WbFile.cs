using Calendar.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.File
{
    internal static class WbFile
    {
        private const string USERTEXT_FILE = "user.text";

        #region  Read , Write
        public static List<User> ReadFile()
        {
            List<User> users = new List<User>();
            try
            {
                StreamReader reader = new StreamReader(USERTEXT_FILE);

                int size = int.Parse(reader.ReadLine());
                for (int i = 0; i < size; i++)
                {
                    string temp = reader.ReadLine();
                    string[] sp = temp.Split('|');

                    string id = sp[0];
                    string pw = sp[1];

                    users.Add(new User(id, pw));
                }
                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine("파일 최초 실행" + ex.Message);
                return users;
            }
        }
        public static void WirteFile(List<User> users)
        {
            try
            {
                StreamWriter writer = new StreamWriter(USERTEXT_FILE);

                writer.WriteLine(users.Count);
                foreach (User user in users)
                {
                    writer.WriteLine($"{user.Id}|{user.Pw}");
                }
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(" WriteFileError : " + ex.Message);
                throw;
            }
        }
        #endregion

    }
}
