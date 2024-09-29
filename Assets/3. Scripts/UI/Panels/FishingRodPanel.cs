using System;
using System.Collections.Generic;
using _3._Scripts.Config;
using _3._Scripts.UI.Elements;
using _3._Scripts.UI.Panels.Base;
using UnityEngine;

namespace _3._Scripts.UI.Panels
{
    public class FishingRodPanel : SimplePanel
    {
        [SerializeField] private FishingRodUIItem prefab;
        [SerializeField] private Transform container;

        private readonly List<FishingRodUIItem> _list = new();
        public event Action ONOpen;
        public override void Initialize()
        {
            base.Initialize();
            InitializeList();
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            ONOpen?.Invoke();
            UpdateAllList();
        }

        private void InitializeList()
        {
            var list = Configuration.Instance.AllUpgrades;
            foreach (var item in list)
            {
                var table = Instantiate(prefab, container);
                table.Initialize(item);
                table.ONSelect += UpdateAllList;
                _list.Add(table);
            }
        }

        private void UpdateAllList()
        {
            foreach (var item in _list)
            {
                item.UpdateButtonState();
            }
        }
    }
}