using JobBoardPlatform.Core.Common.Exceptions.BaseExceptionModel;
using JobBoardPlatform.Core.Common.Exceptions.ErrorModel;

namespace JobBoardPlatform.Application.Common.Exceptions.BaseAppExceptionModel;

public abstract class AppException : BaseException
{
    public AppException(string message, string statusCode, Exception innerException) : base(message, $"BusinessException_{statusCode}", innerException)
    {
    }

    public AppException(Error error, Exception? innerException = null) : base(error.Message, error.Code, innerException)
    {
    }
}
