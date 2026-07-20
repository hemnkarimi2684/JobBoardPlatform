using JobBoardPlatform.Application.Common.Exceptions.BaseAppExceptionModel;
using JobBoardPlatform.Core.Common.Exceptions.ErrorModel;

namespace JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;

public class NotFoundException : AppException
{
    public NotFoundException(string message, Exception? innerException = null) : base(message, "NOTFOUND", innerException)
    {
    }

    public NotFoundException(Error error, Exception? innerException = null) : base(error, innerException)
    {
    }
}