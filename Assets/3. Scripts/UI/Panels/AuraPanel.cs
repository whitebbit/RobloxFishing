using System.Collections.Generic;
using _3._Scripts.Config;
using _3._Scripts.Localization;
using _3._Scripts.UI.Elements;
using _3._Scripts.UI.Panels.Base;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace _3._Scripts.UI.Panels
{
    public class AuraPanel : SimplePanel
    {
        [SerializeField] private AuraItem selectedAura;
        [SerializeField] private AuraItem auraItemPrefab;
        [SerializeField] private Transform container;
        [SerializeField] private LocalizeStringEvent selectedAuraBooster;
        [SerializeField] private Button selectButton;

        private AuraItem _currentSlot;
        private readonly List<AuraItem> _auraItems = new();

        public override void Initialize()
        {
            base.Initialize();
            selectButton.onClick.AddListener(SelectAura);
            SpawnAuraItems();
            selectedAuraBooster.gameObject.SetActive(false);
            selectButton.gameObject.SetActive(false);
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            foreach (var auraItem in _auraItems)
            {
                auraItem.UpdateLockState();
            }
        }

        private void SpawnAuraItems()
        {
            var items = Configuration.Instance.AllAuras;
            foreach (var aura in items)
            {
                var item = Instantiate(auraItemPrefab, container);

                item.Initialize(aura);
                item.SetAction(OnClick);

                _auraItems.Add(item);
            }
        }

        private void OnClick(AuraItem slot)
        {
            _currentSlot = slot;

            selectedAura.Initialize(slot.Data);
            selectedAuraBooster.SetVariable("value", slot.Data.Booster);
            selectedAuraBooster.gameObject.SetActive(true);
            selectButton.gameObject.SetActive(!slot.Locked);
        }

        private void SelectAura()
        {
            GBGames.saves.auraSaves.SetCurrent(_currentSlot.Data.ID);
            Player.Player.instance.PlayerAura.Initialize(_currentSlot.Data.ID);
        }
    }
}