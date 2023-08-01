using Microservices.Common.Enums;
using System.Globalization;

namespace Microservices.Common.Services
{
    public static class CurrentLanguage
    {
        public static Language Key => SupportedLanguages.Single(x => x.Value == CultureInfo.CurrentCulture.TwoLetterISOLanguageName).Key;
        public static string TwoLetterISOLanguageName => CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

        private static Dictionary<Language, string> SupportedLanguages => new()
        {
            { Language.Arabic, "ar" },
            { Language.English, "en" }
        };
    }
}