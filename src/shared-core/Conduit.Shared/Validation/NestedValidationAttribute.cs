using System.ComponentModel.DataAnnotations;

namespace Conduit.Shared.Validation;

[AttributeUsage(AttributeTargets.Property |
                AttributeTargets.Field |
                AttributeTargets.Parameter)]
public class NestedValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value is null)
        {
            return null;
        }

        var newContext = new ValidationContext(value)
        {
            DisplayName = validationContext.DisplayName,
            MemberName = validationContext.MemberName
        };

        var results = new List<ValidationResult>();
        _ = Validator.TryValidateObject(value, newContext, results);
        return results.FirstOrDefault();
    }
}
