using System.Collections.Generic;
using Newtonsoft.Json;

namespace Y._Imported_Assets.GBGamesPlugin.Scripts.Analytics.Utils
{
    public static class DictionaryExtensions
    {
        public static string ToJson<T, T1>(this Dictionary<T, T1> dictionary)
        {
            return JsonConvert.SerializeObject(dictionary);
        }
    }
}