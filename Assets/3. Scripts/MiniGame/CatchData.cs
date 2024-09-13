using _3._Scripts.UI.Enums;
using UnityEngine;

namespace _3._Scripts.MiniGame
{
    [CreateAssetMenu(fileName = "CatchData", menuName = "ScriptableObjects/CatchData", order = 0)]
    public class CatchData : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private Sprite icon;
        [SerializeField] private float dropChance;
        [SerializeField] private float reward;
        [SerializeField] private Rarity rarity;
        [SerializeField] private CatchType type;

        public string ID => id;
        public CatchType Type => type;
        public Rarity Rarity => rarity;
        public Sprite Icon => icon;
        public float DropChance => dropChance;
        public float Reward => reward;
    }
}