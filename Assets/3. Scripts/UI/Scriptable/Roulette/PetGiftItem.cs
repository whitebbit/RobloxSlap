﻿using _3._Scripts.Pets;
using _3._Scripts.Pets.Scriptables;
using _3._Scripts.Saves;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.UI.Scriptable.Roulette
{
    [CreateAssetMenu(fileName = "PetGiftItem", menuName = "ScriptableObjects/RouletteItem/Pet Gift Item", order = 0)]
    public class PetGiftItem : GiftItem
    {
        [SerializeField] private PetData data;

        public override Sprite Icon()
        {
            return data.Icon;
        }

        public override string Title()
        {
            return "";
        }

        public override void OnReward()
        {
            var maxBooster = GBGames.saves.petsSave.GetMaxBooster();
            GBGames.saves.petsSave.Unlock(data, Random.Range(maxBooster, maxBooster + 10));
            PetUnlocker.SelectBest();
        }
    }
}