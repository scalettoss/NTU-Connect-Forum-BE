using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace ForumBE.Helpers
{
    public class ConvertStringToSlug
    {
        public static string ToSlug(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            string slug = input.ToLowerInvariant();

            slug = RemoveDiacritics(slug);

            slug = Regex.Replace(slug, @"\s+", "-");

            slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");

            slug = Regex.Replace(slug, @"-+", "-").Trim('-');

            return slug;
        }

        private static string RemoveDiacritics(string text)
        {
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
