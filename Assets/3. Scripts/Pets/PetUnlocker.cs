﻿using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Interactive.Interfaces;
using _3._Scripts.Pets.Scriptables;
using _3._Scripts.UI;
using _3._Scripts.UI.Elements;
using _3._Scripts.UI.Panels;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using TMPro;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Pets
{
    public class PetUnlocker : MonoBehaviour, IInteractive
    {
        [SerializeField] private List<PetData> data = new();
        [Tab("Price Settings")]
        [SerializeField] private float price;
        [SerializeField] private TMP_Text priceText;
        
        [Tab("Interactive Settings")]
        [SerializeField] private Transform eggModel;
        [SerializeField] private Transform canvas;
        
        [Tab("UI Settings")]
        [SerializeField] private RectTransform content;
        [SerializeField] private PetUnlockerSlot slotPrefab;
        
        private void Start()
        {
            InitializeUI();
            PopulatePetSlots();
            SetInitialVisibility();
        }
        
        private void InitializeUI()
        {
            priceText.text = $"<sprite index=0>{WalletManager.ConvertToWallet((decimal) price)}";
        }

        private void PopulatePetSlots()
        {
            data.Sort((x, y) => y.DropPercent.CompareTo(x.DropPercent));
            foreach (var petData in data)
            {
                var petSlot = Instantiate(slotPrefab, content);
                petSlot.Initialize(petData.Icon, petData.DropPercent);
            }
        }

        private void SetInitialVisibility()
        {
            canvas.gameObject.SetActive(false);
            eggModel.gameObject.SetActive(true);
        }

        private PetData GetRandomPet()
        {
            var totalWeight = data.Sum(d => d.DropPercent);
            var randomValue = Random.Range(0, totalWeight);
            var cumulativeWeight = 0f;

            foreach (var petData in data)
            {
                cumulativeWeight += petData.DropPercent;
                if (randomValue <= cumulativeWeight)
                {
                    return petData;
                }
            }
            return null;
        }

        public void StartInteract()
        {
            eggModel.gameObject.SetActive(false);
            canvas.gameObject.SetActive(true);
        }

        public void Interact()
        {
            if (GBGames.saves.petsSave.MaxUnlocked(25) || !WalletManager.TrySpend(CurrencyType.Second, price))
                return;

            var panel = UIManager.Instance.GetPanel<PetUnlockerPanel>();
            if (panel.Enabled) return;

            panel.Enabled = true;
            panel.UnlockPet(GetRandomPet());
        }

        public void StopInteract()
        {
            eggModel.gameObject.SetActive(true);
            canvas.gameObject.SetActive(false);
        }
    }
}
