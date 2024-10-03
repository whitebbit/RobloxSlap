using System;
using System.Linq;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.Tutorial
{
    public class TutorialController : MonoBehaviour
    {
        private void Start()
        {
            TutorialSystem.StepStart("01_training");
        }

        private void OnEnable()
        {
            WalletManager.OnFirstCurrencyChange += (_, f1) =>
            {
                if (f1 >= 120)
                {
                    TutorialSystem.StepComplete("01_training");
                    TutorialSystem.StepStart("02_fight");
                }
                else if (f1 >= 5000)
                {
                    TutorialSystem.StepStart("03_portal");
                }
            };
        }
    }
}