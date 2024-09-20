using System;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        public static event Action GameVisibleStateCallback;
        public static event Action GameHiddenStateCallback;
        
        private static void OnGameVisibilityStateChanged()
        {
            /*switch (state)
            {
                case VisibilityState.Visible:
                    Message("Visibility state - Visible");
                    GameVisibleStateCallback?.Invoke();
                    GameplayStarted();
                    break;
                case VisibilityState.Hidden:
                    Message("Visibility state - Hidden");
                    GameHiddenStateCallback?.Invoke();
                    GameplayStopped();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }*/
        }
    }
}