using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisERP.Static.Extensions
{
    public static class Numbers
    {
        public static decimal RoundTo2(this decimal d)
        {
            return decimal.Round(d, 2, MidpointRounding.AwayFromZero);
        }
    }
}
