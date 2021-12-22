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
    public string UniqueConstraintName { get; set; }
    public string UniqueConstraintValue { get; set; }
    public ThesisERPUniqueConstraintException(string constraintName, string constraintValue) : base($"Error. '{constraintName}' must be unique. An entry with {constraintName}: '{constraintValue}' already exists. Action failed.")
    {
        UniqueConstraintName = constraintName;
        UniqueConstraintValue = constraintValue;
    }
}
