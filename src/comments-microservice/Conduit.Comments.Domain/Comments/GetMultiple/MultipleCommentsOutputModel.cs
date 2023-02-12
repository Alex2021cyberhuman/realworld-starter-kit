using System.ComponentModel.DataAnnotations;
using Conduit.Comments.Domain.Comments.Models;

namespace Conduit.Comments.Domain.Comments.GetMultiple;

public class MultipleCommentsOutputModel
{
    [Required]
    public List<CommentOutputModel> Comments { get; set; } = new();
}
