using _3._Scripts.UI.Extensions;
using _3._Scripts.UI.Scriptable.Roulette;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.UI.Elements
{
    public class RewardItem : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text title;

        public void Initialize(GiftItem data)
        {
            icon.sprite = data.Icon();
            title.text = data.Title();
            icon.ScaleImage();
        }
    }
}