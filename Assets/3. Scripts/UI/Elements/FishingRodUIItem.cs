using _3._Scripts.UI.Extensions;
using _3._Scripts.UI.Scriptable.Shop;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

namespace _3._Scripts.UI.Elements
{
    public class FishingRodUIItem : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text boosterText;
        [SerializeField] private Button selectButton;

        private UpgradeItem _data;

        public void Initialize(UpgradeItem item)
        {
            _data = item;
            icon.sprite = item.Icon;
            icon.ScaleImage();

            boosterText.text = $"+{item.Booster}";
            selectButton.onClick.AddListener(OnClick);
            UpdateButtonState();
        }

        public void UpdateButtonState()
        {
            selectButton.interactable = WalletManager.FirstCurrency >= _data.Price;
        }

        private void OnClick()
        {
            GBGames.saves.upgradeSaves.SetCurrent(_data.ID);
            Player.Player.instance.UpgradeHandler.SetUpgrade(_data.ID);
        }
    }
}