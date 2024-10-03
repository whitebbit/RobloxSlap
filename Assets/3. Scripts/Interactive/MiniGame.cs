using System;
using _3._Scripts.Enemies;
using _3._Scripts.Enemies.Scriptable;
using _3._Scripts.Inputs;
using _3._Scripts.Inputs.Enums;
using _3._Scripts.Interactive.Interfaces;
using _3._Scripts.MiniGame;
using _3._Scripts.Tutorial;
using _3._Scripts.UI;
using _3._Scripts.UI.Panels;
using Cinemachine;
using DG.Tweening;
using GBGamesPlugin;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Interactive
{
    public class MiniGame : MonoBehaviour, IInteractive
    {
        [Tab("Fight components")] [SerializeField]
        private Enemy enemyPrefab;

        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [Tab("Transforms")] [SerializeField] private Transform playerPoint;
        [SerializeField] private Transform enemyPoint;

        public EnemyData EnemyData { get; private set; }
        private Fighter _enemy;
        private bool _fightStarted;

        public void Initialize(EnemyData data)
        {
            if (_enemy != null) return;

            var enemy = Instantiate(enemyPrefab, transform);

            EnemyData = data;

            enemy.transform.localPosition = enemyPoint.localPosition;
            enemy.Initialize(EnemyData);

            _enemy = enemy;
        }

        public void StartInteract()
        {
            if (_fightStarted) return;

            InputHandler.Instance.SetActionButtonType(ActionButtonType.Fight);
        }

        public void Interact()
        {
            if (_fightStarted) return;

            _fightStarted = true;

            var panel = UIManager.Instance.GetPanel<MiniGamePanel>();
            var player = Player.Player.instance;

            panel.Enabled = true;
            panel.StartMiniGame(Player.Player.instance, _enemy, EnemyData.RewardCount, EndFight);

            player.PetsHandler.SetState(false);
            player.Teleport(playerPoint.position);
            player.transform.DOLookAt(_enemy.transform.position, 0, AxisConstraint.Y);

            CameraController.Instance.SwapTo(virtualCamera);
            TutorialSystem.StepComplete("fight");
            
            GBGames.saves.bossFightsCount += 1;
            GBGames.ReportBossEvent();

            Player.Player.instance.OnStart();
            _enemy.OnStart();
            
        }

        private void EndFight()
        {
            _fightStarted = false;

            Player.Player.instance.PetsHandler.SetState(true);
            CameraController.Instance.SwapToMain();
            UIManager.Instance.GetPanel<MiniGamePanel>().Enabled = false;
            Player.Player.instance.OnEnd();

            _enemy.OnEnd();
        }

        public void StopInteract()
        {
            InputHandler.Instance.SetActionButtonType(ActionButtonType.Training);
        }
    }
}