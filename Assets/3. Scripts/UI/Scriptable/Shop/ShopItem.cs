using System;
using _3._Scripts.Currency.Enums;
using _3._Scripts.UI.Enums;
using GBGamesPlugin;
using UnityEngine;
using VInspector;

namespace _3._Scripts.UI.Scriptable.Shop
{
    public abstract class ShopItem : ScriptableObject
    {
        [Tab("Shop Item")] [SerializeField] private string id;
        [Header("UI")] [SerializeField] private Sprite icon;
        [SerializeField] private Rarity rarity;
        [Header("Purchase")] 
        [SerializeField] private PurchaseType purchaseType;
        [SerializeField] private float price;
        [SerializeField] private CurrencyType currencyType;

        public abstract string Title();
        public string ID => id;
        public Sprite Icon => icon;
        public Rarity Rarity => rarity;
        public PurchaseType PurchaseType => purchaseType;
        public CurrencyType CurrencyType => currencyType;
        public float Price => price;
    }
}