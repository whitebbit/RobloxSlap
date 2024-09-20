using System;
using System.Collections;
using System.Collections.Generic;
using _3._Scripts.Singleton;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.Leaderboard
{
    public class Leaderboard : Singleton<Leaderboard>
    {
        [SerializeField] private string leaderboardName;
        [Space] [SerializeField] private List<LeaderboardEntryView> leaderboardEntryViews = new();

        private void Awake()
        {
            gameObject.SetActive(false);

            
        }
        
        public void UpdateScore(int score)
        {
            
        }
        
    }
}