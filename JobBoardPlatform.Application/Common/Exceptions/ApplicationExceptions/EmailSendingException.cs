using JobBoardPlatform.Application.Common.Exceptions.BaseAppExceptionModel;
using JobBoardPlatform.Core.Common.Exceptions.ErrorModel;

namespace JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;

public class EmailSendingException : AppException
{
    public EmailSendingException(string message, Exception? innerException = null) : base(message, "EMAILSENDINGEXCEPTION", innerException)
    {
    }

    public EmailSendingException(Error error, Exception? innerException = null) : base(error, innerException)
    {
    }
}
