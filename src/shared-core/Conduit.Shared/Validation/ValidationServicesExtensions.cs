using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Conduit.Shared.Validation;

public static class ValidationServicesExtensions
{
    public static IMvcBuilder RegisterValidateModelAttribute(
        this IMvcBuilder builder)
    {
        return builder.AddMvcOptions(x =>
            x.Filters.Add(ValidateModelAttribute.Instance));
    }

    public static IServiceCollection DisableDefaultModelValidation(
        this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
        var serviceDescriptor = services.FirstOrDefault(s =>
            s.ServiceType == typeof(IObjectModelValidator));
        if (serviceDescriptor != null)
        {
            services.Remove(serviceDescriptor);
            services.Add(new(typeof(IObjectModelValidator),
                _ => new EmptyModelValidator(), ServiceLifetime.Singleton));
        }

        return services;
    }

    private class EmptyModelValidator : IObjectModelValidator
    {
        public void Validate(
            ActionContext actionContext,
            ValidationStateDictionary? validationState,
            string prefix,
            object? model)
        {
        }
    }
}
