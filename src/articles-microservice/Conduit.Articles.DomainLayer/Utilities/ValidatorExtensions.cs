using Conduit.Articles.DomainLayer.Exceptions;

namespace Conduit.Articles.DomainLayer.Utilities;

public static class ValidatorExtensions
{
    public static async Task ValidateAndThrowAsync<T>(
        this IValidator<T> validator,
        T entityToValidate,
        CancellationToken cancellationToken = default)
    {
        var results =
            await validator.ValidateAsync(entityToValidate, cancellationToken);
        if (results.Any())
        {
            throw new InvalidRequestException(results);
        }
    }
}
