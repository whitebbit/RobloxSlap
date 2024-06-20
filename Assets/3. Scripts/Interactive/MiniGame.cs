using System;
using _3._Scripts.Enemies;
using _3._Scripts.Enemies.Scriptable;
using _3._Scripts.Interactive.Interfaces;
using _3._Scripts.MiniGame;
using _3._Scripts.UI;
using _3._Scripts.UI.Panels;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Interactive
{
    public class MiniGame : MonoBehaviour, IInteractive
    {
        [Tab("Fight components")]
        [SerializeField] private EnemyData enemyData;
        [SerializeField] private Enemy enemyPrefab;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [Tab("Transforms")] 
        [SerializeField] private Transform playerPoint;
        [SerializeField] private Transform enemyPoint;
        [SerializeField] private Transform useTutorialObject;
        private Fighter _enemy;
        
        private bool _fightStarted;

        private void Awake()
        {
            var enemy = Instantiate(enemyPrefab, transform);
            
            enemy.transform.localPosition = enemyPoint.localPosition;
            enemy.Initialize(enemyData);
            
            _enemy = enemy;
        }

        private void Start()
        {
            useTutorialObject.gameObject.SetActive(false);
        }

        public void StartInteract()
        {
            useTutorialObject.gameObject.SetActive(true);
        }

        public void Interact()
        {
            if (_fightStarted) return;

            var panel = UIManager.Instance.GetPanel<MiniGamePanel>();
            var player = Player.Player.instance;

            panel.Enabled = true;
            panel.StartMiniGame(Player.Player.instance, _enemy, enemyData.RewardCount, EndFight);

            useTutorialObject.gameObject.SetActive(false);

            player.PetsHandler.SetState(false);
            player.Teleport(playerPoint.position);
            player.transform.DOLookAt(_enemy.transform.position, 0, AxisConstraint.Y);

            CameraController.Instance.SwapTo(virtualCamera);

            _fightStarted = true;
            
            Player.Player.instance.OnStart();
            _enemy.OnStart();
        }

        private void EndFight()
        {
            _fightStarted = false;

            Player.Player.instance.PetsHandler.SetState(true);
            CameraController.Instance.SwapToMain();
            UIManager.Instance.GetPanel<MiniGamePanel>().Enabled = false;
            useTutorialObject.gameObject.SetActive(true);
            
            Player.Player.instance.OnEnd();
            _enemy.OnEnd();
        }

        public void StopInteract()
        {
            useTutorialObject.gameObject.SetActive(false);
        }
    }
}