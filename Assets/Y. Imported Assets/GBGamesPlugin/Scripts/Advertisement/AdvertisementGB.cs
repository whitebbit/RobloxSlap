using System;
using System.Collections;
using _3._Scripts.UI.Panels;
using CAS;
using CAS.AdObject;
using GBGamesPlugin.Enums;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        public static bool NowAdsShow { get; private set; }
        public static bool CanShowInterstitial => Time.time - _lastAdShowTime >= _adInterval;

        public static UnityEvent InterstitialOnClosed => instance.interstitial.OnAdClosed;
        public static  event Action InterstitialOnFailedToShow;
        
        public void NowAdsShownState(bool state) => NowAdsShow = state;

        private IEnumerator FirstSessionActivate()
        {
            HideBanner();
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

            var eventParameters = new AdEventParameters(AdEventPlacement.InAds, AdType.Interstitial);

            if (!instance.interstitial.IsAdReady)
            {
                OnAdShown(AdType.Interstitial);
                InterstitialOnFailedToShow?.Invoke();
                NotificationPanel.Instance.ShowNotification("ad_not_ready");
                ReportAdsEvent(AdEventType.VideoAdsAvailable, AdEventResult.NotAvailable, eventParameters);
                return;
            }

            ReportAdsEvent(AdEventType.VideoAdsAvailable, AdEventResult.Success, eventParameters);
            instance.interstitial.Present();
        }

        #endregion

        #region Rewarded

        private static float _lastReportTime;

        /// <summary>
        /// Показать рекламу за вознаграждение.
        /// </summary>
        public static void ShowRewarded(UnityAction onRewarded, AdEventPlacement adEventPlacement)
        {
            if (NowAdsShow) return;

            var eventParameters = new AdEventParameters(adEventPlacement, AdType.Rewarded);

            if (!instance.rewarded.IsAdReady)
            {
                NotificationPanel.Instance.ShowNotification("ad_not_ready");
                if (!(Time.time - _lastReportTime >= 60)) return;
                _lastReportTime = Time.time;
                ReportAdsEvent(AdEventType.VideoAdsAvailable, AdEventResult.NotAvailable, eventParameters);
                return;
            }

            ReportAdsEvent(AdEventType.VideoAdsAvailable, AdEventResult.Success, eventParameters);

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