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
        [SerializeField] private List<Timer> timers = new(); // Список таймеров
        [SerializeField] private Image notificationImage; // Уведомление

        private Button _button;
        private int _currentTimerIndex = 0; // Индекс текущего таймера
        private float _elapsedTime = 0f; // Время, прошедшее с начала отсчета

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClick);
        }

        private void Start()
        {
            notificationImage.gameObject.SetActive(false);

            // Сортируем таймеры по времени
            timers = timers.OrderBy(t => t.DurationInSeconds).ToList();

            // Запускаем корутину для отслеживания таймеров
            StartCoroutine(CheckTimers());
        }

        private void OnButtonClick()
        {
            notificationImage.gameObject.SetActive(false);
        }

        private IEnumerator CheckTimers()
        {
            while (_currentTimerIndex < timers.Count)
            {
                var currentTime = timers[_currentTimerIndex].DurationInSeconds - _elapsedTime;

                if (currentTime > 0)
                {
                    yield return new WaitForSeconds(currentTime);
                }

                _elapsedTime += currentTime; 

                if (!notificationImage.gameObject.activeSelf)
                {
                    notificationImage.gameObject.SetActive(true);
                    notificationImage.transform.DOScale(1.25f, 0.5f).SetLoops(-1, LoopType.Yoyo);
                }

                _currentTimerIndex++;
            }

            notificationImage.gameObject.SetActive(false);
        }
    }
}