namespace Conduit.Auth.Domain.Services.ApplicationLayer.Outcomes
{
    public static class Outcome
    {
        public static Outcome<T> New<T>(
            OutcomeType type = OutcomeType.Successful,
            T? result = default)
        {
            return new(result, type);
        }

        public static Outcome<T> New<T>(T? result = default)
        {
            return new(result, OutcomeType.Successful);
        }
    }

    public class Outcome<T>
    {
        internal Outcome(T? result, OutcomeType type)
        {
            Result = result;
            Type = type;
        }

        public T? Result { get; }

        public OutcomeType Type { get; }
    }
}
