using System.Linq;
using _3._Scripts.UI.Scriptable.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Elements.ShopSlots
{
    public class CharacterShopSlot : BaseShopSlot
    {
        [Tab("UI")] 
        [SerializeField] private TMP_Text title;
        [SerializeField] private Image glow;
        [SerializeField] private RawImage icon;
        [SerializeField] private Image table;
        [SerializeField] private Image backGlow;

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
    }
}