using _3._Scripts.Characters;
using _3._Scripts.Player;
using _3._Scripts.Wallet;
using UnityEngine;
using VInspector;

namespace _3._Scripts.UI.Scriptable.Shop
{
    [CreateAssetMenu(fileName = "UpgradeItem", menuName = "Shop Items/Upgrade Item", order = 0)]
    public class UpgradeItem : ShopItem
    {
        [SerializeField] private float booster;
        [SerializeField] private Color color;

        [Tab("Prefab")] [SerializeField]
        private FishingRod prefab;

        
        [SerializeField] private Material mainMaterial;
        [SerializeField] private Material secondMaterial;

        public FishingRod Prefab => prefab;
        public Material MainMaterial => mainMaterial;
        public Material SecondMaterial => secondMaterial;
        public float Booster => booster;
        public Color Color => color;


        public override string Title()
        {
            return $"x{WalletManager.ConvertToWallet((decimal) booster)}<sprite index=1>";
        }
    }
}