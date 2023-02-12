using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Conduit.Shared.Localization;

public static class LocalizationExtensions
{
    public static IMvcBuilder Localize<TResourceType>(
        this IMvcBuilder mvcBuilder,
        IList<CultureInfo> supportedCultures)
    {
        mvcBuilder.AddMvcLocalization(o => o.ResourcesPath = "Resources")
            .Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.DefaultRequestCulture = new(supportedCultures.First());
                options.RequestCultureProviders.Clear();
                options.RequestCultureProviders.Add(
                new QueryStringRequestCultureProvider());
                options.RequestCultureProviders.Add(
                new AcceptLanguageHeaderRequestCultureProvider());
            }).AddSingleton(serviceProvider => (IStringLocalizer)serviceProvider
            .GetRequiredService<IStringLocalizer<TResourceType>>());
        return mvcBuilder;
    }
}
