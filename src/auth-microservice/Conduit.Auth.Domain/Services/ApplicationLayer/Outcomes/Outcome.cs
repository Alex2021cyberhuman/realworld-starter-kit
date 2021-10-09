namespace Conduit.Auth.Domain.Services.ApplicationLayer.Outcomes
{
    public static class Outcome
    {
        public static Outcome<T> New<T>(
            OutcomeType type = OutcomeType.Successful,
            T? result = default) =>
            new(result, type);
    }

    public class Outcome<T>
    {
        public Outcome(T? result, OutcomeType type)
        {
            Result = result;
            Type = type;
        }

        public T? Result { get; }

        public OutcomeType Type { get; }
    }
}