using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsumerMaster
{
    public class DIColumn
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public string name;
        public string expected;
        public bool isCentered;

        public DIColumn(string n, string e, bool c)
        {
            name = n;
            expected = e;
            isCentered = c;
        }
    }
}