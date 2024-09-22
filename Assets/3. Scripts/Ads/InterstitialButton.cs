using System;
using _3._Scripts.Config;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.Ads
{
    public class InterstitialButton : MonoBehaviour
    {
        private void Awake()
        {
          
            GetComponent<Button>().onClick.AddListener(GBGames.ShowInterstitial);
        }
    }
}