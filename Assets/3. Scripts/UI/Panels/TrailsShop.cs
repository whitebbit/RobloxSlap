using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Currency.Enums;
using _3._Scripts.UI.Elements;
using _3._Scripts.UI.Elements.ShopSlots;
using _3._Scripts.UI.Interfaces;
using _3._Scripts.UI.Scriptable.Shop;
using _3._Scripts.Wallet;
using GBGamesPlugin;

namespace _3._Scripts.UI.Panels
{
    public class TrailsShop : ShopPanel<TrailItem, ShopSlot>, IOfferIndicator
    {
        protected override IEnumerable<TrailItem> ShopItems()
        {
            return Configuration.Instance.AllTrails.OrderBy(obj => obj.Price);
        }

        protected override bool ItemUnlocked(string id)
        {
            return GBGames.saves.trailSaves.Unlocked(id);
        }

        protected override bool IsSelected(string id)
        {
            return GBGames.saves.trailSaves.IsCurrent(id);
        }

        protected override bool Select(string id)
        {
            if (!ItemUnlocked(id)) return false;
            if (IsSelected(id)) return false;
            
            GBGames.saves.trailSaves.SetCurrent(id);
            GBGames.instance.Save();
            Player.Player.instance.TrailHandler.SetTrail(id);
            SetSlotsState();
            return true;
        }

        protected override bool Buy(string id)
        {
            if (ItemUnlocked(id)) return false;

            var slot = GetSlot(id).Data;

            if (!WalletManager.TrySpend(slot.CurrencyType, slot.Price)) return false;
            
            GBGames.saves.trailSaves.Unlock(id);
            Select(id);
            return true;
        }

        protected override void OnSpawnItems(ShopSlot slot, TrailItem data)
        {
            slot.SetIconColor(data.Color);
        }

        public void ShowOffer()
        {
            var panel = UIManager.Instance.GetPanel<OfferPanel>();
            var list = Configuration.Instance.AllTrails.ToList();
            var current = list.FirstOrDefault(u => GBGames.saves.trailSaves.IsCurrent(u.ID));
            var currentIndex = list.IndexOf(current);
            var nextItem = list[(currentIndex + 1) % list.Count];

            if(currentIndex + 1 >= list.Count ) return;

            panel.Enabled = true;
            
            panel.SetOffer(nextItem, () =>
            {
                var id = nextItem.ID;
                GBGames.saves.trailSaves.Unlock(id);
                GBGames.saves.trailSaves.SetCurrent(id);
                GBGames.instance.Save();
                Player.Player.instance.TrailHandler.SetTrail(id);
            });
            
            panel.SetRarity(nextItem.Rarity);
            panel.SetIconColor(nextItem.Color);
            panel.SetBoosterText(nextItem.Title());
        }
    }
}