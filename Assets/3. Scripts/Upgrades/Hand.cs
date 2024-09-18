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

                transform1.localPosition = new Vector3(-0.000780000002f,0.000760000024f,0.000590000011f);
                transform1.localEulerAngles = new Vector3(34.8264999f,25.0290527f,260.1604f);
                transform1.localScale = Vector3.one;
            }

            _fishingRod.Initialize(item);

            Player.Player.instance.UpgradeHandler.FishingRod = _fishingRod;
        }
    }
}