using System.Collections.Generic;
using _3._Scripts.UI.Enums;
using _3._Scripts.UI.Scriptable.Roulette;
using Firebase;
using Firebase.Extensions;
using GBGamesPlugin.Enums;
using Io.AppMetrica;
using Y._Imported_Assets.GBGamesPlugin.Scripts.Analytics.Utils;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        private static AdEventParameters _currentAdEventParameters;
        private static FirebaseApp _app;

        private static void InitializeFirebase()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    _app = FirebaseApp.DefaultInstance;
                }
                else
                {
                    UnityEngine.Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                }
            });
        }

        private static void ReportAdsEvent(AdEventType eventType, AdEventResult result,
            AdEventParameters eventParameters = null)
        {
            eventParameters ??= _currentAdEventParameters;

            if (eventParameters == null) return;

            var parameters = new Dictionary<string, string>
            {
                {"ad_type", eventParameters.adType.GetString()},
                {"placement", eventParameters.adPlacement.GetString()},
                {"result", result.GetString()}
            };

            _currentAdEventParameters = eventParameters;
            ReportEvent(eventType.GetString(), parameters);
        }

        public static void ReportLevelEvent(LevelEventType eventType, int worldNumber = -1, int stageNumber = -1)
        {
            var parameters = new Dictionary<string, int>
            {
                {"world_number", worldNumber == -1 ? saves.worldID + 1 : worldNumber},
                {"stage_number", stageNumber == -1 ? saves.stageID + 1 : stageNumber}
            };
            
            Firebase.Analytics.Parameter[] levelUpParameters =
            {
                new(
                    "world_number", worldNumber == -1 ? saves.worldID + 1 : worldNumber),
                new(
                    "stage_number", stageNumber == -1 ? saves.stageID + 1 : stageNumber)
            };
            
            Firebase.Analytics.FirebaseAnalytics.LogEvent(eventType.GetString(), levelUpParameters);
            ReportEvent(eventType.GetString(), parameters);
            AppMetrica.SendEventsBuffer();
        }

        public static void ReportTutorialEvent(string stepName)
        {
            var parameters = new Dictionary<string, string>
            {
                {"step_name", $"{stepName}"}
            };


            ReportEvent("tutorial", parameters);
            AppMetrica.SendEventsBuffer();
        }

        public static void ReportPlayerLevelEvent(int playerLevel)
        {
            var parameters = new Dictionary<string, int>
            {
                {"level", playerLevel}
            };

            ReportEvent("level_up", parameters);
        }

        public static void ReportSkinUnlockEvent(string skinName, Rarity skinRarity, PurchaseType unlockType)
        {
            var parameters = new Dictionary<string, string>
            {
                {"skin_name", skinName},
                {"skin_rarity", skinRarity.GetString()},
                {"unlock_type", unlockType.GetString()},
            };

            ReportEvent("skin_unlock", parameters);
        }

        public static void ReportGiftProgressEvent(int giftNumber, GiftItem gift)
        {
            var parameters = new Dictionary<string, string>
            {
                {"gift_name", $"{giftNumber}_{gift.GetString()}"},
            };

            ReportEvent("gift_progress", parameters);
        }

        public static void ReportGiftOpenEvent(GiftOpenType type)
        {
            var parameters = new Dictionary<string, string>
            {
                {"open_type", type.GetString()},
            };

            ReportEvent("open_gift", parameters);
        }

        public static void ReportBoosterChangeStateEvent(string boosterEventName, bool state)
        {
            var parameters = new Dictionary<string, string>
            {
                {"state", state ? "on" : "off"},
            };

            ReportEvent(boosterEventName, parameters);
        }

        public static void ReportUINavigationEvent(string placement)
        {
            var parameters = new Dictionary<string, string>
            {
                {"placement", placement},
            };

            ReportEvent("UI_navigation", parameters);
        }

        public static void ReportDailyRewardEvent(int numberDay, int countDay)
        {
            var parameters = new Dictionary<string, int>
            {
                {"number_day", numberDay},
                {"count_day", countDay},
            };

            ReportEvent("daily_rewards", parameters);
        }

        public static void ReportAchievementsEvent(string name)
        {
            var parameters = new Dictionary<string, string>
            {
                {"name_achiv", name},
            };

            ReportEvent("achievements", parameters);
        }

        public static void ReportBossEvent()
        {
            var parameters = new Dictionary<string, int>
            {
                {"boss_count", saves.bossFightsCount},
            };

            ReportEvent("boss_fight", parameters);
        }

        private static void ReportEvent<T, T1>(string eventName, Dictionary<T, T1> parameters)
        {
            var json = parameters.ToJson();

            AppMetrica.ReportEvent(eventName, json);
            ReportEventMessage(eventName, json);
        }
    }
}