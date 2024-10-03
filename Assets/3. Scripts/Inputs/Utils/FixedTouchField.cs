using _3._Scripts.Config;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace _3._Scripts.Inputs.Utils
{
    public class FixedTouchField : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private float minMovementThreshold; // Минимальный порог движения
        [SerializeField] private float smoothingSpeed = 5f; // Скорость сглаживания


        private int _pointerId;
        private Vector2 _startTouchPosition;
        private Vector2 _currentTouchPosition;
        private Vector2 _previousTouchPosition;
        private Vector2 _smoothedAxis; // Сглаженное направление

        public Vector2 Axis { get; private set; }

        public bool Pressed { get; private set; }

        private void Update()
        {
            if (Pressed)
            {
                var distance = Vector2.Distance(_currentTouchPosition, _previousTouchPosition);

                if (distance >= minMovementThreshold)
                {
                    // Calculate raw axis
                    var rawAxis = _currentTouchPosition - _startTouchPosition;

                    // Smooth the axis movement over time using unscaledDeltaTime for frame independence
                    _smoothedAxis = Vector2.Dot(_smoothedAxis.normalized, rawAxis.normalized) < minMovementThreshold 
                        ? rawAxis 
                        : Vector2.Lerp(_smoothedAxis, rawAxis, smoothingSpeed * Time.unscaledDeltaTime);

                    Axis = _smoothedAxis;
                }
                else
                {
                    Axis = Vector2.zero;
                    _startTouchPosition = _currentTouchPosition;
                }

                _previousTouchPosition = _currentTouchPosition;
            }
            else
            {
                Axis = Vector2.zero;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Pressed) return;
            Pressed = true;
            _pointerId = eventData.pointerId;
            _startTouchPosition = eventData.position;
            _currentTouchPosition = _startTouchPosition;
            _previousTouchPosition = _startTouchPosition;
            Axis = Vector2.zero;
            _smoothedAxis = Vector2.zero; // Обнуление сглаженного вектора при новом касании
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (Pressed && eventData.pointerId == _pointerId)
            {
                _currentTouchPosition = eventData.position;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!Pressed || eventData.pointerId != _pointerId) return;
            Pressed = false;
            Axis = Vector2.zero;
            _smoothedAxis = Vector2.zero; // Сброс сглаженного вектора при отпускании
        }
    }
}