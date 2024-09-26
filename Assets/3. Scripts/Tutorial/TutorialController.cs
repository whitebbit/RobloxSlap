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
            TutorialSystem.StepStart("training");
        }

        private void OnEnable()
        {
            WalletManager.OnFirstCurrencyChange += (_, f1) =>
            {
                if (f1 >= 120)
                {
                    TutorialSystem.StepComplete("training");
                    TutorialSystem.StepStart("fight");
                }
                else if (f1 >= 5000)
                {
                    TutorialSystem.StepStart("portal");
                }
            };
        }
    }
}