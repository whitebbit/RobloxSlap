using System;
using _3._Scripts.Ads;
using _3._Scripts.UI.Panels.Base;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.UI.Panels
{
    public class NetworkPanel : SimplePanel
    {
        private void Update()
        {
            if (!ConnectionChecker.InternetReachability()) return;
            Enabled = false;
        }
    }
}