using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsumerMaster
{
    public class SPColumn
    {
        public string name;
        public Type type;

        public SPColumn(string s, Type t)
        {
            name = s;
            type = t;
        }
    }
}