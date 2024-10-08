using System;
using _3._Scripts.Actions.Interfaces;
using _3._Scripts.Currency.Enums;
using _3._Scripts.UI;
using _3._Scripts.UI.Effects;
using _3._Scripts.UI.Panels;
using _3._Scripts.Wallet;
using DG.Tweening;
using GBGamesPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.Actions
{
    public class TrainingObject : MonoBehaviour, IActionable
    {
        [SerializeField] private Transform playerPoint;
        [SerializeField] private Transform shakeObject;
        [SerializeField] private ParticleSystem hitParticle;
        [SerializeField] private TMP_Text rewardText;
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private CurrencyCounterEffect effect;
        [SerializeField] private Slider healthBar;
        
        public event Action OnDestroy;
        public bool Blocked { get; set; }

        private Vector3 _startPosition;
        private Vector3 _startSize;
        
        private float _health;
        private float _currentHealth;
        private float _currentReward;

        private Training _training;

        public Transform PlayerPoint => playerPoint;

        public void Initialize(Training training, float reward, float health)
        {
            _training = training;

            _currentHealth = health;
            _health = health;
            _startPosition = shakeObject.localPosition;
            _startSize = shakeObject.localScale;
            _currentReward = reward;
            
            healthBar.maxValue = health;
            healthBar.value = health;

            healthText.text = WalletManager.ConvertToWallet((decimal) health);
            rewardText.text = $"{WalletManager.ConvertToWallet((decimal) reward)}<sprite index=1>";

            SetHealthBarState(false);
        }

        public void Refresh()
        {
            _currentHealth = _health;
            shakeObject.gameObject.SetActive(true);
            shakeObject.localScale = _startSize;
            rewardText.gameObject.SetActive(true);
            shakeObject.localPosition = _startPosition;
            
            healthBar.value = _health;
            healthText.text = WalletManager.ConvertToWallet((decimal) _health);
        }

        public void Action()
        {
            if (_currentHealth <= 0) return;

            var training = Player.Player.instance.GetTrainingStrength();

            shakeObject.DOShakePosition(0.25f, 0.25f, 50).OnComplete(() => shakeObject.localPosition = _startPosition);
            hitParticle.Play();
            
            _currentHealth -= training;
            healthBar.value = _currentHealth;
            healthText.text = WalletManager.ConvertToWallet((decimal) _currentHealth);

            if (_currentHealth > 0) return;

            Destroy();
        }

        public void SetHealthBarState(bool state) => healthBar.gameObject.SetActive(state);
        
        private void Destroy()
        {
            var trainBooster = UIManager.Instance.GetPanel<TrainingPanel>().GetBooster();
            var obj = CurrencyEffectPanel.Instance.SpawnEffect(effect, CurrencyType.First, _currentReward * trainBooster);

            obj.Initialize(CurrencyType.First, _currentReward * trainBooster);

            shakeObject.gameObject.SetActive(false);
            rewardText.gameObject.SetActive(false);

            OnDestroy?.Invoke();

            GBGames.saves.achievementSaves.Update("slap_100", _currentReward * trainBooster);
            GBGames.saves.achievementSaves.Update("slap_10000", _currentReward * trainBooster);
            GBGames.saves.achievementSaves.Update("slap_1000000", _currentReward * trainBooster);
        }
        
        public bool CanAction()
        {
            return _currentHealth > 0 && _training.TrainingStarted && !Blocked;
        }
        
    }
}