using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GBGamesPlugin
{
    [CreateAssetMenu(menuName = "GBGamesSettings", fileName = "GBGamesSettings")]
    public class GBGamesSettings : ScriptableObject
    {
        [Header("Editor")]
        [Tooltip("Вывод отладочных сообщений в консоль")]
        public bool debug = true;

        [Header("Advertisement")] 
        public float intervalAfterInterstitial;
        public float intervalAfterReward;

        
        [Header("Storage")]
        [Tooltip("Использовать переодическое сохранение")]
        public bool autoSaveByInterval = true;
        [ConditionallyVisible(nameof(autoSaveByInterval)), Tooltip("Интервал переодических сохранений в минутах")]
        public int saveInterval;
        [Tooltip("Использовать переодическое сохранение")]
        public bool saveOnChangeVisibilityState = true;
    }
}