using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Stages;
using _3._Scripts.UI.Elements;
using _3._Scripts.UI.Panels.Base;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _3._Scripts.UI.Panels
{
    public class AutoFightPanel : SimplePanel
    {
        [SerializeField] private Transform container;
        [SerializeField] private EnemySelectButton enemySelectButtonPrefab;

        private readonly List<EnemySelectButton> _enemySelectButtons = new();


        protected override void OnOpen()
        {
            base.OnOpen();
            InitializeButtons();
        }

        protected override void OnClose()
        {
            ClearButtons();
            base.OnClose();
        }

        public void AddListenersToButtons(UnityAction action)
        {
            foreach (var button in _enemySelectButtons)
            {
                button.AddListener(action);
            }
        }

        private void InitializeButtons()
        {
            var enemies = StageController.Instance.CurrentStage.EnemyData.OrderBy(e => e.ComplexityType).ToList();
            for (var i = 0; i < StageController.Instance.CurrentStage.MiniGames.Count; i++)
            {
                var button = Instantiate(enemySelectButtonPrefab, container);
                button.Initialize(enemies[i]);
                _enemySelectButtons.Add(button);
            }
        }

        private void ClearButtons()
        {
            foreach (var child in _enemySelectButtons)
            {
                Destroy(child.gameObject);
            }

            _enemySelectButtons.Clear();
        }
    }
}