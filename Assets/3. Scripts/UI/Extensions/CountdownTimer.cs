﻿using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _3._Scripts.UI.Extensions
{
    [Serializable]
    public class CountdownTimer
    {
        [SerializeField] private TMP_Text timerText;
        private int _countdownValue;

        private event Action CountdownComplete;

        public void StartCountdown(Action countdownComplete, int countdownValue = 3)
        {
            var countdownSequence = DOTween.Sequence();
            _countdownValue = countdownValue;
            UpdateTimerText();
            CountdownComplete = countdownComplete;
        
            for (var i = _countdownValue; i > 0; i--)
            {
                countdownSequence.Append(timerText.DOFade(1, 0.25f)) // Появление текста
                    .Append(timerText.transform.DOScale(1.5f, 0.25f).SetEase(Ease.OutBack)) // Увеличение текста
                    .AppendInterval(0.25f) // Пауза перед исчезновением
                    .Append(timerText.DOFade(0, 0.25f)) // Исчезновение текста
                    .Join(timerText.transform.DOScale(1f, 0.25f).SetEase(Ease.InBack)) // Возвращение к исходному размеру
                    .AppendCallback(() => _countdownValue--) // Уменьшение значения таймера
                    .AppendCallback(UpdateTimerText); // Обновление текста
            }

            countdownSequence.Append(timerText.DOFade(1, 0.25f)) // Появление нуля
                .Append(timerText.transform.DOScale(1.5f, 0.25f).SetEase(Ease.OutBack))
                .AppendInterval(0.5f)
                .Append(timerText.DOFade(0, 0.25f)) // Исчезновение нуля
                .Join(timerText.transform.DOScale(1f, 0.25f).SetEase(Ease.InBack))
                .AppendCallback(OnCountdownComplete); // Завершение таймера
        }

        private void UpdateTimerText()
        {
            timerText.text = _countdownValue.ToString();
        }

        private void OnCountdownComplete()
        {
            timerText.text = "0";
            CountdownComplete?.Invoke();
        }
    }
}