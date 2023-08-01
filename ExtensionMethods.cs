using Microservices.Common.Enums;
using Microservices.Common.Models;
using Microservices.Common.Services;
using System.Xml.Serialization;

namespace Microservices.Common
{
    public static class ExtensionMethods
    {
        private const char _localizationSplittingCharacter = ';';
        public static string? ToLocalesString(this string[] item)
        {
            if (item != null && item.Length > 1)
            {
                return string.Join(';', item.Select(i => i?.Trim()));
            }

            return null;
        }

        public static string? ToLocale(this TranslationDto item)
        {
            if (item == default)
            {
                return null;
            }

            return CurrentLanguage.Key switch
            {
                Language.English => item.En,
                _ => item.Ar,
            };
        }

        public static TranslationDto? ToTranslationDto(this string item)
        {
            if (string.IsNullOrEmpty(item))
                return default;

            var arr = item.IndexOf(_localizationSplittingCharacter) > -1
                ? item.Split(_localizationSplittingCharacter, StringSplitOptions.None).Select(i => i.Trim()).ToArray()
                : new string[] { item, item };

            return new TranslationDto { Ar = arr[0], En = arr[1] };
        }

        public static string? ToLocale(this string item)
        {
            if (string.IsNullOrEmpty(item))
            {
                return null;
            }

            var arr = item.IndexOf(_localizationSplittingCharacter) > -1
                ? item.Split(_localizationSplittingCharacter, StringSplitOptions.None).Select(i => i.Trim()).ToArray()
                : new string[] { item, item };

            return arr[(int)CurrentLanguage.Key];
        }

        public static string? ToLocale(this string item, Language lang)
        {
            if (string.IsNullOrEmpty(item))
            {
                return null;
            }

            var arr = item.IndexOf(_localizationSplittingCharacter) > -1
                ? item.Split(_localizationSplittingCharacter, StringSplitOptions.None).Select(i => i.Trim()).ToArray()
                : new string[] { item, item };

            return arr[(int)lang];
        }

        public static string? ToXML(this object item)
        {
            try
            {
                using var stringwriter = new StringWriter();
                var serializer = new XmlSerializer(item.GetType());
                serializer.Serialize(stringwriter, item);
                return stringwriter.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //SharePoint special characters replacement
        public static string ToNeutralizedString(this string item)
        {
            var specialCharacters = new List<char> { '#', '%', '*', ':', '<', '>', '?', '/', '|' };
            if (!string.IsNullOrEmpty(item))
                if (specialCharacters.Any(x => item.Contains(x)))
                    specialCharacters.ForEach(c => item = item.Replace(c, '_'));
            return item;
        }
    }
}