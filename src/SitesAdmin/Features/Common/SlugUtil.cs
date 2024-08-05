using SitesAdmin.Features.Common.Interfaces;
using System.Text;

namespace SitesAdmin.Features.Common
{
    public static class SlugUtil
    {
        public static string Slugify(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException("input");
            }

            var stringBuilder = new StringBuilder();
            foreach (char c in input.ToArray())
            {
                if (Char.IsLetterOrDigit(c))
                {
                    stringBuilder.Append(c);
                }
                else if (c == ' ')
                {
                    stringBuilder.Append("-");
                }
            }

            return stringBuilder.ToString().ToLower();
        }

        public static void SetDefaultSlug(ISluggable sluggable)
        {
            if (string.IsNullOrEmpty(sluggable.Slug))
            {
                sluggable.Slug = SlugUtil.Slugify(sluggable.GetSlugDisplayName());
            }
        }
    }
}
