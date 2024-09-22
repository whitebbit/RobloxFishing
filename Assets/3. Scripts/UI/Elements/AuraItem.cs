using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Aura.Scriptables;
using _3._Scripts.Config;
using _3._Scripts.Localization;
using _3._Scripts.UI.Enums;
using _3._Scripts.UI.Structs;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Elements
{
    public class AuraItem : MonoBehaviour
    {
        [Tab("Components")] [SerializeField] private Image auraIcon;
        [SerializeField] private Image rarityImage;
        [Space] [SerializeField] private TMP_Text rarityText;
        [SerializeField] private TMP_Text dropChanceText;
        [SerializeField] private LocalizeStringEvent rarityStringEvent;
        [Space] [SerializeField] private Button button;
        
        [Space] [SerializeField] private Transform lockerObject;
        public AuraData Data { get; private set; }
        
        public bool Locked => !GBGames.saves.catchSave.CatchIsCaught(Data.ID);
        public void Initialize(AuraData data)
        {
            var currentRarity = Configuration.Instance.GetRarityTable(data.Rarity);

            Data = data;
            auraIcon.sprite = data.Icon;
            rarityImage.color = currentRarity.MainColor;
            rarityText.color = currentRarity.MainColor;
            dropChanceText.text = $"{WalletManager.ConvertToPercentageOrFraction(data.DropChance)}";
            rarityStringEvent.SetReference(currentRarity.TitleID);
            rarityImage.gameObject.SetActive(true);
        }
        
        public void SetAction(Action<AuraItem> action)
        {
            button.onClick.AddListener(() => action?.Invoke(this));
        }
        
        public void UpdateLockState()
        {
            lockerObject.gameObject.SetActive(Locked);
        }
    }
}