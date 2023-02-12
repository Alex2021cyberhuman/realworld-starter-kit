using System.Text;
using System.Text.RegularExpressions;
using Conduit.Articles.DomainLayer.Utilities;

namespace Conduit.Articles.BusinessLogicLayer;

public class Slugilizator : ISlugilizator
{
    private static readonly Dictionary<string, string> CharReplacements = new()
    {
        ["а"] = "a",
        ["б"] = "b",
        ["в"] = "v",
        ["г"] = "g",
        ["д"] = "d",
        ["е"] = "ye",
        ["ё"] = "yo",
        ["ж"] = "zh",
        ["з"] = "z",
        ["и"] = "ee",
        ["й"] = "i",
        ["к"] = "k",
        ["л"] = "l",
        ["м"] = "m",
        ["н"] = "n",
        ["о"] = "o",
        ["п"] = "p",
        ["р"] = "r",
        ["с"] = "s",
        ["т"] = "t",
        ["у"] = "u",
        ["ф"] = "f",
        ["х"] = "h",
        ["ц"] = "ts",
        ["ч"] = "ch",
        ["ш"] = "sh",
        ["щ"] = "sh",
        ["ы"] = "i",
        ["э"] = "e",
        ["ю"] = "yu",
        ["я"] = "ya"
    };

    private static readonly Regex RemoveInvalidCharactersRegex =
        new(@"[^a-z0-9\s-]", RegexOptions.Compiled);

    private static readonly Regex RemoveSpacesRegex =
        new(@"\s+", RegexOptions.Compiled);

    public string GetSlug(
        string title)
    {
        return GenerateSlug(title);
    }

    private static string ReplaceCharacters(
        string phrase)
    {
        var builder = new StringBuilder(phrase);
        for (var i = 0; i < builder.Length; i++)
        {
            var oldString = char.ToLower(builder[i]).ToString();
            var contains =
                CharReplacements.TryGetValue(oldString, out var replacement);
            if (contains)
            {
                builder.Replace(oldString, replacement, i, replacement!.Length);
                i += replacement.Length - 1;
            }
        }

        return builder.ToString();
    }

    private static string GenerateSlug(
        string phrase)
    {
        var str = ReplaceCharacters(phrase.ToLower());

        str = RemoveInvalidCharactersRegex.Replace(str, string.Empty);
        str = RemoveSpacesRegex.Replace(str, "-");

        if (str.Length >= 45)
        {
            str = str[..45].Trim();
        }

        var number = GenerateNumber();
        str = str.Insert(str.Length, "-" + number);
        return str;
    }

    private static string GenerateNumber()
    {
        var random = new Random();

        var number =
            $"{DateTimeOffset.UtcNow:yyyyMMddHHmmss}{Environment.CurrentManagedThreadId % 1000:d3}{Thread.GetDomainID() % 1000:d3}{random.Next(0, 100000):d5}";

        return number;
    }
}
