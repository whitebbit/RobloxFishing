using System;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.UI;
using _3._Scripts.UI.Panels;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.Interactive
{
    public class UpgradeZone : MonoBehaviour
    {
        [SerializeField] private Transform arrow;

        private void OnEnable()
        {
            arrow.gameObject.SetActive(false);
            UIManager.Instance.GetPanel<FishingRodPanel>().ONOpen += OnONOpen;
        }

        private void OnDisable()
        {
            if(UIManager.Instance != null)
                UIManager.Instance.GetPanel<FishingRodPanel>().ONOpen -= OnONOpen;
        }

        private void OnONOpen()
        {
            arrow.gameObject.SetActive(false);
        }

        private void Update()
        {
            TryShowNotification();
        }

        private float _timeToCheck = 30;

        private static bool UpgradeShopPredicate()
        {
            var current = Configuration.Instance.AllUpgrades
                .FirstOrDefault(c => GBGames.saves.upgradeSaves.IsCurrent(c.ID));

            if (current == null) return false;

            var upgrade = Configuration.Instance.AllUpgrades
                .Where(c => c.Price <= WalletManager.FirstCurrency && !GBGames.saves.upgradeSaves.Unlocked(c.ID) &&
                            !GBGames.saves.upgradeSaves.IsCurrent(c.ID))
                .OrderByDescending(c => c.Booster)
                .FirstOrDefault(c => c.Booster > current.Booster);

            return upgrade != null;
        }

        private void TryShowNotification()
        {
            _timeToCheck -= Time.deltaTime;

            if (_timeToCheck > 0) return;

            _timeToCheck = 30;

            arrow.gameObject.SetActive(UpgradeShopPredicate());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Player.Player _)) return;

            UIManager.Instance.GetPanel<FishingRodPanel>().Enabled = true;
        }
    }
}