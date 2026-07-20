using JobBoardPlatform.Core.Common.Exceptions.ErrorModel;

namespace JobBoardPlatform.WebApi.ResultPattern;

public class Result<TData> : Result
{
    public Result(TData? data, bool isSuccess) : base(isSuccess)
    {
        Data = data;
    }

    public TData? Data { get; private set; }

    public static Result<TData> Success(TData? data) =>
        new Result<TData>(data, true);
}

public class Result
{
    public Result(bool isSuccess, Error? error = null)
    {
        if (isSuccess && error != null)
            throw new InvalidOperationException("A successful result cannot have an error.");

        if (!isSuccess && error == null)
            throw new InvalidOperationException("A failed result must have an error.");

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; private set; }

    public bool IsFailure => !IsSuccess;

    public Error? Error { get; private set; }

    public static Result Success() =>
        new Result(true);

    public static Result Failure(Error error) =>
        new Result(false, error);
}
