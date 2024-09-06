using System;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Wallet;
using DG.Tweening;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.UI.Elements
{
    public class CharacterShopNotification : MonoBehaviour
    {
        [SerializeField] private Transform notification;

        private void OnEnable()
        {
            WalletManager.OnSecondCurrencyChange += OnChange;
        }

        private void OnDisable()
        {
            WalletManager.OnSecondCurrencyChange -= OnChange;
        }

        private Tween _currentTween;

        private void OnChange(float _, float newValue)
        {
            // Получаем текущего персонажа игрока
            var current = Configuration.Instance.AllCharacters
                .FirstOrDefault(c => GBGames.saves.characterSaves.IsCurrent(c.ID));

            // Проверяем, есть ли персонажи, которые соответствуют условиям
            var character = Configuration.Instance.AllCharacters
                .Where(c => c.Price <= newValue && !GBGames.saves.characterSaves.Unlocked(c.ID))
                .OrderByDescending(c => c.Booster)
                .FirstOrDefault(c => c.Booster > current.Booster);

            // Включаем или отключаем уведомление
            var state = character != null;

            notification.gameObject.SetActive(state);
            if (state)
            {
                _currentTween = notification.DOScale(1.25f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                _currentTween.Kill();
                _currentTween = null;
            }
        }
    }
}