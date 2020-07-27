using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsumerMaster
{
    public class AWCServices
    {
        string[,] codeNameArray = new string[12, 2]
        {
            { "W1726", "Companion W/B"},
            { "W1726:U4", "Companion W/O"},
            { "W7061", "H&C 1:1 Degreed Staff"},
            { "W7060", "H&C 1:1 W/B"},
            { "W7060:U4", "H&C 1:1 W/O"},
            { "W7069", "H&C 2:1 Enhanced W/B"},
            { "W7068", "H&C 2:1 W/B"},
            { "W9863", "Respite 1:1 Enhanced 15 min W/B"},
            { "W9862", "Respite 15 min W/B"},
            { "W9862:U4", "Respite 15 min W/O"},
            { "W9798", "Respite 24HR W/B"},
            { "W9798:U4", "Respite 24 HR W/O"}
        };

        public void BuildAggregatorArray()
        {
            for (int i = 0; i < codeNameArray.GetLength(0); i++)
            {
                for (int j = 0; j < codeNameArray.GetLength(1); j++)
                {

                    string formatted = String.Format("ODP / {0} / [1}", codeNameArray[i, 0], codeNameArray[i, 1]);

                    string s = codeNameArray[i, j];
                    Console.WriteLine(s);
                }
            }
        }
    }
}