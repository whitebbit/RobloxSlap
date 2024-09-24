﻿using System;
using System.Linq;
using _3._Scripts.Actions;
using _3._Scripts.Actions.Interfaces;
using _3._Scripts.Ads;
using _3._Scripts.Boosters;
using _3._Scripts.Config;
using _3._Scripts.Detectors;
using _3._Scripts.Detectors.Interfaces;
using _3._Scripts.Inputs;
using _3._Scripts.Inputs.Interfaces;
using _3._Scripts.Interactive.Interfaces;
using _3._Scripts.Sounds;
using _3._Scripts.UI;
using _3._Scripts.UI.Scriptable.Shop;
using GBGamesPlugin;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Player
{
    public class PlayerAction : MonoBehaviour
    {
        [SerializeField] private float baseAnimationTime = 1;
        [SerializeField] private BaseDetector<IActionable> detector;

        private IInput _input;
        private bool _isOnCooldown;
        private float _cooldownTimer;
        private PlayerAnimator _animator;

        private IActionable _actionable;

        private void Awake()
        {
            _animator = GetComponent<PlayerAnimator>();
            detector.OnFound += DetectorOnFound;
        }

        private void DetectorOnFound(IActionable obj)
        {
            _actionable = obj;
            if (obj is not Training) return;

            BoostersHandler.Instance.TrainingAdBooster.ShowPromotion(30);
            BoostersHandler.Instance.AutoClickerAdBooster.ShowPromotion(30);
        }

        private void Start()
        {
            _input = InputHandler.Instance.Input;
            _animator.Event += AnimatorAction;
        }

        private float _timeToClick = 1;

        private void Update()
        {
            if (UIManager.Instance.Active || InterstitialsTimer.Instance.Active) return;
            if (_input.GetAction())
            {
                DoAction();
            }

            if (!BoostersHandler.Instance.GetBoosterState("auto_clicker")) return;

            _timeToClick -= Time.deltaTime;

            if (!(_timeToClick <= 0)) return;

            _timeToClick = BoostersHandler.Instance.GetBoosterState("auto_clicker_booster") ? 0.2f : 1;
            DoAction();
        }

        private void DoAction()
        {
            if (_actionable == null) return;
            if (!_actionable.CanAction()) return;
            if (_isOnCooldown) return;

            _isOnCooldown = true;

            _animator.DoAction(baseAnimationTime);
        }

        private void AnimatorAction(string id)
        {
            switch (id)
            {
                case "Action":
                    if (_actionable == null) return;

                    SoundManager.Instance.PlayOneShot("action");
                    _actionable.Action();
                    break;
                case "ActionEnd":
                    _isOnCooldown = false;
                    break;
            }
        }
    }
}