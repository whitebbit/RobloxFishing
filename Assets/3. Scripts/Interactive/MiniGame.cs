using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Enemies;
using _3._Scripts.Enemies.Scriptable;
using _3._Scripts.Inputs;
using _3._Scripts.Interactive.Interfaces;
using _3._Scripts.MiniGame;
using _3._Scripts.Player;
using _3._Scripts.Tutorial;
using _3._Scripts.UI;
using _3._Scripts.UI.Elements;
using _3._Scripts.UI.Enums;
using _3._Scripts.UI.Panels;
using Cinemachine;
using DG.Tweening;
using GBGamesPlugin;
using UnityEngine;
using VInspector;
using Random = Unity.Mathematics.Random;

namespace _3._Scripts.Interactive
{
    public class MiniGame : MonoBehaviour, IInteractive
    {
        [Tab("Fight components")] [SerializeField]
        private Enemy enemyPrefab;

        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [Tab("Catch List")] [SerializeField] private CatchElement catchElementPrefab;
        [SerializeField] private Transform fishContainer;
        [SerializeField] private Transform treasureContainer;

        [Tab("Transforms")] [SerializeField] private Transform playerPoint;
        [SerializeField] private Transform lookPoint;
        [SerializeField] private Transform enemyPoint;
        [SerializeField] private Transform useTutorialObject;

        private EnemyData _enemyData;
        private Fighter _enemy;
        private bool _fightStarted;

        private List<CatchElement> _catchElements = new();

        private void OnEnable()
        {
            GBGames.saves.catchSave.OnCatchListUpdate += UpdateCatchList;
        }

        private void OnDisable()
        {
            GBGames.saves.catchSave.OnCatchListUpdate -= UpdateCatchList;
        }

        private void UpdateCatchList()
        {
            foreach (var element in _catchElements)
            {
                var state = GBGames.saves.catchSave.CatchUnlocked(_enemyData.ID, element.Data.ID);
                element.SetUnlockedState(state);
            }
        }

        public void Initialize(EnemyData data)
        {
            if (_enemy != null) return;

            var enemy = Instantiate(enemyPrefab, transform);

            _enemyData = data;

            enemy.transform.localPosition = enemyPoint.localPosition;
            enemy.Initialize(_enemyData);

            _enemy = enemy;

            InitializeCatchList();
        }

        private void InitializeCatchList()
        {
            foreach (GameObject obj in fishContainer)
            {
                Destroy(obj);
            }

            foreach (GameObject obj in treasureContainer)
            {
                Destroy(obj);
            }

            var fish = _enemyData.CatchData.Where(d => d.Type == CatchType.Fish);
            var treasure = _enemyData.CatchData.Where(d => d.Type == CatchType.Treasure);

            foreach (var data in fish)
            {
                var element = Instantiate(catchElementPrefab, fishContainer);
                element.Initialize(data);
                _catchElements.Add(element);
            }

            foreach (var data in treasure)
            {
                var element = Instantiate(catchElementPrefab, treasureContainer);
                element.Initialize(data);
                _catchElements.Add(element);
            }

            UpdateCatchList();
        }

        private void Start()
        {
            useTutorialObject.gameObject.SetActive(false);
        }

        public void StartInteract()
        {
            if (_fightStarted) return;

            useTutorialObject.gameObject.SetActive(true);
        }

        public void Interact()
        {
            if (_fightStarted) return;

            _fightStarted = true;

            var panel = UIManager.Instance.GetPanel<MiniGamePanel>();
            var player = Player.Player.instance;

            panel.Enabled = true;
            panel.StartFishing(_enemyData.ID, Player.Player.instance, _enemy, _enemyData.CatchData, StartFishing,
                EndFishing);

            useTutorialObject.gameObject.SetActive(false);

            player.PetsHandler.SetState(false);
            player.Teleport(playerPoint.position);
            player.transform.DOLookAt(lookPoint.transform.position, 0, AxisConstraint.Y);

            useTutorialObject.gameObject.SetActive(false);

            CameraController.Instance.SwapTo(virtualCamera);

            useTutorialObject.gameObject.SetActive(false);
        }

        private void StartFishing()
        {
            
            FishingLine.Instance.SetState(true);
            FishingLine.Instance.SetTarget(lookPoint.transform);
        }

        private void EndFishing()
        {
            var panel = UIManager.Instance.GetPanel<MiniGamePanel>();
            var player = Player.Player.instance;

            _fightStarted = false;

            player.PetsHandler.SetState(true);
            panel.Enabled = false;
            
            FishingLine.Instance.SetState(false);
            CameraController.Instance.SetShake(0);
            CameraController.Instance.SwapToMain();
            InputHandler.Instance.SetState(true);
        }

        public void StopInteract()
        {
            useTutorialObject.gameObject.SetActive(false);
        }
    }
}