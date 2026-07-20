using JobBoardPlatform.Application.Common.Exceptions.BaseAppExceptionModel;
using JobBoardPlatform.Core.Common.Exceptions.ErrorModel;

namespace JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;

public class ForbiddenException : AppException
{
    public ForbiddenException(string message, Exception? innerException = null) : base(message, "FORIBIDDEN", innerException)
    {
    }

    public ForbiddenException(Error error, Exception? innerException = null) : base(error, innerException)
    {
    }
}
