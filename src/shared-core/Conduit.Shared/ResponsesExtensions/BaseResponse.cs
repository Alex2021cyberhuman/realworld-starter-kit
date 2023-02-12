namespace Conduit.Shared.ResponsesExtensions;

public abstract class BaseResponse
{
    public BaseResponse()
    {        
    }

    public BaseResponse(Error error, string? errorDescription = null)
    {
        Error = error;
        if (!IsSuccess && errorDescription is null)
        {
            ErrorDescription = Error.ToString();
        }
        else
        {
            ErrorDescription = errorDescription;
        }
    }

    public Error Error { get; }

    public string? ErrorDescription { get; }

    public bool IsSuccess => Error.None == Error;
}
