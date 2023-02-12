using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Conduit.Shared.Validation;

public class Validation : IEnumerable<ValidationResult>
{
    public Validation(string errorDescription) : this(new List<ValidationResult> { new(errorDescription) })
    {
    }

    public Validation(
        List<ValidationResult>? results = null)
    {
        Results = results ?? new();
    }

    public List<ValidationResult> Results { get; set; }

    public IEnumerator<ValidationResult> GetEnumerator()
    {
        return Results.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public static implicit operator bool(
        Validation validation)
    {
        return validation.Results.Any() == false;
    }
}
