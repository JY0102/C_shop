using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Calendar.Data;
using Calendar.File;
using Calendar.Json;
using Calendar.NetWork;

namespace Calendar_Server
{
    internal class MainControl
    {
        #region 싱글톤
        public static MainControl Singleton { get; private set; } = null;
        static MainControl() { Singleton = new MainControl(); }
        private MainControl() { }
        #endregion

        List<User> users = null;
        MyServer server = null;
        public Dictionary<string, Schedule> schedules { get; set; } = null;

        #region 기능 메서드
        public void InsertUser(Socket socket, string msg)
        {
            try
            {
                string[] sp = msg.Split('#');
                string id = sp[0];
                string pw = sp[1];


                // id pw 중복처리
                User temp = users.Find(acc => acc.Id == id);
                if (temp != null)
                    throw new Exception("이미 회원가입 된 아이디 입니다");

                //회원 정보 저장
                users.Add(new User(id, pw));
                // 회원 전용 일정 저장
                schedules.Add(id, new Schedule());

                string packet = Packet.InsertUserAck(true, null);
                server.SendData(socket, packet);
            }
            catch (Exception ex)
            {
                string packet = Packet.InsertUserAck(false, ex.Message);
                server.SendData(socket, packet);
            }
        }        // 회원가입
        public void LoginUser(Socket socket, string msg)
        {
            try
            {
                string[] sp = msg.Split('#');
                string id = sp[0];
                string pw = sp[1];

                // id pw 체킹
                User temp = users.Find(acc => (acc.Id == id) && (acc.Pw == pw));
                if (temp == null)
                    throw new Exception("로그인 정보가 없습니다.");

                string packet = Packet.LoginUserAck(true, null , id);
                server.SendData(socket, packet);
            }
            catch (Exception ex)
            {
                string packet = Packet.LoginUserAck(false, ex.Message , null);
                server.SendData(socket, packet);
            }
        }         // 로그인
        public void AddSchedule(Socket socket, string msg)
        {
            try
            {
                #region 패킷파싱
                string[] sp = msg.Split('#');
                string id = sp[0];
                string date = sp[1];
                string info = sp[2];
                #endregion

                // 스케쥴 추가
                // [추가] UpdateSchedule
                schedules[id].Add(date, info);
                // 업데이트 된 스케쥴 전송
                UpdateSchedule(socket, id);

                string packet = Packet.AddScheduleAck(true, null);
                server.SendData(socket, packet);
            }
            catch (Exception ex)
            {
                string packet = Packet.AddScheduleAck(false, ex.Message);
                server.SendData(socket, packet);
            }
        }       // 일정 추가
        public void DelSchedule(Socket socket, string msg)
        {
            try
            {
                #region  패킷 파싱
                string[] sp = msg.Split('#');
                string id = sp[0];
                string date = sp[1];
                string info = sp[2];
                #endregion

                // 스케쥴 제거
                schedules[id].Remvoe(date , info);
                UpdateSchedule(socket, id);

                string packet = Packet.DelScheduleAck(true, null);
                server.SendData(socket, packet);
            }
            catch (Exception ex)
            {
                string packet = Packet.DelScheduleAck(false, ex.Message);
                server.SendData(socket, packet);
            }
        }       // 일정 삭제
        public void DeleteUser(Socket socket, string msg)
        {
            try
            {
                #region 패킷 파싱
                string[] sp = msg.Split('#');
                string id = sp[0];
                string pw = sp[1];
                #endregion

                // id pw 체킹
                User temp = users.Find(acc => (acc.Id == id) && (acc.Pw == pw));
                if (temp == null)
                    throw new Exception("ID PW가 틀렸습니다.");

                schedules.Remove(id);

                string packet = Packet.DeleteUserAck(true, null);
                server.SendData(socket, packet);
            }
            catch (Exception ex)
            {
                string packet = Packet.DeleteUserAck(false, ex.Message);
                server.SendData(socket, packet);
            }
        }        // 회원 탈퇴
        #endregion

        #region  내부 메서드
        private void UpdateSchedule(Socket socket, string id)
        {
            try
            {
                var json = WbJson.UpdateJson(id , schedules);
                if (json == null)
                    throw new Exception("나도모름");

                string packet = Packet.UpdateScheduleAck(true, null , json);
                server.SendData(socket, packet);
            }
            catch (Exception ex)
            {
                string packet = Packet.UpdateScheduleAck(false, ex.Message , null);
                server.SendData(socket, packet);
            }

        }   // 일정 업데이트
        #endregion

        #region Init  , Exit
        public void Init()
        {
            server = MyServer.Singleton;
            schedules = WbJson.ReadJson();
            users = WbFile.ReadFile();
        }
        public void Exit()
        {
            WbJson.WriteJson(schedules);
            WbFile.WirteFile(users);
            server.Dispose();
        }
        #endregion
    }
}

