using System;
using _3._Scripts.Singleton;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace _3._Scripts
{
    public class CameraController : Singleton<CameraController>
    {
        [SerializeField] private CinemachineVirtualCameraBase mainCamera;

        private CinemachineVirtualCameraBase _currentCam;

        private void Start()
        {
            SwapToMain();
        }

        public void SwapTo(CinemachineVirtualCameraBase cam)
        {
            _currentCam = cam;
            mainCamera.Priority = -1;
            _currentCam.Priority = 100;
        }

        public void SetShake(float intensity)
        {
            var channelPerlin = (_currentCam as CinemachineVirtualCamera)?.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            if (channelPerlin is not null) 
                channelPerlin.m_AmplitudeGain = intensity;
        }
        
        public void SwapToMain()
        {
            if (_currentCam != null)
            {
                _currentCam.Priority = -1;
                _currentCam = null;
            }
            mainCamera.Priority = 100;
        }
        
    }
}