using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Data
{

    internal class Schedule
    {
        public Dictionary<string, List<string>> ScheduleData { get; set; }
        public Schedule()
        {
            ScheduleData = new Dictionary<string, List<string>>();
        }

        public void Add(string date , string info)
        {
            if (ScheduleData.ContainsKey(date))
            {
                ScheduleData[date].Add(info);
            }
            else
            {
                ScheduleData.Add(date, new List<string>());
                ScheduleData[date].Add(info);
            }
        }
        public void Remvoe(string date, string info)
        {
            try
            {
                if (info == null)
                {
                    Remvoe(date);
                }

                ScheduleData.ContainsKey(date);
                ScheduleData[date].Remove(info);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void Remvoe(string date)
        {
            try
            {
                ScheduleData.ContainsKey(date);

                ScheduleData.Remove(date);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
