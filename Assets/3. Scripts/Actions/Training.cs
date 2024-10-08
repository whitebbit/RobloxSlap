using System.Collections.Generic;
using _3._Scripts.Actions.Scriptable;
using _3._Scripts.Inputs;
using _3._Scripts.Inputs.Enums;
using _3._Scripts.Interactive.Interfaces;
using _3._Scripts.UI;
using _3._Scripts.UI.Panels;
using _3._Scripts.Wallet;
using Cinemachine;
using DG.Tweening;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Actions
{
    public class Training : MonoBehaviour, IInteractive
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        private Vector3 _startPosition;
        private List<TrainingObject> _trainingObjects = new();
        private int _currentObjectIndex;

        public bool TrainingStarted { get; private set; }

        public void Initialize(TrainingConfig config)
        {
            if (_trainingObjects.Count > 0) return;

            var startPosition = Vector3.zero;
            var transform1 = Player.Player.instance.transform;

            foreach (var trainingObject in config.TrainingObjects)
            {
                var obj = Instantiate(trainingObject.Prefab, transform);

                obj.transform.localPosition = startPosition;
                obj.Initialize(this, trainingObject.Reward, trainingObject.Health);
                obj.OnDestroy += OnTrainingObjectDestroy;

                startPosition -= transform.forward * 3;
                _trainingObjects.Add(obj);
            }

            virtualCamera.LookAt = transform1;
            virtualCamera.Follow = transform1;
        }

        public void Restart()
        {
            var player = Player.Player.instance;

            if (_currentObjectIndex < _trainingObjects.Count)
                _trainingObjects[_currentObjectIndex].SetHealthBarState(false);

            _currentObjectIndex = 0;

            foreach (var trainingObject in _trainingObjects)
            {
                trainingObject.Refresh();
            }

            _trainingObjects[_currentObjectIndex].SetHealthBarState(true);

            player.Teleport(_trainingObjects[0].PlayerPoint.position);
            player.transform.DOLookAt(_trainingObjects[0].transform.position, 0f, AxisConstraint.Y);
        }

        public void StartInteract()
        {
            if (TrainingStarted) return;
            InputHandler.Instance.SetActionButtonType(ActionButtonType.Training);
        }

        public void Interact()
        {
            if (TrainingStarted) return;

            var panel = UIManager.Instance.GetPanel<TrainingPanel>();
            var player = Player.Player.instance;

            panel.Enabled = true;
            panel.StartTraining(this);

            player.PetsHandler.SetState(false);
            player.PlayerMovement.Blocked = true;
            player.Teleport(_trainingObjects[0].PlayerPoint.position);
            player.transform.DOLookAt(_trainingObjects[0].transform.position, 0.1f, AxisConstraint.Y);

            _trainingObjects[_currentObjectIndex].SetHealthBarState(true);

            InputHandler.Instance.SetActionButtonType(ActionButtonType.Base);
            InputHandler.Instance.SetMovementState(false);
            CameraController.Instance.SwapTo(virtualCamera);

            TrainingStarted = true;
        }

        public void StopInteract()
        {
            InputHandler.Instance.SetActionButtonType(ActionButtonType.Base);
        }

        public void StopTraining()
        {
            TrainingStarted = false;
            InputHandler.Instance.SetActionButtonType(ActionButtonType.Base);

            foreach (var obj in _trainingObjects)
            {
                obj.Refresh();
                obj.SetHealthBarState(false);
            }
            _currentObjectIndex = 0;
        }

        private void OnTrainingObjectDestroy()
        {
            _trainingObjects[_currentObjectIndex].SetHealthBarState(false);
            _currentObjectIndex += 1;

            if (_currentObjectIndex >= _trainingObjects.Count)
            {
                Restart();
                return;
            }

            var player = Player.Player.instance;
            var obj = _trainingObjects[_currentObjectIndex];
            player.transform.DOMove(obj.PlayerPoint.position, 0.5f)
                .OnStart(() =>
                {
                    obj.Blocked = true;
                    player.PlayerAnimator.SetSpeed(1);
                })
                .OnComplete(() =>
                {
                    obj.Blocked = false;
                    player.PlayerAnimator.SetSpeed(0);
                    _trainingObjects[_currentObjectIndex].SetHealthBarState(true);
                });
        }
    }
}