using System.Collections.Generic;
using System.Linq;
using _3._Scripts.MiniGame;
using _3._Scripts.UI.Structs;
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
        [Tab("Rarity")] 
        [SerializeField] private List<RarityTable> rarityTables;

        public CatchData Data { get; private set; }
        public void Initialize(CatchData data)
        {
            Data = data;
            icon.sprite = data.Icon;
            rareImage.color = rarityTables.FirstOrDefault(r => r.Rarity == data.Rarity).MainColor;
            chanceText.text = $"{data.DropChance}%";
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