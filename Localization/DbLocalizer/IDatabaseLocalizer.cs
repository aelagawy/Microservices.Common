using Microservices.Common.Enums;

namespace Microservices.Common.Localization.DbLocalizer
{
    public interface IDatabaseLocalizer
    {
        string this[string key] { get; set; }
        string this[string key, params object[] arguments] { get; }
        /// <summary>
        /// Will set the value for localized key with the current language, unless language argument is defined explicitly.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        Task<bool> Set(string key, string value, Language? language = null);
        Task<bool> SetRange(Dictionary<string, string> keyValuePairs, Language? language = null);
    }
}