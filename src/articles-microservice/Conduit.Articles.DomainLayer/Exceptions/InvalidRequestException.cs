using System.ComponentModel.DataAnnotations;

namespace Conduit.Articles.DomainLayer.Exceptions;

public class InvalidRequestException : BadRequestException
{
    public InvalidRequestException(
        IEnumerable<ValidationResult> validationResults)
    {
        ValidationResults = validationResults;
    }

    public IEnumerable<ValidationResult> ValidationResults { get; set; }
}
