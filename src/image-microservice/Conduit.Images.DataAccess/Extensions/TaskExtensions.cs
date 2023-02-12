namespace Conduit.Images.DataAccess.Extensions;

public static class TaskExtensions
{
    public static async Task SingleResult(this Task<int> task, Exception? exception = null, int expected = 1)
    {
        var result = await task;
        if (result != expected)
        {
            throw exception ?? new SingleResultException(result, expected);
        }
    }

    public class SingleResultException : ApplicationException
    {
        public SingleResultException(int result, int expected) : base($"Expected result is {expected}; Received result is {result}")
        {
        }
    }
}
