using _3._Scripts.Ads;
using _3._Scripts.Boosters;
using _3._Scripts.Detectors;
using _3._Scripts.Inputs;
using _3._Scripts.Interactive.Interfaces;
using _3._Scripts.UI;
using UnityEngine;

namespace _3._Scripts.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private BaseDetector<IInteractive> detector;
        private IInteractive _interactive;

        private void Start()
        {
            detector.OnFound += SetInteractive;
        }

        private void Update()
        {
            if (_interactive != null &&
                (InputHandler.Instance.Input.GetInteract() || InputHandler.Instance.Input.GetAction()) &&
                !UIManager.Instance.Active && !InterstitialsTimer.Instance.Active)
            {
                _interactive.Interact();
            }
        }

        private void SetInteractive(IInteractive newInteractive)
        {
            if (_interactive == newInteractive) return;

            _interactive?.StopInteract();

            _interactive = newInteractive;

            _interactive?.StartInteract();

            ShowPromotion(newInteractive);
        }

        private void ShowPromotion(IInteractive newInteractive)
        {
            if (newInteractive is Interactive.MiniGame)
            {
                BoostersHandler.Instance.RewardAdBooster.ShowPromotion(30);
            }
        }
    }
}