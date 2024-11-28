using System;
using System.Collections.Generic;
using _3._Scripts.Actions.Scriptable;
using _3._Scripts.Inputs;
using _3._Scripts.Inputs.Enums;
using _3._Scripts.Interactive.Interfaces;
using _3._Scripts.Localization;
using _3._Scripts.UI;
using _3._Scripts.UI.Panels;
using _3._Scripts.Wallet;
using Cinemachine;
using DG.Tweening;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.Localization.Components;
using VInspector;

namespace _3._Scripts.Actions
{
    public class Training : MonoBehaviour, IInteractive
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private Transform cameraPoint;
        [SerializeField] private LocalizeStringEvent requiredText;
        [SerializeField] private LocalizeStringEvent rewardText;

        private Vector3 _startPosition;
        private readonly List<TrainingObject> _trainingObjects = new();
        private int _currentObjectIndex;
        public bool TrainingStarted { get; private set; }

        private float _requiredCount;

        private void Update()
        {
            if (TrainingStarted)
            {
                cameraPoint.position = Player.Player.instance.transform.position;
            }
        }

        public void Initialize(TrainingConfig config)
        {
            if (_trainingObjects.Count > 0) return;

            var startPosition = Vector3.zero;
            var startHealth = 1;

            foreach (var trainingObject in config.TrainingObjects)
            {
                var obj = Instantiate(trainingObject, transform);

                obj.transform.localPosition = startPosition;
                obj.transform.localEulerAngles = new Vector3(0, 180, 0);
                
                obj.Initialize(this, startHealth, config.Count);
                obj.OnDestroy += OnTrainingObjectDestroy;

                startPosition -= Vector3.forward * 3;
                startHealth += 1;
                _trainingObjects.Add(obj);
            }

            _requiredCount = config.RequiredCount;

            requiredText.SetVariable("value", WalletManager.ConvertToWallet((decimal) config.RequiredCount));
            rewardText.SetVariable("value", WalletManager.ConvertToWallet((decimal) config.Count));

            SetTextState(true);
        }

        private bool _restarted;

        private void SetTextState(bool state)
        {
            requiredText.gameObject.SetActive(state);
            rewardText.gameObject.SetActive(state);
        }

        public void Restart()
        {
            if (_restarted) return;

            var player = Player.Player.instance;
            var obj = _trainingObjects[0];

            _restarted = true;
            _currentObjectIndex = 0;

            _currentTween?.Kill();
            _currentTween = player.Teleport(obj.PlayerPoint.position, 1)
                .OnComplete(() =>
                {
                    foreach (var trainingObject in _trainingObjects)
                    {
                        trainingObject.Refresh();
                    }

                    player.PlayerAnimator.SetSpeed(0);
                    player.transform.DOLookAt(obj.transform.position, 0f, AxisConstraint.Y);
                    _restarted = false;
                })
                .OnStart(() =>
                {
                    foreach (var trainingObject in _trainingObjects)
                    {
                        trainingObject.Blocked = true;
                    }

                    player.PlayerAnimator.SetSpeed(1);
                    player.transform.DOLookAt(obj.transform.position, 0.25f, AxisConstraint.Y);
                });
        }

        public void StartInteract()
        {
            if (TrainingStarted || !CanInteract()) return;
            InputHandler.Instance.SetActionButtonType(ActionButtonType.Training);
        }

        public void Interact()
        {
            if (TrainingStarted || !CanInteract()) return;

            var panel = UIManager.Instance.GetPanel<TrainingPanel>();
            var player = Player.Player.instance;

            panel.Enabled = true;
            panel.StartTraining(this);

            player.PetsHandler.SetState(false);
            player.PlayerMovement.Blocked = true;
            player.Teleport(_trainingObjects[0].PlayerPoint.position);
            player.transform.DOLookAt(_trainingObjects[0].transform.position, 0.1f, AxisConstraint.Y);

            InputHandler.Instance.SetActionButtonType(ActionButtonType.Base);
            InputHandler.Instance.SetMovementState(false);
            CameraController.Instance.SwapTo(virtualCamera);

            SetTextState(false);

            foreach (var trainingObject in _trainingObjects)
            {
                trainingObject.SetReward();
                trainingObject.SetRewardTextState(true);
            }

            TrainingStarted = true;
        }

        public void StopInteract()
        {
            InputHandler.Instance.SetActionButtonType(ActionButtonType.Base);
        }

        public void StopTraining()
        {
            TrainingStarted = false;
            _restarted = false;
            InputHandler.Instance.SetActionButtonType(ActionButtonType.Base);

            foreach (var obj in _trainingObjects)
            {
                obj.Refresh();
                obj.SetRewardTextState(false);
            }

            _currentObjectIndex = 0;
            _currentTween?.Kill();
            SetTextState(true);
        }

        private Tween _currentTween;

        private void OnTrainingObjectDestroy()
        {
            _currentObjectIndex += 1;

            if (_currentObjectIndex >= _trainingObjects.Count)
            {
                Restart();
                return;
            }

            var player = Player.Player.instance;
            var obj = _trainingObjects[_currentObjectIndex];

            _currentTween?.Kill();
            _currentTween = player.transform.DOMove(obj.PlayerPoint.position, 0.5f)
                .OnStart(() =>
                {
                    obj.Blocked = true;
                    player.PlayerAnimator.SetSpeed(1);
                })
                .OnComplete(() =>
                {
                    obj.Blocked = false;
                    player.PlayerAnimator.SetSpeed(0);
                });

            Debug.Log("!!!!!!!!!");
        }

        public bool CanInteract() => WalletManager.FirstCurrency >= _requiredCount;
    }
}