using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WowFeedGrabber 
{
    public static class HtmlConvert 
    {
        private static readonly Dictionary<string, string> characterPlaceholders = new Dictionary<string, string>()
                                                                            {
                                                                                { "&amp;", "&" },
                                                                                { "&lt;", "<" },
                                                                                { "&gt;", ">" },
                                                                                { "&quot;", "'" },
                                                                                { "&apos;", "'" }
                                                                            };
        public static string ToPlainText(string html)
        {
            if (html == null)
                throw new ArgumentNullException("html");

            return html.StripHtmlTags()
                       .ReplaceCharacterPlaceholders();
        }

        private static string StripHtmlTags(this string html)
        {
            return Regex.Replace(html, "<(/?)([^>]+)>", String.Empty);
        }

        private static string ReplaceCharacterPlaceholders(this string text)
        {
            foreach (var placeholder in characterPlaceholders)
                text = text.Replace(placeholder.Key, placeholder.Value);

            return text;
        }
    }
}
