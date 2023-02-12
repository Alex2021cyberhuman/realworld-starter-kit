using System.ComponentModel.DataAnnotations;

namespace Conduit.Articles.DomainLayer.Utilities;

public interface IValidator<in TEntityToValidate>
{
    Task<IEnumerable<ValidationResult>> ValidateAsync(
        TEntityToValidate entityToValidate,
        CancellationToken cancellationToken = default);
}
