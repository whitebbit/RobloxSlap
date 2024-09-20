
using System.Collections.Generic;


namespace GBGamesPlugin
{
    public partial class GBGames
    {

        public static T GetRemoteValue<T>(string id, T defaultValue = default)
        {
            /*var configValue = _data.FirstOrDefault(d => d.name == id);

            if (configValue == null || !remoteConfigIsSupported)
            {
                return defaultValue;
            }

            try
            {
                if (typeof(T) == typeof(string))
                {
                    return (T) (object) configValue.value;
                }

                if (typeof(T) == typeof(int))
                {
                    return (T) (object) int.Parse(configValue.value);
                }

                if (typeof(T) == typeof(float))
                {
                    return (T) (object) float.Parse(configValue.value);
                }

                if (typeof(T) == typeof(bool))
                {
                    return (T) (object) bool.Parse(configValue.value);
                }

                throw new NotSupportedException($"Type {typeof(T)} is not supported");
            }
            catch
            {
                return defaultValue;
            }*/
            return defaultValue;
        }

        private static void LoadRemoteConfig(Dictionary<string, object> options = default)
        {
            
        }
        
    }
}