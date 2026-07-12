using JobBoardPlatform.Application.Common.Exceptions.BaseAppExceptionModel;
using JobBoardPlatform.Core.Common.Exceptions.ErrorModel;

namespace JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;

public class ValidationException : AppException
{
    public ValidationException(string message, Exception? innerException = null) : base(message, "BADREQUEST", innerException)
    {
    }

    public ValidationException(Error error, Exception? innerException = null) : base(error, innerException)
    {
    }
}
