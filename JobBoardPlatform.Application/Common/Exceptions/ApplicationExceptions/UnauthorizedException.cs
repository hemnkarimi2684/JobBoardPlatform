using JobBoardPlatform.Application.Common.Exceptions.BaseAppExceptionModel;
using JobBoardPlatform.Core.Common.Exceptions.ErrorModel;

namespace JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message, Exception? innerException = null) : base(message, "UNAUTHORIZED", innerException)
    {
    }

    public UnauthorizedException(Error error, Exception? innerException = null) : base(error, innerException)
    {
    }
}
