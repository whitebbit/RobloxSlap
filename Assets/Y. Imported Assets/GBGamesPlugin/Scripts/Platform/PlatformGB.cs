
using UnityEngine;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
    
        /// <summary>
        /// Если платформа предоставляет данные об языке пользователя — то это будет язык, который установлен у пользователя на платформе. Если не предоставляет — это будет язык браузера пользователя. Формат: ISO 639-1. Пример: ru, en.
        /// </summary>
        public static string language => GetLanguageCode(Application.systemLanguage);

        private static string GetLanguageCode(SystemLanguage lang)
        {
            return lang switch
            {
                SystemLanguage.English => "en",
                SystemLanguage.Russian => "ru",
                _ => "en"
            };
        }
    }
}