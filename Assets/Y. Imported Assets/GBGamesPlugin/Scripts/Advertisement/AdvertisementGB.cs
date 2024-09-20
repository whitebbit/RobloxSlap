using System;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        public static bool NowAdsShow => true;
        public static bool CanShowInterstitial => false;

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

        public static event Action BannerLoadingCallback;
        public static event Action BannerShownCallback;
        public static event Action BannerHiddenCallback;
        public static event Action BannerFailedCallback;

        private void OnBannerStateChanged()
        {
            /*switch (state)
            {
                case BannerState.Loading:
                    BannerLoadingCallback?.Invoke();
                    Message("Banner state - loading");
                    break;
                case BannerState.Shown:
                    BannerShownCallback?.Invoke();

                    Message("Banner state - shown");
                    break;
                case BannerState.Hidden:
                    BannerHiddenCallback?.Invoke();

                    Message("Banner state - hidden");
                    break;
                case BannerState.Failed:
                    BannerFailedCallback?.Invoke();

                    Message("Banner state - failed", LoggerState.error);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }*/
        }

        #endregion

        #region Interstitial

        /// <summary>
        /// Минимальный интервал между показами межстраничной рекламы.
        /// </summary>
        /*public static int minimumDelayBetweenInterstitial
        {
            get => Bridge.advertisement.minimumDelayBetweenInterstitial;
            set => Bridge.advertisement.SetMinimumDelayBetweenInterstitial(value);
        }*/

        //public static bool CanShowInterstitial => Time.time - _lastAdShowTime >= minimumDelayBetweenInterstitial;
        private static float _lastAdShowTime;

        /// <summary>
        /// Показать межстраничную рекламу.
        /// </summary>
        public static void ShowInterstitial()
        {
            
        }

        public static event Action InterstitialLoadingCallback;
        public static event Action InterstitialOpenedCallback;
        public static event Action InterstitialClosedCallback;
        public static event Action InterstitialFailedCallback;

        private void OnInterstitialStateChanged()
        {
            /*switch (state)
            {
                case InterstitialState.Loading:
                    InterstitialLoadingCallback?.Invoke();
                    Message("Interstitial state - loading");
                    break;
                case InterstitialState.Opened:
                    InterstitialOpenedCallback?.Invoke();
                    Message("Interstitial state - opened");
                    _lastAdShowTime = Time.time;
                    if (_inGame)
                        PauseController.Pause(true);
                    break;
                case InterstitialState.Closed:
                    PauseController.Pause(false);
                    
                    InterstitialClosedCallback?.Invoke();
                    Message("Interstitial state - closed");
                    break;
                case InterstitialState.Failed:
                    InterstitialFailedCallback?.Invoke();
                    Message("Interstitial state - failed");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }*/
        }

        #endregion

        #region Rewarded

        /// <summary>
        /// Показать рекламу за вознаграждение.
        /// </summary>
        public static void ShowRewarded(Action onRewarded)
        {
            RewardedCallback = onRewarded;
        }

        private static event Action RewardedCallback;

        public static event Action RewardedLoadingCallback;
        public static event Action RewardedOpenedCallback;
        public static event Action RewardedClosedCallback;
        public static event Action RewardedFailedCallback;

        private void OnRewardedStateChanged()
        {
            /*switch (state)
            {
                case RewardedState.Loading:
                    RewardedLoadingCallback?.Invoke();
                    Message("Rewarded state - loading");
                    break;
                case RewardedState.Opened:
                    RewardedOpenedCallback?.Invoke();
                    Message("Rewarded state - opened");
                    PauseController.Pause(true);
                    break;
                case RewardedState.Rewarded:
                    RewardedCallback?.Invoke();
                    RewardedCallback = null;
                    break;
                case RewardedState.Closed:
                    PauseController.Pause(false);
                    RewardedClosedCallback?.Invoke();
                    Message("Rewarded state - closed");
                    break;
                case RewardedState.Failed:
                    RewardedFailedCallback?.Invoke();
                    Message("Rewarded state - failed");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }*/
        }

        #endregion
    }
}