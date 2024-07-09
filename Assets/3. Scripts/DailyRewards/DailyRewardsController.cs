using System;
using System.Collections.Generic;
using _3._Scripts.Singleton;
using _3._Scripts.UI.Scriptable.Roulette;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.DailyRewards
{
    public class DailyRewardsController : Singleton<DailyRewardsController>
    {
        [SerializeField] private List<GiftItem> rewards = new();

        public List<GiftItem> Rewards => rewards;

        private void Start()
        {
            CheckDailyLogin();
        }

        private void CheckDailyLogin()
        {
            var today = DateTime.Today;
            var data = GBGames.saves.dailyReward;

            if (data.lastLoginDate == today.AddDays(-1))
            {
                data.currentStreak++;

                if (data.currentStreak > 7)
                {
                    data.currentStreak = 1;
                    data.claimedRewards.Clear();
                }
            }
            else
            {
                data.currentStreak = 1;
                data.claimedRewards.Clear();
            }

            data.lastLoginDate = today;
        }


        public void ClaimReward()
        {
            var data = GBGames.saves.dailyReward;
            var totalRewardsToClaim = data.currentStreak;

            for (var i = 0; i < totalRewardsToClaim; i++)
            {
                if (Claimed(i)) continue;
                var reward = rewards[i % rewards.Count];
                reward.OnReward();
                data.claimedRewards.Add(i);
            }
        }


        public bool Claimed(int dayIndex)
        {
            var data = GBGames.saves.dailyReward;
            return data.claimedRewards.Contains(dayIndex );
        }
    }
}