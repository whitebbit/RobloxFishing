using System.Collections.Generic;
using _3._Scripts.Rangs.Scriptables;
using _3._Scripts.Stages;
using _3._Scripts.UI.Elements;
using _3._Scripts.UI.Panels.Base;
using UnityEngine;
using VInspector;

namespace _3._Scripts.UI.Panels
{
    public class FishIndexPanel : SimplePanel
    {
        [Tab("Catch List")] [SerializeField] private Transform catchListContainer;
        [SerializeField] private CatchList catchListPrefab;

        [Tab("Rang List")] [SerializeField] private List<RangData> rangsData = new();
        [SerializeField] private Transform rangListContainer;
        [SerializeField] private RangList rangListPrefab;

        private readonly List<CatchList> _catchLists = new();
        private readonly List<RangList> _rangLists = new();

        public override void Initialize()
        {
            base.Initialize();
            InitializeCatchList();
            InitializeRangList();
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            UpdateLists();
        }

        private void UpdateLists()
        {
            foreach (var catchList in _catchLists)
            {
                catchList.UpdateCatchList();
            }

            foreach (var rangList in _rangLists)
            {
                rangList.UpdateState();
            }
        }

        private void InitializeCatchList()
        {
            foreach (var (key, value) in StageController.Instance.GetAllData())
            {
                var catchList = Instantiate(catchListPrefab, catchListContainer);
                catchList.Initialize(key, value, 0);
                _catchLists.Add(catchList);
            }
        }

        private void InitializeRangList()
        {
            foreach (var obj in rangsData)
            {
                var rangList = Instantiate(rangListPrefab, rangListContainer);
                rangList.Initialize(obj);
                _rangLists.Add(rangList);
            }
        }
    }
}