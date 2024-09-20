using System;
using _3._Scripts.Inputs.Enums;
using _3._Scripts.Inputs.Interfaces;
using _3._Scripts.Singleton;
using UnityEngine;


namespace _3._Scripts.Inputs
{
    public class InputHandler : Singleton<InputHandler>
    {
        [SerializeField] private MobileInput mobileInput;
        private DesktopInput _desktopInput;

        public IInput Input
        {
            get
            {
                if (!mobileInput.gameObject.activeSelf)
                    mobileInput.gameObject.SetActive(true);
                
                        //UnityEngine.Input.multiTouchEnabled = true;
                return mobileInput;
            }
        }

        private void Start()
        {
            SetActionButtonType(ActionButtonType.Training);
        }

        public void SetActionButtonType(ActionButtonType type)
        {
            mobileInput.SetActionButtonType(type);
        }
        
        public void SetState(bool state)
        {
            mobileInput.SetState(state);
        }
    }
}