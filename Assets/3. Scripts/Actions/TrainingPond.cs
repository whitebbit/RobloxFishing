using System;
using DG.Tweening;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Actions
{
    public class TrainingPond : MonoBehaviour
    {
        [Tab("Components")] [SerializeField] private Transform pond;
        [SerializeField] private Player.Player player;

        private bool _active;

        private void Start()
        {
            SetState(false);
        }

        public void SetState(bool state)
        {
            if (state) Activate();
            else Deactivate();
        }

        private void Deactivate()
        {
            _active = false;
            pond.gameObject.SetActive(false);
        }

        private void Activate()
        {
            if (_active) return;

            var playerTransform = player.transform;
            var position = playerTransform.position + playerTransform.forward * 2f;
            position.y = 0;
            
            _active = true;
            transform.position = position;

            pond.gameObject.SetActive(true);
            pond.transform.DOMoveY(-1, 0.25f).From().SetEase(Ease.InOutBack);
        }
    }
}