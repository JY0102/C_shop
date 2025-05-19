using Riot.Contracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace Riot_Search_Client.Converts
{
    /// <summary>
    /// 시간 변환 컨버터 - Unix 타임스탬프를 "몇 시간 전" 형식으로 변환
    /// </summary>
    internal class TimeConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            string format = "yyyy-MM-dd tt h:mm:ss";

            string temp = value.ToString().Trim();

            CultureInfo temp1 = new CultureInfo("ko-KR");

            DateTime time = DateTime.ParseExact(temp, format, temp1);


            TimeSpan diff = DateTime.Now - time;

            if (diff.TotalDays < 1)
            {
                if (diff.TotalHours < 1)
                {
                    return $"{diff.TotalMinutes.ToString("N0")} 분 전";
                }

                return $"{diff.TotalHours.ToString("N0")} 시간 전";
            }

            return $"{diff.TotalDays.ToString("N0")} 일 전";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 승리/패배에 따른 배경색 변환 컨버터
    /// </summary>
    internal class WinConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool win)
            {
                if (win)
                {
                    var color = (Color)ColorConverter.ConvertFromString("#FFD4E4FE");
                    return new SolidColorBrush(color);

                }
                else
                {
                    var color = (Color)ColorConverter.ConvertFromString("#FFFFEEEE");
                    return new SolidColorBrush(color);
                }
            }

            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 승리/패배를 한글 텍스트로 변환하는 컨버터
    /// </summary>
    internal class WinToStringConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool win)
            {
                if (win)
                    return "승리";
                else
                    return "패배";
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// KDA 수치에 따른 색상 변환 컨버터
    /// </summary>
    internal class KdaConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double Kda;

            if (double.TryParse(value.ToString(), out Kda))
            {
                if (Kda < 2.0)
                {
                    return Brushes.LightGray;
                }
                else if (Kda < 3.0)
                {
                    return Brushes.Green;
                }
                else if (Kda < 4.0)
                {
                    return Brushes.Blue;
                }
                else
                {
                    return Brushes.Red;
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 챔피언 레벨 표시 컨버터
    /// </summary>
    internal class ChampLevelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "Lv.1";

            if (int.TryParse(value.ToString(), out int level))
            {
                return $"Lv.{level}";
            }

            return $"Lv.{value}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 팀 어시스트 계산 컨버터
    /// </summary>
    internal class TeamAssistsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Match match = (Match)value;

                // 블루팀 어시스트 계산
                if ((string)parameter == "블루팀")
                {
                    int blueAssists = match.participants
                                .Where(p => p.teamid== "블루팀")
                                .Sum(p => int.Parse(p.assists));
                    return blueAssists.ToString();
                }
                // 레드팀 어시스트 계산
                else if ((string)parameter == "레드팀")
                {
                    int redAssists = match.participants
                                .Where(p => p.teamid== "레드팀")
                                .Sum(p => int.Parse(p.assists));
                    return redAssists.ToString(); 
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"팀 통계 설정 오류: {ex.Message}");
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 챔피언 딜량 맥스 값 계산 컨버터
    /// </summary>
    internal class DamageConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var participants = (List<Participant>)value;

                var top = participants.OrderByDescending(m => int.Parse(m.totalDamageDealtToChampions)).First();

                return int.Parse(top.totalDamageDealtToChampions);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DamageConvert 오류: {ex.Message}");
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 챔피언 받은 피해량 맥스 값 계산 컨버터
    /// </summary>
    internal class TakenConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var participants = (List<Participant>)value;

                var top = participants.OrderByDescending(m => int.Parse(m.totalDamageTaken)).First();

                return int.Parse(top.totalDamageTaken);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"TakenConvert 오류: {ex.Message}");
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 선취점 여부
    /// </summary>
    internal class FirstBloodConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (bool.Parse(value.ToString()))
            {
                return "선취점 달성";
            }
            else
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 중앙에 표시되는 승리여부
    /// </summary>
    internal class LastConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var match = (Match)value;

            foreach (var item in match.participants)
            {
                if (item.win)
                    return item.teamid + "승리";
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
