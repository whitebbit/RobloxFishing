using _3._Scripts.Currency.Enums;
using _3._Scripts.UI.Enums;
using _3._Scripts.UI.Scriptable.Shop;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Pets.Scriptables
{
    [CreateAssetMenu(fileName = "PetData", menuName = "ScriptableObjects/Pets/PetData", order = 0)]
    public class PetData : ShopItem
    {
        //[SerializeField] private string id;
        [Tab("Pet")]
        [Header("Main")]
        [SerializeField] private float dropPercent;
        //[SerializeField] private Rarity rarity;
        [SerializeField] private Pet prefab;
        [Header("Booster")] 
        [SerializeField] private CurrencyType boosterType;
        [Space]
        [SerializeField] private int minBooster;
        [SerializeField] private int maxBooster;
        /*[Tab("UI")] 
        [SerializeField] private Sprite icon;*/

        //public Sprite Icon => icon;
        //public Rarity Rarity => rarity;
        public Pet Prefab => prefab;
        public CurrencyType BoosterType => boosterType;
        public int RandomBooster => Random.Range(minBooster, maxBooster);
        public float DropPercent => dropPercent;
        public override string Title()
        {
            return "";
        }

        //public string ID => id;
        
    }
}