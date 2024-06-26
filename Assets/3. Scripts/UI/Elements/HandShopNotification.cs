using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.UI.Elements
{
    public class HandShopNotification : MonoBehaviour
    {
        [SerializeField] private Transform notification;

        private void OnEnable()
        {
            OnChange(WalletManager.SecondCurrency, WalletManager.SecondCurrency);
            WalletManager.OnSecondCurrencyChange += OnChange;
        }

        private void OnDisable()
        {
            WalletManager.OnSecondCurrencyChange -= OnChange;
        }

        private void OnChange(float _, float newValue)
        {
            var current =
                Configuration.Instance.AllUpgrades.FirstOrDefault(u => u.ID == GBGames.saves.upgradeSaves.current);

            var character = Configuration.Instance.AllUpgrades.FirstOrDefault(u =>
                current is not null && u.Price <= newValue && !GBGames.saves.upgradeSaves.Unlocked(u.ID) &&
                u.Price > current.Price);

            notification.gameObject.SetActive(character != null);
        }
    }
    
}