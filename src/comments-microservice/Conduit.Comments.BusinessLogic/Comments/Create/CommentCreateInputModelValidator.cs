using Conduit.Comments.Domain.Comments.Create;
using Conduit.Shared.Validation;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Conduit.Comments.BusinessLogic.Comments.Create;

public class CommentCreateInputModelValidator :
    AbstractValidator<CommentCreateInputModel>,
    ICommentCreateInputModelValidator
{
    public const string CommentBody = "CommentName";

    public CommentCreateInputModelValidator(
        IStringLocalizer stringLocalizer)
    {
        RuleFor(x => x.Comment.Body).NotEmpty()
            .WithName(stringLocalizer.GetString(CommentBody)).MaximumLength(300)
            .WithName(stringLocalizer.GetString(CommentBody));
    }

    async Task<Validation> ICommentCreateInputModelValidator.ValidateAsync(
        CommentCreateInputModel commentCreateInputModel)
    {
        var results = await ValidateAsync(commentCreateInputModel);
        return results.ToValidation();
    }
}
