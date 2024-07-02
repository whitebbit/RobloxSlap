using System;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Localization;
using _3._Scripts.Stages.Enums;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace _3._Scripts.Stages
{
    public class StagePortal : MonoBehaviour
    {
        [SerializeField] private TeleportType type;
        private float _teleportPrice;
        [SerializeField] private LocalizeStringEvent text;

        public TeleportType Type => type;

        public void SetPrice(float price)
        {
            _teleportPrice = price;
            switch (type)
            {
                case TeleportType.Next:
                    text.SetReference("teleport_price");
                    text.SetVariable("price", WalletManager.ConvertToWallet((decimal) _teleportPrice));
                    break;
                case TeleportType.Previous:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private void Start()
        {
            switch (type)
            {
                case TeleportType.Next:
                    text.SetReference("teleport_price");
                    text.SetVariable("price", WalletManager.ConvertToWallet((decimal) _teleportPrice));
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
                    
                    if (GBGames.saves.stageID > StageController.Instance.CurrentID)
                    {
                        StageController.Instance.TeleportToNextStage();
                        Debug.Log("1");
                    }
                    else
                    {
                        if (WalletManager.SecondCurrency >= _teleportPrice)
                        {
                            StageController.Instance.TeleportToNextStage();
                            Debug.Log("2");
                        }
                    }

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