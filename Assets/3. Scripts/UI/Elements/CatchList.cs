using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Enemies.Scriptable;
using _3._Scripts.MiniGame;
using _3._Scripts.UI.Enums;
using GBGamesPlugin;
using UnityEngine;
using VInspector;

namespace _3._Scripts.UI.Elements
{
    public class CatchList : MonoBehaviour
    {
        [Tab("Catch List")] [SerializeField] private CatchElement catchElementPrefab;
        [SerializeField] private Transform fishContainer;
        [SerializeField] private Transform treasureContainer;

        private readonly List<CatchElement> _catchElements = new();
        private List<CatchData> _catchData = new();
        private int _stageID;
        
        private void OnEnable()
        {
            UpdateCatchList();
            GBGames.saves.catchSave.OnCatchListUpdate += UpdateCatchList;
        }

        private void OnDisable()
        {
            GBGames.saves.catchSave.OnCatchListUpdate -= UpdateCatchList;
        }

        public void UpdateCatchList()
        {
            var saves = GBGames.saves.catchSave;
            foreach (var element in _catchElements)
            {
                var state = saves.CatchUnlocked(_stageID, element.Data.ID);
                element.SetUnlockedState(state);
            }
        }

        public void Initialize(int stageID, List<CatchData> catchData)
        {
            _catchData = catchData;
            _stageID = stageID;
            
            ClearList();
            SpawnList();
            UpdateCatchList();
        }

        private void SpawnList()
        {
            var fish = _catchData.Where(d => d.Type == CatchType.Fish);
            var treasure = _catchData.Where(d => d.Type == CatchType.Treasure);

            foreach (var data in fish)
            {
                var element = Instantiate(catchElementPrefab, fishContainer);
                element.Initialize(data);
                _catchElements.Add(element);
            }

            foreach (var data in treasure)
            {
                var element = Instantiate(catchElementPrefab, treasureContainer);
                element.Initialize(data);
                _catchElements.Add(element);
            }
        }

        private void ClearList()
        {
            foreach (var obj in _catchElements)
            {
                Destroy(obj.gameObject);
            }

            _catchElements.Clear();
        }
    }
}