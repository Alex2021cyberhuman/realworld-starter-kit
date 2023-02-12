using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Conduit.Shared.Validation;

[Serializable]
public sealed class
    ConduitCamelCaseSerializableError : Dictionary<string, IEnumerable<string>>
{
    public ConduitCamelCaseSerializableError(
        ModelStateDictionary modelState)
    {
        if (modelState.IsValid)
        {
            return;
        }

        foreach (var (key, value) in modelState)
        {
            var keyPath = CamelCase(key.Split('.').Last());
            var errors = value.Errors;
            SetErrors(errors, keyPath);
        }
    }

    private void SetErrors(
        ModelErrorCollection errors,
        string targetKey)
    {
        if (errors.Count > 0)
        {
            var errorMessages = errors.Where(x =>
                    string.IsNullOrWhiteSpace(x.ErrorMessage) == false)
                .Select(x => x.ErrorMessage).ToArray();

            this[targetKey] = errorMessages;
        }
    }

    private static string CamelCase(
        string subKey)
    {
        var subKeyArray = subKey.ToArray();
        subKeyArray[0] = char.ToLower(subKeyArray[0]);
        return new(subKeyArray);
    }
}
