using System;
using System.Collections;
using _3._Scripts.UI;
using _3._Scripts.UI.Panels;
using UnityEngine;

namespace _3._Scripts.Ads
{
    public class ConnectionChecker : MonoBehaviour
    {
        [SerializeField] private float checkInterval = 180f;

        private void Start()
        {
            StartCoroutine(IntervalCheckConnection());
        }

        public static bool InternetReachability()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }

        private IEnumerator IntervalCheckConnection()
        {
            yield return new WaitForSeconds(checkInterval);
            var panel = UIManager.Instance.GetPanel<NetworkPanel>();
            var state = !InternetReachability() && !panel.Enabled;
            if (state)
                panel.Enabled = true;
        }
    }
}