using _3._Scripts.MiniGame;
using _3._Scripts.UI.Enums;
using UnityEngine;

namespace _3._Scripts.Aura.Scriptables
{
    [CreateAssetMenu(fileName = "AuraData", menuName = "ScriptableObjects/AuraData", order = 0)]
    public class AuraData : ScriptableObject
    {
        [SerializeField] private CatchData treasure;
        [SerializeField] private Transform prefab;
        [SerializeField] private float booster;

        public float DropChance => treasure.DropChance;
        public Sprite Icon => treasure.Icon;
        public Rarity Rarity => treasure.Rarity;
        public float Booster => booster;
        public Transform Prefab => prefab;
        public string ID => treasure.ID;
    }
}