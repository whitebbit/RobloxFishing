using System;
using _3._Scripts.Actions;
using _3._Scripts.Boosters;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Inputs;
using _3._Scripts.UI;
using _3._Scripts.UI.Effects;
using _3._Scripts.UI.Panels;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Player
{
    public class PlayerTraining : MonoBehaviour
    {
        [Tab("Settings")] [SerializeField] private CurrencyType currencyType;
        [SerializeField] private CurrencyCounterEffect effect;

        [SerializeField] private TrainingPond pond;
        private static FishingRod FishingRod => Player.instance.UpgradeHandler.FishingRod;
        private bool _trainingStarted;
        private float _timerToDeactivate = 2;

        private void Start()
        {
            FishingRod.SetState(false);
        }

        private void Update()
        {
            StartTraining();
            Training();
            TryEndTraining();
        }

        private void TryEndTraining()
        {
            if (UIManager.Instance.Active && _trainingStarted)
            { 
                //EndTraining();
                return;
            }

            EndByTimer();
            EndByDistance();
        }

        private void EndByDistance()
        {
            if (!_trainingStarted) return;

            if (Vector3.Distance(transform.position, pond.transform.position) >= 4f)
                EndTraining();
        }

        private void EndByTimer()
        {
            if (!_trainingStarted) return;

            _timerToDeactivate -= Time.deltaTime;

            if (_timerToDeactivate > 0) return;

            EndTraining();
        }

        private void EndTraining()
        {
            _timerToDeactivate = 2;
            _trainingStarted = false;
            FishingRod.SetState(false);
            FishingLine.Instance.SetState(false);
            FishingLine.Instance.SetTarget(null);
            pond.SetState(false);
        }

        private void StartTraining()
        {
            if (_trainingStarted || UIManager.Instance.Active) return;
            if (!InputHandler.Instance.Input.GetAction() &&
                !BoostersHandler.Instance.GetBoosterState("auto_clicker")) return;

            _trainingStarted = true;
            FishingRod.SetState(true);

            FishingLine.Instance.SetState(true);
            FishingLine.Instance.SetStartPoint(FishingRod.FishingLineStartPoint.transform);
            FishingLine.Instance.SetTarget(pond.FishingFloatPoint);
            pond.SetState(true);
        }

        private float _timer;

        private void Training()
        {
            if (UIManager.Instance.Active) return;

            if (!_trainingStarted) return;

            if (InputHandler.Instance.Input.GetAction())
            {
                DoStrength();
            }

            if (!BoostersHandler.Instance.GetBoosterState("auto_clicker")) return;
            _timer += Time.deltaTime;

            if (!(_timer >= 1f)) return;
            DoStrength();
            _timer = 0;
        }

        private void DoStrength()
        {
            var training = Player.instance.GetTrainingStrength();
            var obj = CurrencyEffectPanel.Instance.SpawnEffect(effect, currencyType, training);

            _timerToDeactivate = 2;
            obj.Initialize(currencyType, training);
        }
    }
}