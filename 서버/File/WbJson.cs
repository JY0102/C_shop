using Calendar.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;


namespace Calendar.Json
{
    internal static class WbJson
    {
        private const string JSONTEXT_FILE = "calendar.json";
        private static object Update = new object();

        #region 기능
        public static string UpdateJson(string id , Dictionary<string, Schedule> schedules)
        {
            var data = new Dictionary<string, Schedule>() { { id , schedules[id] } };
            string json = JsonConvert.SerializeObject(data , Formatting.Indented);

            Thread  th = new Thread(JsonThread);
            th.IsBackground = true;
            th.Start(schedules);

            return json;
        }
        #endregion

        #region Json 값 저장하는 Thread
        private static void JsonThread(object obj)
        {
            lock(Update)
            {
                try
                {
                    var data = (Dictionary<string, Schedule>)obj;
                    WriteJson(data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("JsonThread Error : " + ex.Message);
                }
            }
            Thread.Sleep(50);
        }
        #endregion

        #region  Read , Write
        public static Dictionary<string, Schedule> ReadJson()
        {
            try
            {
                // 파일 존재 여부 체크
                if (!System.IO.File.Exists(JSONTEXT_FILE))
                    throw new Exception("파일 최초 실행.....");

                // 파일 내용 읽기
                string json = System.IO.File.ReadAllText(JSONTEXT_FILE);

                // JSON 파싱해서 Dictionary로 변환         // return 값이 new Dictionary 와 동일
                var data = JsonConvert.DeserializeObject<Dictionary<string, Schedule>>(json);

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("READJSON: " + ex.Message);
                return new Dictionary<string, Schedule>();
            }
        }
        public static void WriteJson(Dictionary<string, Schedule> schedules)
        {
            try
            {
                string json = JsonConvert.SerializeObject(schedules, Formatting.Indented);
                System.IO.File.WriteAllText(JSONTEXT_FILE, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("WirteJson ERROR" + ex.Message);
                throw;
            }
        }
        #endregion
    }
}
