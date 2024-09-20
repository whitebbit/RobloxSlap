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

        [SerializeField] private Image selectImage;
        [SerializeField] private Image selectedImage;
        
        [Tab("Localization")] 
        
        private Button _button;
        public ShopItem Data { get; protected set; }

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        public abstract void SetView(ShopItem item);
        
        public void SetAction(Action action) => _button.onClick.AddListener(() => action?.Invoke());

        public void Select()
        {
            selectedImage.gameObject.SetActive(true);
            selectImage.gameObject.SetActive(false);
            price.gameObject.SetActive(false);

        }

        public void Unselect()
        {
            selectedImage.gameObject.SetActive(false);
            price.gameObject.SetActive(false);
            selectImage.gameObject.SetActive(true);
        }

        public void Lock()
        {
            price.text = $"<sprite index=0>{WalletManager.ConvertToWallet((decimal) Data.Price)}";
            selectedImage.gameObject.SetActive(false);
            selectImage.gameObject.SetActive(false);
        }
    }
}