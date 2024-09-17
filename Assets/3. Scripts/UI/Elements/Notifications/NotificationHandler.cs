using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.FSM.Base;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.UI.Elements.Notifications
{
    public class NotificationHandler : MonoBehaviour
    {
        [SerializeField] private List<NotificationItem> notificationItems = new();

        private List<BaseNotification> _notifications = new();

        private void Start()
        {
            _notifications.Add(new BaseNotification(GetNotificationObject("character"),
                new FuncPredicate(CharacterShopPredicate)));
            
            _notifications.Add(new BaseNotification(GetNotificationObject("upgrade"),
                new FuncPredicate(UpgradeShopPredicate)));
            
            _notifications.Add(new BaseNotification(GetNotificationObject("aura"),
                new FuncPredicate(UpgradeShopPredicate)));
        }

        private static bool CharacterShopPredicate()
        {
            var current = Configuration.Instance.AllCharacters
                .FirstOrDefault(c => GBGames.saves.characterSaves.IsCurrent(c.ID));
            
            if (current == null) return false;

            var character = Configuration.Instance.AllCharacters
                .Where(c => c.Price <= WalletManager.SecondCurrency && !GBGames.saves.characterSaves.Unlocked(c.ID))
                .OrderByDescending(c => c.Booster)
                .FirstOrDefault(c => c.Booster > current.Booster);

            return character != null;
        }
        private static bool UpgradeShopPredicate()
        {
            var current = Configuration.Instance.AllUpgrades
                .FirstOrDefault(c => GBGames.saves.upgradeSaves.IsCurrent(c.ID));
            
            if (current == null) return false;
            
            var upgrade = Configuration.Instance.AllUpgrades
                .Where(c => c.Price <= WalletManager.FirstCurrency && !GBGames.saves.upgradeSaves.Unlocked(c.ID))
                .OrderByDescending(c => c.Booster)
                .FirstOrDefault(c => c.Booster > current.Booster);
            
            return  upgrade != null;
        }
        
        private Transform GetNotificationObject(string id)
        {
            return notificationItems.FirstOrDefault(n => n.NotificationID == id)?.NotificationObject;
        }
    }
}