using System;
using System.Linq;
using _3._Scripts.Boosters;
using _3._Scripts.Characters;
using _3._Scripts.Config;
using _3._Scripts.MiniGame;
using _3._Scripts.Pets;
using _3._Scripts.Saves;
using _3._Scripts.Trails;
using _3._Scripts.Upgrades;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.Player
{
    public class Player : Fighter
    {
        [SerializeField] private TrailRenderer trail;

        public PetsHandler PetsHandler { get; private set; }
        public TrailHandler TrailHandler { get; private set; }
        public CharacterHandler CharacterHandler { get; private set; }
        public UpgradeHandler UpgradeHandler { get; private set; }
        public PlayerAnimator PlayerAnimator { get; private set; }

        public static Player instance;
        private CharacterController _characterController;

        private void Awake()
        {
            if (instance == null)
                instance = this;

            PlayerAnimator = GetComponent<PlayerAnimator>();
            PetsHandler = new PetsHandler();
            CharacterHandler = new CharacterHandler();
            UpgradeHandler = new UpgradeHandler(CharacterHandler);
            TrailHandler = new TrailHandler(GetComponent<PlayerMovement>(), trail);
            _characterController = GetComponent<CharacterController>();
        }

        public override FighterData FighterData()
        {
            var photo =
                Configuration.Instance.AllCharacters.FirstOrDefault(c => c.ID == GBGames.saves.characterSaves.current)
                    ?.Icon;

            var health = Configuration.Instance.AllCharacters.FirstOrDefault(
                h => h.ID == GBGames.saves.characterSaves.current).Booster;
            return new FighterData
            {
                photo = photo,
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

        public float GetTrainingStrength(float strengthPerClick)
        {
            var hand = Configuration.Instance.AllUpgrades.FirstOrDefault(
                h => h.ID == GBGames.saves.upgradeSaves.current).Booster;
            var pets = GBGames.saves.petsSave.selected.Sum(p => p.booster);
            /*var character = Configuration.Instance.AllCharacters.FirstOrDefault(
                h => h.ID == GBGames.saves.characterSaves.current).Booster;
                */

            return (strengthPerClick + pets) * hand;
        }

        public void Teleport(Vector3 position)
        {
            _characterController.enabled = false;
            transform.position = position;
            _characterController.enabled = true;
        }

        private void Start()
        {
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

        private void Update()
        {
            if (isFight && Input.GetMouseButtonDown(0))
            {
                Slap();
            }
        }

        private void InitializeCharacter()
        {
            var id = GBGames.saves.characterSaves.current;
            CharacterHandler.SetCharacter(id, transform);
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