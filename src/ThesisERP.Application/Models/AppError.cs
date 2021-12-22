using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using ThesisERP.Core.Exceptions;

namespace ThesisERP.Application.Models;

public class AppError
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static AppError GetAppErrorFromAppException(Exception exception)
    {
        var appError = new AppError()
        {
            StatusCode = StatusCodes.Status500InternalServerError,
            Message = "Internal Server Error. Please try again."
        };

        if (exception is not ThesisERPException) 
        {            
            return appError; 
        }

        switch (exception)
        {
            case ThesisERPUniqueConstraintException:
                appError.StatusCode = StatusCodes.Status409Conflict;                
                break;

            case ThesisERPException:
                appError.StatusCode = StatusCodes.Status400BadRequest;                
                break;            
        }
        appError.Message = exception.Message;

        return appError;
    }
}
