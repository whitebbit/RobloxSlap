using System;
using _3._Scripts.Localization;
using _3._Scripts.Singleton;
using _3._Scripts.Wallet;
using DG.Tweening;
using GBGamesPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace _3._Scripts.UI.Elements
{
    public class AfterInterstitialReward : MonoBehaviour
    {
        [SerializeField] private CanvasGroup rewardObject;
        [SerializeField] private int baseReward;
        [SerializeField] private LocalizeStringEvent adCountText;
        [SerializeField] private TMP_Text rewardCount;

        private int _adAmount;

        private void Start()
        {
            rewardObject.alpha = 0;
        }

        private void OnEnable()
        {
            GBGames.OnAdClosed.AddListener(ShowReward);
        }

        private void OnDisable()
        {
            GBGames.OnAdClosed.RemoveListener(ShowReward);
        }

        private void ShowReward()
        {
            _adAmount += 1;
            adCountText.SetVariable("value", _adAmount);
            rewardCount.text = $"+{WalletManager.ConvertToWallet((baseReward * _adAmount))}<sprite index=0>";

            rewardObject.DOFade(1, 0.2f).OnComplete(() => rewardObject.DOFade(0, 0.2f).SetDelay(1f));
            WalletManager.SecondCurrency += baseReward * _adAmount;
        }
    }
}