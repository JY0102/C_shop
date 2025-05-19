using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace Riot.Contracts
{
    public class Match
    {
        public string matchId { get; set; }

        public string GameNickName { get; set; }

        public string _queueId = null;
        public string queueId
        {
            get => _queueId;
            set
            {
                switch (value)
                {
                    case "420": _queueId = "솔로랭크"; break;
                    case "440": _queueId = "자유랭크"; break;
                    case "400": _queueId = "일반"; break;
                    case "450": _queueId = "칼바람"; break;
                    default: _queueId = value; break;
                }
            }
        }

        private DateTime _gameEndTimestamp { get; set; }
        public string gameEndTimestamp
        {
            get => _gameEndTimestamp.ToString();
            set
            {
                if (long.TryParse(value.ToString(), out long timestamp))
                    _gameEndTimestamp = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).LocalDateTime;
                else
                    _gameEndTimestamp = DateTime.Parse(value);
            }
        }

        public List<Team> teams { get; set; }
        public Team blueTeam => teams?[0];
        public Team redTeam => teams?.Count > 1 ? teams[1] : null;
        public List<Participant> participants { get; set; }

        // 검색한 사용자의 puuid를 저장
        private string _searchedUserPuuid = null;

        public int idx { get; set; } = -1;

        // MyPart는 participants에서 검색한 사용자를 직접 찾도록 수정
        public Participant MyPart
        {
            get => participants[idx];
        }

        public List<Participant> BluePart => participants.GetRange(0, 5);

        public List<Participant> RedPart => participants.GetRange(5, 5);

        public void SetMyPart(string puuid)
        {
            if (participants == null || string.IsNullOrEmpty(puuid)) return;

            // 검색한 사용자의 puuid 저장
            _searchedUserPuuid = puuid;

            for (int i = 0; i < participants.Count && i < 10; i++)
            {
                // 검색한 사용자를 찾으면 idx에 해당 인덱스를 저장
                if (participants[i].puuid == puuid)
                {
                    idx = i;
                }

                // 팀별 킬 관여율 계산
                if (i < 5 && blueTeam != null)
                {
                    decimal teamkill = decimal.Parse(blueTeam.champion ?? "0");
                    decimal kills = decimal.Parse(participants[i].kills ?? "0");
                    decimal assist = decimal.Parse(participants[i].assists ?? "0");
                    decimal percent = (teamkill <= 0 || kills <= 0) ? 0 : (kills + assist) / teamkill;
                    participants[i].killPercent = $"{percent:P0}";
                }
                else if (i >= 5 && redTeam != null)
                {
                    decimal teamkill = decimal.Parse(redTeam.champion ?? "0");
                    decimal kills = decimal.Parse(participants[i].kills ?? "0");
                    decimal assist = decimal.Parse(participants[i].assists ?? "0");
                    decimal percent = (teamkill <= 0 || kills <= 0) ? 0 : (kills + assist) / teamkill;
                    participants[i].killPercent = $"{percent:P0}";
                }
            }
        }

        public void SortPIdx()
        {
            if (participants == null) return;

            //정렬
            participants.Sort((x, y) => x.P_idx.CompareTo(y.P_idx));

            // 정렬 후 idx를 다시 찾기 (puuid 기준)
            if (!string.IsNullOrEmpty(_searchedUserPuuid))
            {
                for (int i = 0; i < participants.Count; i++)
                {
                    if (participants[i].puuid == _searchedUserPuuid)
                    {
                        idx = i;
                        break;
                    }
                }
            }
        }
    }

    public class Team
    {
        public Dictionary<string, Dictionary<string, string>> feats
        {
            set
            {
                if (value?.ContainsKey("FIRST_BLOOD") == true &&
                    value["FIRST_BLOOD"]?.ContainsKey("featState") == true &&
                    value["FIRST_BLOOD"]["featState"] == "3")
                {
                    FirstBlood = true;
                    value.Clear();
                }
            }
        }

        public bool FirstBlood { get; set; } = false;

        public Dictionary<string, Dictionary<string, string>> objectives
        {
            set => SetObjectives(value);
        }

        private void SetObjectives(Dictionary<string, Dictionary<string, string>> objs)
        {
            if (objs == null) return;
            foreach (var obj in objs)
            {
                if (!obj.Value.ContainsKey("kills")) continue;
                switch (obj.Key)
                {
                    case "atakhan": atakhan = obj.Value["kills"]; break;
                    case "baron": baron = obj.Value["kills"]; break;
                    case "champion": champion = obj.Value["kills"]; break;
                    case "dragon": dragon = obj.Value["kills"]; break;
                    case "horde": horde = obj.Value["kills"]; break;
                    case "riftHerald": riftHerald = obj.Value["kills"]; break;
                    case "inhibitor": inhibitor = obj.Value["kills"]; break;
                    case "tower": tower = obj.Value["kills"]; break;
                }
            }
        }

        public string atakhan { get; set; }
        public string baron { get; set; }
        public string champion { get; set; }
        public string dragon { get; set; }
        public string horde { get; set; }
        public string riftHerald { get; set; }
        public string inhibitor { get; set; }
        public string tower { get; set; }
    }

    public class Participant
    {
        [JsonProperty("participantId")]
        public int P_idx { get; set; }

        private string teamName;
        public string teamid
        {
            get => teamName;
            set
            {
                if (value == null) return;
                else if (value != "100" && value != "200")
                    teamName = value;
                else
                    teamName = value == "100" ? "블루팀" : "레드팀";
            }
        }

        public bool win { get; set; }
        public string championName { get; set; }
        public byte[] championImg { get; set; }

        public string GameName => $"{riotIdGameName}#{riotIdTagline}";
        public string riotIdGameName { get; set; }
        public string riotIdTagline { get; set; }

        public string puuid { get; set; }

        public string summoner1Id { get; set; }
        public string summoner2Id { get; set; }
        public byte[] summoner1Id_Img { get; set; }
        public byte[] summoner2Id_Img { get; set; }

        public string champLevel { get; set; }

        public string item0 { get; set; }
        public string item1 { get; set; }
        public string item2 { get; set; }
        public string item3 { get; set; }
        public string item4 { get; set; }
        public string item5 { get; set; }
        public string item6 { get; set; }
        public byte[] item0_Img { get; set; }
        public byte[] item1_Img { get; set; }
        public byte[] item2_Img { get; set; }
        public byte[] item3_Img { get; set; }
        public byte[] item4_Img { get; set; }
        public byte[] item5_Img { get; set; }
        public byte[] item6_Img { get; set; }

        private string _totalMinionsKilled { get; set; }
        public string totalMinionsKilled
        {
            get => "CS:" + _totalMinionsKilled;
            set
            {
                if (value.IndexOf("CS") == 0)
                    _totalMinionsKilled = value.Split(':')[1];
                else
                    _totalMinionsKilled = value;
            }
        }

        private string _goldEarned { get; set; }
        public string goldEarned
        {
            get => "골드 : " + _goldEarned;
            set
            {
                if (int.TryParse(value, out int temp))
                    _goldEarned = temp.ToString("N0");
                else if (value != null)
                {
                    if (value.IndexOf("골드") == 0)
                        _goldEarned = value.Split(':')[1];
                    else
                        _goldEarned = value;
                }
            }
        }

        public int _totalDamageDealtToChampions { get; set; }

        public string totalDamageDealtToChampions
        {
            get => _totalDamageDealtToChampions.ToString();
            set => _totalDamageDealtToChampions = int.TryParse(value, out int temp) ? temp : 0;
        }

        public int _totalDamageTaken { get; set; }
        public string totalDamageTaken
        {
            get => _totalDamageTaken.ToString();
            set => _totalDamageTaken = int.TryParse(value, out int temp) ? temp : 0;
        }

        private Dictionary<string, object> temp { get; set; }
        public Dictionary<string, object> challenges
        {
            get => temp;
            set
            {
                if (value?.ContainsKey("kda") == true &&
                    double.TryParse(value["kda"].ToString(), out double tempVal))
                {
                    kda = tempVal.ToString("F2");
                    value.Clear();
                }
            }
        }

        public string kda { get; set; }
        public string K_D_A => $"{kills} / {deaths} / {assists}";
        public string kills { get; set; }
        public string deaths { get; set; }
        public string assists { get; set; }
        public string killPercent { get; set; }

        public string primaryRune { get; set; }
        public byte[] primaryRune_Img { get; set; }
        public string subRune { get; set; }
        public byte[] subRune_Img { get; set; }
    }
}