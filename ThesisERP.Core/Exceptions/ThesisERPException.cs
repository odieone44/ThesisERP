using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisERP.Core.Exceptions;

public class ThesisERPException : Exception
{
    public ThesisERPException(string errorMessage) : base(errorMessage)
    {
    }
}
