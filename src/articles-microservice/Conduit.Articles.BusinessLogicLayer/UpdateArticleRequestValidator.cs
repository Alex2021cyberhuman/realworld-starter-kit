using System.ComponentModel.DataAnnotations;
using Conduit.Articles.DomainLayer.Models;
using Conduit.Shared.Validation;
using FluentValidation;

namespace Conduit.Articles.BusinessLogicLayer;

public class UpdateArticleRequestValidator :
    AbstractValidator<UpdateArticle.Request>,
    DomainLayer.Utilities.IValidator<UpdateArticle.Request>
{
    async Task<IEnumerable<ValidationResult>>
        DomainLayer.Utilities.IValidator<UpdateArticle.Request>.ValidateAsync(
            UpdateArticle.Request entityToValidate,
            CancellationToken cancellationToken)
    {
        var fluentValidationResult =
            await ValidateAsync(entityToValidate, cancellationToken);
        var validation = fluentValidationResult.ToValidation();
        return validation;
    }
}
