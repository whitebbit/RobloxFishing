using System;
using _3._Scripts.Localization;
using _3._Scripts.UI.Extensions;
using _3._Scripts.UI.Scriptable.Shop;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace _3._Scripts.UI.Elements
{
    public class FishingRodUIItem : MonoBehaviour
    {
        [SerializeField] private RawImage icon;
        [SerializeField] private TMP_Text boosterText;
        [SerializeField] private Button selectButton;
        [SerializeField] private LocalizeStringEvent buttonText;

        private UpgradeItem _data;

        public void Initialize(UpgradeItem item)
        {
            _data = item;
            icon.texture = RuntimeFishingRodIconRenderer.Instance.GetTexture2D(item);

            boosterText.text = $"+{item.Booster}";
            selectButton.onClick.AddListener(OnClick);
            UpdateButtonState();
        }

        public void UpdateButtonState()
        {
            var state = WalletManager.FirstCurrency >= _data.Price;
            selectButton.interactable = state;
            if (state)
            {
                var selectedState = GBGames.saves.upgradeSaves.IsCurrent(_data.ID);
                buttonText.SetReference(selectedState ? "selected" : "select");
            }
            else
            {
                buttonText.SetReference("locked");
            }
        }

        public event Action ONSelect;
        private void OnClick()
        {
            
            GBGames.saves.upgradeSaves.SetCurrent(_data.ID);
            Player.Player.instance.UpgradeHandler.SetUpgrade(_data.ID);
            UpdateButtonState();
            ONSelect?.Invoke();

        }
    }
}