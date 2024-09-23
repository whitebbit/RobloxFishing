using System;
using _3._Scripts.UI;
using _3._Scripts.UI.Panels;
using UnityEngine;

namespace _3._Scripts.Interactive
{
    public class UpgradeZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Player.Player _)) return;

            UIManager.Instance.GetPanel<FishingRodPanel>().Enabled = true;
        }
    }
}