using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.UI.Utils
{
    public class ButtonTimerObserver : MonoBehaviour
    {

        [SerializeField] private List<Timer> timers = new();
        [SerializeField] private Image notificationImage;

        private Button _button;
        private readonly List<float> _times = new();
        private int _currentTimer;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            notificationImage.gameObject.SetActive(false);

            foreach (var timer in timers.OrderBy(t => t.DurationInSeconds))
            {
                _times.Add(timer.DurationInSeconds);
            }

            _button.onClick.AddListener(() => notificationImage.gameObject.SetActive(false));

            StartCoroutine(CheckTimers());
        }

        private IEnumerator CheckTimers()
        {
            while (_currentTimer < _times.Count)
            {
                var currentTime = _times[_currentTimer] - _times.Take(_currentTimer).ToList().Sum();
                yield return new WaitForSeconds(currentTime);
                _currentTimer += 1;

                if (notificationImage.gameObject.activeSelf) continue;

                notificationImage.gameObject.SetActive(true);
                notificationImage.transform.DOScale(1.25f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            }
        }
    }
}