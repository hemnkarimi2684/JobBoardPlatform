using JobBoardPlatform.Core.Common.Exceptions.BaseExceptionModel;
using JobBoardPlatform.Core.Common.Exceptions.ErrorModel;

namespace JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;

/// <summary>
/// اکسپشن های در لایه دامین
/// </summary>
public class DomainException : BaseException
{
    public DomainException(string message, string code, Exception? innerException = null) : base(message, $"DomainException_{code}", innerException)
    {
    }

    public DomainException(Error error, Exception? innerException = null) : base(error.Message, $"DomainException_{error.Code}", innerException)
    {

    }
}
