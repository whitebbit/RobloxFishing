using _3._Scripts.Player;
using _3._Scripts.UI.Scriptable.Shop;
using UnityEngine;

namespace _3._Scripts.Upgrades
{
    public class Hand : MonoBehaviour
    {
        private FishingRod _fishingRod;

        public void Initialize(UpgradeItem item)
        {
            if (_fishingRod == null)
            {
                _fishingRod = Instantiate(item.Prefab, transform);

                var transform1 = _fishingRod.transform;

                transform1.localPosition = new Vector3(-0.202000007f,0.591000021f,-0.89200002f);
                transform1.localEulerAngles = new Vector3(24.615036f,113.971527f,72.0930099f);
                transform1.localScale = new Vector3(0.119999997f, 0.119999997f, 0.119999997f);
            }

            _fishingRod.Initialize(item);

            Player.Player.instance.UpgradeHandler.FishingRod = _fishingRod;
        }
    }
}