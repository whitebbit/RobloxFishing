using System.Collections.Generic;
using _3._Scripts.UI.Scriptable.Roulette;
using UnityEngine;

namespace _3._Scripts.Rangs.Scriptables
{
    [CreateAssetMenu(fileName = "RangData", menuName = "ScriptableObjects/RangData", order = 0)]
    public class RangData : ScriptableObject
    {
        [SerializeField] private string rangNameID;
        [SerializeField] private int countToUnlock;
        [SerializeField] private List<GiftItem> rewards = new();

        public string RangNameID => rangNameID;
        public int CountToUnlock => countToUnlock;
        public List<GiftItem> Rewards => rewards;
        
    }
}