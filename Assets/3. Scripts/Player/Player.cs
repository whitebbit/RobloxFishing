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
        public PlayerAura PlayerAura { get; private set; }

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
            PlayerAura = new PlayerAura(transform);
            _characterController = GetComponent<CharacterController>();
        }

        public override FighterData FighterData()
        {
            var photo =
                Configuration.Instance.AllCharacters.FirstOrDefault(c => c.ID == GBGames.saves.characterSaves.current)
                    ?.Icon;

            return new FighterData
            {
                photo = photo,
                health = 0,
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
            UpgradeHandler.FishingRod.SetState(true);
            Animator().SetTrigger("StartFishing");
        }

        public override void OnEnd()
        {
            base.OnEnd();
            UpgradeHandler.FishingRod.SetState(false);
        }

        public override void PutFish()
        {
            base.PutFish();
            Animator().SetTrigger("PutFish");
        }

        public override void EndFishing()
        {
            base.EndFishing();
            Animator().SetTrigger("FishingEnd");
        }

        public float GetTrainingStrength()
        {
            var upgrade =
                Configuration.Instance.AllUpgrades.FirstOrDefault(u => u.ID == GBGames.saves.upgradeSaves.current);
            var petsBooster = GBGames.saves.petsSave.selected.Sum(p => p.booster);

            if (upgrade is null) return 1;
           
            var value = upgrade.Booster + upgrade.Booster * petsBooster / 100;
            return value;

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
            InitializeAura();
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

        private void InitializeAura()
        {
            var id = GBGames.saves.auraSaves.current;
            PlayerAura.Initialize(id);
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