using CAS;
using GBGamesPlugin.Enums;

namespace GBGamesPlugin
{
    public class AdEventParameters
    {
        public AdType adType;
        public AdEventPlacement adPlacement;

        public AdEventParameters(AdEventPlacement adPlacement, AdType adType)
        {
            this.adPlacement = adPlacement;
            this.adType = adType;
        }
    }
}