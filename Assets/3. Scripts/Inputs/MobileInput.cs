using System;
using _3._Scripts.Inputs.Enums;
using _3._Scripts.Inputs.Interfaces;
using _3._Scripts.Inputs.Utils;
using _3._Scripts.Singleton;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.Inputs
{
    public class MobileInput : MonoBehaviour, IInput
    {
        [Tab("Input Components")]
        [SerializeField] private Joystick joystick;
        [SerializeField] private FixedTouchField touchField;
        [SerializeField] private FixedButton jumpButton;
        [SerializeField] private FixedButton actionButton;
        [SerializeField] private FixedButton interactButton;
        [Tab("Action Components")] 
        [SerializeField] private Image baseImage;
        [SerializeField] private TMP_Text fightText;
        [SerializeField] private TMP_Text trainingText;
        [Space] 
        [SerializeField] private Image buttonImage;
        [SerializeField] private Sprite baseSprite;
        [SerializeField] private Sprite trainingSprite;
        [SerializeField] private Sprite fightSprite;
        
        
        private CanvasGroup _canvas;
        
        private void Awake()
        {
            _canvas = GetComponent<CanvasGroup>();
        }
        
        public void SetState(bool state)
        {
            _canvas.alpha = state ? 1 : 0;
        }
        
        public Vector2 GetMovementAxis()
        {
            return joystick.Direction.normalized;
        }

        public Vector2 GetLookAxis()
        {
            return touchField.Axis.normalized;
        }

        public bool GetAction()
        {
            return actionButton.ButtonDown;
        }

        public bool GetJump()
        {
            return jumpButton.ButtonDown;
        }

        public bool GetInteract()
        {
            return interactButton.ButtonDown;
        }

        public bool CanLook()
        {
            return touchField.Pressed;
        }

        public void CursorState()
        {
            
        }

        public void SetMovementState(bool state)
        {
            joystick.gameObject.SetActive(state);
            touchField.gameObject.SetActive(state);
            jumpButton.gameObject.SetActive(state);
        }
        
        public void SetActionButtonType(ActionButtonType type)
        {
            switch (type)
            {
                case ActionButtonType.Training:
                    baseImage.gameObject.SetActive(false);
                    trainingText.gameObject.SetActive(true);
                    fightText.gameObject.SetActive(false);

                    buttonImage.sprite = trainingSprite;
                    break;
                case ActionButtonType.Fight:
                    baseImage.gameObject.SetActive(false);
                    trainingText.gameObject.SetActive(false);
                    fightText.gameObject.SetActive(true);

                    buttonImage.sprite = fightSprite;
                    break;
                case ActionButtonType.Base:
                    baseImage.gameObject.SetActive(true);
                    trainingText.gameObject.SetActive(false);
                    fightText.gameObject.SetActive(false);

                    buttonImage.sprite = baseSprite;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}