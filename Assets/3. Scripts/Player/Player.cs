﻿using System;
using System.Linq;
using _3._Scripts.Boosters;
using _3._Scripts.Characters;
using _3._Scripts.Config;
using _3._Scripts.MiniGame;
using _3._Scripts.Pets;
using _3._Scripts.Saves;
using _3._Scripts.Trails;
using _3._Scripts.UI;
using _3._Scripts.UI.Panels;
using _3._Scripts.Upgrades;
using _3._Scripts.Wallet;
using DG.Tweening;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.Player
{
    public class Player : Fighter
    {
        [SerializeField] private TrailRenderer trail;
        [SerializeField] private Character character;
        [SerializeField] private PlayerLevel level;

        public PetsHandler PetsHandler { get; private set; }
        public TrailHandler TrailHandler { get; private set; }
        public CharacterHandler CharacterHandler { get; private set; }
        public UpgradeHandler UpgradeHandler { get; private set; }
        public PlayerAnimator PlayerAnimator { get; private set; }
        public PlayerAction PlayerAction { get; private set; }
        public PlayerMovement PlayerMovement { get; private set; }

        public static Player instance;
        private CharacterController _characterController;

        private void Awake()
        {
            if (instance == null)
                instance = this;

            PlayerAnimator = GetComponent<PlayerAnimator>();
            PetsHandler = new PetsHandler();
            CharacterHandler = new CharacterHandler(character);
            UpgradeHandler = new UpgradeHandler(CharacterHandler);
            TrailHandler = new TrailHandler(GetComponent<PlayerMovement>(), trail);
            _characterController = GetComponent<CharacterController>();
            PlayerAction = GetComponent<PlayerAction>();
            PlayerMovement = GetComponent<PlayerMovement>();
        }

        public override FighterData FighterData()
        {
            var character =
                Configuration.Instance.AllCharacters.FirstOrDefault(c => c.ID == GBGames.saves.characterSaves.current);

            var health = Configuration.Instance.AllCharacters.FirstOrDefault(
                h => h.ID == GBGames.saves.characterSaves.current).Booster;

            return new FighterData
            {
                photo = RuntimeSkinIconRenderer.Instance.GetTexture2D(character.ID, character.Skin),
                health = BoostersHandler.Instance.GetBoosterState("health_booster") ? health * 2 : health,
                strength = BoostersHandler.Instance.GetBoosterState("slap_booster")
                    ? WalletManager.FirstCurrency * 2
                    : WalletManager.FirstCurrency
            };
        }

        protected override PlayerAnimator Animator()
        {
            return PlayerAnimator;
        }

        public override void OnStart()
        {
            base.OnStart();
            level.gameObject.SetActive(false);
        }

        public override void OnEnd()
        {
            base.OnEnd();
            level.gameObject.SetActive(true);
        }

        public float GetTrainingStrength()
        {
            var hand = Configuration.Instance.AllUpgrades.FirstOrDefault(
                h => h.ID == GBGames.saves.upgradeSaves.current).Booster;
            var pets = GBGames.saves.petsSave.selected.Sum(p => p.booster);

            var strength = WalletManager.FirstCurrency * 0.1f;
            var booster = BoostersHandler.Instance.GetBoosterState("train_booster") ? 2 : 1;
            
            return (strength + pets) * hand * booster ;
        }

        public Tween Teleport(Vector3 position, float speed = 0)
        {
            _characterController.enabled = false;
            var tween = transform.DOMove(position, speed).SetEase(Ease.Linear);
            _characterController.enabled = true;
            return tween;
        }

        private void Start()
        {
            InitializeAnimation();
            Initialize();
        }

        private void Initialize()
        {
            InitializeCharacter();
            InitializeTrail();
            InitializePets();
            InitializeUpgrade();
        }

        public void Reborn()
        {
            WalletManager.FirstCurrency = 0;
            WalletManager.SecondCurrency = 0;

            GBGames.saves.petsSave = new PetSave();
            GBGames.saves.characterSaves = new SaveHandler<string>();
            GBGames.saves.upgradeSaves = new SaveHandler<string>();

            DefaultDataProvider.Instance.SetPlayerDefaultData();

            Initialize();
        }

        private float _timeToSlap;

        private void Update()
        {
            if (!isFight) return;

            _timeToSlap += Time.deltaTime;

            if (!BoostersHandler.Instance.GetBoosterState("auto_fight") && !Input.GetMouseButtonDown(0)) return;

            if (!(_timeToSlap >= 0.275f)) return;

            _timeToSlap = 0;
            Slap();
        }

        private void InitializeCharacter()
        {
            var id = GBGames.saves.characterSaves.current;
            CharacterHandler.SetCharacter(id);
        }

        public void InitializeUpgrade()
        {
            var id = GBGames.saves.upgradeSaves.current;
            UpgradeHandler.SetUpgrade(id);
        }

        private void InitializeTrail()
        {
            var id = GBGames.saves.trailSaves.current;
            TrailHandler.SetTrail(id);
        }

        private void InitializePets()
        {
            var player = transform;
            var position = player.position + player.right * 2;
            var pets = GBGames.saves.petsSave.unlocked.OrderByDescending(p => p.booster).ToList();

            PetsHandler.ClearPets();

            for (var i = 0; i < 3; i++)
            {
                if (pets.Count == i) break;
                PetsHandler.CreatePet(pets[i], position);
            }
        }

        private void OnDestroy()
        {
            instance = null;
        }
    }
}