using Calendar.NetWork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Calendar_Server
{
    internal class App
    {
        #region 싱글톤
        public static App Singleton { get; private set; } = null;
        static App() { Singleton = new App(); }
        private App() { }
        #endregion

        private const int SERVER_PORT = 7000;
        static MainControl con = null;

        #region 1. CallBack Message
        public void LogMessage(Socket socket, string message)
        {
            Console.WriteLine($"[log] {message}");
        }
        public void PacketMessage(Socket socket, string message)
        {
            IPEndPoint ip = (IPEndPoint)socket.RemoteEndPoint;

            string[] sp = message.Split('@');
            // 해당 ip 가 요청한 flag
            Console.WriteLine($"{ip.Address} , {ip.Port}: FLAG : {sp[0]}");

            #region 1. 데이터 파싱
            switch (int.Parse(sp[0]))
            {
                case Packet.PACKET_INSERTUSER:  con.InsertUser(socket, sp[1]);  break;
                case Packet.PACKET_LOGINUSER:   con.LoginUser(socket, sp[1]);   break;
                case Packet.PACKET_ADDSCHEDULE: con.AddSchedule(socket, sp[1]); break;
                case Packet.PACKET_DELSCHEDULE: con.DelSchedule(socket, sp[1]); break;
                case Packet.PACKET_DELETEUSER:  con.DeleteUser(socket, sp[1]);  break;
                default: Console.WriteLine("없는 flag 입니다"); break;
            }
            #endregion
        }
        #endregion
        #region Init , Exit
        public void Init()
        {
            con = MainControl.Singleton;

            MyServer.Singleton.Start(SERVER_PORT, LogMessage, PacketMessage);
            con.Init();
        }
        public void Exit()
        {
            con.Exit();
        }
        #endregion
    }
}
