using System;
using _3._Scripts.Player;
using _3._Scripts.Sounds;
using UnityEngine;

namespace _3._Scripts.MiniGame
{
    public abstract class Fighter : MonoBehaviour
    {
        protected bool isFight;
        public event Action OnSlap;

        public void InitializeAnimation()
        {
            Animator().Event += AnimatorAction;
        }

        public virtual void StartFight()
        {
            isFight = true;
            Animator().SetBool("IsFight", true);
        }

        public void GetHit() => Animator().SetTrigger("GetHit");

        public virtual void OnStart(){}
        public virtual void OnEnd(){}
        public virtual void EndFight(bool win)
        {
            OnSlap = null;
            if (!win) Animator().SetTrigger("Dead");
            Animator().SetBool("IsFight", false);
            isFight = false;
        }

        public abstract FighterData FighterData();
        protected abstract PlayerAnimator Animator();
        private void AnimatorAction(string id)
        {
            switch (id)
            {
                case "Action":
                    OnSlap?.Invoke();
                    SoundManager.Instance.PlayOneShot("action");
                    break;
            }
        }
        protected void Slap()
        {
            Animator().SetTrigger("Slap");
        }
    }
}