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
        [SerializeField] private CurrencyCounterEffect effect;

        public event Action OnDestroy;
        public bool Blocked { get; set; }

        private Vector3 _startPosition;
        private Vector3 _startSize;

        private float _health;
        private float _currentHealth;
        private float _currentReward;
        private float _clickBooster;

        private Training _training;

        public Transform PlayerPoint => playerPoint;
        private float Reward => _health * Player.Player.instance.GetTrainingStrength(_clickBooster);

        public void Initialize(Training training, float health, float clickBooster)
        {
            _training = training;
            _clickBooster = clickBooster;
            _currentHealth = health;
            _health = health;
            _startPosition = shakeObject.localPosition;
            _startSize = shakeObject.localScale;

            SetRewardTextState(false);
        }

        public void SetReward() =>
            rewardText.text = $"{WalletManager.ConvertToWallet((decimal) Reward)}<sprite index=1>";

        public void SetRewardTextState(bool state) => rewardText.gameObject.SetActive(state);

        public void Refresh()
        {
            _currentHealth = _health;
            shakeObject.gameObject.SetActive(true);
            shakeObject.localScale = _startSize;
            rewardText.gameObject.SetActive(true);
            shakeObject.localPosition = _startPosition;
            Blocked = false;
        }

        public void Action()
        {
            if (_currentHealth <= 0) return;

            shakeObject.DOShakePosition(0.25f, 0.25f, 50).OnComplete(() => shakeObject.localPosition = _startPosition);
            hitParticle.Play();

            _currentHealth -= 1;

            if (_currentHealth > 0) return;

            Destroy();
        }


        private void Destroy()
        {
            var trainBooster = UIManager.Instance.GetPanel<TrainingPanel>().GetBooster();
            var train = Reward * trainBooster;
            var obj = CurrencyEffectPanel.Instance.SpawnEffect(effect, CurrencyType.First, train);

            obj.Initialize(CurrencyType.First, train);

            shakeObject.gameObject.SetActive(false);
            rewardText.gameObject.SetActive(false);

            OnDestroy?.Invoke();

            GBGames.saves.achievementSaves.Update("slap_100", train);
            GBGames.saves.achievementSaves.Update("slap_10000", train);
            GBGames.saves.achievementSaves.Update("slap_1000000", train);
        }

        public bool CanAction()
        {
            Debug.Log(gameObject.name);
            Debug.Log($"_currentHealth > 0 - {_currentHealth > 0}");
            Debug.Log($"_training.TrainingStarted - {_training.TrainingStarted}");
            Debug.Log($"!Blocked - {!Blocked}");
            return _currentHealth > 0 && _training.TrainingStarted && !Blocked;
        }
    }
}