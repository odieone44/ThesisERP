using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisERP.Core.Extensions
{
    public static class Numbers
    {
        public static decimal RoundTo(this decimal d, int digits)
        {
            return decimal.Round(d, digits, MidpointRounding.AwayFromZero);
        }
    }
}
