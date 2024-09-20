using System;
using _3._Scripts.Config;
using Cinemachine;
using UnityEngine;

namespace _3._Scripts
{
    public class CameraSettingsProvider : MonoBehaviour
    {
        [SerializeField] private float mobileXMaxSpeed;
        [SerializeField] private float mobileYMaxSpeed;

        private CinemachineFreeLook _camera;

        private void Awake()
        {
            _camera = GetComponent<CinemachineFreeLook>();
        }

        private void Start()
        {
            _camera.m_XAxis.m_MaxSpeed = mobileXMaxSpeed;
            _camera.m_YAxis.m_MaxSpeed = mobileYMaxSpeed;
        }

#if UNITY_EDITOR

        private void Update()
        {
            _camera.m_XAxis.m_MaxSpeed = mobileXMaxSpeed;
            _camera.m_YAxis.m_MaxSpeed = mobileYMaxSpeed;
        }
#endif
    }
}