using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Conduit.Shared.Validation;

public static class ValidationExtensions
{
    public static ModelStateDictionary ToModelStateDictionary(
        this Validation validation)
    {
        var modelStateDictionary = new ModelStateDictionary();
        foreach (var result in validation)
        {
            if (result.ErrorMessage != null)
            {
                modelStateDictionary.AddModelError(
                    result.MemberNames.FirstOrDefault() ?? string.Empty,
                    result.ErrorMessage);
            }
        }

        return modelStateDictionary;
    }

    public static Validation ToValidation(
        this ValidationResult fluentValidationResult)
    {
        var validationResults = fluentValidationResult.Errors.Select(x =>
            new System.ComponentModel.DataAnnotations.ValidationResult(
                x.ErrorMessage, new[] { x.PropertyName })).ToList();
        var validation = new Validation(validationResults);
        return validation;
    }

    public static IActionResult ToBadRequest(
        this Validation? validation)
    {
        return validation is null
            ? new StatusCodeResult(422)
            : new ObjectResult(new
            {
                errors =
                    new ConduitCamelCaseSerializableError(
                        validation.ToModelStateDictionary())
            })
            { StatusCode = 422 };
    }
}
