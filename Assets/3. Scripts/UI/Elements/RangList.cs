using System.Collections.Generic;
using _3._Scripts.Localization;
using _3._Scripts.Rangs.Scriptables;
using _3._Scripts.UI.Scriptable.Roulette;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Elements
{
    public class RangList : MonoBehaviour
    {
        [Tab("Progress")]
        [SerializeField] private LocalizeStringEvent title;
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text progressText;
        [Tab("Reward")]
        [SerializeField] private Transform rewardContainer;
        [SerializeField] private RewardItem rewardItemPrefab;
        [Tab("Button")]
        [SerializeField] private Button getRewardButton;
        [SerializeField] private  LocalizeStringEvent buttonStateText;
        
        private RangData _data;

        public void Initialize(RangData data)
        {
            _data = data;
            title.SetReference(data.RangNameID);
            
            InitializedRewards();
            UpdateState();
        }
        
        public void UpdateState()
        {
            var percent = (float) GBGames.saves.catchSave.catchList.Count / _data.CountToUnlock;
            var state = (percent * 100) >= 100;
            
            slider.value = percent;
            progressText.text = $"{WalletManager.ConvertToWallet((decimal) (percent * 100))}%";
            buttonStateText.SetReference(GBGames.saves.rangSaves.GetState(_data.RangNameID) ? "received" : "receive");
            getRewardButton.gameObject.SetActive(state);
            getRewardButton.onClick.AddListener(GetReward);
        }
        
        private void InitializedRewards()
        {
            foreach (var reward in _data.Rewards)
            {
                var rewardItem = Instantiate(rewardItemPrefab, rewardContainer);
                rewardItem.Initialize(reward);
            }
        }
        
        private void GetReward()
        {
            if (GBGames.saves.rangSaves.GetState(_data.RangNameID)) return;

            foreach (var reward in _data.Rewards)
            {
                reward.OnReward();
            }
            GBGames.saves.rangSaves.SetState(_data.RangNameID, true);
            buttonStateText.SetReference(GBGames.saves.rangSaves.GetState(_data.RangNameID) ? "received" : "receive");
        }
    }
}