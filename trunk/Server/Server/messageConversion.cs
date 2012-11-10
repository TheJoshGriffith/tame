using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tame
{
    class messageConversion
    {
        public static bool convertMessage(string msg)
        {
            string adlv = "AddLevel";
            if (msg.Equals(adlv))
            {

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
