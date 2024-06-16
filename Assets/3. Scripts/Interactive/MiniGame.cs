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
        [SerializeField] private float rewardCount;

        [Tab("Fight components")] [SerializeField]
        private CinemachineVirtualCamera virtualCamera;

        [SerializeField] private Fighter enemy;
        [Tab("Transforms")] [SerializeField] private Transform playerPoint;
        [SerializeField] private Transform useTutorialObject;


        private bool _fightStarted;

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
            panel.StartMiniGame(Player.Player.instance, enemy, rewardCount, EndFight);

            useTutorialObject.gameObject.SetActive(false);

            player.PetsHandler.SetState(false);
            player.Teleport(playerPoint.position);
            player.transform.DOLookAt(enemy.transform.position, 0, AxisConstraint.Y);

            CameraController.Instance.SwapTo(virtualCamera);

            _fightStarted = true;
        }

        private void EndFight()
        {
            _fightStarted = false;

            Player.Player.instance.PetsHandler.SetState(true);
            CameraController.Instance.SwapToMain();
            UIManager.Instance.GetPanel<MiniGamePanel>().Enabled = false;
            useTutorialObject.gameObject.SetActive(true);
        }

        public void StopInteract()
        {
            useTutorialObject.gameObject.SetActive(false);
        }
    }
}