using System;
using _3._Scripts.Singleton;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Player
{
    public class FishingLine : Singleton<FishingLine>
    {
        [SerializeField] private LineRenderer fishingLine;
        [SerializeField] private Transform fishingFloat;
        
        private Transform _startPoint;
        private Transform _target;

        private void Start()
        {
            SetState(false);
        }

        public void SetState(bool state) => fishingLine.gameObject.SetActive(state);
        
        public void SetStartPoint(Transform target) => _startPoint = target;
        public void SetTarget(Transform target) => _target = target;

        private void Update()
        {
            if (_target == null) return;

            var position = _target.transform.position;
            
            fishingLine.positionCount = 2;
            fishingLine.SetPosition(0, _startPoint.transform.position);
            fishingLine.SetPosition(1, position);

            fishingFloat.transform.position = position;
        }
    }
}