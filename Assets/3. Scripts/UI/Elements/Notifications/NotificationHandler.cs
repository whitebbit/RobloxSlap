using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.FSM.Base;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.UI.Elements.Notifications
{
    public class NotificationHandler : MonoBehaviour
    {
        [SerializeField] private List<NotificationItem> notificationItems = new();

        private readonly List<BaseNotification> _notifications = new();
        private float _timeToCheck = 5;

        private void Start()
        {
            _notifications.Add(new BaseNotification(GetNotificationObject("character"),
                new FuncPredicate(CharacterShopPredicate)));

            _notifications.Add(new BaseNotification(GetNotificationObject("upgrade"),
                new FuncPredicate(UpgradeShopPredicate)));
        }

        private void Update()
        {
            TryShowNotification();
        }

        public void HideNotification(string id)
        {
            var notification = _notifications.FirstOrDefault(n => n.ID == id);
            notification?.HideNotification();
        }

        private void TryShowNotification()
        {
            _timeToCheck -= Time.deltaTime;
            if (_timeToCheck > 0) return;

            _timeToCheck = 5;
            foreach (var notification in _notifications)
            {
                notification.ShowNotification();
            }
        }

        private static bool CharacterShopPredicate()
        {
            var unlockedSkins = Configuration.Instance.AllCharacters.Where(c => GBGames.saves.characterSaves.Unlocked(c.ID))
                .OrderByDescending(c => c.Booster) 
                .ToList();
            
            var bestSkin = unlockedSkins.First();
            
            if (bestSkin == null) return false;

            var character = Configuration.Instance.AllCharacters
                .Where(c => c.Price <= WalletManager.SecondCurrency && !GBGames.saves.characterSaves.Unlocked(c.ID))
                .OrderByDescending(c => c.Booster)
                .FirstOrDefault(c => c.Booster > bestSkin.Booster);

            return character != null;
        }

        private static bool UpgradeShopPredicate()
        {
            
            var unlocked = Configuration.Instance.AllUpgrades.Where(c => GBGames.saves.upgradeSaves.Unlocked(c.ID))
                .OrderByDescending(c => c.Booster) 
                .ToList();
            
            var best = unlocked.First();
            
            if (best == null) return false;

            var upgrade = Configuration.Instance.AllUpgrades
                .Where(c => c.Price <= WalletManager.SecondCurrency && !GBGames.saves.upgradeSaves.Unlocked(c.ID))
                .OrderByDescending(c => c.Booster)
                .FirstOrDefault(c => c.Booster > best.Booster);

            return upgrade != null;
        }


        private NotificationItem GetNotificationObject(string id)
        {
            return notificationItems.FirstOrDefault(n => n.NotificationID == id);
        }
    }
}