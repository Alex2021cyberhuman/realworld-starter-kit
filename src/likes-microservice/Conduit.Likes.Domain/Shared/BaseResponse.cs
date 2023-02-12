namespace Conduit.Likes.Domain.Shared;

public abstract class BaseResponse
{
    protected BaseResponse(
        Error error)
    {
        Error = error;
    }

    public bool IsSuccess => Error == Error.None;

    public Error Error { get; set; }
}
