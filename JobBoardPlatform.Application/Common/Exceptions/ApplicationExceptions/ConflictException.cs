using JobBoardPlatform.Application.Common.Exceptions.BaseAppExceptionModel;
using JobBoardPlatform.Core.Common.Exceptions.ErrorModel;

namespace JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;

public class ConflictException : AppException
{
    public ConflictException(string message, Exception? innerException = null) : base(message, "CONFLICT", innerException)
    {
    }

    public ConflictException(Error error, Exception? innerException = null) : base(error, innerException)
    {
    }
}
