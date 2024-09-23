using System.Collections.Generic;
using System.Linq;

using _3._Scripts.Enemies.Scriptable;
using _3._Scripts.Pets;
using _3._Scripts.Stages.Enums;
using _3._Scripts.Stages.Scriptable;
using UnityEngine;


namespace _3._Scripts.Stages
{
    public class Stage : MonoBehaviour
    {
        [Header("Main")] [SerializeField] private StageConfig config;
        [SerializeField] private Transform spawnPoint;

        public List<Interactive.MiniGame> MiniGames { get; set; }
        public IEnumerable<EnemyData> EnemyData => config.Enemies;
        
        public Transform SpawnPoint => spawnPoint;
        public float GiftBooster => config.GiftBooster;
        public int ID => config.ID;

        public void Initialize()
        {
            InitializeEnemy();
            InitializePetUnlocker();
            InitializeTeleport();
        }

        private void InitializeTeleport()
        {
            var obj = GetComponentsInChildren<Teleport>()
                .FirstOrDefault(s => s.Type == TeleportType.Next || s.Type == TeleportType.New);
            if (obj != null)
                obj.SetPrice(config.TeleportPrice);
        }

        private void InitializeEnemy()
        {
            MiniGames = GetComponentsInChildren<Interactive.MiniGame>().ToList();
            var enemyIndex = 0;
            foreach (var miniGame in MiniGames)
            {
                miniGame.Initialize(config.Enemies[enemyIndex]);
                enemyIndex++;
                if (enemyIndex >= config.Enemies.Count) break;
            }
        }
        
        private void InitializePetUnlocker()
        {
            var obj = GetComponentInChildren<PetUnlocker>();
            obj.Initialize(config.PetUnlocker);
        }

        private void OnValidate()
        {
            gameObject.name = $"Stage_{ID}";
        }
        
    }
}