using System;
using _3._Scripts.Actions.Interfaces;
using _3._Scripts.Config;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Localization;
using _3._Scripts.UI.Effects;
using _3._Scripts.UI.Panels;
using _3._Scripts.Wallet;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Localization.Components;
using VInspector;

namespace _3._Scripts.Actions
{
    public class Training: MonoBehaviour, IActionable
    {
        
        [Tab("Settings")]
        [SerializeField] private CurrencyType currencyType;
        [SerializeField] private CurrencyCounterEffect effect;
        [SerializeField] private ParticleSystem particle;
        
        [Tab("Count")]
        [SerializeField] private float count;
        [SerializeField] private LocalizeStringEvent countText;
        [Tab("Required")]
        [SerializeField] private float requiredCount;
        [SerializeField] private LocalizeStringEvent requiredText;
        private void Start()
        {
            requiredText.SetVariable("value", WalletManager.ConvertToWallet((decimal) requiredCount));
            countText.SetVariable("value", WalletManager.ConvertToWallet((decimal) count));
        }

        public void Action()
        {
            if(WalletManager.GetQuantityByType(currencyType) < requiredCount) return;
            
            var position = transform.localPosition;
            var training = Player.Player.instance.GetTrainingStrength(count);
            var obj = CurrencyEffectPanel.Instance.SpawnEffect(effect, currencyType, training);
            
            obj.Initialize(currencyType, training);
            transform.DOShakePosition(0.25f, 0.25f, 50).OnComplete(() => transform.localPosition = position);
            particle.Play();
        }
    }
}