namespace BlogAPIDotnet6.Helper;

public class SlugHelper
{
    public static string GenerateSlug(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return "";

        // Substitua caracteres especiais por hífens
        string slug = input.ToLower().Trim()
            .Replace(" ", "-")
            .Replace("á", "a")
            .Replace("é", "e")
            .Replace("í", "i")
            .Replace("ó", "o")
            .Replace("ú", "u")
            .Replace("ã", "a")
            .Replace("õ", "o")
            .Replace("ç", "c")
            .Replace(".", "")
            .Replace("!", "")
            .Replace("?", "");

        // Remove caracteres não alfanuméricos ou hífens repetidos
        slug = new string(slug
            .Where(c => Char.IsLetterOrDigit(c) || c == '-')
            .ToArray());

        // Remove hífens duplicados
        while (slug.Contains("--"))
        {
            slug = slug.Replace("--", "-");
        }

        return slug;
    }
}