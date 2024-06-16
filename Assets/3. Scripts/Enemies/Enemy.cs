using System;
using System.Collections;
using _3._Scripts.Enemies.Scriptable;
using _3._Scripts.Localization;
using _3._Scripts.MiniGame;
using _3._Scripts.MiniGame.Interfaces;
using _3._Scripts.Player;
using _3._Scripts.Wallet;
using UnityEngine;
using UnityEngine.Localization.Components;
using VInspector;

namespace _3._Scripts.Enemies
{
    public class Enemy : Fighter
    {
        [Tab("Components")]
        [SerializeField] private EnemyData data;
        [SerializeField, Min(1)] private float attackSpeed;
        [SerializeField] private PlayerAnimator animator;
        [Tab("Texts")] [SerializeField] private LocalizeStringEvent nameText;
        [SerializeField] private LocalizeStringEvent complexityText;
        [SerializeField] private LocalizeStringEvent recommendationText;

        public EnemyData Data => data;
        private FighterData _fighterData;

        private void Start()
        {
            animator.SetGrounded(true);
            animator.SetSpeed(0);

            _fighterData = new FighterData
            {
                health = data.Health,
                strength = data.Strength,
                photo = data.Icon
            };
            
            InitializeText();
        }

        public override void StartFight()
        {
            base.StartFight();
            StartCoroutine(SlapCoroutine());
        }

        public override void EndFight(bool win)
        {
            base.EndFight(win);
            StopAllCoroutines();
        }

        public override FighterData FighterData()
        {
            return _fighterData;
        }

        protected override PlayerAnimator Animator()
        {
            return animator;
        }

        private IEnumerator SlapCoroutine()
        {
            while (isFight)
            {
                Slap();
                yield return new WaitForSeconds(attackSpeed);
            }
        }

        private void InitializeText()
        {
            nameText.SetReference(data.LocalizationID);
            complexityText.TextToComplexity(data.ComplexityType);
            recommendationText.SetVariable("value", WalletManager.ConvertToWallet((decimal) (data.Health / 25)));
        }
    }
}