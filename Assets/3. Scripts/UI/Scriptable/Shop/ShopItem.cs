using System;
using _3._Scripts.Currency.Enums;
using _3._Scripts.UI.Enums;
using UnityEngine;
using VInspector;

namespace _3._Scripts.UI.Scriptable.Shop
{
    public abstract class ShopItem : ScriptableObject
    {
        [Tab("Shop Item")] [SerializeField] private string id;
        [Header("UI")] [SerializeField] private Sprite icon;
        [SerializeField] private Rarity rarity;
        [Header("Currency")] [SerializeField] private CurrencyType currencyType;
        [SerializeField] private float price;


        public abstract string Title();
        public virtual string ID => id;
        public virtual Sprite Icon => icon;
        public virtual Rarity Rarity => rarity;

        public CurrencyType CurrencyType => currencyType;
        public float Price => price;
    }
}