using System.Linq;
using _3._Scripts.Aura.Scriptables;
using _3._Scripts.Config;
using UnityEngine;

namespace _3._Scripts.Player
{
    public class PlayerAura
    {
        public AuraData Data { get; private set; }

        private Transform _currentAura;
        private readonly Transform _parent;

        public PlayerAura(Transform parent)
        {
            _parent = parent;
        }

        public void Initialize(string id)
        {
            Data = Configuration.Instance.AllAuras.FirstOrDefault(u => u.ID == id);

            if (_currentAura != null)
                Object.Destroy(_currentAura);

            if (Data == null) return;
            if (Data.Prefab == null) return;

            _currentAura = Object.Instantiate(Data.Prefab, _parent);
        }

        private const float AuraCoefficient = 1.0f;

        public float CalculateCatchChance(float chance)
        {
            if (Data == null) return chance;

            var auraMultiplier = Data.Booster / 100.0f;
            var finalChance = chance + (1 - chance) * (1 - Mathf.Exp(-AuraCoefficient * auraMultiplier));

            return finalChance;
        }
    }
}