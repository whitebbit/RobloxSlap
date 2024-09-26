using System;
using System.Collections;
using CAS;
using UnityEngine;
using UnityEngine.Events;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        public static bool NowAdsShow { get; private set; }
        public static bool CanShowInterstitial => Time.time - _lastAdShowTime >= _adInterval;

        public static UnityEvent OnInterstitialClosed => instance.interstitial.OnAdClosed;
        public static UnityEvent OnRewardedClosed => instance.rewarded.OnAdClosed;

        public void NowAdsShownState(bool state) => NowAdsShow = state;

        private IEnumerator FirstSessionActivate()
        {
            yield return new WaitForSeconds(60);
            ShowBanner();
            yield return new WaitForSeconds(120);
            saves.firstSession = false;
            Save();
        }
        
        #region Banner
        
        
        /// <summary>
        /// Показать баннер.
        /// </summary>
        public static void ShowBanner()
        {
            instance.banner.gameObject.SetActive(true);
        }

        /// <summary>
        /// Скрыть баннер.
        /// </summary>
        public static void HideBanner()
        {
            instance.banner.gameObject.SetActive(false);
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

            instance.interstitial.Present();
        }

        #endregion

        #region Rewarded

        /// <summary>
        /// Показать рекламу за вознаграждение.
        /// </summary>
        public static void ShowRewarded(UnityAction onRewarded)
        {
            if (NowAdsShow) return;

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