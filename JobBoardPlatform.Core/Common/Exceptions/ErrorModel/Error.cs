namespace JobBoardPlatform.Core.Common.Exceptions.ErrorModel;

/// <summary>
/// کلاس پایه ارور
/// </summary>
public class Error
{
    public Error(string message, string code)
    {
        Message = message;
        Code = code;
    }

    public string Message { get; private set; } 

    public string Code { get; private set; } 
}
