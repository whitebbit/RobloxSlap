using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.UI;
using _3._Scripts.UI.Panels;
using DG.Tweening;
using UnityEngine;

namespace _3._Scripts.Interactive
{
    public class InteractiveGift : MonoBehaviour
    {
        [SerializeField] private List<Timer> timers = new();
        [SerializeField] private Timer timer;
        [SerializeField] private Transform arrow;
        [SerializeField] private Transform particles;
        
        private readonly List<float> _times = new();
        private int _currentTimer;
        private void Start()
        {
            arrow.gameObject.SetActive(false);
            particles.gameObject.SetActive(false);

            foreach (var t in timers.OrderBy(t => t.DurationInSeconds))
            {
                _times.Add(t.DurationInSeconds);
            }

            StartCoroutine(CheckTimers());

            UIManager.Instance.GetPanel<FreeGiftsPanel>().ONOpen += () =>
            {
                arrow.gameObject.SetActive(false);
                particles.gameObject.SetActive(false);
                timer.gameObject.SetActive(true);
            };
        }
        
        private IEnumerator CheckTimers()
        {
            while (_currentTimer < _times.Count)
            {
                var currentTime = _times[_currentTimer] - _times.Take(_currentTimer).ToList().Sum();
                timer.StartTimer(currentTime);
                
                yield return new WaitForSeconds(currentTime);
                
                _currentTimer += 1;
                timer.gameObject.SetActive(false);
                arrow.gameObject.SetActive(true);
                particles.gameObject.SetActive(true);

            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if(!other.TryGetComponent(out Player.Player _)) return;

            UIManager.Instance.GetPanel<FreeGiftsPanel>().Enabled = true;
            arrow.gameObject.SetActive(false);
            particles.gameObject.SetActive(false);
            timer.gameObject.SetActive(true);
        }
    }
}