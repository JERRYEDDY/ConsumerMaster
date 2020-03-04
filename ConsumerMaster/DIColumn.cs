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
        public Type type;
        public int method;  //1 - Expected, 2 - Equal To 0, 3 - Not Equal To 1

        public DIColumn(string n, string e, bool c, Type t, int m)
        {
            name = n;
            expected = e;
            isCentered = c;
            type = t;
            method = m;
        }
    }
}