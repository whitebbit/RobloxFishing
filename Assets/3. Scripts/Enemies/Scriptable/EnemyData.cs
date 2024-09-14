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
        [SerializeField] private string id;
        [Tab("UI")] 
        [SerializeField] private string localizationID;
        [Tab("Settings")]
        [SerializeField] private float strength;
        [SerializeField] private List<CatchData> catchData = new();
        [SerializeField] private ComplexityType complexityType;
        [Tab("View")]
        [SerializeField] private Transform npcModel;

        public string ID => id;
        public Transform NpcModel => npcModel;
        public string LocalizationID => localizationID;
        public float Strength => strength;

        public List<CatchData> CatchData => catchData;
        public ComplexityType ComplexityType => complexityType;
        
    }
}