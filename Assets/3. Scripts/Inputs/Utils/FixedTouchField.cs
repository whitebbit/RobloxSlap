using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace _3._Scripts.Inputs.Utils
{
    public class FixedTouchField : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        private int _pointerId;
        private Vector2 _startTouchPosition;
        private Vector2 _currentTouchPosition;
        private Vector2 _axis;

        public Vector2 Axis => _axis;
        public bool Pressed { get; private set; }

        private void Update()
        {
            if (Pressed)
            {
                _axis = _currentTouchPosition - _startTouchPosition;
            }
            else
            {
                _axis = Vector2.zero;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Pressed) return;
            Pressed = true;
            _pointerId = eventData.pointerId;
            _startTouchPosition = eventData.position;
            _currentTouchPosition = _startTouchPosition;
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
            _axis = Vector2.zero;
        }
    }
}