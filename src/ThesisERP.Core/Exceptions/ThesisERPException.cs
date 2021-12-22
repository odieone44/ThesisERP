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

public class ThesisERPUniqueConstraintException : ThesisERPException
{
    public ThesisERPUniqueConstraintException() : base($"Action resulted in duplicate value for a property that should be unique. Check related api schema for information and try again.")
    {
    }
}
