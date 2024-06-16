using System;
using System.Linq;
using _3._Scripts.Boosters;
using _3._Scripts.Config;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Player;
using _3._Scripts.UI.Panels;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.Income
{
    public class IncomeController : MonoBehaviour
    {
        //[SerializeField] private CurrencyEffectPanel effectPanel;

        private void Awake()
        {
           // GetComponent<PlayerAction>().Action += DoIncome;
        }

        private void DoIncome()
        {
            //var obj = effectPanel.SpawnEffect();
            var income = GetIncome();

           // obj.Initialize(CurrencyType.First, income);
            WalletManager.FirstCurrency += GetIncome();
        }

        private float GetIncome()
        {
            var character =
                Configuration.Instance.AllCharacters.FirstOrDefault(c => GBGames.saves.characterSaves.IsCurrent(c.ID));
            var booster = BoostersHandler.Instance.X2Income ? 2 : 1;
            return (character.Booster) * booster;
        }
    }
}