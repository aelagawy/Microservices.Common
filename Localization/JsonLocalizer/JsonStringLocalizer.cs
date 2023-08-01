using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Text;

namespace Microservices.Common.Localization.JsonLocalizer
{
    public class JsonStringLocalizer : IStringLocalizer
    {
        private readonly IDistributedCache _cache;
        private readonly JsonSerializer _serializer = new();
        private readonly string _resourcesRootLocation = @"wwwroot/Resources";
        public JsonStringLocalizer(IDistributedCache cache)
        {
            _cache = cache;
        }

        public LocalizedString this[string name]
        {
            get
            {
                string? value = GetString(name);
                return new LocalizedString(name, value ?? name, value == null);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var actualValue = this[name];
                return !actualValue.ResourceNotFound
                    ? new LocalizedString(name, string.Format(actualValue.Value, arguments), false)
                    : actualValue;
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            string filePath = $"{_resourcesRootLocation}/{Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName}.json";
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var sReader = new StreamReader(stream);
            using var reader = new JsonTextReader(sReader);
            while (reader.Read())
            {
                if (reader.TokenType != JsonToken.PropertyName)
                {
                    continue;
                }

                string? key = (string?)reader.Value;
                reader.Read();
                string value = _serializer.Deserialize<string>(reader);
                yield return new LocalizedString(key, value, false);
            }
        }

        private string? GetString(string key)
        {
            string relativeFilePath = $"{_resourcesRootLocation}/{Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName}.json";
            string fullFilePath = Path.GetFullPath(relativeFilePath);
            if (File.Exists(fullFilePath))
            {
                string cacheKey = $"locale_{Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName}_{key}";
                string? cacheValue = _cache.GetString(cacheKey);
                if (!string.IsNullOrEmpty(cacheValue))
                {
                    return cacheValue;
                }

                string? result = GetValueFromJSON(key, Path.GetFullPath(relativeFilePath));
                if (!string.IsNullOrEmpty(result))
                {
                    _cache.SetString(cacheKey, result);
                }

                return result;
            }

            return default;
        }

        private string? GetValueFromJSON(string propertyName, string filePath)
        {
            if (propertyName == null)
            {
                return default;
            }

            if (filePath == null)
            {
                return default;
            }

            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var sReader = new StreamReader(stream);
            using var reader = new JsonTextReader(sReader);
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName && reader.Path == propertyName)
                {
                    reader.Read();
                    return _serializer.Deserialize<string>(reader);
                }
            }
            return default;
        }
    }
}
