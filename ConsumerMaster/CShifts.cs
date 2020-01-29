using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsumerMaster
{
    public class CShifts
    {
        public DateTime Start;
        public DateTime Finish;

        public CShifts(DateTime start, DateTime finish)
        {
            Start = start;
            Finish = finish;
        }
    }
}