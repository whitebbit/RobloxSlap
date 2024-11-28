using System;
using _3._Scripts.Singleton;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.Tutorial
{
    public static class TutorialSystem
    {
        public static event Action<string> TutorialStepStart;
        public static event Action<string> TutorialStepComplete;

        public static void StepComplete(string stepName)
        {
            if (GBGames.saves.tutorialStates.ContainsKey(stepName)) return;

            TutorialStepComplete?.Invoke(stepName);

            GBGames.saves.tutorialStates.TryAdd(stepName, true);
        }

        public static void StepStart(string stepName)
        {
            if (GBGames.saves.tutorialStates.ContainsKey(stepName)) return;
            TutorialStepStart?.Invoke(stepName);
        }
    }
}