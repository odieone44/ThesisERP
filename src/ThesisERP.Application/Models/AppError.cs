using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
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

    public static AppError GetAppErrorFromAppException(ThesisERPException exception)
    {
        var appError = new AppError();

        switch (exception)
        {
            case ThesisERPUniqueConstraintException ux:
                appError.StatusCode = StatusCodes.Status409Conflict;
                appError.Message = ux.Message;
                break;
            case ThesisERPException ex:
                appError.StatusCode = StatusCodes.Status400BadRequest;
                appError.Message = ex.Message;
                break;            
        }
        return appError;
    }
}
