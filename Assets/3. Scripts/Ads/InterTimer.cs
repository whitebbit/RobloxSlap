using System;
using System.Collections;
using _3._Scripts.Config;
using _3._Scripts.Localization;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace _3._Scripts.Ads
{
    public class InterTimer : MonoBehaviour
    {
        [SerializeField] private GameObject secondsPanelObject;
        [SerializeField] private LocalizeStringEvent localizedText;

        private bool _isAdShowing;
        private Coroutine _checkTimerCoroutine;
        private Coroutine _adShowCoroutine;
        private Coroutine _backupTimerCoroutine;
        public bool Active { get; private set; }
        public bool Blocked { get; set; }

        private void Start()
        {
            if (!Configuration.Instance.InterByTime) return;

            if (secondsPanelObject)
                secondsPanelObject.SetActive(false);

            StartCoroutine(CheckTimerAd());
        }

        private IEnumerator CheckTimerAd()
        {
            var checking = true;
            while (checking)
            {
                if (CanShow())
                {
                    _objSecCounter = 2;
                    if (secondsPanelObject)
                        secondsPanelObject.SetActive(true);

                    StartCoroutine(TimerAdShow());
                    yield return checking = false;
                }
                
                yield return new WaitForSeconds(1.0f);
            }
        }

        private int _objSecCounter = 2;

        private IEnumerator TimerAdShow()
        {
            Active = true;

            while (Active)
            {
                if (_objSecCounter > 0)
                {
                    UpdateLocalizedText(_objSecCounter);
                    _objSecCounter--;

                    yield return new WaitForSeconds(1.0f);
                }

                StartCoroutine(BackupTimerClosure());
                GBGames.ShowInterstitial();

                while (!GBGames.NowAdsShow)
                    yield return null;

                secondsPanelObject.SetActive(false);
                _objSecCounter = 2;
                StartCoroutine(CheckTimerAd());
                
                Active = false;
            }
        }

        private IEnumerator BackupTimerClosure()
        {
            yield return new WaitForSeconds(2.5f);

            if (_objSecCounter <= 0) yield break;
            
            secondsPanelObject.SetActive(false);
            _objSecCounter = 2;
            Active = false;
            StopCoroutine(TimerAdShow());
        }

        private void UpdateLocalizedText(int value)
        {
            localizedText.SetVariable("value", value);
        }

        private bool CanShow()
        {
            return !GBGames.NowAdsShow && !Blocked && GBGames.CanShowInterstitial;
        }
    }
}