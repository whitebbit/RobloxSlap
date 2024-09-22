using System;
using CAS;
using UnityEngine;
using UnityEngine.Events;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        public static bool NowAdsShow { get; private set; }
        public static bool CanShowInterstitial => Time.time - _lastAdShowTime >= _adInterval;

        public void NowAdsShownState(bool state) => NowAdsShow = state;

        #region Banner

        /// <summary>
        /// Показать баннер.
        /// </summary>
        public static void ShowBanner()
        {
        }

        /// <summary>
        /// Скрыть баннер.
        /// </summary>
        public static void HideBanner()
        {
        }

        #endregion

        #region Interstitial

        private static float _lastAdShowTime;
        private static float _adInterval;

        /// <summary>
        /// Показать межстраничную рекламу.
        /// </summary>
        public static void ShowInterstitial()
        {
            if (!CanShowInterstitial) return;
            if (NowAdsShow) return;

            OnAdShown(AdType.Interstitial);
            instance.interstitial.Present();
        }

        #endregion

        #region Rewarded

        /// <summary>
        /// Показать рекламу за вознаграждение.
        /// </summary>
        public static void ShowRewarded(Action onRewarded)
        {
            if (NowAdsShow) return;

            OnAdShown(AdType.Rewarded);
            instance.rewarded.Present(onRewarded);
        }

        #endregion

        private static void OnAdShown(AdType type)
        {
            _lastAdShowTime = Time.time;
            _adInterval = type switch
            {
                AdType.Interstitial => instance.settings.intervalAfterInterstitial,
                AdType.Rewarded => instance.settings.intervalAfterReward,
                AdType.Banner => 0,
                AdType.AppOpen => 0,
                AdType.Native => 0,
                AdType.None => 0,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}