using System;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Singleton;
using GBGamesPlugin;
using TMPro;
using UnityEngine;

namespace _3._Scripts.Wallet
{
    public class HealthManager : Singleton<HealthManager>
    {
        [SerializeField] private TMP_Text text;

        private void Start()
        {
            ChangeValue();
        }

        public void ChangeValue()
        {
            var health = Configuration.Instance.AllCharacters.FirstOrDefault(
                h => h.ID == GBGames.saves.characterSaves.current).Booster;
            text.text = WalletManager.ConvertToWallet((decimal) health);
        }
    }
}