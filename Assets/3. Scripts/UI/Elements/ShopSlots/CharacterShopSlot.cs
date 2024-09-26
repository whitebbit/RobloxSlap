using System;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.UI.Scriptable.Shop;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Elements.ShopSlots
{
    public class CharacterShopSlot : BaseShopSlot
    {
        [Tab("UI")] [SerializeField] private TMP_Text title;
        [SerializeField] private Image glow;
        [SerializeField] private RawImage icon;
        [SerializeField] private Image table;
        [SerializeField] private Image backGlow;
        [SerializeField] private Image getByReward;

        public override void SetView(ShopItem item)
        {
            var rarity = rarityTables.FirstOrDefault(r => r.Rarity == item.Rarity);
            if (item is CharacterItem characterData)
            {
                var characterImage = RuntimeSkinIconRenderer.Instance.GetTexture2D(item.ID, characterData.Skin);
                icon.texture = characterImage;
            }

            table.sprite = rarity.Table;
            glow.color = rarity.MainColor;
            backGlow.color = rarity.AdditionalColor;
            title.text = item.Title();
            Data = item;
        }

        private void OnEnable()
        {
            if (Data is not CharacterItem characterData) return;
            
            var characterImage = RuntimeSkinIconRenderer.Instance.GetTexture2D(Data.ID, characterData.Skin);
            icon.texture = characterImage;

        }

        public override void Select()
        {
            base.Select();
            getByReward.gameObject.SetActive(false);

        }

        public override void Unselect()
        {
            base.Unselect();
            getByReward.gameObject.SetActive(false);

        }

        public override void Lock()
        {
            getByReward.gameObject.SetActive(ItsRewardSkin());
            price.gameObject.SetActive(!ItsRewardSkin());

            price.text = $"<sprite index=0> {WalletManager.ConvertToWallet((decimal) Data.Price)}";

            selectedImage.gameObject.SetActive(false);
            selectImage.gameObject.SetActive(false);
        }

        public bool ItsRewardSkin()
        {
            var list = Configuration.Instance.AllCharacters.ToList();
            var unlockedSkins = list.Where(c => GBGames.saves.characterSaves.Unlocked(c.ID))
                .OrderByDescending(c => c.Booster) // Sort by Booster value
                .ToList();
            
            var bestSkin = unlockedSkins.First(); // The highest Booster skin
            var currentIndex = list.IndexOf(bestSkin);

            // Ensure the target skin exists in the list
            if (currentIndex < 0 || currentIndex + 2 >= list.Count) return false;

            var targetSkin = list[currentIndex + 2]; // Get the second skin after the best one

            return targetSkin.ID == Data.ID;
        }
    }
}