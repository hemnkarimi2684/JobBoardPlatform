using JobBoardPlatform.Core.Common.Exceptions.ErrorModel;

namespace JobBoardPlatform.Core.Common.Exceptions.BaseExceptionModel;

/// <summary>
/// مدل پایه کلاس اکسپشن
/// </summary>
public abstract class BaseException : Exception
{
    public BaseException(string message, string code, Exception? innerException = null) : base(message, innerException)
    {
        Code = code;
    }

    public string Code { get; private set; }
}
