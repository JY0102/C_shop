
namespace Calendar.NetWork
{
    internal class Packet
    {
        #region FLAG
        public const int PACKET_INSERTUSER         = 1;
        public const int PACKET_INSERTUSER_ACK     = 11;

        public const int PACKET_LOGINUSER          = 2;
        public const int PACKET_LOGINUSER_ACK      = 21;

        public const int PACKET_ADDSCHEDULE        = 3;
        public const int PACKET_ADDSCHEDULE_ACK    = 31;

        public const int PACKET_DELSCHEDULE        = 4;
        public const int PACKET_DELSCHEDULE_ACK    = 41;

        public const int PACKET_UPDATESCHEDULE     = 5;
        public const int PACKET_UPDATESCHEDULE_ACK = 51;

        public const int PACKET_DELETEUSER         = 6;
        public const int PACKET_DELETEUSER_ACK     = 61;
        #endregion

        #region Server -> Client
        public static string InsertUserAck(bool flag, string info)
        {
            string packet = PACKET_INSERTUSER_ACK + "@";

            packet += flag + "#";
            packet += info + "";

            return packet;
        }
        public static string LoginUserAck(bool flag, string info , string id)
        {
            string packet = PACKET_LOGINUSER_ACK + "@";

            packet += flag + "#";
            packet += info + "#";
            packet += id;

            return packet;
        }
        public static string AddScheduleAck(bool flag , string info)
        {
            string packet = PACKET_ADDSCHEDULE_ACK + "@";

            packet += flag + "#";
            packet += info;

            return packet;
        }
        public static string DelScheduleAck(bool flag, string info)
        {
            string packet = PACKET_DELSCHEDULE_ACK + "@";

            packet += flag + "#";
            packet += info;

            return packet;
        }
        public static string UpdateScheduleAck(bool flag, string info, string json)
        {
            string packet = PACKET_UPDATESCHEDULE_ACK + "@";

            packet += flag + "#";
            packet += info + "#";
            packet += json;

            return packet;
        }
        public static string DeleteUserAck(bool flag, string info)
        {
            string packet = PACKET_DELETEUSER_ACK + "@";

            packet += flag + "#";
            packet += info;

            return packet;
        }
        #endregion
    }
}
