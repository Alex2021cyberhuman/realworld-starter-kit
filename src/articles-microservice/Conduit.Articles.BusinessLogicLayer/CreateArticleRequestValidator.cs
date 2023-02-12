using System.ComponentModel.DataAnnotations;
using Conduit.Articles.DomainLayer.Models;
using Conduit.Shared.Validation;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Conduit.Articles.BusinessLogicLayer;

public class CreateArticleRequestValidator :
    AbstractValidator<CreateArticle.Request>,
    DomainLayer.Utilities.IValidator<CreateArticle.Request>
{
    public CreateArticleRequestValidator(
        IStringLocalizer stringLocalizer)
    {
        RuleFor(x => x.Body.Article.Title).NotEmpty()
            .WithName(stringLocalizer.GetCreateArticleTitlePropertyName());
        RuleFor(x => x.Body.Article.Description).NotEmpty()
            .WithName(stringLocalizer
                .GetCreateArticleDescriptionPropertyName());
        RuleFor(x => x.Body.Article.Body).NotEmpty()
            .WithName(stringLocalizer.GetCreateArticleBodyPropertyName());
        RuleFor(x => x.Body.Article.TagList).NotEmpty()
            .WithName(stringLocalizer.GetCreateArticleTagListPropertyName());
    }

    async Task<IEnumerable<ValidationResult>>
        DomainLayer.Utilities.IValidator<CreateArticle.Request>.ValidateAsync(
            CreateArticle.Request entityToValidate,
            CancellationToken cancellationToken)
    {
        var fluentValidationResult =
            await ValidateAsync(entityToValidate, cancellationToken);
        var validation = fluentValidationResult.ToValidation();
        return validation;
    }
}
