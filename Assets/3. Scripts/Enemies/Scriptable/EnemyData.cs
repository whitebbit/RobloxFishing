using System;
using System.Collections.Generic;
using _3._Scripts.MiniGame;
using UnityEngine;
using UnityEngine.Serialization;
using VInspector;

namespace _3._Scripts.Enemies.Scriptable
{
    [CreateAssetMenu(fileName = "Enemy Data", menuName = "ScriptableObjects/EnemyData", order = 0)]
    public class EnemyData : ScriptableObject
    {       
        [Tab("UI")] 
        [SerializeField] private string localizationID;
        [Tab("Settings")]
        [SerializeField] private float strength;

        [SerializeField] private float additionalDropChance;
        
        [SerializeField] private List<CatchData> catchData = new();
        [SerializeField] private ComplexityType complexityType;
        [Tab("View")]
        [SerializeField] private Material skin;
        
        public float AdditionalDropChance => additionalDropChance;
        public Material Skin => skin;    
        public string LocalizationID => localizationID;
        public float Strength => strength;

        public List<CatchData> CatchData => catchData;
        public ComplexityType ComplexityType => complexityType;
        
    }
}