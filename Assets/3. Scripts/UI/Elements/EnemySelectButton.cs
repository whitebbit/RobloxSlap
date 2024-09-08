﻿using System;
using System.Linq;
using _3._Scripts.Boosters;
using _3._Scripts.Enemies.Scriptable;
using _3._Scripts.Stages;
using _3._Scripts.UI.Panels;
using _3._Scripts.Wallet;
using TMPro;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace _3._Scripts.UI.Elements
{
    public class EnemySelectButton : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text strengthText;
        [SerializeField] private LocalizeStringEvent complexityText;

        private EnemyData _data;
        [SerializeField] private Button button;
        
        public void AddListener(UnityAction action) => button.onClick.AddListener(action);

        public void Initialize(EnemyData data)
        {
            _data = data;
            icon.sprite = data.Icon;
            strengthText.text = $"{WalletManager.ConvertToWallet((decimal) (data.Health / 25))} <sprite index=1>";
            complexityText.TextToComplexity(data.ComplexityType);
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            var miniGame =
                StageController.Instance.CurrentStage.MiniGames.FirstOrDefault(m =>
                    m.EnemyData.ComplexityType == _data.ComplexityType);

            if (miniGame == null) return;
            
            BoostersHandler.Instance.CurrentMiniGame = miniGame;
            miniGame.Interact();
            UIManager.Instance.GetPanel<AutoFightPanel>().Enabled = false;
        }
    }
}