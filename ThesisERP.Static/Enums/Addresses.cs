using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisERP.Static.Enums
{
    public static class Addresses
    {
        public enum AddressTypes
        {
            billiing = 0,
            shipping = 1,
            general = 2
        }

        public enum EntityTypes 
        {
            client = 0,
            supplier = 1,
            document = 2,
            order = 3,
            warehouse = 4
        }

        public enum CountryCodes
        {

        }
    }
}
