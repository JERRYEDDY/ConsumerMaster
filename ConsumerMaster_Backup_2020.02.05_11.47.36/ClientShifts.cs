using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsumerMaster
{
    public class ClientShifts
    {
        public string ID;
        public string Name;
        public string StaffID;
        public string StaffName;
        public DateTime Start;
        public DateTime Finish;
        public int Duration;

        public ClientShifts(string id, string name, string staffID, string staffName, DateTime start, DateTime finish, int duration)
        {
            ID = id;
            Name = name;
            StaffID = staffID;
            StaffName = staffName;
            Start = start;
            Finish = finish;
            Duration = duration;
        }
    }
}