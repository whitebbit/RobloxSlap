using System;
using System.Collections.Generic;

namespace _3._Scripts.Saves
{
    [Serializable]
    public class DailyRewardSave
    {
        public DateTime lastLoginDate= DateTime.MinValue;
        public List<int> claimedRewards= new();
        public int currentStreak = 1;
    }
}