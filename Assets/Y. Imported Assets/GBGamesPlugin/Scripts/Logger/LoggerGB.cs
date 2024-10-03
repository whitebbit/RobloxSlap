using UnityEngine;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        private static void Message(string message, LoggerState state = LoggerState.log)
        {
            if (!instance.settings.debug) return;
            var prefix = state switch
            {
                LoggerState.log => "<color=green>[LOG]</color>",
                LoggerState.warning => "<color=yellow>[WARNING]</color>",
                LoggerState.error => "<color=red>[ERROR]</color>",
                _ => ""
            };
            Debug.Log($"{prefix} {message}");
        }

        private static void ReportEventMessage(string eventType, string json)
        {
            if (!instance.settings.debug) return;
            Debug.Log($"{eventType}, {json}");
        }
    }
}