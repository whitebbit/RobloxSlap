using System;
using UnityEngine;

namespace _3._Scripts.UI.Elements.Notifications
{
    [Serializable]
    public class NotificationItem
    {
        [SerializeField] private string notificationID;
        [SerializeField] private Transform notificationObject;

        public string NotificationID => notificationID;
        public Transform NotificationObject => notificationObject;
    }
}