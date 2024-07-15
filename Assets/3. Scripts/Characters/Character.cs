using System;
using System.Collections;
using System.Collections.Generic;
using _3._Scripts.UI.Scriptable.Shop;
using _3._Scripts.Upgrades;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace _3._Scripts.Characters
{
    [ExecuteInEditMode]
    public class Character : MonoBehaviour
    {
        [SerializeField] private Hand right;
        [SerializeField] private Hand left;
        
        [SerializeField]private Animator animator;

#if UNITY_EDITOR
        private void OnValidate()
        {
            animator = GetComponent<Animator>();
            if(right != null  && left != null) return;
            FindAndAddHandComponent(transform);
        }

        private void FindAndAddHandComponent(IEnumerable parent)
        {
            if(right != null  && left != null) return;
            
            foreach (Transform child in parent)
            {
                switch (child.name)
                {
                    case "mixamorig:RightHand":
                    {
                        right = child.GetComponent<Hand>();
                        if (right == null)
                        {
                            right = Undo.AddComponent<Hand>(child.gameObject);
                        }

                        break;
                    }
                    case "mixamorig:LeftHand":
                    {
                        left = child.GetComponent<Hand>();
                        if (left == null)
                        {
                            left = Undo.AddComponent<Hand>(child.gameObject);
                        }
                        break;
                    }
                }
                FindAndAddHandComponent(child);
            }
        }
#endif

        public void Initialize()
        {
            Player.Player.instance.PlayerAnimator.SetAvatar(animator.avatar);
        }

        public void SetUpgrade(UpgradeItem upgrade)
        {
            right.Initialize(upgrade);
            left.Initialize(upgrade);
        }
    }
}