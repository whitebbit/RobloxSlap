using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Actions;
using _3._Scripts.Actions.Interfaces;
using _3._Scripts.Ads;
using _3._Scripts.Boosters;
using _3._Scripts.Inputs;
using _3._Scripts.Inputs.Enums;
using _3._Scripts.Inputs.Interfaces;
using _3._Scripts.Player;
using _3._Scripts.UI.Panels.Base;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _3._Scripts.UI.Panels
{
    public class TrainingPanel : SimplePanel
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text tutorialText;
        [SerializeField] private Button stopButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private List<Transform> deactivateComponents = new();

        private Training _training;
        
        public override void Initialize()
        {
            base.Initialize();
            stopButton.onClick.AddListener(Stop);
            restartButton.onClick.AddListener(Restart);
        }

        public void StartTraining(Training training)
        {
            _training = training;
            
            SetComponentsState(false);
            DoTextAnimation();
        }
        
        private float _timeToClick = 1;
        private void Update()
        {
            HandleInput();
            slider.value -= 0.01f;
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
                slider.value += 0.2f;

            if (!BoostersHandler.Instance.GetBoosterState("auto_clicker")) return;

            _timeToClick -= Time.deltaTime;

            if (!(_timeToClick <= 0)) return;

            _timeToClick = BoostersHandler.Instance.GetBoosterState("auto_clicker_booster") ? 0.2f : 1;
            slider.value += 0.2f;
        }

        private void Stop()
        {
            _training.StopTraining();
            
            SetComponentsState(true);

            CameraController.Instance.SwapToMain();
            InputHandler.Instance.SetActionButtonType(ActionButtonType.Base);            
            InputHandler.Instance.SetMovementState(true);
            Player.Player.instance.PlayerMovement.Blocked = false;

            Enabled = false;
        }

        private void Restart()
        {
            _training.Restart();    
        }
        
        private Sequence _sequence;

        private void DoTextAnimation()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            tutorialText.alpha = 1;

            var randomRotation = Random.Range(-5f, 5f);
            var randomScale = Random.Range(1f, 1.3f);

            _sequence.Append(tutorialText.DOFade(0, 0.5f))
                .Join(tutorialText.transform.DOScale(randomScale, 0.5f))
                .Join(tutorialText.transform.DORotate(new Vector3(0, 0, randomRotation), 0.5f))
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutQuad);

            tutorialText.gameObject.SetActive(true);
        }
        
        private readonly HashSet<GameObject> _deactivatedObjects = new();
        
        private void SetComponentsState(bool state)
        {
            if (state)
            {
                foreach (var component in deactivateComponents.Where(component =>
                    _deactivatedObjects.Contains(component.gameObject)))
                {
                    GameObject o;
                    (o = component.gameObject).SetActive(true);
                    _deactivatedObjects.Remove(o);
                }
            }
            else
            {
                foreach (var component in deactivateComponents.Where(component => component.gameObject.activeSelf))
                {
                    GameObject o;
                    (o = component.gameObject).SetActive(false);
                    _deactivatedObjects.Add(o);
                }
            }
        }

        public float GetBooster()
        {
            return slider.value switch
            {
                >= 0 and <= 0.5f => 1,
                >= 0.5f and <= 0.8f => 1.5f,
                >= 0.8f => 2f,
                _ => 1
            };
        }
    }
}