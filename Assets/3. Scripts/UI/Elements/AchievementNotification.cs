using DG.Tweening;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.UI.Elements
{
    public class AchievementNotification : MonoBehaviour
    {
        [SerializeField] private Transform notification;

        private void OnEnable()
        {
            GBGames.saves.achievementSaves.OnAchievementComplete += SetState;
        }

        private void OnDisable()
        {
            GBGames.saves.achievementSaves.OnAchievementComplete -= SetState;
        }

        private void SetState()
        {
            if (notification.gameObject.activeSelf) return;
            
            notification.gameObject.SetActive(true);
            notification.DOScale(1.25f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        }
    }
}