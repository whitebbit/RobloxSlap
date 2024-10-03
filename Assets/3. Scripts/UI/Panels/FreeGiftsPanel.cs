using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Stages;
using _3._Scripts.UI.Elements;
using _3._Scripts.UI.Panels.Base;
using UnityEngine;

namespace _3._Scripts.UI.Panels
{
    public class FreeGiftsPanel : SimplePanel
    {
        [SerializeField] private List<GiftSlot> slots;
        [SerializeField] private Transform notification;


        public event Action ONOpen;

        public override void Initialize()
        {
            InTransition = transition;
            OutTransition = transition;
            slots = slots.OrderBy(s => s.TimeToTake).ToList();

            var number = 1;
            
            foreach (var slot in slots)
            {
                slot.Number = number;
                slot.Initialize();

                number += 1;
            }
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            foreach (var slot in slots)
            {
                slot.Initialize();
            }

            ONOpen?.Invoke();
            notification.gameObject.SetActive(false);
        }
    }
}