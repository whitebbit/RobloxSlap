using System.Collections;
using System.Collections.Generic;
using CAS;
using CAS.AdObject;
using UnityEngine;
using UnityEngine.SceneManagement;
using VInspector;

namespace GBGamesPlugin
{
    public partial class GBGames : MonoBehaviour
    {
        public static GBGames instance { get; private set; }
        [Tab("Main")] [SerializeField] private GBGamesSettings settings;

        [Tab("Advertisement")] 
        [SerializeField] private InterstitialAdObject interstitial;
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
            yield return new WaitForSeconds(1);
            Storage();
            yield return new WaitForSeconds(2);
            RemoteConfig();
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
            
            OnInterstitialClosed.AddListener(() => OnAdShown(AdType.Interstitial));
            OnRewardedClosed.AddListener(() => OnAdShown(AdType.Rewarded));

            if (saves.firstSession)
            {
                HideBanner();
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

        private void RemoteConfig()
        {
        }
    }
}