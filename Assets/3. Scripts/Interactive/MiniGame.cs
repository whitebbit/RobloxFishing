using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Enemies;
using _3._Scripts.Enemies.Scriptable;
using _3._Scripts.Inputs;
using _3._Scripts.Interactive.Interfaces;
using _3._Scripts.MiniGame;
using _3._Scripts.Player;
using _3._Scripts.Stages;
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
        [Tab("Catch List")] [SerializeField] private CatchList catchList;

        [Tab("Transforms")] [SerializeField] private Transform playerPoint;
        [SerializeField] private Transform lookPoint;
        [SerializeField] private Transform enemyPoint;
        [SerializeField] private Transform useTutorialObject;

        public EnemyData EnemyData { get; private set; }
        private Fighter _enemy;
        private bool _fightStarted;


        public void Initialize(EnemyData data)
        {
            if (_enemy != null) return;

            var enemy = Instantiate(enemyPrefab, transform);

            EnemyData = data;

            enemy.transform.localPosition = enemyPoint.localPosition;
            enemy.Initialize(EnemyData);

            _enemy = enemy;

            catchList.Initialize(StageController.Instance.CurrentStageID, data.CatchData, data.AdditionalDropChance);
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
            panel.StartFishing(Player.Player.instance, _enemy, EnemyData, StartFishing,
                EndFishing);

            useTutorialObject.gameObject.SetActive(false);

            player.PetsHandler.SetState(false);
            player.Teleport(playerPoint.position);
            player.transform.DOLookAt(lookPoint.transform.position, 0, AxisConstraint.Y);

            useTutorialObject.gameObject.SetActive(false);
            
            FishingLine.Instance.SetState(false);
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