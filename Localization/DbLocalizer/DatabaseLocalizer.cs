using Microservices.Common.Enums;
using Microservices.Common.Interfaces;
using Microservices.Common.Models;
using Microservices.Common.Services;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Common.Localization.DbLocalizer
{
    public class DatabaseLocalizer : IDatabaseLocalizer
    {
        private readonly IApplicationDbContextBase _context;
        public DatabaseLocalizer(IApplicationDbContextBase context)
        {
            _context = context;
        }

        public virtual string this[string key]
        {
            set => Set(key, value, CurrentLanguage.Key).GetAwaiter();
            get => Get(key).Result;
        }

        public virtual string this[string key, params object[] arguments]
        {
            get => string.Format(Get(key).Result, arguments);
        }

        public async Task<bool> Set(string key, string value, Language? language)
        {
            var _language = language ?? CurrentLanguage.Key;

            var localizedString = await _context.LocalizedStrings.FirstOrDefaultAsync(x => x.Key == key);
            if (localizedString == default)
            {
                localizedString = _context.LocalizedStrings.Add(new DatabaseLocalizedString()
                {
                    Key = key
                }).Entity;
            }

            switch (_language)
            {
                case Language.English:
                    localizedString.English = value;
                    break;

                case Language.Arabic:
                    localizedString.Arabic = value;
                    break;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetRange(Dictionary<string, string> keyValuePairs, Language? language)
        {
            var _language = language ?? CurrentLanguage.Key;

            foreach (var keyValuePair in keyValuePairs)
            {
                var localizedString = await _context.LocalizedStrings
                    .FirstOrDefaultAsync(x => x.Key == keyValuePair.Key);
                if (localizedString == default)
                {
                    localizedString = _context.LocalizedStrings.Add(new DatabaseLocalizedString()
                    {
                        Key = keyValuePair.Key
                    }).Entity;
                }

                switch (_language)
                {
                    case Language.English:
                        localizedString.English = keyValuePair.Value;
                        break;

                    case Language.Arabic:
                        localizedString.Arabic = keyValuePair.Value;
                        break;
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<string> Get(string key)
        {
            var localizedString = await _context.LocalizedStrings.FirstOrDefaultAsync(x => x.Key == key);
            if (localizedString == null)
            {
                return key;
            }

            return CurrentLanguage.Key switch
            {
                Language.English => localizedString.English,
                _ => localizedString.Arabic,
            };
        }
    }
}