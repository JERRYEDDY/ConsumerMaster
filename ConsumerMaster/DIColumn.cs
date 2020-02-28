using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsumerMaster
{
    public class DIColumn
    {
        public string name;
        public string value;
        public string expected;

        public DIColumn(string n, string v, string e)
        {
            name = n;
            value = v;
            expected = e;
        }
    }
}