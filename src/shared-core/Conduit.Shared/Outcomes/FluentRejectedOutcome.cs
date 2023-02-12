using FluentValidation.Results;

namespace Conduit.Shared.Outcomes;

public class FluentRejectedOutcome<T> : Outcome<T>
{
    public FluentRejectedOutcome(
        ValidationResult validationResult) : base(default, OutcomeType.Rejected)
    {
        ValidationResult = validationResult;
    }

    public ValidationResult ValidationResult { get; }
}
