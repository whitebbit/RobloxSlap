using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Singleton;
using _3._Scripts.UI;
using _3._Scripts.UI.Panels;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.Boosters
{
    public class BoostersHandler : Singleton<BoostersHandler>
    {
        [Tab("Buttons")] [SerializeField] private BoosterButtonSwitcher autoClickerButton;
        [SerializeField] private AutoFightBooster autoFightBooster;
        [SerializeField] private AdBoosterButton rewardAdBooster;
        [SerializeField] private AdBoosterButton slapAdBooster;
        [SerializeField] private Transform slapBoosterView;
        [Tab("Debug")] [SerializeField] private List<BoosterState> boosters = new();

        public Interactive.MiniGame CurrentMiniGame { get; set; }

        private void ChangeBoosterState(string boosterName, bool state)
        {
            var booster = boosters.FirstOrDefault(b => b.name == boosterName);

            if (booster == null)
            {
                boosters.Add(new BoosterState
                {
                    name = boosterName,
                    state = state
                });
                return;
            }

            booster.state = state;
        }

        public bool GetBoosterState(string boosterName)
        {
            var booster = boosters.FirstOrDefault(b => b.name == boosterName);
            return booster?.state ?? false;
        }

        private void Start()
        {
            slapBoosterView.gameObject.SetActive(false);

            InitializeButtons();
        }

        private void InitializeButtons()
        {
            autoClickerButton.onActivateBooster += () => ChangeBoosterState("auto_clicker", true);
            autoClickerButton.onDeactivateBooster += () => ChangeBoosterState("auto_clicker", false);

            autoFightBooster.onActivateBooster += () =>
            {
                ChangeBoosterState("auto_fight", true);
                StartCoroutine(MiniGameCoroutine());
            };
            autoFightBooster.onDeactivateBooster += () =>
            {
                ChangeBoosterState("auto_fight", false);
                StopAllCoroutines();
            };
            
            rewardAdBooster.onActivateBooster += () => ChangeBoosterState("reward_booster", true);
            rewardAdBooster.onDeactivateBooster += () => ChangeBoosterState("reward_booster", false);

            slapAdBooster.onActivateBooster += () =>
            {
                slapBoosterView.gameObject.SetActive(true);
                ChangeBoosterState("slap_booster", true);
            };
            slapAdBooster.onDeactivateBooster += () =>
            {
                slapBoosterView.gameObject.SetActive(false);
                ChangeBoosterState("slap_booster", false);
            };
        }
        
        private IEnumerator MiniGameCoroutine()
        {
            while (GetBoosterState("auto_fight"))
            {
                yield return new WaitUntil(() => !UIManager.Instance.GetPanel<MiniGamePanel>().Enabled);
                yield return new WaitForSeconds(3);

                if (CurrentMiniGame == null) continue;
                if (!GetBoosterState("auto_fight")) break;
                
                CurrentMiniGame.Interact();
            }
        }
    }
}