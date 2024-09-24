using _3._Scripts.Config;
using _3._Scripts.Localization;
using _3._Scripts.UI.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace _3._Scripts.UI.Panels
{
    public class OfferPanel : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private LocalizeStringEvent titleText;
        [SerializeField] private LocalizeStringEvent rarityText;

        public void SetOffer<T>(T panel, Sprite sprite) where T : UIPanel
        {
            Clear();
            SetVariables(panel, sprite);
        }

        public void SetIconColor(Color color) => icon.color = color;

        public void SetRarity(Rarity rarity)
        {
            var rarityTable = Configuration.Instance.GetRarityTable(rarity);
            rarityText.SetReference(rarityTable.TitleID);
            rarityText.gameObject.SetActive(true);
        }

        private void SetVariables<T>(T panel, Sprite sprite) where T : UIPanel
        {
            icon.sprite = sprite;
            titleText.SetReference(panel switch
            {
                PetsPanel => "offer_pet",
                UpgradesShop => "offer_upgrade",
                TrailsShop => "offer_trail",
                _ => ""
            });
        }

        private void Clear()
        {
            rarityText.gameObject.SetActive(false);
            icon.color = Color.white;
        }
    }
}