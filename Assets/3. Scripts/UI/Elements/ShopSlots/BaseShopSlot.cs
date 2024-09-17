using System;
using System.Collections.Generic;
using _3._Scripts.Localization;
using _3._Scripts.UI.Scriptable.Shop;
using _3._Scripts.UI.Structs;
using _3._Scripts.Wallet;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Elements.ShopSlots
{
    public abstract class BaseShopSlot : MonoBehaviour
    { 
        [Tab("Rarity Tables")] 
        [SerializeField] protected List<RarityTable> rarityTables = new();
        [Tab("Currency")] 
        [SerializeField] private TMP_Text price;
        [Tab("Localization")] 
        [SerializeField] private string selectKey;
        [SerializeField] private string selectedKey;
        
        private Button _button;
        public ShopItem Data { get; protected set; }

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        public abstract void SetView(ShopItem item);
        
        public void SetAction(Action action) => _button.onClick.AddListener(() => action?.Invoke());

        public async void Select()
        {
            price.text = await selectedKey.GetTranslate();
        }

        public async void Unselect()
        {
            price.text = await selectKey.GetTranslate();
        }

        public void Lock()
        {
            price.text = $"<sprite index=0>{WalletManager.ConvertToWallet((decimal) Data.Price)}";
        }
    }
}