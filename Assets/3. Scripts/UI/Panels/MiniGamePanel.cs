using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Ads;
using _3._Scripts.Boosters;
using UnityEngine;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Inputs;
using _3._Scripts.Localization;
using _3._Scripts.MiniGame;
using _3._Scripts.Player;
using _3._Scripts.Stages;
using _3._Scripts.UI.Effects;
using _3._Scripts.UI.Elements;
using _3._Scripts.UI.Enums;
using _3._Scripts.UI.Extensions;
using _3._Scripts.UI.Panels.Base;
using DG.Tweening;
using GBGamesPlugin;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Panels
{
    public class MiniGamePanel : SimplePanel
    {
        [Tab("Components")] [SerializeField] private CatchList catchList;
        [SerializeField] private Button stopButton;
        [SerializeField] private List<Transform> deactivateComponents = new();

        [Tab("Slider")] [SerializeField] private Slider slider;
        [SerializeField] private Image background;
        [SerializeField] private Image fill;
        [Space] [SerializeField] private Color backgroundWaitColor;
        [SerializeField] private Color backgroundPutColor;
        [SerializeField] private Color fillWaitColor;
        [SerializeField] private Color fillPutColor;
        [Tab("Text")] [SerializeField] private LocalizeStringEvent text;
        [SerializeField] private string waitTextID;
        [SerializeField] private string putTextID;


        [Tab("Reward")] [SerializeField] private CatchElement rewardCatch;
        [SerializeField] private CurrencyType rewardType;
        [SerializeField] private CurrencyCounterEffect effect;

        private event Action OnEnd;
        private event Action OnStartFishing;

        private Fighter _player;
        private Fighter _enemy;

        private Tween _currentTween;

        private List<CatchData> _catchData = new();

        private float _fillAmount = 0.5f;
        private const float FillRate = 0.5f;
        private const float WinThreshold = 0.95f;
        private bool _started;

        private void Start()
        {
            stopButton.onClick.AddListener(StopFishing);
        }

        private void Update()
        {
            if (!_started) return;
            if (InterstitialsTimer.Instance.Active) return;

            HandleInput();
            UpdateFillAmount();
            slider.value = _fillAmount;
            CheckGameEnd();
        }

        public void StartFishing(Fighter player, Fighter enemy, List<CatchData> catchData,
            Action onStart, Action onEnd)
        {
            InitializeFighters(player, enemy);
            StartWaitStep();

            _fillAmount = 0.5f;
            _catchData = catchData;
            rewardCatch.SetState(false);
            catchList.Initialize(StageController.Instance.CurrentStageID, catchData);

            SetComponentsState(false);

            InputHandler.Instance.SetState(false);
            FishingLine.Instance.SetState(false);

            OnEnd = onEnd;
            OnStartFishing = onStart;
        }

        private void StopFishing()
        {
            rewardCatch.SetState(false);
            _currentTween?.Kill();
            _player.EndFishing();
            _player.OnEnd();

            _started = false;
            _currentTween = null;

            OnEnd?.Invoke();

            SetComponentsState(true);
        }

        private void StartWaitStep()
        {
            _player.OnStart();

            background.color = backgroundWaitColor;
            fill.color = fillWaitColor;

            text.SetReference(waitTextID);

            slider.value = 0;
            _currentTween = slider.DOValue(1, 3f).OnComplete(StartPutStep).SetDelay(2f)
                .OnStart(() => OnStartFishing?.Invoke());
        }

        private void StartPutStep()
        {
            background.color = backgroundPutColor;
            fill.color = fillPutColor;

            text.SetReference(putTextID);

            slider.value = _fillAmount;
            _started = true;
            _player.PutFish();

            CameraController.Instance.SetShake(1);
        }

        private void SetComponentsState(bool state)
        {
            foreach (var component in deactivateComponents)
            {
                component.gameObject.SetActive(state);
            }
        }

        private void InitializeFighters(Fighter player, Fighter enemy)
        {
            _player = player;
            _enemy = enemy;
        }

        private void HandlePlayerWin()
        {
            if (!_started) return;

            var catchReward = GetRandomCatch();
            var rewardC = BoostersHandler.Instance.GetBoosterState("reward_booster")
                ? catchReward.Reward * 2
                : catchReward.Reward;
            var effectInstance = CurrencyEffectPanel.Instance.SpawnEffect(effect, rewardType, rewardC);

            effectInstance.Initialize(rewardType, rewardC);

            rewardCatch.Initialize(catchReward);
            rewardCatch.SetState(true);
            _player.EndFishing();
            _started = false;

            CameraController.Instance.SetShake(0);
            FishingLine.Instance.SetState(false);
            GBGames.saves.catchSave.AddCatch(StageController.Instance.CurrentStageID, catchReward.ID);
            StartCoroutine(RestartGame());
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _fillAmount += GetPlayerFillRate();
            }
        }

        private void UpdateFillAmount()
        {
            _fillAmount -= GetEnemyDecreaseRate() * Time.deltaTime;
            _fillAmount = Mathf.Clamp(_fillAmount, 0f, 1f);
        }

        private void CheckGameEnd()
        {
            switch (_fillAmount)
            {
                case <= 0:
                    StartCoroutine(RestartGame());
                    break;
                case >= WinThreshold:
                    HandlePlayerWin();
                    break;
            }
        }

        private IEnumerator RestartGame()
        {
            yield return new WaitForSeconds(2f);
            StartFishing(_player, _enemy, _catchData, OnStartFishing, OnEnd);
        }

        private CatchData GetRandomCatch()
        {
            var playerAura = Player.Player.instance.PlayerAura;
            var totalWeight = _catchData.Sum(d => playerAura.CalculateCatchChance(d.DropChance));
            var randomValue = UnityEngine.Random.Range(0, totalWeight);
            var cumulativeWeight = 0f;

            foreach (var petData in _catchData)
            {
                cumulativeWeight += playerAura.CalculateCatchChance(petData.DropChance);
                if (randomValue <= cumulativeWeight)
                {
                    return petData;
                }
            }

            return null;
        }

        private float GetPlayerFillRate() => FillRate / 4 * (_player.FighterData().strength /
                                                             (_player.FighterData().strength +
                                                              _enemy.FighterData().strength));

        private float GetEnemyDecreaseRate() => FillRate * (_enemy.FighterData().strength /
                                                            (_player.FighterData().strength +
                                                             _enemy.FighterData().strength));
    }
}