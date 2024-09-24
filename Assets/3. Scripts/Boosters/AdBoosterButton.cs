using System;
using System.Collections;
using DG.Tweening;
using GBGamesPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.Boosters
{
    [RequireComponent(typeof(Button))]
    public class AdBoosterButton : MonoBehaviour
    {
        [Tab("View")] [SerializeField] private CanvasGroup canvasGroup;

        [SerializeField] private Image cooldownImage;
        [SerializeField] private Image adImage;
        [SerializeField] private TMP_Text freeText;
        [SerializeField] private Slider slider;
        [Tab("Settings")] [SerializeField] private string id;
        [SerializeField] private float timeToDeactivate;

        public Action onActivateBooster;
        public Action onDeactivateBooster;
        private Button _button;
        private bool _used;

        private bool _promoted;
        private float _lastPromotionTime;

        private Tween _currentTween;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        public void ShowPromotion(float timeToDeactivated)
        {
            if (_promoted) return;
            if (!(Time.time - _lastPromotionTime >= 60)) return;

            _lastPromotionTime = Time.time;
            _promoted = true;
            gameObject.SetActive(true);
            canvasGroup.DOFade(1, 0.25f);
            slider.value = 1;
            slider.gameObject.SetActive(true);
            _currentTween = slider.DOValue(0, timeToDeactivated).OnComplete(HidePromotion).SetEase(Ease.Linear);

            freeText.gameObject.SetActive(CanUseForFree());
            adImage.gameObject.SetActive(!CanUseForFree());
        }

        private void Start()
        {
            _button.onClick.AddListener(OnCLick);
            _lastPromotionTime = Time.time - 60;
            HidePromotion();
        }

        private void OnCLick()
        {
            if (_used) return;

            if (CanUseForFree())
            {
                GBGames.saves.freeUsedBoosters.Add(id);
                Activate();
            }
            else
                GBGames.ShowRewarded(Activate);
        }

        private void Activate()
        {
            _used = true;
            onActivateBooster?.Invoke();
            cooldownImage.fillAmount = 1;
            cooldownImage.DOFillAmount(0, timeToDeactivate).SetEase(Ease.Linear).OnComplete(() =>
            {
                onDeactivateBooster?.Invoke();
                adImage.gameObject.SetActive(true);
                cooldownImage.fillAmount = 0;
                _used = false;
                HidePromotion();
            });
            slider.gameObject.SetActive(false);
            adImage.gameObject.SetActive(false);
            freeText.gameObject.SetActive(false);
            _currentTween.Kill();
        }

        private void HidePromotion()
        {
            _promoted = false;
            canvasGroup.alpha = 0;
            gameObject.SetActive(false);
        }

        private bool CanUseForFree()
        {
            return !GBGames.saves.freeUsedBoosters.Contains(id);
        }
    }
}