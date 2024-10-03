using System;
using _3._Scripts.Currency.Enums;
using _3._Scripts.UI.Enums;
using _3._Scripts.UI.Scriptable.Roulette;
using CAS;
using GBGamesPlugin.Enums;

namespace Y._Imported_Assets.GBGamesPlugin.Scripts.Analytics.Utils
{
    public static class EventStringExtensions
    {
        public static string GetString(this AdEventType adEventType)
        {
            return adEventType switch
            {
                AdEventType.VideoAdsAvailable => "video_ads_available",
                AdEventType.VideoAdsStarted => "video_ads_started",
                AdEventType.VideoAdsSuccess => "video_ads_success",
                _ => throw new ArgumentOutOfRangeException(nameof(adEventType), adEventType, null)
            };
        }
        
        public static string GetString(this AdType adType)
        {
            return adType switch
            {
                AdType.Banner => "banner",
                AdType.Interstitial => "interstitial",
                AdType.Rewarded => "rewarded",
                AdType.AppOpen => "app_open",
                AdType.Native => "native",
                AdType.None => "None",
                _ => throw new ArgumentOutOfRangeException(nameof(adType), adType, null)
            };
        }
        
        public static string GetString(this AdEventPlacement adPlacement)
        {
            return adPlacement switch
            {
                AdEventPlacement.InAds => "in_ads123",
                AdEventPlacement.ADPalmX2 => "ad_palm_x2",
                AdEventPlacement.ADSpeedX2 => "ad_speed_x2",
                AdEventPlacement.ADRewardX2 => "ad_reward_x2",
                AdEventPlacement.ADOfferPet => "ad_offer_pet",
                AdEventPlacement.ADGetRandomPet => "ad_get_random_pet",
                AdEventPlacement.ADMenuCharacter => "ad_menu_character",
                AdEventPlacement.ADLocationCharacter => "ad_location_character",
                AdEventPlacement.ADOfferTrail => "ad_offer_trail",
                AdEventPlacement.ADOfferHand => "ad_offer_hand",
                _ => throw new ArgumentOutOfRangeException(nameof(adPlacement), adPlacement, null)
            };
        }
        public static string GetString(this AdEventResult result)
        {
            return result switch
            {
                AdEventResult.Success => "success",
                AdEventResult.NotAvailable => "not_available",
                AdEventResult.Waited => "waited",
                AdEventResult.Start => "start",
                AdEventResult.Failed => "failed",
                AdEventResult.Canceled => "canceled",
                AdEventResult.Watched => "watched",
                AdEventResult.Clicked => "clicked",
                _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
            };
        }
        
        public static string GetString(this LevelEventType type)
        {
            return type switch
            {
                LevelEventType.LevelStart => "level_start",
                LevelEventType.LevelFinish => "level_finish",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        
        public static string GetString(this Rarity rarity)
        {
            return rarity switch
            {
                Rarity.Rare => "rare",
                Rarity.Mythical => "mythical",
                Rarity.Legendary => "legendary",
                _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
            };
        }
        
        public static string GetString(this PurchaseType type)
        {
            return type switch
            {
                PurchaseType.Currency => "currency",
                PurchaseType.Ad => "ads",
                _ => throw new ArgumentOutOfRangeException(nameof(type),type, null)
            };
        }
        public static string GetString(this GiftOpenType type)
        {
            return type switch
            {
                GiftOpenType.UI => "ui_button",
                GiftOpenType.Location => "location_use",
                _ => throw new ArgumentOutOfRangeException(nameof(type),type, null)
            };
        }
        
        public static string GetString(this GiftItem gift)
        {
            switch (gift)
            {
                case CurrencyGiftItem currencyGiftItem:
                {
                    var currency = currencyGiftItem.Type == CurrencyType.First ? "strength" : "cups";
                    return $"{currency}_x{currencyGiftItem.Count}";
                }
                case PetGiftItem petGift:
                    return $"pet_{petGift.ID}"; 
            }

            return "";
        }
    }
}