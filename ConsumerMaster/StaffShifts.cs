using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsumerMaster
{
    public class StaffShifts
    {
        public string StaffID;
        public string StaffName;
        public string ClientID;
        public string ClientName;
        public DateTime Start;
        public DateTime Finish;
        public int Duration;

        public StaffShifts(string staffID, string staffName, string clientID, string clientName, DateTime start, DateTime finish, int duration)
        {
            StaffID = staffID;
            StaffName = staffName;
            ClientID = clientID;
            ClientName = clientName;
            Start = start;
            Finish = finish;
            Duration = duration;
        }
    }
}