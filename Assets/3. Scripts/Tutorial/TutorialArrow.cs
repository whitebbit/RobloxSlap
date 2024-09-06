using System;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Interactive;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.Tutorial
{
    public class TutorialArrow : MonoBehaviour
    {
        [SerializeField] private string stepName;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Transform player;
        [SerializeField] private Transform target;

        private bool _active;

        private void Start()
        {
            lineRenderer.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            TutorialSystem.TutorialStepStart += OnTutorialStepStart;
            TutorialSystem.TutorialStepComplete += OnTutorialStepComplete;
        }


        private void OnDisable()
        {
            TutorialSystem.TutorialStepStart -= OnTutorialStepStart;
            TutorialSystem.TutorialStepComplete -= OnTutorialStepComplete;
        }

        private void OnTutorialStepStart(string obj)
        {
            if (obj != stepName) return;

            lineRenderer.gameObject.SetActive(true);
            _active = true;
        }

        private void OnTutorialStepComplete(string obj)
        {
            if (obj != stepName) return;

            lineRenderer.gameObject.SetActive(false);
            _active = false;
        }


        private void Update()
        {
            if (!_active) return;
            lineRenderer.positionCount = 2;

            lineRenderer.SetPosition(0, player.position);
            lineRenderer.SetPosition(1, target.position + Vector3.up);
        }
    }
}