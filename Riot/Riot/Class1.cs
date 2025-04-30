using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Riot
{
    public class Match
    {
        public Dictionary<string, string> Info { get; set; }
        public List<Dictionary<string, object>> Participants { get; set; }

        public Match(Dictionary<string, string> temp1, List<Dictionary<string, object>> temp2)
        {
            Info = temp1;
            Participants = temp2;
        }

    }

    public class OpenApi
    {
        /// <summary>
        /// 정보들
        /// </summary>
        private Dictionary<string, object> Info { get; set; } = null;
        // 10명의 참가자
        private List<Dictionary<string, object>> Participants { get; set; } = null;
        // 매치정보
        private Dictionary<string, string> MatchInfo { get; set; } = null;

        public Dictionary<string, Match> Matches = new Dictionary<string, Match>();

        private string API_KEY = string.Empty;
        public string My_Puuid = string.Empty;

        public void AddKey(string key)
        {
            API_KEY = key;
        }

        #region JSON PARSING

        private string SearchPuuid(string gameName, string tagLine)
        {
            try
            {
                string url = $"https://asia.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{gameName}/{tagLine}";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.Headers.Add("Accept-Language", "ko-KR,ko;q=0.9,en-US;q=0.8,en;q=0.7");
                request.Headers.Add("Accept-Charset", "application/x-www-form-urlencoded; charset=UTF-8");
                request.Headers.Add("X-Riot-Token", API_KEY);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"{response.StatusCode}");

                // 응답 본문 읽기
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        string responseText = reader.ReadToEnd();

                        var temp = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);
                        return temp["puuid"];
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Search Error : " + ex.Message);
            }
        }
        private List<string> SearchMatch(string puuid, string type, int count)
        {
            try
            {
                string url = $"https://asia.api.riotgames.com/lol/match/v5/matches/by-puuid/{puuid}/ids";

                url += $"?type={type}"; // normal , ranked
                url += "&start=0";
                url += $"&count={count}";
                url += $"&api_key={API_KEY}";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.Headers.Add("Accept-Language", "ko-KR,ko;q=0.9,en-US;q=0.8,en;q=0.7");
                request.Headers.Add("Accept-Charset", "application/x-www-form-urlencoded; charset=UTF-8");
                request.Headers.Add("Origin", "https://developer.riotgames.com");

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"{response.StatusCode}");

                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        string responseText = reader.ReadToEnd();

                        return JsonConvert.DeserializeObject<List<string>>(responseText);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($" SEARCH MATCH v5 : {ex.Message}");
            }

        }
        private Dictionary<string, Dictionary<string, object>> MatchData(string matchid)
        {
            try
            {
                string url = $"https://asia.api.riotgames.com/lol/match/v5/matches/{matchid}";

                url += $"?api_key={API_KEY}";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.Headers.Add("Accept-Language", "ko-KR,ko;q=0.9,en-US;q=0.8,en;q=0.7");
                request.Headers.Add("Accept-Charset", "application/x-www-form-urlencoded; charset=UTF-8");
                request.Headers.Add("Origin", "https://developer.riotgames.com");

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"{response.StatusCode}");

                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        string responseText = reader.ReadToEnd();

                        return JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(responseText);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($" MATCH v5 ERROR : {ex.Message}");
            }
        }
        private void Make_Match(Dictionary<string, object> info)
        {
            Participants = new List<Dictionary<string, object>>();
            Info = new Dictionary<string, object>();
            MatchInfo = new Dictionary<string, string>();

            #region 매치정보

            MatchInfo.Add("gameMode", info["gameMode"].ToString());
            MatchInfo.Add("gameEndTimestamp", info["gameEndTimestamp"].ToString());

            var teams = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(info["teams"].ToString());

            bool once = true;

            foreach (var team in teams)
            {
                // true = 블루팀 , false = 레드팀
                bool flag = false;

                #region 승리팀
                if (team["teamId"].ToString() == "100" && once)
                {
                    if (bool.Parse(team["win"].ToString()))
                        MatchInfo.Add("Wins", "블루팀");
                    else
                        MatchInfo.Add("Wins", "레드팀");

                    flag = true;
                }
                #endregion

                #region 퍼블 여부
                var feat = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(team["feats"].ToString());
                if (feat["FIRST_BLOOD"]["featState"].ToString() == "3" && once)
                {
                    if (flag)
                    {
                        MatchInfo.Add("FirstBlood", "블루팀");
                    }
                    else
                    {
                        MatchInfo.Add("FirstBlood", "레드팀");
                    }

                    once = false;
                }
                #endregion

                #region 오브젝트 챙긴 개수
                var objectives = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(team["objectives"].ToString());

                foreach (var objective in objectives)
                {
                    if (flag)
                    {
                        MatchInfo.Add("B_" + objective.Key, objective.Value["kills"].ToString());
                    }
                    else
                    {
                        MatchInfo.Add("R_" + objective.Key, objective.Value["kills"].ToString());
                    }
                }
                #endregion
            }
            #endregion

            #region 유저 10명의 정보

            var participants = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(info["participants"].ToString());

            foreach (var participant in participants)
            {
                if (participant["teamId"].ToString() == "100")
                    Info.Add("teamId", "블루팀");
                else
                    Info.Add("teamId", "레드팀");

                // 챔피언
                Info.Add("championName", participant["championName"]);
                // 라이엇 닉넴 , puuid
                Info.Add("riotIdGameName", $"{participant["riotIdGameName"]}#{participant["riotIdTagline"]}");
                Info.Add("puuid", participant["puuid"]);
                // 스펠
                Info.Add("summoner1Id", participant["summoner1Id"]);
                Info.Add("summoner2Id", participant["summoner2Id"]);

                // 룬 (나중에)
                // 걍안할래 귀찮

                // 레벨
                Info.Add("champLevel", participant["champLevel"]);
                // KDA
                var challenges = JsonConvert.DeserializeObject<Dictionary<string, object>>(participant["challenges"].ToString());
                Info.Add("kda", challenges["kda"]);
                Info.Add("k/d/a", $"{participant["kills"]}/{participant["deaths"]}/{participant["assists"]}");
                // 아이템
                for (int i = 0; i < 7; i++)
                {
                    Info.Add($"item{i}", participant[$"item{i}"]);
                }
                // CS
                Info.Add("totalMinionsKilled", participant["totalMinionsKilled"]);
                // 골드
                Info.Add("goldEarned", participant["goldEarned"]);
                // 딜량 , 받은 피해량
                Info.Add("totalDamageDealtToChampions", participant["totalDamageDealtToChampions"]);
                Info.Add("totalDamageTaken", participant["totalDamageTaken"]);

                Participants.Add(Info);
                Info = new Dictionary<string, object>();
            }

            #endregion
        }

        #endregion

        /// <summary>
        /// OpenApi 에서 데이터를 검색
        /// </summary>
        /// <param name="gameName"> 닉네임#태그 </param>
        /// <param name="type"> 노말(normal) , 랭크(ranked) </param>
        /// <param name="count"> 전적 검색할 개수 </param>
        /// <returns> JSON형식으로 저장된 정보들 </returns>
        ///
        public Dictionary<string, Match> Search(string gameName, string type, int count)
        {
            try
            {
                string[] sp = gameName.Split('#');

                string puuid = SearchPuuid(sp[0], sp[1]);

                // matches 는 matchId 가 있음
                // normal , ranked
                var matches = SearchMatch(puuid, type, count);

                foreach (var match in matches)
                {
                    var info = MatchData(match)["info"];
                    Make_Match(info);

                    var temp1 = new Match(MatchInfo, Participants);
                    Matches.Add(match, new Match(MatchInfo, Participants));
                }

                return Matches;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// JSON 으로 파싱해줌 혹시 모를 디버깅 용
        /// </summary>
        /// <returns></returns>
        public string DebugJson()
        {
            return JsonConvert.SerializeObject(Matches, Newtonsoft.Json.Formatting.Indented);
        }
    }
}
