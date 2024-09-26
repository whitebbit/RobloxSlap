using System;
using System.Collections;
using System.Collections.Generic;
using _3._Scripts.FSM.Base;
using DG.Tweening;
using UnityEngine;

namespace _3._Scripts.UI.Elements.Notifications
{
    [Serializable]
    public class BaseNotification
    {
        public string ID { get; private set; }
        private Transform _notificationObject;
        private FuncPredicate _funcPredicate;

        private bool _active;
        private Tween _currentTween;

        public BaseNotification(NotificationItem item, FuncPredicate predicateToShow)
        {
            ID = item.NotificationID;
            _notificationObject = item.NotificationObject;
            _funcPredicate = predicateToShow;
        }

        public void HideNotification()
        {
            if(!_active) return;

            _active = false;

            _notificationObject.transform.localScale = Vector3.one;
            _notificationObject.gameObject.SetActive(false);

            _currentTween?.Kill();
            _currentTween = null;
        }

        public void ShowNotification()
        {
            if(_active) return;
            
            if(!_funcPredicate.Evaluate()) return;
            
            _active = true;
            _notificationObject.gameObject.SetActive(true);
            _currentTween = _notificationObject.DOScale(1.25f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        }
    }
}