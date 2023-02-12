using System.ComponentModel.DataAnnotations;
using Conduit.Shared.Validation;

namespace Conduit.Comments.Domain.Comments.Create;

public class CommentCreateInputModel
{
    [Required]
    [NestedValidation]
    public CommentInnerModel Comment { get; set; } = new();

    public class CommentInnerModel
    {
        [Required]
        [MaxLength(300)]
        public string Body { get; set; } = string.Empty;
    }
}
