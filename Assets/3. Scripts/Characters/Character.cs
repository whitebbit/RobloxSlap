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
        
        private Animator _animator;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(right != null  && left != null) return;
            FindAndAddHandComponent(transform);
        }

        private void FindAndAddHandComponent(Transform parent)
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
        
        private void Start()
        {
            _animator.enabled = true;
            Player.Player.instance.PlayerAnimator.SetAvatar(_animator.avatar);
            StartCoroutine(DelayFixAnimation());
        }

        public void SetUpgrade(UpgradeItem upgrade)
        {
            right.Initialize(upgrade);
            left.Initialize(upgrade);
        }

        private IEnumerator DelayFixAnimation()
        {
            yield return null;
            Player.Player.instance.PlayerAnimator.SetState(false);
            yield return null;
            Player.Player.instance.PlayerAnimator.SetState(true);
            _animator.enabled = false;
        }
    }
}