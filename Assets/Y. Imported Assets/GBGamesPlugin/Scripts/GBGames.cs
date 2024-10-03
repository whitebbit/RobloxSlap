using System.Collections;
using System.Collections.Generic;
using CAS;
using CAS.AdObject;
using GBGamesPlugin.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;
using VInspector;

namespace GBGamesPlugin
{
    public partial class GBGames : MonoBehaviour
    {
        public static GBGames instance { get; private set; }
        [Tab("Main")] [SerializeField] private GBGamesSettings settings;

        [Tab("Advertisement")] [SerializeField]
        private InterstitialAdObject interstitial;

        [SerializeField] private RewardedAdObject rewarded;
        [SerializeField] private BannerAdObject banner;

        private static bool _inGame;

        private void Awake()
        {
            StartCoroutine(Initialize());
        }

        private IEnumerator Initialize()
        {
            _inGame = true;
            Singleton();
            Advertisement();
            Analytics();
            yield return new WaitForSeconds(1);
            Storage();
            yield return new WaitForSeconds(2);
            Game();
        }

        private void Singleton()
        {
            transform.SetParent(null);
            gameObject.name = "GBGames";

            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void Advertisement()
        {
            OnAdShown(AdType.Interstitial);

            instance.interstitial.OnAdClosed.AddListener(() =>
            {
                OnAdShown(AdType.Interstitial);
                ReportAdsEvent(AdEventType.VideoAdsSuccess, AdEventResult.Watched);
            });
            instance.rewarded.OnAdClosed.AddListener(() =>
            {
                OnAdShown(AdType.Rewarded);
                ReportAdsEvent(AdEventType.VideoAdsSuccess, AdEventResult.Watched);
            });

            instance.interstitial.OnAdShown.AddListener(() =>
                ReportAdsEvent(AdEventType.VideoAdsStarted, AdEventResult.Start));
            instance.rewarded.OnAdShown.AddListener(() =>
                ReportAdsEvent(AdEventType.VideoAdsStarted, AdEventResult.Start));
            
            instance.interstitial.OnAdFailedToShow.AddListener(_ =>
            {
                OnAdShown(AdType.Interstitial);
                ReportAdsEvent(AdEventType.VideoAdsStarted, AdEventResult.Failed);
            });
            instance.rewarded.OnAdFailedToShow.AddListener(_ =>
                ReportAdsEvent(AdEventType.VideoAdsStarted, AdEventResult.Failed));
            
            
            if (saves.firstSession)
            {
                StartCoroutine(FirstSessionActivate());
            }
            else
            {
                ShowBanner();
            }
        }

        private void Storage()
        {
            Load();
            if (instance.settings.autoSaveByInterval)
                StartCoroutine(IntervalSave());
        }

        private void Game()
        {
            if (instance.settings.saveOnChangeVisibilityState)
                GameHiddenStateCallback += Save;
        }

        private void Analytics()
        {
            InitializeFirebase();
        }
    }
}