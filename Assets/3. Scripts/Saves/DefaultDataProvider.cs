﻿using System;
using _3._Scripts.Singleton;
using _3._Scripts.UI.Scriptable.Shop;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.Saves
{
    public class DefaultDataProvider : Singleton<DefaultDataProvider>
    {
        [SerializeField] private CharacterItem defaultCharacter;
        [SerializeField] private TrailItem defaultTrail;
        [SerializeField] private UpgradeItem defaultUpgrade;
        

        public void SetDefault()
        {
            if (GBGames.saves.defaultLoaded) return;
            
            SetPlayerDefaultData();

            GBGames.saves.defaultLoaded = true;
            GBGames.instance.Save();
        }

        public void SetPlayerDefaultData()
        {
            GBGames.saves.characterSaves.current = defaultCharacter.ID;
            GBGames.saves.characterSaves.Unlock(defaultCharacter.ID);

            GBGames.saves.trailSaves.current = defaultTrail.ID;
            GBGames.saves.trailSaves.Unlock(defaultTrail.ID);

            GBGames.saves.upgradeSaves.current = defaultUpgrade.ID;
            GBGames.saves.upgradeSaves.Unlock(defaultUpgrade.ID);
        }
    }
}