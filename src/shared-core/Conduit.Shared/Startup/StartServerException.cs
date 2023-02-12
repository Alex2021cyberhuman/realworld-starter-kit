namespace Conduit.Shared.Startup;

public class StartServerException : ApplicationException
{
    public StartServerException(
        Exception inner) : base("Cannot start server", inner)
    {
    }
}
