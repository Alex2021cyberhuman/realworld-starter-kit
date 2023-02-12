using Microsoft.Extensions.Localization;

namespace Conduit.Articles.BusinessLogicLayer;

public static class ValidationLocalization
{
    public static string GetCreateArticleTitlePropertyName(
        this IStringLocalizer localizer)
    {
        return localizer["CreateArticleTitle"];
    }

    public static string GetCreateArticleDescriptionPropertyName(
        this IStringLocalizer localizer)
    {
        return localizer["CreateArticleDescription"];
    }

    public static string GetCreateArticleBodyPropertyName(
        this IStringLocalizer localizer)
    {
        return localizer["CreateArticleBody"];
    }

    public static string GetCreateArticleTagListPropertyName(
        this IStringLocalizer localizer)
    {
        return localizer["CreateArticleTagList"];
    }

    public static string GetUpdateArticleTitlePropertyName(
        this IStringLocalizer localizer)
    {
        return localizer["UpdateArticleTitle"];
    }

    public static string GetUpdateArticleDescriptionPropertyName(
        this IStringLocalizer localizer)
    {
        return localizer["UpdateArticleDescription"];
    }

    public static string GetUpdateArticleBodyPropertyName(
        this IStringLocalizer localizer)
    {
        return localizer["UpdateArticleBody"];
    }

    public static string GetUpdateArticleTagList(
        this IStringLocalizer localizer)
    {
        return localizer["UpdateArticleTagList"];
    }
}
