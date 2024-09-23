using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.MiniGame;
using _3._Scripts.UI.Extensions;
using _3._Scripts.UI.Structs;
using _3._Scripts.Wallet;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Elements
{
    public class CatchElement : MonoBehaviour
    {
        [Tab("Components")]
        [SerializeField] private Image icon;
        [SerializeField] private Image rareImage;
        [SerializeField] private TMP_Text chanceText;
        [SerializeField] private CanvasGroup canvasGroup;
        public CatchData Data { get; private set; }
        public void Initialize(CatchData data, float additionalDropChance =0)
        {
            Data = data;
            icon.sprite = data.Icon;
            icon.ScaleImage();
            rareImage.color = Configuration.Instance.GetRarityTable(data.Rarity).MainColor;
            chanceText.text = $"{WalletManager.ConvertToPercentageOrFraction(data.DropChance + additionalDropChance)}";
        }

        public void SetUnlockedState(bool state)
        {
            icon.color = state ? Color.white : Color.black;
        }
        
        public void SetState(bool state)
        {
            if (state)
            {
                gameObject.SetActive(true);
                canvasGroup.DOFade(1, 0.25f);
            }
            else
            {
                gameObject.SetActive(false);
                canvasGroup.alpha = 0;
            }
        }
        
    }
}