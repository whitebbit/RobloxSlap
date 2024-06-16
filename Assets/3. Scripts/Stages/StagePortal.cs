using System;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Localization;
using _3._Scripts.Stages.Enums;
using _3._Scripts.Wallet;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace _3._Scripts.Stages
{
    public class StagePortal : MonoBehaviour
    {
        [SerializeField] private TeleportType type;
        [SerializeField] private float teleportPrice;
        [SerializeField] private LocalizeStringEvent text;

        private void Start()
        {
            switch (type)
            {
                case TeleportType.Next:
                    text.SetReference("teleport_price");
                    text.SetVariable("price", WalletManager.ConvertToWallet((decimal) teleportPrice));
                    break;
                case TeleportType.Previous:
                    text.SetReference("teleport_return");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
                
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Player.Player _)) return;

            switch (type)
            {
                case TeleportType.Next:
                    if (WalletManager.SecondCurrency < teleportPrice) return;

                    StageController.Instance.TeleportToNextStage();
                    break;
                case TeleportType.Previous:
                    StageController.Instance.TeleportToPreviousStage();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}